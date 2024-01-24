using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Yarn.Unity;

public class NPCInteract : MonoBehaviour
{
    public GameObject npc; // assign at Inspector
    public List<Sprite> npcSprite; // assign at Inspector
    public List<GameObject> cameraTarget; // assign at Inspector

    public GameObject bloodPackCanvas; // assign at Inspector
    public GameObject nextDayButton; // assign at Inspector

    public CameraControl cameraControl;
    public DialogueControl dialogueControl;
    public YarnControl yarnControl;
    public DialogueRunner dialogueRunner;

    int count;
    int npcIndex;
    int spriteIndex;
    bool selectBlood;

    Coroutine npcCoroutine;

    List<NPCSO> npcInfos; // get from DialogueControl
    List<DialogueInfo> dialogueSum;

    void Start(){
        npcInfos = dialogueControl.npcInfos; // test

        GetStoreDialogues();
        count = dialogueSum.Count;

        npcIndex = 0;
        spriteIndex = 0;
        selectBlood = false;

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
        if(count == 0 || npcSprite.Count == 0){
            ReadyToMoveNextDay();
            npcCoroutine = null;
            yield break;
        }
        
        // spriteIndex = GetSpriteIndex();
        SetStoreDialogues();

        yield return ActiveSprite();

        yield return new WaitUntil(() => dialogueRunner.IsDialogueRunning); // wait until mouse click
        // tell what they want
        yield return new WaitUntil(() => !dialogueRunner.IsDialogueRunning); // wait until dialogue ends
        
        // if sellect sell
        if(yarnControl.isSell){
            CreateVirtualCamera(yarnControl.targetIndex); // move camera
            yield return new WaitUntil(() => cameraControl.mainCam.IsBlending);
            yield return new WaitUntil(() => !cameraControl.mainCam.IsBlending); // wait until camera move ends
            
            bloodPackCanvas.SetActive(true);
            
            selectBlood = false;
            yield return new WaitUntil(() => selectBlood); // wait until select blood -> button in Blood pack canvas
            
            // YarnControl.sellInfo = CalculateSellInfo(); // Evaluate about blood pack
            bloodPackCanvas.SetActive(false);
            
            CreateVirtualCamera(0); // camera move to default target
            yield return new WaitUntil(() => cameraControl.mainCam.IsBlending);
            yield return new WaitUntil(() => !cameraControl.mainCam.IsBlending); // wait until camera move ends
        
            yarnControl.isSell = false;
        }
        
        StartDialogue(yarnControl.nodeName); // tell their evaluation or end dialogue
        yield return new WaitUntil(() => !dialogueRunner.IsDialogueRunning);

        yield return StartCoroutine(DeActiveSprite());

        yield return new WaitForSecondsRealtime(1);

        npcIndex++;

        if(npcIndex < count){
            Debug.Log("another NPC Interact triggered...");
            yield return StartCoroutine(StartCustomer());
        }
        
        if(npcIndex == count){ // for trigger only at final interaction
            Debug.Log("Available Move to Next day...");
            ReadyToMoveNextDay();
            npcCoroutine = null;
        }
    }
    
    // from DialogueControl
    void GetStoreDialogues(){
        dialogueControl.GetAllDialogues(WhereNodeStart.Store, WhenNodeStart.Click);
        dialogueSum = dialogueControl.allDialogues;
    }

    // to InteractObjInfo
    void SetStoreDialogues(){
        InteractObjInfo interactObjInfo = npc.GetComponent<InteractObjInfo>();
        if(interactObjInfo == null){
            interactObjInfo = npc.AddComponent<InteractObjInfo>();
        }

        interactObjInfo._interactType = InteractType.NpcInteraction;
        interactObjInfo._nodeName = dialogueSum[npcIndex].dialogueName;
    }

    int GetSpriteIndex(string npcName){
        bool isExist = false;
        int index = 0;
        
        foreach(NPCSO npcInfo in npcInfos){
            if(npcInfo.npcName == npcName){
                isExist = true;
                break;
            }
            index++;
        }

        if(!isExist){
            index = -1;
        }
        
        return index;
    }
    
    IEnumerator ActiveSprite(){
        spriteIndex = GetSpriteIndex(dialogueSum[npcIndex].npcName);

        if(spriteIndex < 0 || spriteIndex > npcInfos.Count -1 || npcInfos[spriteIndex].sprites[0] == null){
            Debug.Log("There is no sprite in NPCInfos...");
            yield break;
        }

        npc.GetComponent<SpriteRenderer>().sprite = npcInfos[spriteIndex].sprites[0];
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

    void CreateVirtualCamera(int targetIndex){
        InteractObjInfo interactObjInfo = gameObject.GetComponent<InteractObjInfo>();
        if(interactObjInfo == null){
            interactObjInfo = gameObject.AddComponent<InteractObjInfo>();
        }
        
        if(cameraTarget == null || cameraTarget.Count == 0){
            Debug.Log("Target list is empty...");
            return;
        }

        if(targetIndex > cameraTarget.Count -1 || cameraTarget[targetIndex] == null){
            Debug.Log("There is no camera target in list...");
            return;
        }

        interactObjInfo.SetVirtualCameraInfo(cameraTarget[targetIndex], false, null, 5.4f, 0.25f, Cinemachine.CinemachineBlendDefinition.Style.EaseInOut, 1.5f);
        cameraControl.ChangeCam(interactObjInfo);
    }   

    public void ChangeSelectBloodAsTrue(){
        selectBlood = true;
    }

    // test
    float CalculateSellInfo(){
        return 5.0f;
    }
    
    // test
    void ReadyToMoveNextDay(){
        nextDayButton.SetActive(true);
    }


}
