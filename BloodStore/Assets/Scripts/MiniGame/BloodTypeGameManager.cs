using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class BloodTypeGameManager : MonoBehaviour
{
    public List<GameObject> deckList; // assign at inspector
    public TextMeshProUGUI nextCardText; // assign at inspector

    public List<List<MinigameDeck>> deckStatusList;
    public MinigameDeck previousDeck;
    public MinigameDeck currentDeck;
    string randomBloodType;
    bool isPlayerTurn;
    bool isPlayerWin;


    /// <summary>
    /// 판과 패 초기화
    /// </summary>
    void Start(){
        StartCoroutine(MiniGame());
    }

    IEnumerator MiniGame(){
        SetGame();
        yield return StartCoroutine(SetTurn());
        while(!isGameEnd()){
            yield return StartCoroutine(PlayOneTurn());
        }
        if(isPlayerWin)
            Debug.Log("Player Win!");
        else
            Debug.Log("Player lose!");
    }

    void SetGame(){
        if(deckList == null || deckList.Count == 0){
            Debug.Log("deck texts list is empty...");
            return;
        }
        if(nextCardText == null){
            Debug.Log("Next hand text is empty...");
            return;
        }

        foreach(GameObject deck in deckList){
            TextMeshProUGUI text = deck.transform.GetComponentInChildren<TextMeshProUGUI>();
            text.text = "";
        }
        nextCardText.text = "";
        
        deckStatusList = new List<List<MinigameDeck>>();
        for(int i=0; i<3; i++){
            deckStatusList.Add(new List<MinigameDeck>());
            for(int j=0; j<3; j++){
                MinigameDeck newMDeck = new MinigameDeck(i, j, false, false, "", deckList[i*3+j]);
                deckStatusList[i].Add(newMDeck);
            }
        }

        previousDeck = null;
        currentDeck = null;
        randomBloodType = MakeRandomBloodType();
        nextCardText.text = randomBloodType;
        isPlayerWin = false;
    }

    IEnumerator SetTurn(){
        isPlayerTurn = (UnityEngine.Random.value < 0.5f);
        yield return null;
    }

    IEnumerator PlayOneTurn(){
        while(!Input.GetMouseButtonDown(0)){
            ChangeMouseCursor();
            yield return null;
        }

        if(previousDeck != null){
            PutOnTheDeck();
            CheckDeckStatus(previousDeck.indexs, randomBloodType);
            randomBloodType = MakeRandomBloodType();
            nextCardText.text = randomBloodType;
            previousDeck = null;
            currentDeck = null;
            isPlayerTurn = !isPlayerTurn;
        }
    }

    bool isGameEnd(){
        if(isMakeOneLine(0, 4, 8) || isMakeOneLine(1, 4, 7) || isMakeOneLine(2, 4, 6) || isMakeOneLine(3, 4, 5)){
            isPlayerWin = deckStatusList[1][1].isPlayer;
            return true;
        }

        if(isMakeOneLine(0, 3, 6) || isMakeOneLine(0, 1, 2)){
            isPlayerWin = deckStatusList[0][0].isPlayer;
            return true;
        }

        if(isMakeOneLine(6, 7, 8) || isMakeOneLine(2, 5, 8)){
            isPlayerWin = deckStatusList[2][2].isPlayer;
            return true;
        }

        return false;
    }

    bool isMakeOneLine(int i1, int i2, int i3){
        if(deckStatusList[i2/3][i2%3].isFilled && deckStatusList[i1/3][i1%3].isFilled && deckStatusList[i3/3][i3%3].isFilled
        && deckStatusList[i1/3][i1%3].isPlayer == deckStatusList[i3/3][i3%3].isPlayer && deckStatusList[i3/3][i3%3].isPlayer == deckStatusList[i2/3][i2%3].isPlayer){
            return true;
        }

        return false;
    }

    // void Update(){
    //     if(Input.GetMouseButtonDown(0)){
    //         if(previousDeck != null){
    //             PutOnTheDeck();
    //             CheckDeckStatus(previousDeck.index, randomBloodType);
    //             randomBloodType = MakeRandomBloodType();
    //             nextCardText.text = randomBloodType;
    //             previousDeck = null;
    //             currentDeck = null;
    //             isPlayerTurn = !isPlayerTurn;
    //         }
    //     }
    //     ChangeMouseCursor();
    // }

    void ChangeMouseCursor(){
        bool isOutOfDeck = true;
        for(int i=0; i<3; i++){
            for(int j=0; j<3; j++){
                MinigameDeck deck = deckStatusList[i][j];

                if(deck.isFilled)
                    continue;
                
                RectTransform rectTransform = deck.realObject.GetComponent<RectTransform>();
                Vector2 localMousePosition = rectTransform.InverseTransformPoint(Input.mousePosition);
                if(rectTransform.rect.Contains(localMousePosition)){
                    currentDeck = deck;
                    isOutOfDeck = false;
                    break;
                }
            }
        }

        // foreach(MinigameDeck deck in deckStatusList){
        //     if(deck.isFilled)
        //         continue;
        //     RectTransform rectTransform = deck.realObject.GetComponent<RectTransform>();
        //     Vector2 localMousePosition = rectTransform.InverseTransformPoint(Input.mousePosition);
        //     if(rectTransform.rect.Contains(localMousePosition)){
        //         currentDeck = deck;
        //         isOutOfDeck = false;
        //         break;
        //     }
        // }

        if(isOutOfDeck){
            if(previousDeck != null){
                previousDeck.ChangeText("");
            }
            previousDeck = null;
            currentDeck = null;
            return;
        }

        if(currentDeck != null && (previousDeck == null || previousDeck != currentDeck)){
            if(previousDeck != null){
                previousDeck.ChangeText("");
            }
            currentDeck.ChangeText(randomBloodType, new Color32(0,0,0,128));
            previousDeck = currentDeck;
        }
    }

    void PutOnTheDeck(){
        Color32 newColor = (isPlayerTurn) ? new Color32(255, 0, 0, 255) : new Color32(0, 0, 255, 255);
        previousDeck.ChangeText(randomBloodType, newColor);
        previousDeck.isFilled = true;
        previousDeck.isPlayer = isPlayerTurn;
        previousDeck.bloodType = randomBloodType;
    }

    void CheckDeckStatus(int[] idxs, string newBloodType){
        if(idxs[0] > 0 && deckStatusList[idxs[0]-1][idxs[1]].isFilled)
        {
            MinigameDeck upDeck = deckStatusList[idxs[0]-1][idxs[1]];

            if(!isAbleToGiveBlood(newBloodType, upDeck.bloodType)){
                upDeck.ChangeText("", new Color32(0,0,0,255));
                upDeck.isFilled = false;
            }
        }

        if(idxs[1] > 0 && deckStatusList[idxs[0]][idxs[1]-1].isFilled)
        {
            MinigameDeck leftDeck = deckStatusList[idxs[0]][idxs[1]-1];

            if(!isAbleToGiveBlood(newBloodType, leftDeck.bloodType)){
                leftDeck.ChangeText("", new Color32(0,0,0,255));
                leftDeck.isFilled = false;
            }
        }

        if(idxs[0] < 2 && deckStatusList[idxs[0]+1][idxs[1]].isFilled)
        {
            MinigameDeck downDeck = deckStatusList[idxs[0]+1][idxs[1]];

            if(!isAbleToGiveBlood(newBloodType, downDeck.bloodType)){
                downDeck.ChangeText("", new Color32(0,0,0,255));
                downDeck.isFilled = false;
            }
        }

        if(idxs[1] < 2 && deckStatusList[idxs[0]][idxs[1]+1].isFilled)
        {
            MinigameDeck rightDeck = deckStatusList[idxs[0]][idxs[1]+1];

            if(!isAbleToGiveBlood(newBloodType, rightDeck.bloodType)){
                rightDeck.ChangeText("", new Color32(0,0,0,255));
                rightDeck.isFilled = false;
            }
        }
        
        // if(idxs - 3 >= 0 && deckStatusList[idxs-3].isFilled){
        //     if(!isAbleToGiveBlood(newBloodType, deckStatusList[idxs-3].bloodType)){
        //         deckStatusList[idxs-3].ChangeText("", new Color32(0,0,0,255));
        //         deckStatusList[idxs-3].isFilled = false;
        //     }
        // }

        // if(idxs-1 >=0 && (idxs-1)/3 == idxs/3 && deckStatusList[idxs-1].isFilled){
        //     if(!isAbleToGiveBlood(newBloodType, deckStatusList[idxs-1].bloodType)){
        //         deckStatusList[idxs-1].ChangeText("", new Color32(0,0,0,255));
        //         deckStatusList[idxs-1].isFilled = false;
        //     }
        // }

        // if((idxs+1)/3 == idxs/3 && deckStatusList[idxs+1].isFilled){
        //     if(!isAbleToGiveBlood(newBloodType, deckStatusList[idxs+1].bloodType)){
        //         deckStatusList[idxs+1].ChangeText("", new Color32(0,0,0,255));
        //         deckStatusList[idxs+1].isFilled = false;
        //     }
        // }

        // if(idxs + 3 < 9 && deckStatusList[idxs+1].isFilled){
        //     if(!isAbleToGiveBlood(newBloodType, deckStatusList[idxs+3].bloodType)){
        //         deckStatusList[idxs+3].ChangeText("", new Color32(0,0,0,255));
        //         deckStatusList[idxs+3].isFilled = false;
        //     }
        // }
    }

    bool isAbleToGiveBlood(string newBloodType, string currentBloodType){
        if(newBloodType == "A+"){
            return (currentBloodType == "A+" || currentBloodType == "AB+");
        }
        else if(newBloodType == "B+"){
            return (currentBloodType == "B+" || currentBloodType == "AB+");
        }
        else if(newBloodType == "AB+"){
            return (currentBloodType == "AB+");
        }
        else{
            return true;
        }
    }

    string MakeRandomBloodType(){
        float[] bloodtypeRate = {0.38f, 0.1f, 0.03f, 0.49f}; // A B AB O
        float randomPoint = UnityEngine.Random.value;

        int index = -1;
        if(randomPoint == 1){
            index = bloodtypeRate.Length - 1;
        }
        else{
            float cumulativeProb = 0f;
            for(int i=0; i<bloodtypeRate.Length; i++){
                cumulativeProb += bloodtypeRate[i];
                if(randomPoint < cumulativeProb){
                    index = i;
                    break;
                }
            }
        }

        string bloodtype;
        if(index == 0){
            bloodtype = "A+";
        }
        else if(index == 1){
            bloodtype = "B+";
        }
        else if(index == 2){
            bloodtype = "AB+";
        }
        else if(index == 3){
            bloodtype = "O+";
        }
        else{
            bloodtype = "ERROR";
        }

        return bloodtype;
    }
}

[Serializable]
public class MinigameDeck{
    public int[] indexs;
    public bool isFilled;
    public bool isPlayer;
    public string bloodType;
    public GameObject realObject;
    TextMeshProUGUI realText;

    public MinigameDeck(){
        ;
    }

    public MinigameDeck(int x, int y, bool isFilled, bool isPlayer, string bloodType, GameObject realObject){
        this.indexs = new int[2];
        this.indexs[0] = x;
        this.indexs[1] = y;
        this.isFilled = isFilled;
        this.isPlayer = isPlayer;
        this.bloodType = bloodType;
        this.realObject = realObject;

        realText = realObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void ChangeText(string content){
        realText.text = content;
    }

    public void ChangeText(string content, Color32 color){
        realText.text = content;
        realText.color = color;
    }
}