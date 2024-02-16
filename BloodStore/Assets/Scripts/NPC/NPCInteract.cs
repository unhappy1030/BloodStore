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

    public GameObject filteringCanvas; // assign at Inspector
    public GameObject bloodPackCanvas; // assign at Inspector
    public GameObject nextDayButton; // assign at Inspector

    public CameraControl cameraControl;
    public DialogueControl dialogueControl;
    public YarnControl yarnControl;
    public DialogueRunner dialogueRunner;
    public BloodSellProcess bloodSellProcess; // assign at inspector

    int count;
    int npcIndex;
    int spriteIndex;

    Coroutine npcCoroutine;

    // List<NPCSO> npcs; // get from DialogueControl
    List<DialogueInfo> dialogueSum;
    List<Sprite> npcSprites;

    BloodSellStatus sellStatus;

    void Start(){
        sellStatus = BloodSellStatus.None;

        // npcs = dialogueControl.npcs; // test
        CameraControl.targetsForYarn = new(cameraTarget);

        GetStoreDialogues();

        npcIndex = 0;
        spriteIndex = 0;

        npc.SetActive(false);

        filteringCanvas.SetActive(false);
        bloodPackCanvas.SetActive(false);
        nextDayButton.SetActive(false);

        npcCoroutine = StartCoroutine(StartCustomer());
    }

    private void OnDestroy()
    {
        if(npcCoroutine != null){
            StopCoroutine(npcCoroutine);
            npcCoroutine = null;
        }
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
            filteringCanvas.SetActive(true);

            yield return new WaitUntil(() => bloodSellProcess.isBloodSellFinish);
            bloodSellProcess.isBloodSellFinish = false;
            filteringCanvas.SetActive(false);

            ChangeSellStatus();

            YarnControl.sellInfo = CalculateSellInfo(npcIndex);
            GameManager.Instance.sellCount++;
            GameManager.Instance.totalPoint += YarnControl.sellInfo;
                
            if(yarnControl.nodeName != ""){
                StartDialogue(yarnControl.nodeName); // tell their evaluation or end dialogue
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
    void GetStoreDialogues(){
        dialogueControl.GetAllDialogues(WhereNodeStart.Store, WhenNodeStart.Click);
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

    public void StartDialogue(string nodeName){
        if(!dialogueRunner.IsDialogueRunning)
        {
            if(dialogueRunner.NodeExists(nodeName))
                dialogueRunner.StartDialogue(nodeName);
            else
                Debug.Log(nodeName + " is not Exist...");
        }
        else
        {
            Debug.Log("Other Dialogue is running...");
        }
    }

    public void ChangeSellStatus(){
        if(bloodSellProcess.isBloodSelected){
            sellStatus = BloodSellStatus.SelectBlood;

            if(bloodSellProcess.isFiltered){
                sellStatus = BloodSellStatus.Filtered;
            }
        }

        // YarnControl.sellStatus = sellStatus;
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
    
    // test
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

        if(ratio > 0.5f)
        {
            point = 5;
        }
        else if(ratio > 0.25f)
        {
            point = 3;
        }
        else
        {
            point = 2;
        }

        point *= dialogueSum[index].weight;

        if(sellStatus == BloodSellStatus.Filtered){
            float extraPoint = UnityEngine.Random.Range(0, 5-point);
            float newWeight = UnityEngine.Random.Range(0.8f, 1f);
            point += extraPoint * newWeight;
        }

        point = Mathf.Round(point * 100.0f)/100.0f;
        
        return point;
    }
    
    // test
    void ReadyToMoveNextDay(){
        nextDayButton.SetActive(true);
    }


}
