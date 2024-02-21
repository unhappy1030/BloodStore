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

    int count;
    int npcIndex;

    Coroutine npcCoroutine;

    // List<NPCSO> npcs; // get from DialogueControl
    List<DialogueInfo> dialogueSum;
    List<Sprite> npcSprites;

    BloodSellStatus sellStatus;

    void Start(){
        sellStatus = BloodSellStatus.None;

        // npcs = dialogueControl.npcs; // test
        CameraControl.targetsForYarn = new(cameraTarget);

        npcIndex = 0;

        npc.SetActive(false);

        endSellProcessButton.SetActive(false);
        bloodPackCanvas.SetActive(false);
        nextDayButton.SetActive(false);

        StartCoroutine(GetStoreDialogues());
    }

    private void OnDestroy()
    {
        if(npcCoroutine != null){
            StopCoroutine(npcCoroutine);
            npcCoroutine = null;
        }
    }
    
    IEnumerator GetStoreDialogues(){
        dialogueControl.GetAllDialogues(WhereNodeStart.Store, WhenNodeStart.SceneLoad);
        dialogueControl.ShuffleAndSortDialogue();

        dialogueSum = dialogueControl.allDialogues;
        count = dialogueSum.Count;

        if(count > 0){
            npcIndex = 0;

            while(npcIndex < count){
                yarnControl.ChangeUIImg(0);
                GameManager.Instance.StartDialogue(dialogueSum[npcIndex].dialogueName);
                yield return new WaitUntil(() => !dialogueRunner.IsDialogueRunning);
                
                yield return new WaitForSeconds(1.5f); 
                npcIndex++;
            }
        }

        yield return new WaitForSeconds(1.5f);
        
        npcIndex = 0;
        GetCustomerDialogues();
        npcCoroutine = StartCoroutine(StartCustomer());
    }

    IEnumerator StartCustomer(){
        if(count == 0){
            ReadyToMoveNextDay();
            npcCoroutine = null;
            yield break;
        }
        
        SetStoreDialogues(npcIndex);
        GetBloodTaste(npcIndex);

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

            YarnControl.sellInfo = CalculateSellInfo(npcIndex);
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

        npcIndex++;

        if(npcIndex < count){
            yield return StartCoroutine(StartCustomer());
        }
        
        if(npcIndex == count){ // for trigger only at final interaction
            ReadyToMoveNextDay();
            npcCoroutine = null;
        }
    }
    
    // from DialogueControl
    void GetCustomerDialogues(){
        dialogueControl.GetAllDialogues(WhereNodeStart.Store, WhenNodeStart.Click);
        dialogueControl.AddRandomNPC();
        dialogueControl.ShuffleAndSortDialogue();

        dialogueSum = dialogueControl.allDialogues;
        count = dialogueSum.Count;
    }

    // to InteractObjInfo
    void SetStoreDialogues(int index){
        InteractObjInfo interactObjInfo = npc.GetComponent<InteractObjInfo>();
        if(interactObjInfo == null){
            interactObjInfo = npc.AddComponent<InteractObjInfo>();
        }

        interactObjInfo._interactType = InteractType.StartDialogue;
        interactObjInfo._nodeName = dialogueSum[index].dialogueName;
    }
    
    void GetBloodTaste(int index){
        tasteStr = ""; // reset
        tasteStr = dialogueSum[index].tasteLine;
    }

    IEnumerator ActiveSprite(){
        npcSprites = dialogueSum[npcIndex].sprites;

        if(npcSprites == null || npcSprites.Count == 0){
            Debug.Log("There is no sprites in index " + npcIndex + " dialogue Info...");
            yield break;
        }

        npc.GetComponent<SpriteRenderer>().sprite = npcSprites[0];

        npc.SetActive(true);
        
        npc.GetComponent<BoxCollider2D>().enabled = false;
        yield return GameManager.Instance.StartCoroutine(GameManager.Instance.FadeOutSprite(npc.GetComponent<SpriteRenderer>(), 0.05f));
        npc.GetComponent<BoxCollider2D>().enabled = true;
    }

    IEnumerator DeActiveSprite(){
        npc.GetComponent<BoxCollider2D>().enabled = false;
        yield return GameManager.Instance.StartCoroutine(GameManager.Instance.FadeInSprite(npc.GetComponent<SpriteRenderer>(), 0.05f));
        npc.SetActive(false);
    }

    public Sprite GetSpriteForDialogueView(int spriteIndex){
        return dialogueSum[npcIndex].sprites[spriteIndex];
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
        foreach(string taste in dialogueSum[index].tastes){
            if(taste != ""){
                totalCount++;
            }
        }
        
        int count = 0;
        foreach(string taste in dialogueSum[index].tastes){
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
        point *= dialogueSum[index].weight;
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

        point = Mathf.Round(point * 100.0f)/100.0f;
        
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
