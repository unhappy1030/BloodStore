using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueControl : MonoBehaviour
{
    int totalCount = 0;
    int maxCount = 6;

    public List<NPCSO> npcs; // assign at inspector
    Dictionary<string, int> npcConditions;

    public NormalNPCSO normalNPCs;
    
    List<NPCSO> ableNPCs;
    public List<DialogueInfo> allDialogues;

    void Awake(){
        ResetCondition();
    }

    void ResetCondition(){
        npcConditions = new();
        npcConditions.Clear();

        foreach(NPCSO npc in npcs){
            npcConditions.Add(npc.npcName, 0);
        }

        Debug.Log("Reset Condition...");
    }
    
    public void GetAbleNPC(){
        if(ableNPCs == null) // reset
        {
            ableNPCs = new();
        }
        else
        {
            ableNPCs.Clear();
        }

        if(npcs == null || npcs.Count == 0){
            Debug.Log("There is no NPC Information...");
            return;
        }

        foreach(NPCSO npcInfo in npcs){
            if(npcInfo.AbleNPC(GameManager.Instance.day)){
                ableNPCs.Add(npcInfo);
            }
        }
    }

    public void GetAllDialogues(WhereNodeStart where, WhenNodeStart when){
        GetAbleNPC();

        if(allDialogues == null) // reset
        {
            allDialogues = new();
        }
        else
        {
            allDialogues.Clear();
            totalCount = 0;
        }

        if(ableNPCs == null || ableNPCs.Count == 0){
            Debug.Log("There is no able NPC in this Condition...");
            return;
        }

        int index = 0;

        foreach(NPCSO ableNpcInfo in ableNPCs){
            string npcName = ableNpcInfo.npcName;
            List<Dialogue> list 
                = ableNpcInfo.GetDialogues(where, when, GameManager.Instance.day, npcConditions[npcName]);

            totalCount += list.Count;
            
            foreach(Dialogue dialogue in list){
                allDialogues.Add(new());
                allDialogues[index].npcName = ableNpcInfo.npcName;
                allDialogues[index].sprites = new(ableNpcInfo.sprites);
                allDialogues[index].priority = dialogue.priority;
                allDialogues[index].dialogueName = dialogue.dialogueName;
                index++;
            }
        }

        Debug.Log("before add total Count : " + totalCount);

        AddRandomNPC(); // test

        ListShuffle<DialogueInfo>(allDialogues);
        allDialogues.Sort(ComparePriority);
    }

    void AddRandomNPC(){
        List<Sprite> normalSprites = new(normalNPCs.normalNPCSprites);

        if(normalSprites == null || normalSprites.Count == 0){
            Debug.Log("There is no sprite in normalNpc...");
            return;
        }

        if (totalCount > 4){
            return;
        }

        int addCount = UnityEngine.Random.Range(1, maxCount - totalCount + 1);
        Debug.Log("add Count : "+ addCount);

        ListShuffle<Sprite>(normalSprites);

        int index = allDialogues.Count;

        for(int i=0; i<addCount; i++){
            List<Sprite> tempSprite = new()
            {
                normalSprites[i]
            };

            allDialogues.Add(new());
            allDialogues[index].npcName = "";
            allDialogues[index].sprites = tempSprite;
            allDialogues[index].priority = 0;
            allDialogues[index].dialogueName = "Normal"; // test
            index++;
        }

        totalCount += addCount;
        Debug.Log("total NPC Count : " + totalCount);
    }

    // 내림차순
    int ComparePriority(DialogueInfo dialogue1, DialogueInfo dialogue2){
        if(dialogue1.priority > dialogue2.priority)
            return -1;
        else if(dialogue1.priority < dialogue2.priority)
            return 1;
        
        return 0;
    }

    void ListShuffle<T>(List<T> list){
        for(int i=list.Count - 1; i>0; i--){
            int randIndex = UnityEngine.Random.Range(0, i+1);

            T temp = list[i];
            list[i] = list[randIndex];
            list[randIndex] = temp;
        }
    }

    // int Shuffle(DialogueInfo dialogue1, DialogueInfo dialogue2){
    //     return UnityEngine.Random.Range(-1, 2);
    // }

    public void SetCondition(string name, int condition){
        npcConditions[name] = condition;
        Debug.Log("Set "+ name.ToString() + " Condition to " + condition.ToString() + "...");
    }

    /*
//     int dayCount = 0;
//     int conditionCount = 0;
//     int totalCount = 0;

//     public List<NPCInfo> npcInfos; // assign at inspector
//     public Dictionary<string, int> npcConditions;
    
//     List<NPCInfo> ableNPCInfos;
//     List<DialogueInfo> dayDialogues;
//     List<DialogueInfo> condDialogues;
//     public List<DialogueInfo> ableDialogues;

//     void Awake(){
//         ResetCondition();
//     }

//     void ResetCondition(){
//         npcConditions = new();
//         npcConditions.Clear();

//         for(int i=0; i<npcInfos.Count; i++){
//             npcConditions.Add(npcInfos[i].npcName, 0);
//         }

//         Debug.Log("Reset Condition...");
//     }

//     public void GetAbleNPC(){
//         if(ableNPCInfos == null) // reset
//         {
//             ableNPCInfos = new();
//         }
//         else
//         {
//             ableNPCInfos.Clear();
//         }

//         if(npcInfos == null || npcInfos.Count == 0){
//             Debug.Log("There is no NPC Information...");
//             return;
//         }

//         foreach(NPCInfo npcInfo in npcInfos){
//             if(npcInfo.AbleNPC(GameManager.Instance.day)){
//                 ableNPCInfos.Add(npcInfo);
//             }
//         }
//     }

//     public void GetDayDialogue(WhereNodeStart where, WhenNodeStart when){
//         if(dayDialogues == null) // reset
//         {
//             dayDialogues = new();
//         }
//         else
//         {
//             dayDialogues.Clear();
//         }

//         if(ableNPCInfos == null || ableNPCInfos.Count == 0){
//             Debug.Log("There is no able NPC in this day...");
//             return;
//         }

//         int index = 0;

//         foreach(NPCInfo ableNpcInfo in ableNPCInfos){
//             List<DayDialogue> list = ableNpcInfo.GetDayDialogues(where, when, GameManager.Instance.day);

//             dayCount += list.Count;

//             foreach(DayDialogue day in list){
//                 dayDialogues.Add(new());
//                 dayDialogues[index].npcName = ableNpcInfo.npcName;
//                 dayDialogues[index].priority = day.priority;
//                 dayDialogues[index].dialogueName = day.dialogueName;
//                 Debug.Log("Able day name " + index + " : " + dayDialogues[index].npcName);

//                 index++;
//             }
//         }
//     }
    
//     public void GetCondDialogue(WhereNodeStart where, WhenNodeStart when){
//         if(condDialogues == null) // reset
//         {
//             condDialogues = new();
//         }
//         else
//         {
//             condDialogues.Clear();
//         }

//         if(ableNPCInfos == null || ableNPCInfos.Count == 0){
//             Debug.Log("There is no able NPC in this Condition...");
//             return;
//         }

//         int index = 0;

//         foreach(NPCInfo ableNpcInfo in ableNPCInfos){
//             string npcName = ableNpcInfo.npcName;
//             List<CondDialogue> list 
//                 = ableNpcInfo.GetCondDialogues(where, when, npcConditions[npcName], GameManager.Instance.day);

//             conditionCount += list.Count;
            
//             foreach(CondDialogue cond in list){
//                 condDialogues.Add(new());
//                 condDialogues[index].npcName = ableNpcInfo.npcName;
//                 condDialogues[index].priority = cond.priority;
//                 condDialogues[index].dialogueName = cond.dialogueName;
//                 Debug.Log("Able Cond name " + index + " : " + condDialogues[index].npcName);
//                 index++;
//             }
//         }
//     }

//     public void SetAllDialogues(){
//         if(ableDialogues == null) // reset
//         {
//             ableDialogues = new();
//         }
//         else
//         {
//             ableDialogues.Clear();
//         }

//         foreach(DialogueInfo day in dayDialogues){
//             ableDialogues.Add(day);
//             Debug.Log("Able day name : " + day.npcName);
//         }

//         foreach(DialogueInfo cond in condDialogues){
//             ableDialogues.Add(cond);
//             Debug.Log("Able Cond name : " + cond.npcName);
//         }

//         foreach(DialogueInfo info in ableDialogues){
//             Debug.Log("Name : " + info.npcName.ToString());
//         }
//         ableDialogues.Sort(ComparePriority);
//     }

//     // 내림차순
//     int ComparePriority(DialogueInfo dialogue1, DialogueInfo dialogue2){
//         if(dialogue1.priority > dialogue2.priority)
//             return -1;
//         else if(dialogue1.priority < dialogue2.priority)
//             return 1;
//         else
//             return 0;
//     }

//     public void SetCondition(string name, int condition){
//         npcConditions[name] = condition;
//         Debug.Log("Set "+ name.ToString() + " Condition to " + condition.ToString() + "...");
//     }
*/
}

[Serializable]
public class DialogueInfo{
    public string npcName;
    public List<Sprite> sprites;
    public int priority;
    public string dialogueName;
}
