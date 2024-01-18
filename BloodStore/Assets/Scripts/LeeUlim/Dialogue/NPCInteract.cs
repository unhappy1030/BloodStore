using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Yarn.Unity;

public class NPCInteract : MonoBehaviour
{
    public int count;
    public int npcIndex = 0;
    public int spriteIndex = 0;
    public bool finishCreateCam = false;
    public bool selectBlood = false;
    // public bool isInteractble = true;
    public GameObject npc;
    public List<Sprite> npcSprite;
    public List<GameObject> cameraTarget;

    public GameObject bloodPackCanvas;
    public GameObject nextDayButton;

    public CameraControl cameraControl;
    public YarnControl yarnControl;
    public DialogueRunner dialogueRunner;

    void Start(){
        bloodPackCanvas.SetActive(false);
        nextDayButton.SetActive(false);

        StartCoroutine(StartInteraction());
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
            yield return new WaitUntil(() => selectBlood); // wait until select blood
            
            // YarnControl.sellInfo = CalculateSellInfo(); // Evaluate about blood pack
            bloodPackCanvas.SetActive(false);
            
            CreateVirtualCamera(0); // camera move to default target
            yield return new WaitUntil(() => cameraControl.mainCam.IsBlending);
            yield return new WaitUntil(() => !cameraControl.mainCam.IsBlending); // wait intil camera move ends
        
            StartDialogue(yarnControl.nodeName); // tell their evaluation or end dialogue
            yield return new WaitUntil(() => !dialogueRunner.IsDialogueRunning);
            yarnControl.isSell = false;
        }

        DeActiveSprite(spriteIndex);

        yield return new WaitForSecondsRealtime(2);

        npcIndex++;

        if(npcIndex < count){
            Debug.Log("another NPC Interact triggered...");
            yield return StartCoroutine(StartInteraction());
        }
        
        Debug.Log("Available Move to Next day...");
        
        if(npcIndex == count){ // for trigger only at final interaction
            ReadyToMoveNextDay();
        }
    }

    // test
    int GetSpriteIndex(){
        int index;
        index = Random.Range(0, npcSprite.Count);
        return index;
    }

    void ReadyToMoveNextDay(){
        nextDayButton.SetActive(true);
    }

    IEnumerator ActiveSprite(int spriteIndex){
        npc.GetComponent<SpriteRenderer>().sprite = npcSprite[spriteIndex];
        
        npc.GetComponent<BoxCollider2D>().enabled = false;
        yield return GameManager.Instance.StartCoroutine(GameManager.Instance.FadeOutSprite(npc.GetComponent<SpriteRenderer>(), 0.05f));
        npc.GetComponent<BoxCollider2D>().enabled = true;
    }

    void DeActiveSprite(int spriteIndex){
        npc.GetComponent<BoxCollider2D>().enabled = false;
        GameManager.Instance.StartCoroutine(GameManager.Instance.FadeInSprite(npc.GetComponent<SpriteRenderer>(), 0.05f));
    }

    public void StartDialogue(string nodeName){
        if(!dialogueRunner.IsDialogueRunning){
            if(dialogueRunner.NodeExists(nodeName))
                dialogueRunner.StartDialogue(nodeName);
            else
                Debug.Log(nodeName + " is not Exist...");
        }else
            Debug.Log("Other Dialogue is running...");
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
        finishCreateCam = true;
        cameraControl.ChangeCam(interactObjInfo);
    }   

    public void ChangeSelectBloodAsTrue(){
        selectBlood = true;
    }

    // test
    float CalculateSellInfo(){
        return 5.0f;
    }
}
