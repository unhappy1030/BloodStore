using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class BloodTypeGameManager : MonoBehaviour
{
    public List<GameObject> deckList; // assign at inspector
    public TextMeshProUGUI nextCardText; // assign at inspector

    public List<MinigameDeck> deckStatusList;
    public MinigameDeck previousDeck;
    public MinigameDeck currentDeck;
    string randomBloodType;
    bool isPlayerTurn;


    /// <summary>
    /// 판과 패 초기화
    /// </summary>
    void Start(){
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
        
        deckStatusList = new List<MinigameDeck>();
        for(int i=0; i<9; i++){
            MinigameDeck newMDeck = new MinigameDeck(i, false, false, "", deckList[i]);
            deckStatusList.Add(newMDeck);
        }

        previousDeck = null;
        currentDeck = null;
        randomBloodType = MakeRandomBloodType();
        nextCardText.text = randomBloodType;
        isPlayerTurn = true;
    }

    /// <summary>
    /// 
    /// </summary>
    void Update(){
        if(Input.GetMouseButtonDown(0)){
            if(previousDeck != null){
                PutOnTheDeck();
                CheckDeckStatus(previousDeck.index, randomBloodType);
                randomBloodType = MakeRandomBloodType();
                nextCardText.text = randomBloodType;
                previousDeck = null;
                currentDeck = null;
                isPlayerTurn = !isPlayerTurn;
            }
        }
        ChangeMouseCursor();
    }

    void ChangeMouseCursor(){
        bool isOutOfDeck = true;
        foreach(MinigameDeck deck in deckStatusList){
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

    void CheckDeckStatus(int newDeckIndex, string newBloodType){
        
        if(newDeckIndex - 3 >= 0 && deckStatusList[newDeckIndex-3].isFilled){
            if(!isAbleToGiveBlood(newBloodType, deckStatusList[newDeckIndex-3].bloodType)){
                deckStatusList[newDeckIndex-3].ChangeText("", new Color32(0,0,0,255));
                deckStatusList[newDeckIndex-3].isFilled = false;
            }
        }

        if(newDeckIndex-1 >=0 && (newDeckIndex-1)/3 == newDeckIndex/3 && deckStatusList[newDeckIndex-1].isFilled){
            if(!isAbleToGiveBlood(newBloodType, deckStatusList[newDeckIndex-1].bloodType)){
                deckStatusList[newDeckIndex-1].ChangeText("", new Color32(0,0,0,255));
                deckStatusList[newDeckIndex-1].isFilled = false;
            }
        }

        if((newDeckIndex+1)/3 == newDeckIndex/3 && deckStatusList[newDeckIndex+1].isFilled){
            if(!isAbleToGiveBlood(newBloodType, deckStatusList[newDeckIndex+1].bloodType)){
                deckStatusList[newDeckIndex+1].ChangeText("", new Color32(0,0,0,255));
                deckStatusList[newDeckIndex+1].isFilled = false;
            }
        }

        if(newDeckIndex + 3 < 9 && deckStatusList[newDeckIndex+1].isFilled){
            if(!isAbleToGiveBlood(newBloodType, deckStatusList[newDeckIndex+3].bloodType)){
                deckStatusList[newDeckIndex+3].ChangeText("", new Color32(0,0,0,255));
                deckStatusList[newDeckIndex+3].isFilled = false;
            }
        }
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
        int randomInt = UnityEngine.Random.Range(0, 4);
        string bloodtype;
        if(randomInt == 0){
            bloodtype = "A+";
        }
        else if(randomInt == 1){
            bloodtype = "B+";
        }
        else if(randomInt == 2){
            bloodtype = "AB+";
        }
        else{
            bloodtype = "O+";
        }
        return bloodtype;
    }
}

[Serializable]
public class MinigameDeck{
    public int index;
    public bool isFilled;
    public bool isPlayer;
    public string bloodType;
    public GameObject realObject;
    TextMeshProUGUI realText;

    public MinigameDeck(){
        ;
    }

    public MinigameDeck(int index, bool isFilled, bool isPlayer, string bloodType, GameObject realObject){
        this.index = index;
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