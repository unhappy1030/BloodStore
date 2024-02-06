using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Yarn.Unity;

public class NPCInteract : MonoBehaviour
{
    public GameObject npc; // assign at Inspector
    public List<GameObject> cameraTarget; // assign at Inspector
    public static string tasteStr; // static warning

    public GameObject bloodPackCanvas; // assign at Inspector
    public GameObject nextDayButton; // assign at Inspector

    public CameraControl cameraControl;
    public DialogueControl dialogueControl;
    public YarnControl yarnControl;
    public DialogueRunner dialogueRunner;

    int count;
    int npcIndex;
    int spriteIndex;

    Coroutine npcCoroutine;

    // List<NPCSO> npcs; // get from DialogueControl
    List<DialogueInfo> dialogueSum;
    List<Sprite> npcSprites;

    void Start(){
        // npcs = dialogueControl.npcs; // test
        CameraControl.targetsForYarn = new(cameraTarget);

        GetStoreDialogues();

        npcIndex = 0;
        spriteIndex = 0;

        npc.SetActive(false);

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
            yield return new WaitUntil(() => YarnControl.isSelect);
            YarnControl.isSelect = false;
            yarnControl.isSell = false;

            YarnControl.sellInfo = CalculateSellInfo(npcIndex);
                
            if(yarnControl.nodeName != ""){
                StartDialogue(yarnControl.nodeName); // tell their evaluation or end dialogue
                yield return new WaitUntil(() => !dialogueRunner.IsDialogueRunning);
                yarnControl.nodeName = "";
            }

            YarnControl.sellInfo = 0;
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

        List<string> allTastes = new (dialogueSum[index].tastes);
        foreach(string taste in allTastes){
            tasteStr += taste;
            tasteStr += " ";
        }
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
        BloodPackUITest bloodPackUI = bloodPackCanvas.GetComponentInChildren<BloodPackUITest>();

        if(bloodPackUI == null){
            Debug.Log("There is no bloodPackUITest...");
            return -1;
        }

        List<string> select = bloodPackUI.GetTogleCondition();
        
        int count = 0;
        foreach(string taste in dialogueSum[index].tastes){
            for(int i=0; i<3; i++){
                if(select[i] == taste){
                    count++;            
                    break;
                }
            }
        }
        Debug.Log("correct select Count : " + count);

        if(count == dialogueSum[index].tastes.Count){
            return 5.0f;
        }else{
            return 0;
        }
    }
    
    // test
    void ReadyToMoveNextDay(){
        nextDayButton.SetActive(true);
    }


}
