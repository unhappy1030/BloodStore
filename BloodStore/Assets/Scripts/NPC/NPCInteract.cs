using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Yarn.Unity;

public enum BloodSellStatus{
    None,
    SelectBlood,
    Filtered
}

public class NPCInteract : MonoBehaviour
{
    public GameObject npc; // assign at Inspector
    public List<GameObject> cameraTarget; // assign at Inspector
    public static string tasteStr; // static warning

    public GameObject endSellProcessButton; // assign at Inspector
    public GameObject bloodPackCanvas; // assign at Inspector
    public GameObject nextDayButton; // assign at Inspector

    public CameraControl cameraControl;
    public DialogueControl dialogueControl;
    public YarnControl yarnControl;
    public DialogueRunner dialogueRunner;
    public BloodSellProcess bloodSellProcess; // assign at inspector
    public Tutorial tutorial; // assign at inspector

    Coroutine npcCoroutine;

    // List<NPCSO> npcs; // get from DialogueControl
    List<Sprite> npcSprites;

    BloodSellStatus sellStatus;

    void Start(){
        sellStatus = BloodSellStatus.None;

        // npcs = dialogueControl.npcs; // test
        CameraControl.targetsForYarn = new(cameraTarget);

        npc.SetActive(false);

        endSellProcessButton.SetActive(false);
        bloodPackCanvas.SetActive(false);
        nextDayButton.SetActive(false);

        StartCoroutine(WaitUntilTutorialEnds());
    }

    private void OnDestroy()
    {
        if(npcCoroutine != null){
            StopCoroutine(npcCoroutine);
            npcCoroutine = null;
        }
    }

    IEnumerator WaitUntilTutorialEnds(){
        if(GameManager.Instance.isTurotial){
            yield return new WaitUntil(() => tutorial.isTutorialFinish);
            yield return new WaitForSeconds(1f);
        }
        
        StartCoroutine(GetStoreDialogues());
    }
    
    IEnumerator GetStoreDialogues(){
        dialogueControl.GetAllDialogues(WhereNodeStart.Store, WhenNodeStart.SceneLoad, false);

        if(dialogueControl.count > 0){
            while(dialogueControl.npcIndex < dialogueControl.count){
                yarnControl.ChangeUIImg(0);
                GameManager.Instance.StartDialogue(dialogueControl.allDialogues[dialogueControl.npcIndex].dialogueName);
                yield return new WaitUntil(() => !dialogueRunner.IsDialogueRunning);
                
                yield return new WaitForSeconds(1.5f); 
                dialogueControl.npcIndex++;
            }
        }

        yield return new WaitForSeconds(1.5f);
        
        dialogueControl.npcIndex = 0;
        dialogueControl.GetAllDialogues(WhereNodeStart.Store, WhenNodeStart.Click, true);
        npcCoroutine = StartCoroutine(StartCustomer());
    }

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
        
        // if sellect sell
        if(yarnControl.isSell){
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
    
    // to InteractObjInfo
    void SetStoreDialogues(int index){
        InteractObjInfo interactObjInfo = npc.GetComponent<InteractObjInfo>();
        if(interactObjInfo == null){
            interactObjInfo = npc.AddComponent<InteractObjInfo>();
        }

        interactObjInfo._interactType = InteractType.StartDialogue;
        interactObjInfo._nodeName = dialogueControl.allDialogues[index].dialogueName;
    }
    
    void GetBloodTaste(int index){
        tasteStr = ""; // reset
        tasteStr = dialogueControl.allDialogues[index].tasteLine;
    }

    IEnumerator ActiveSprite(){
        npcSprites = dialogueControl.allDialogues[dialogueControl.npcIndex].sprites;

        if(npcSprites == null || npcSprites.Count == 0){
            Debug.Log("There is no sprites in index " + dialogueControl.npcIndex + " dialogue Info...");
            yield break;
        }

        npc.GetComponent<SpriteRenderer>().sprite = npcSprites[0];

        npc.SetActive(true);
        
        npc.GetComponent<BoxCollider2D>().enabled = false;
        yield return GameManager.Instance.StartCoroutine(GameManager.Instance.FadeInSprite(npc.GetComponent<SpriteRenderer>(), 1f));
        npc.GetComponent<BoxCollider2D>().enabled = true;
    }

    IEnumerator DeActiveSprite(){
        npc.GetComponent<BoxCollider2D>().enabled = false;
        yield return GameManager.Instance.StartCoroutine(GameManager.Instance.FadeOutSprite(npc.GetComponent<SpriteRenderer>(), 1f));
        npc.SetActive(false);
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
    void ReadyToMoveNextDay(){
        nextDayButton.SetActive(true);
    }


}
