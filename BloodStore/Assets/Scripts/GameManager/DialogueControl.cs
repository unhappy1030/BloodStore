using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueControl : MonoBehaviour
{
    int totalCount = 0;
    int maxCount = 5;
    public int npcIndex = 0;
    public int count = 0;

    public List<NPCSO> npcs; // assign at inspector

    [Tooltip("sex - Rh - BloodType")]
    public List<BloodTasteSO> tasteSOs; // assign at inspector
    Dictionary<string, int> npcConditions;

    public NormalNPCSO normalNPCs;
    
    List<NPCSO> ableNPCs;
    public List<DialogueInfo> allDialogues;

    void Awake(){
        ResetCondition();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        npcIndex = 0;
        count = 0;
    }

    void OnSceneUnloaded(Scene currentScene)
    {

    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }



    void ResetCondition(){
        npcConditions = new();
        npcConditions.Clear();

        foreach(NPCSO npc in npcs){
            npcConditions.Add(npc.npcName, 0);
        }
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

    public void GetAllDialogues(WhereNodeStart where, WhenNodeStart when, bool isAddRandom){
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
                allDialogues[index].tasteLine = "";
                allDialogues[index].tastes = MakeRandomTastes();
                allDialogues[index].priority = dialogue.priority;
                allDialogues[index].weight = UnityEngine.Random.Range(0.8f, 0.95f); // test
                allDialogues[index].dialogueName = dialogue.dialogueName;
                index++;
            }
        }

        if(isAddRandom){
            AddRandomNPC();
        }

        ShuffleAndSortDialogue();

        count = allDialogues.Count;
        npcIndex = 0;
    }

    // must use in Store
    public void AddRandomNPC(){
        List<Sprite> normalSprites = new(normalNPCs.normalNPCSprites);
        int randominit = 1, max = maxCount, day = GameManager.Instance.day;
        if(normalSprites == null || normalSprites.Count == 0){
            Debug.Log("There is no sprite in normalNpc...");
            return;
        }

        if (totalCount > 4){
            return;
        }
        else if(totalCount < 3){
            randominit = 3 - totalCount;
        }
        if(day > 5 && day < 10){
            max += 1;
            randominit += 1;
        }
        else if(day >= 10 && day < 20){
            max += 2;
            randominit += 2;
        }
        else if(day >= 20 && day < 30){
            max += 3;
            randominit += 3;
        }
        if(max - totalCount + 1 <= randominit){
            randominit = max - totalCount;
        }
        int addCount = UnityEngine.Random.Range(randominit, max - totalCount + 1);

        Debug.Log("RandomRange : (" + randominit.ToString() + ", " + (max - totalCount + 1).ToString() + ")\n normalCount / addCount / sum :" + totalCount.ToString() + "/" + addCount.ToString() + "/" + (totalCount + addCount).ToString());

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
            allDialogues[index].tasteLine = MakeTasteLine(allDialogues[index].tastes);
            allDialogues[index].priority = 0;
            allDialogues[index].weight = UnityEngine.Random.Range(0.8f, 0.95f);
            allDialogues[index].dialogueName = "Normal"; // test
            index++;
        }

        totalCount += addCount;
    }

    // must use in Store
    public void ShuffleAndSortDialogue(){
        ListShuffle<DialogueInfo>(allDialogues);
        allDialogues.Sort(ComparePriority);
    }

    List<string> MakeRandomTastes(){ // test
        List<string> tastes = new();
        List<bool> isSelected = new();
        List<string> randomTaste = new(); // sex + rh + bloodType

        Node random = new Node();
        random.SetAllRandom();

        tastes.Add(random.sex);
        tastes.Add(random.bloodType[1]); // Rh
        tastes.Add(random.bloodType[0]); // bloodType

        int count = tastes.Count;

        for(int i=0; i<count; i++){
            isSelected.Add(false);
        }

        int randomCount = UnityEngine.Random.Range(1, count); // at least more than 1

        int add=0;
        while(add < randomCount){
            int index = UnityEngine.Random.Range(0, count);
            if(!isSelected[index]){
                isSelected[index] = true;
                add++;
            }
        }

        for(int i=0; i<count; i++){
            if(isSelected[i])
            {
                randomTaste.Add(tastes[i]);
            }
            else
            {
                randomTaste.Add(""); // add ""
            }
        }

        return randomTaste;
    }

    string MakeTasteLine(List<string> npcRandomTastes){
        string line;
        
        if(tasteSOs == null || tasteSOs.Count == 0){
            Debug.Log("TastesSO is empty...");
            return null;
        }

        int tasteCount = 0;
        List<string> randomTastes = new();
        for(int i=0; i<npcRandomTastes.Count; i++){
            if(npcRandomTastes[i] != ""){
                randomTastes.Add(npcRandomTastes[i]);
                tasteCount++;
            }
        }

        bool isLine = (tasteCount == 1);
        
        List<Taste> lineInfos = new();
        foreach(string taste in randomTastes){
            foreach(BloodTasteSO tasteSO in tasteSOs){
                foreach(Taste tasteInfo in tasteSO.tastes){
                    if(taste == tasteInfo.tasteName){
                        lineInfos.Add(tasteInfo);
                        break;
                    }
                }
            }
        }

        if(isLine)
        {
            int randIndex = (int)UnityEngine.Random.Range(0, lineInfos[0].sentences.Count);
            line = lineInfos[0].sentences[randIndex];
        }
        else
        {
            int nIndex = (int)UnityEngine.Random.Range(0, lineInfos.Count);
            int index = 0;
            string n = "";
            string a = "";
            foreach(Taste lineInfo in lineInfos){
                if(index == nIndex)
                {
                    int randNIndex = (int)UnityEngine.Random.Range(0, lineInfo.n.Count);
                    if(lineInfo.n[randNIndex] == ""){
                        Debug.Log("There is no content in " + lineInfo.tasteName + " n...");
                        return null;
                    }

                    n = lineInfo.n[randNIndex];
                }
                else
                {
                    int randAIndex = UnityEngine.Random.Range(0, lineInfo.a.Count);
                    if(lineInfo.a[randAIndex] == ""){
                        Debug.Log("There is no content in " + lineInfo.tasteName + " a...");
                        return null;
                    }

                    a += lineInfo.a[randAIndex] + " ";
                }

                index++;
            }

            line = "Do you have " + a + n + "?";
        }

        return line;

        // foreach(string randomTaste in npcRandomTastes){
        //     if(randomTaste == ""){
        //         continue;
        //     }

        //     Taste lineInfo = null;
        //     foreach(BloodTasteSO tasteSO in tasteSOs){
        //         foreach(Taste tasteInfo in tasteSO.tastes){
        //             if(randomTaste == tasteInfo.tasteName){
        //                 lineInfo = tasteInfo;
        //                 break;
        //             }
        //         }
        //     }

        //     if(lineInfo == null){
        //         Debug.Log("There is no correct Taste in BloodTasteSOs...");
        //         return null;
        //     }

        //     if(isLine)
        //     {
        //         int randIndex = UnityEngine.Random.Range(0, lineInfo.sentences.Count);
        //         line = lineInfo.sentences[randIndex];
        //         break;
        //     }
        //     else
        //     {
        //         // int randIndex = UnityEngine.Random.Range(0, lineInfo.words.Count);
        //         // line += " " + lineInfo.words[randIndex];
        //     }
        // }

        // if(!isLine){
        //     line = "Do you have" + line + "?";
        // }
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

    public Sprite GetSpriteForDialogueView(int spriteIndex){
        return allDialogues[npcIndex].sprites[spriteIndex];
    }
}

[Serializable]
public class DialogueInfo{
    public string npcName;
    public List<Sprite> sprites;
    public string tasteLine;
    public List<string> tastes;
    public int priority;
    public float weight;
    public string dialogueName;
}
