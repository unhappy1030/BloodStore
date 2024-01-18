using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class NPCInteract : MonoBehaviour
{
    public int count;
    public int npcIndex = 0;
    public int spriteIndex = 0;
    // public bool isInteractble = true;
    public GameObject npc;
    public List<Sprite> npcSprite;
    public List<GameObject> cameraTarget;

    public GameObject nextDayButton;
    public CameraControl cameraControl;
    public YarnControl yarnControl;
    public DialogueRunner dialogueRunner;

    void Start(){
        nextDayButton.SetActive(false);
        StartCoroutine(StartInteraction());
    }

    IEnumerator StartInteraction(){
        if(count == 0 || npcSprite.Count == 0)
            yield break;

        spriteIndex = GetSpriteIndex();
        npc.GetComponent<SpriteRenderer>().sprite = npcSprite[spriteIndex];
        
        npc.GetComponent<BoxCollider2D>().enabled = false;
        yield return GameManager.Instance.StartCoroutine(GameManager.Instance.FadeOutSprite(npc.GetComponent<SpriteRenderer>(), 0.05f));
        npc.GetComponent<BoxCollider2D>().enabled = true;

        yield return new WaitUntil(() => dialogueRunner.IsDialogueRunning);
        yield return new WaitUntil(() => !dialogueRunner.IsDialogueRunning);
        
        npc.GetComponent<BoxCollider2D>().enabled = false;
        GameManager.Instance.StartCoroutine(GameManager.Instance.FadeInSprite(npc.GetComponent<SpriteRenderer>(), 0.05f));

        yield return new WaitForSecondsRealtime(2);

        npcIndex++;

        if(npcIndex < count){
            Debug.Log("another NPC Interact triggered...");
            yield return StartCoroutine(StartInteraction());
        }
        
        Debug.Log("Available Move to Next day...");
        
        if(npcIndex == count) // for trigger only at final interaction
            ReadyToMoveNextDay();
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

    public void StartDialogue(InteractObjInfo interactObjInfo){
        string nodeName = interactObjInfo._nodeName;
        
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
        cameraControl.ChangeCam(interactObjInfo);
    }   


}
