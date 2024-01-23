using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Yarn;

public class DialogueControl : MonoBehaviour
{
    int dayCount = 0;
    int conditionCount = 0;
    int totalCount = 0;

    public List<NPCInfo> npcInfos; // assign at inspector
    public Dictionary<string, int> npcConditions;
    
    List<NPCInfo> ableNPCInfos;
    List<DialogueInfo> dayDialogues;
    List<DialogueInfo> condDialogues;
    public List<DialogueInfo> ableDialogues;

    void Awake(){
        ResetCondition();
    }

    void ResetCondition(){
        npcConditions = new();
        npcConditions.Clear();

        for(int i=0; i<npcInfos.Count; i++){
            npcConditions.Add(npcInfos[i].npcName, 0);
        }

        Debug.Log("Reset Condition...");
    }

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
            if(npcInfo.AbleNPC(GameManager.Instance.day)){
                ableNPCInfos.Add(npcInfo);
            }
        }
    }

    public void GetDayDialogue(WhereNodeStart where, WhenNodeStart when){
        if(dayDialogues == null) // reset
        {
            dayDialogues = new();
        }
        else
        {
            dayDialogues.Clear();
        }

        if(ableNPCInfos == null || ableNPCInfos.Count == 0){
            Debug.Log("There is no able NPC in this day...");
            return;
        }

        int index = 0;

        foreach(NPCInfo ableNpcInfo in ableNPCInfos){
            List<DayDialogue> list = ableNpcInfo.GetDayDialogues(where, when, GameManager.Instance.day);

            dayCount += list.Count;

            foreach(DayDialogue day in list){
                dayDialogues.Add(new());
                dayDialogues[index].npcName = ableNpcInfo.npcName;
                dayDialogues[index].priority = day.priority;
                dayDialogues[index].dialogueName = day.dialogueName;
                Debug.Log("Able day name " + index + " : " + dayDialogues[index].npcName);

                index++;
            }
        }
    }
    
    public void GetCondDialogue(WhereNodeStart where, WhenNodeStart when){
        if(condDialogues == null) // reset
        {
            condDialogues = new();
        }
        else
        {
            condDialogues.Clear();
        }

        if(ableNPCInfos == null || ableNPCInfos.Count == 0){
            Debug.Log("There is no able NPC in this Condition...");
            return;
        }

        int index = 0;

        foreach(NPCInfo ableNpcInfo in ableNPCInfos){
            string npcName = ableNpcInfo.npcName;
            List<CondDialogue> list 
                = ableNpcInfo.GetCondDialogues(where, when, npcConditions[npcName], GameManager.Instance.day);

            conditionCount += list.Count;
            
            foreach(CondDialogue cond in list){
                condDialogues.Add(new());
                condDialogues[index].npcName = ableNpcInfo.npcName;
                condDialogues[index].priority = cond.priority;
                condDialogues[index].dialogueName = cond.dialogueName;
                Debug.Log("Able Cond name " + index + " : " + condDialogues[index].npcName);
                index++;
            }
        }
    }

    public void SetAllDialogues(){
        if(ableDialogues == null) // reset
        {
            ableDialogues = new();
        }
        else
        {
            ableDialogues.Clear();
        }

        foreach(DialogueInfo day in dayDialogues){
            ableDialogues.Add(day);
            Debug.Log("Able day name : " + day.npcName);
        }

        foreach(DialogueInfo cond in condDialogues){
            ableDialogues.Add(cond);
            Debug.Log("Able Cond name : " + cond.npcName);
        }

        foreach(DialogueInfo info in ableDialogues){
            Debug.Log("Name : " + info.npcName.ToString());
        }
        ableDialogues.Sort(ComparePriority);
    }

    // 내림차순
    int ComparePriority(DialogueInfo dialogue1, DialogueInfo dialogue2){
        if(dialogue1.priority > dialogue2.priority)
            return -1;
        else if(dialogue1.priority < dialogue2.priority)
            return 1;
        else
            return 0;
    }

    public void SetCondition(string name, int condition){
        npcConditions[name] = condition;
        Debug.Log("Set "+ name.ToString() + " Condition to " + condition.ToString() + "...");
    }
}

public class DialogueInfo{
    public string npcName;
    public int priority;
    public string dialogueName;
}
