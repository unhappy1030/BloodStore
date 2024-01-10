using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class NPCInteract : MonoBehaviour
{
    public int count;
    public int npcIndex=0;
    // public bool isInteractble = true;
    public GameObject npc;
    public List<Sprite> npcSprite;

    public GameObject nextDayButton;
    DialogueRunner dialogueRunner;

    void Start(){
        nextDayButton.SetActive(false);
        dialogueRunner = GameManager.Instance.dialogueRunner;
        StartCoroutine(StartInteraction());
    }

    IEnumerator StartInteraction(){
        if(npcSprite.Count == 0 || npcSprite.Count <= npcIndex || npcSprite[npcIndex] == null)
            yield break;
        
        npc.GetComponent<SpriteRenderer>().sprite = npcSprite[npcIndex];
        
        GameManager.Instance.StartCoroutine(GameManager.Instance.FadeOutSprite(npc.GetComponent<SpriteRenderer>(), 0.05f));
        
        yield return new WaitUntil(() => dialogueRunner.IsDialogueRunning);
        yield return new WaitUntil(() => !dialogueRunner.IsDialogueRunning);
        
        GameManager.Instance.StartCoroutine(GameManager.Instance.FadeInSprite(npc.GetComponent<SpriteRenderer>(), 0.05f));

        yield return new WaitForSecondsRealtime(2);

        if(npcIndex <= count-1 && npcIndex <= npcSprite.Count-1){
            npcIndex++;
            Debug.Log("another NPC Interact...");
            yield return StartCoroutine(StartInteraction());
        }
        
        Debug.Log("Available Move to Next day...");
        ReadyToMoveNextDay();
    }

    public void ReadyToMoveNextDay(){
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
}
