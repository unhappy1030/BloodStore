using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class NPCInteract : MonoBehaviour
{
    public int count;
    public int npcIndex=0;
    public GameObject npc;
    public List<Sprite> npcSprite;
    DialogueRunner dialogueRunner;

    void Start(){
        dialogueRunner = GameManager.Instance.dialogueRunner;
        StartCoroutine(StartInteraction());
    }

    IEnumerator StartInteraction(){
        if(npcSprite.Count == 0 || npcSprite.Count <= npcIndex || npcSprite[npcIndex] == null)
            yield break;
        
        npc.GetComponent<SpriteRenderer>().sprite = npcSprite[npcIndex];
        npc.SetActive(true);

        yield return new WaitUntil(() => dialogueRunner.IsDialogueRunning);
        yield return new WaitUntil(() => !dialogueRunner.IsDialogueRunning);
        
        npc.SetActive(false);

        yield return new WaitForSecondsRealtime(2);

        if(npcIndex <= count-1 && npcIndex <= npcSprite.Count-1){
            npcIndex++;
            StartCoroutine(StartInteraction());
        }
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
