using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Yarn.Unity;

/// <summary>
/// 가게 운영 상태 나타내는 enum
/// </summary>
public enum BloodSellStatus{
    None,
    SelectBlood,
    Filtered
}

/// <summary>
/// 가게 운영을 총괄
/// : Store scene -> NPC 오브젝트
/// </summary>
public class NPCInteract : MonoBehaviour
{
    public GameObject npcObject; // assign at Inspector
    public List<GameObject> cameraTargetObjectList; // assign at Inspector
    public static string tasteStatement; // static warning

    public GameObject endSellProcessButton; // assign at Inspector
    public GameObject bloodPackCanvas; // assign at Inspector
    public GameObject nextDayButton; // assign at Inspector

    public CameraControl cameraControl;
    public DialogueControl dialogueControl;
    public YarnControl yarnControl;
    public DialogueRunner dialogueRunner;
    public BloodSellProcess bloodSellProcess; // assign at inspector
    public TutorialManager tutorialManager; // assign at inspector
    public RatingTextControl ratingTextControl; // assign at inspector

    Coroutine npcCoroutine;

    // List<NPCSO> npcs; // get from DialogueControl
    List<Sprite> npcSpritesList;

    BloodSellStatus sellStatus;

    void Start(){
        StartCoroutine(WaitUntilAllsettingsdone());
    }

    private void OnDestroy()
    {
        if(npcCoroutine != null){
            StopCoroutine(npcCoroutine);
            npcCoroutine = null;
        }
    }

    /// <summary>
    /// 가게 운영 시작 전 모든 세팅(UI, fade, 튜토리얼)이 끝날 때까지 기다린 후 npc 대화 시작
    /// </summary>
    /// <returns></returns>
     IEnumerator WaitUntilAllsettingsdone(){
        sellStatus = BloodSellStatus.None;

        // npcs = dialogueControl.npcs; // test
        CameraControl.targetsForYarn = new(cameraTargetObjectList);

        npcObject.SetActive(false);

        endSellProcessButton.SetActive(false);
        bloodPackCanvas.SetActive(false);
        nextDayButton.SetActive(false);
        
        GameManager.Instance.ableToFade = true;

        if(GameManager.Instance.isSceneLoadEnd)
            yield return new WaitUntil(() => !GameManager.Instance.isSceneLoadEnd);
        yield return new WaitUntil(() => GameManager.Instance.isSceneLoadEnd);

        yield return new WaitForSeconds(0.5f);
        
        yield return StartCoroutine(WaitUntilTutorialEnds());
        
        StartCoroutine(GetStoreDialogues());
    }

    /// <summary>
    /// 튜토리얼이 끝날 때까지 기다림
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitUntilTutorialEnds(){
        if(GameManager.Instance.isTurotial){
            yield return new WaitUntil(() => tutorialManager.isTutorialFinish);
            yield return new WaitForSeconds(0.5f);
        }
    }
    
    /// <summary>
    /// 현재 씬에서 상호작용해야 할 대화 dialogueControl에서 불러온 후 대화 시작
    /// , 가게 시작 시 상호작용할 대화 먼저 진행 -> 가게 운영 시작
    /// </summary>
    /// <returns></returns>
    IEnumerator GetStoreDialogues(){
        dialogueControl.GetAllDialogues(WhereNodeStart.Store, WhenNodeStart.SceneLoad, false);

        if(dialogueControl.count > 0){
            while(dialogueControl.npcIndex < dialogueControl.count){
                yarnControl.ChangeUIImg(0);
                GameManager.Instance.StartDialogue(dialogueControl.allDialogues[dialogueControl.npcIndex].dialogueName);
                yield return new WaitUntil(() => !dialogueRunner.IsDialogueRunning);
                
                yield return new WaitForSeconds(1f); 
                dialogueControl.npcIndex++;
            }
        }

        yield return new WaitForSeconds(1.5f);
        
        dialogueControl.npcIndex = 0;
        dialogueControl.GetAllDialogues(WhereNodeStart.Store, WhenNodeStart.Click, true);
        npcCoroutine = StartCoroutine(StartCustomer());
    }

    /// <summary>
    /// 가게 손님 상호작용 총괄 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator StartCustomer(){
        if(dialogueControl.count == 0){
            ReadyToMoveNextDay();
            npcCoroutine = null;
            yield break;
        }
        
        SetStoreDialogues(dialogueControl.npcIndex);
        GetBloodTaste(dialogueControl.npcIndex);

