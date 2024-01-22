using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Yarn.Unity;

public class DialogueInfo{
    public string npcName;
    public NodeInfo nodeInfo;
}

public class NPCInteract : MonoBehaviour
{
    // public bool isInteractble = true;

    public List<NPCInfo> npcInfos;

    public GameObject npc; // assign at Inspector
    public List<Sprite> npcSprite; // assign at Inspector
    public List<GameObject> cameraTarget; // assign at Inspector

    public GameObject bloodPackCanvas; // assign at Inspector
    public GameObject nextDayButton; // assign at Inspector

    public CameraControl cameraControl;
    public YarnControl yarnControl;
    public DialogueRunner dialogueRunner;

    int count;
    int npcIndex;
    int spriteIndex;
    bool selectBlood;

    Coroutine npcCoroutine;

    void Start(){
        npcIndex = 0;
        spriteIndex = 0;
        selectBlood = false;

        npc.SetActive(false);

        bloodPackCanvas.SetActive(false);
        nextDayButton.SetActive(false);

        npcCoroutine = StartCoroutine(StartInteraction());
    }

    private void OnDestroy()
    {
        if(npcCoroutine != null){
            StopCoroutine(npcCoroutine);
            npcCoroutine = null;
        }
    }

    IEnumerator StartInteraction(){
        if(count == 0 || npcSprite.Count == 0)
            yield break;
        
        spriteIndex = GetSpriteIndex();

        yield return ActiveSprite(spriteIndex);

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

        DeActiveSprite(spriteIndex);

        yield return new WaitForSecondsRealtime(2);

        npcIndex++;

        if(npcIndex < count){
            Debug.Log("another NPC Interact triggered...");
            yield return StartCoroutine(StartInteraction());
        }
        
        if(npcIndex == count){ // for trigger only at final interaction
            Debug.Log("Available Move to Next day...");
            ReadyToMoveNextDay();
            npcCoroutine = null;
        }
    }

    // test
    int GetSpriteIndex(){
        int index;
        index = Random.Range(0, npcSprite.Count);
        return index;
    }

    /*
    void SetCount(){
        int dayCount = 0;
        int conditionCount = 0;
        int requiredCount = 0;

        foreach(NPCInfo npcInfo in npcInfos){
            if(npcInfo.startDay >= GameManager.Instance.day){
                dayCount = npcInfo.GetDayCount(GameManager.Instance.day);
            }

            // assign conditionCount here
            // conditionCount = npcInfo.GetConditionCount(condition);
        }

        requiredCount = dayCount + conditionCount; // test
        
        if(requiredCount > 5) // test
        {
            count = requiredCount;
        }
        else
        {
            count = Random.Range(requiredCount, requiredCount + 3); // test
        }
    }
    
    void SetDialogues(){
        int index = 0;

        foreach(NPCInfo npcInfo in npcInfos){
            foreach(NodeInfo nodeInfo in npcInfo.nodeInfos){
                if(nodeInfo.isDay && nodeInfo.num == GameManager.Instance.day
                    || !nodeInfo.isDay && nodeInfo.num == condition)
                {
                    dialogueInfos.Add(new());
                    dialogueInfos[index].npcName = npcInfo.npcName;
                    dialogueInfos[index].nodeInfo = nodeInfo;
                    index++;
                }
            }
        }

        if(dialogueInfos.Count < count){
            while(true){
                NPCInfo randNPC = npcInfos[Random.Range(0, npcInfos.Count)];

                if(randNPC.startDay >= GameManager.Instance.day){
                    break;
                }
            }

            dialogueInfos.Add(new());
            dialogueInfos[index].npcName =
        }
    }
*/

    IEnumerator ActiveSprite(int spriteIndex){
        npc.GetComponent<SpriteRenderer>().sprite = npcSprite[spriteIndex];
        npc.SetActive(true);
        
        npc.GetComponent<BoxCollider2D>().enabled = false;
        yield return GameManager.Instance.StartCoroutine(GameManager.Instance.FadeOutSprite(npc.GetComponent<SpriteRenderer>(), 0.05f));
        npc.GetComponent<BoxCollider2D>().enabled = true;
    }

    void DeActiveSprite(int spriteIndex){
        npc.GetComponent<BoxCollider2D>().enabled = false;
        GameManager.Instance.StartCoroutine(GameManager.Instance.FadeInSprite(npc.GetComponent<SpriteRenderer>(), 0.05f));
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
    
    void ReadyToMoveNextDay(){
        nextDayButton.SetActive(true);
    }


}
