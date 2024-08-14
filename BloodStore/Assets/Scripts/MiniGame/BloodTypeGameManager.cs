using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class BloodTypeGameManager : MonoBehaviour
{
    public List<GameObject> deckObjList; // assign at inspector
    public TextMeshProUGUI nextCardText; // assign at inspector
    public GameObject gameResultObj; // assign at inspector
    public BloodtypeGameAI gameAI; // assign at inspector

    public BloodTypeGameSet gameSetStatus;
    public GameObject previousDeck;
    public GameObject currentDeck;

    /// <summary>
    /// 판과 패 초기화
    /// </summary>
    void Start(){
        StartCoroutine(MiniGame());
    }

    IEnumerator MiniGame(){
        SetGame();
        yield return StartCoroutine(SetTurn());
        while(!gameSetStatus.IsGameEnd()){
            if(gameSetStatus.isPlayerTurn)
                yield return StartCoroutine(PlayerTurn());
            else
                yield return StartCoroutine(ComputerTurn());
        }
        yield return StartCoroutine(EndGame());
    }

    void SetGame(){
        if(deckObjList == null || deckObjList.Count == 0){
            Debug.Log("deck texts list is empty...");
            return;
        }
        if(nextCardText == null){
            Debug.Log("Next hand text is empty...");
            return;
        }
        if(gameResultObj == null){
            Debug.Log("Game result object is empty...");
            return;
        }
        gameResultObj.SetActive(false);

        foreach(GameObject deck in deckObjList){
            TextMeshProUGUI text = deck.transform.GetComponentInChildren<TextMeshProUGUI>();
            text.text = "";
        }
        nextCardText.text = "";

        gameSetStatus = new BloodTypeGameSet();
        nextCardText.text = gameSetStatus.currentBloodtype;

        previousDeck = null;
        currentDeck = null;
    }

    IEnumerator SetTurn(){
        gameSetStatus.isPlayerTurn = (UnityEngine.Random.value < 0.5f);
        if(gameSetStatus.isPlayerTurn)
            Debug.Log("Player first!");
        else
            Debug.Log("Computer first!");
        yield return null;
    }

    IEnumerator PlayerTurn(){
        while(!Input.GetMouseButtonDown(0)){
            ChangeMouseCursor();
            yield return null;
        }

        if(previousDeck != null){
            int targetIdx = deckObjList.IndexOf(previousDeck);
            int[] idxs = new int[2]{targetIdx/3, targetIdx%3};
            gameSetStatus.PutOnTheDeck(idxs);
            UpdateTextStatus();

            gameSetStatus.SetNextRandomBloodType();
            nextCardText.text = gameSetStatus.currentBloodtype;
            gameSetStatus.ChangeTurn();
            previousDeck = null;
            currentDeck = null;
        }
    }

    IEnumerator ComputerTurn(){
        gameAI.isCalculationFinished = false;
        int[] idxs = gameAI.FindOptimalIndex(gameSetStatus);
        yield return new WaitUntil(() => gameAI.isCalculationFinished);
        yield return new WaitForSeconds(0.7f);
        gameAI.isCalculationFinished = false;
        gameSetStatus.PutOnTheDeck(idxs);
        UpdateTextStatus();

        gameSetStatus.SetNextRandomBloodType();
        nextCardText.text = gameSetStatus.currentBloodtype;
        gameSetStatus.ChangeTurn();
        previousDeck = null;
        currentDeck = null;
        Debug.Log("Idx : {" + idxs[0] + "," + idxs[1] + "} / " + gameSetStatus.currentBloodtype);
    }

    void UpdateTextStatus(){
        for(int i=0; i<deckObjList.Count; i++){
            int[] idxs = new int[2]{i/3, i%3};
            if(gameSetStatus.playerDeck[idxs[0], idxs[1]] != null){
                ChangeText(deckObjList[i], gameSetStatus.playerDeck[idxs[0], idxs[1]], new Color32(0,0,255,255));
            }
            else if(gameSetStatus.computerDeck[idxs[0], idxs[1]] != null){
                ChangeText(deckObjList[i], gameSetStatus.computerDeck[idxs[0], idxs[1]], new Color32(255,0,0,255));
            }
            else{
                ResetText(deckObjList[i]);
            }
        }
    }

    void ChangeMouseCursor(){
        bool isOutOfDeck = true;
        for(int i=0; i<deckObjList.Count; i++){
            GameObject deck = deckObjList[i];
            int[] idxs = new int[]{i/3, i%3};

            if(gameSetStatus.IsAnyDeckFilled(idxs))
                continue;
            
            RectTransform rectTransform = deck.GetComponent<RectTransform>();
            Vector2 localMousePosition = rectTransform.InverseTransformPoint(Input.mousePosition);
            if(rectTransform.rect.Contains(localMousePosition)){
                currentDeck = deck;
                isOutOfDeck = false;
                break;
            }
        }

        if(isOutOfDeck){
            if(previousDeck != null){
                ResetText(previousDeck);
            }
            previousDeck = null;
            currentDeck = null;
            return;
        }

        if(currentDeck != null && (previousDeck == null || previousDeck != currentDeck)){
            if(previousDeck != null){
                ResetText(previousDeck);
            }
            ChangeText(currentDeck, gameSetStatus.currentBloodtype, new Color32(0,0,0,128));
            previousDeck = currentDeck;
        }
    }

    void ResetText(GameObject target){
        TextMeshProUGUI targetText = target.GetComponentInChildren<TextMeshProUGUI>();
        targetText.text = "";
    }

    void ChangeText(GameObject target, string content, Color32 textColor){
        TextMeshProUGUI targetText = target.GetComponentInChildren<TextMeshProUGUI>();
        targetText.color = textColor;
        targetText.text = content;
    }

    IEnumerator EndGame(){
        gameResultObj.SetActive(true);
        TextMeshProUGUI gameResultText = gameResultObj.GetComponentInChildren<TextMeshProUGUI>();
        if(gameSetStatus.isPlayerWin)
            gameResultText.text = "You Win!";
        else
            gameResultText.text = "You lose!";
        
        yield return new WaitForSeconds(2);
        gameResultObj.SetActive(false);
    }
}