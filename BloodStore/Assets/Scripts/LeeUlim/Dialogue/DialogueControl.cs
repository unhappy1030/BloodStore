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
                allDialogues[index].tastes = MakeRandomTastes();
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
            allDialogues[index].tastes = MakeRandomTastes();
            allDialogues[index].priority = 0;
            allDialogues[index].dialogueName = "Normal"; // test
            index++;
        }

        totalCount += addCount;
        Debug.Log("total NPC Count : " + totalCount);
    }

    List<string> MakeRandomTastes(){ // test
        List<string> tastes = new();
        List<string> randomTaste = new();
        
        Node random = new Node();
        random.SetAllRandom();

        tastes.Add(random.sex);
        tastes.Add(random.bloodType[0]);
        tastes.Add(random.bloodType[1]);

        int count = tastes.Count;

        int randomCount = UnityEngine.Random.Range(1, count);

        ListShuffle<string>(tastes);

        for(int i=0; i<randomCount; i++){
            randomTaste.Add(tastes[i]);
        }

        return randomTaste;
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


    public void SetCondition(string name, int condition){
        npcConditions[name] = condition;
        Debug.Log("Set "+ name.ToString() + " Condition to " + condition.ToString() + "...");
    }

}

[Serializable]
public class DialogueInfo{
    public string npcName;
    public List<Sprite> sprites;
    public List<string> tastes;
    public int priority;
    public string dialogueName;
}
