using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn;

public class DialogueControl : MonoBehaviour
{
    int dayCount = 0;
    int conditionCount = 0;
    int totalCount = 0;

    public List<NPCInfo> npcInfos; // assign at inspector
    
    List<NPCInfo> ableNPCInfos;
    public List<DialogueInfo> ableDialogues;

    public void GetAbleNPC(){
        if(ableNPCInfos == null) // reset
        {
            ableNPCInfos = new();
        }
        else
        {
            ableNPCInfos.Clear();
        }

        if(npcInfos == null || npcInfos.Count == 0){
            Debug.Log("There is no NPC Information...");
            return;
        }

        foreach(NPCInfo npcInfo in npcInfos){
            if(npcInfo.ableNPC(GameManager.Instance.day)){
                ableNPCInfos.Add(npcInfo);
            }
        }
    }

    public void GetDayDialogue(WhereNodeStart where, WhenNodeStart when){
        if(ableDialogues == null) // reset
        {
            ableDialogues = new();
        }
        else
        {
            ableDialogues.Clear();
        }

        if(ableNPCInfos == null || ableNPCInfos.Count == 0){
            Debug.Log("There is no able NPC in this day...");
            return;
        }

        int index = 0;
        foreach(NPCInfo ableNpcInfo in ableNPCInfos){
            List<DialogueFrame> list = ableNpcInfo.GetAllDialogues(where, when, true, GameManager.Instance.day);
            
            list.Sort(ComparePriority);

            dayCount += ableNpcInfo.GetDialoguesCount(where, when, true, GameManager.Instance.day);
            
            foreach(DialogueFrame frame in list){
                ableDialogues.Add(new());
                ableDialogues[index].npcName = frame.npcName;
                ableDialogues[index].dialogueName = frame.dialogueName;
                index++;
            }
        }
    }
    
    // 내림차순
    int ComparePriority(DialogueFrame dialogue1, DialogueFrame dialogue2){
        if(dialogue1.priority > dialogue2.priority)
            return -1;
        else if(dialogue1.priority < dialogue2.priority)
            return 1;
        else
            return 0;
    }


    /*
    public void GetConditionDialogue(WhereNodeStart where, WhenNodeStart when){
        List<DialogueFrame> total = new();
        foreach(NPCInfo npcInfo in npcInfos){
            List<DialogueFrame> list = npcInfo.GetAllDialogues(where, when, false, Condition);
            
            foreach(DialogueFrame frame in list){
                total.Add(frame);
            }
        }
    }
    */
}

public class DialogueInfo{
    public string npcName;
    public string dialogueName;
}