        yield return ActiveSprite();

        yield return new WaitUntil(() => dialogueRunner.IsDialogueRunning); // wait until mouse click
        // tell what they want
        yield return new WaitUntil(() => !dialogueRunner.IsDialogueRunning); // wait until dialogue ends
        
        if(yarnControl.isSell){ // if sellect sell
            // yield return new WaitUntil(() => YarnControl.isSelect);
            // YarnControl.isSelect = false;
            
            yarnControl.isSell = false;
            endSellProcessButton.SetActive(true);

            yield return new WaitUntil(() => bloodSellProcess.isBloodSellFinish);
            bloodSellProcess.isBloodSellFinish = false;
            endSellProcessButton.SetActive(false);

            ChangeSellStatus();

            YarnControl.sellInfo = CalculateSellInfo(dialogueControl.npcIndex);
            GameManager.Instance.sellCount++;
            GameManager.Instance.totalPoint += YarnControl.sellInfo;

            YarnControl.sellPrice = CalculatePrice(YarnControl.sellInfo);
            sellStatus = BloodSellStatus.None;
            
            if(yarnControl.nodeName != ""){
                GameManager.Instance.StartDialogue(yarnControl.nodeName); // tell their evaluation or end dialogue
                yield return new WaitUntil(() => !dialogueRunner.IsDialogueRunning);
                yarnControl.nodeName = "";
                ratingTextControl.gameObject.SetActive(true);
                ratingTextControl.StartCoroutine(ratingTextControl.ShowRatingTextAnimation(YarnControl.sellInfo));
            }

            YarnControl.sellInfo = 0;
            bloodSellProcess.ResetAllBloodSellStatus();
        }

        yield return StartCoroutine(DeActiveSprite());

        yield return new WaitForSeconds(1);

        dialogueControl.npcIndex++;

        if(dialogueControl.npcIndex < dialogueControl.count){
            yield return StartCoroutine(StartCustomer());
        }
        
        if(dialogueControl.npcIndex == dialogueControl.count){ // for trigger only at final interaction
            ReadyToMoveNextDay();
            npcCoroutine = null;
        }
    }
    
    /// <summary>
    /// InteractObjInfo에 대화 정보 저장
    /// </summary>
    /// <param name="index">현재 진행중인 대화의 index</param>
    void SetStoreDialogues(int index){
        InteractObjInfo interactObjInfo = npcObject.GetComponent<InteractObjInfo>();
        if(interactObjInfo == null){
            interactObjInfo = npcObject.AddComponent<InteractObjInfo>();
        }

        interactObjInfo._interactType = InteractType.StartDialogue;
        interactObjInfo._nodeName = dialogueControl.allDialogues[index].dialogueName;
    }
    
    /// <summary>
    /// 현재 npc의 혈액 취향에 대한 문장 할당, 혈액팩 요구 시 사용됨
    /// </summary>
    /// <param name="index">현재 진행중인 대화의 index</param>
    void GetBloodTaste(int index){
        tasteStatement = ""; // reset
        tasteStatement = dialogueControl.allDialogues[index].tasteLine;
    }

    /// <summary>
    /// npc의 이미지 세팅, 이미지 로드, fade in
    /// </summary>
    /// <returns></returns>
    IEnumerator ActiveSprite(){
        npcSpritesList = dialogueControl.allDialogues[dialogueControl.npcIndex].sprites;

        if(npcSpritesList == null || npcSpritesList.Count == 0){
            Debug.Log("There is no sprites in index " + dialogueControl.npcIndex + " dialogue Info...");
            yield break;
        }

        npcObject.GetComponent<SpriteRenderer>().sprite = npcSpritesList[0];

        npcObject.SetActive(true);
        
        npcObject.GetComponent<Collider2D>().enabled = false;
        yield return GameManager.Instance.StartCoroutine(GameManager.Instance.FadeInSprite(npcObject.GetComponent<SpriteRenderer>(), 0.35f));
        npcObject.GetComponent<Collider2D>().enabled = true;
    }

    /// <summary>
    /// npc 이미지 fade out
    /// </summary>
    /// <returns></returns>
    IEnumerator DeActiveSprite(){
        npcObject.GetComponent<Collider2D>().enabled = false;
        yield return GameManager.Instance.StartCoroutine(GameManager.Instance.FadeOutSprite(npcObject.GetComponent<SpriteRenderer>(), 0.35f));
        npcObject.SetActive(false);
    }

    // public void StartDialogue(string nodeName){
    //     if(!dialogueRunner.IsDialogueRunning)
    //     {
    //         if(dialogueRunner.NodeExists(nodeName))
    //             dialogueRunner.StartDialogue(nodeName);
    //         else
    //             Debug.Log(nodeName + " is not Exist...");
    //     }
    //     else
    //     {
    //         Debug.Log("Other Dialogue is running...");
    //     }
    // }

    /// <summary>
    /// 현재 가게 운영 상태 변경
    /// </summary>
    public void ChangeSellStatus(){
        if(bloodSellProcess.isBloodSelected){
            sellStatus = BloodSellStatus.SelectBlood;

            if(bloodSellProcess.isFiltered){
                sellStatus = BloodSellStatus.Filtered;
            }
        }
    }

    // void CreateVirtualCamera(int targetIndex){
    //     InteractObjInfo interactObjInfo = gameObject.GetComponent<InteractObjInfo>();
    //     if(interactObjInfo == null){
    //         interactObjInfo = gameObject.AddComponent<InteractObjInfo>();
    //     }
        
    //     if(cameraTarget == null || cameraTarget.Count == 0){
    //         Debug.Log("Target list is empty...");
    //         return;
    //     }

    //     if(targetIndex > cameraTarget.Count -1 || cameraTarget[targetIndex] == null){
    //         Debug.Log("There is no camera target in list...");
    //         return;
    //     }

    //     interactObjInfo.SetVirtualCameraInfo(cameraTarget[targetIndex], false, null, 5.4f, 0.25f, Cinemachine.CinemachineBlendDefinition.Style.EaseInOut, 1.5f);
    //     cameraControl.ChangeCam(interactObjInfo);
    // }   
    
    /// <summary>
    /// 현재 판매 결과에 따른 평점 계산
    /// </summary>
    /// <param name="index">현재 진행중인 npc index</param>
    /// <returns>평점 계산 결과</returns>
    float CalculateSellInfo(int index){
        float point = 0;

        if(sellStatus == BloodSellStatus.None){
            return point;
        }

        BloodPackUITest bloodPackUI = bloodPackCanvas.GetComponentInChildren<BloodPackUITest>();

        if(bloodPackUI == null){
            Debug.Log("There is no bloodPackUITest...");
            return -1;
        }

        List<string> select = bloodPackUI.GetTogleCondition();

        int totalCount = 0;
        foreach(string taste in dialogueControl.allDialogues[index].tastes){
            if(taste != ""){
                totalCount++;
            }
        }
        
        int count = 0;
        foreach(string taste in dialogueControl.allDialogues[index].tastes){
            for(int i=0; i<3; i++){
                if(taste != "" && select[i] == taste){
                    count++;            
                    break;
                }
            }
        }

        float ratio = (float)count/totalCount;

        if(ratio > 0.6f || count == totalCount)
        {
            point = 5;
        }
        else if(ratio > 0.3f)
        {
            point = 4;
        }
        else
        {
            point = 2;
        }

        Debug.Log("Point before weight : " + point);
        point *= dialogueControl.allDialogues[index].weight;
        Debug.Log("Point after weight : " + point);

        if(sellStatus == BloodSellStatus.Filtered){
            float pointRatio = point/5;
            float extraPoint = pointRatio * (1- pointRatio) * point;
            
            float newWeight = UnityEngine.Random.Range(0.8f, 1.1f);
            if(newWeight > 1){
                newWeight = 1;
            }

            point += extraPoint * newWeight;
            Debug.Log("Point after filtering : " + point);
        }

        point = (float)Math.Round(point, 2);
        
        if(point > 5){
            point = 5;
        }
        
        return point;
    }
    
    /// <summary>
    /// 현재 판매 평점에 따른 가격 계산
    /// </summary>
    /// <param name="sellInfo">현재 판매에 대한 평점</param>
    /// <returns>가격 계산 결과</returns>
    float CalculatePrice(float sellInfo){        
        if(sellStatus == BloodSellStatus.None){
            return 0;
        }

        float randPriceBase = sellInfo * 10 * sellInfo/5 + 50;

        float price = randPriceBase * UnityEngine.Random.Range(0.8f, 1f);
        price = (float)Math.Round(price, 1);
        return price;
    }

    // test
    /// <summary>
    /// 다음날로 넘어감
    /// </summary>
    void ReadyToMoveNextDay(){
        nextDayButton.SetActive(true);
    }


}
