using System.Collections;
using System.Collections.Generic;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;

public class SelectableCardGroup : MonoBehaviour
{
    public SelectableCardSO cardSO;
    public List<GameObject> cardList;
    void Start(){
        SetCardAll();
    }
    public void SetCardAll(){
        int cardDisplayNum = transform.childCount;
        cardSO.MakeData(cardDisplayNum);
        SetCardDisplayData(cardDisplayNum);
    }
    void SetCardDisplayData(int cardDisplayNum){
        for(int i = 0; i < cardDisplayNum; i++){
            Transform cardDisplayTransfrom = transform.GetChild(i);
            SelectableCardDisplay cardDisplay = cardDisplayTransfrom.GetComponent<SelectableCardDisplay>();
            cardDisplay.SetCardData(cardSO.cards[i], i);
            cardDisplay.gameObject.SetActive(true);
            cardList.Add(cardDisplay.gameObject);
        }
    }
    public void UpdateCardData(){
        for(int i = 0; i < cardList.Count; i++){
            SelectableCardDisplay dp = cardList[i].GetComponent<SelectableCardDisplay>();
            dp.CheckCost(cardSO.cards[i]);
        }
    }
    public void SetOffCard(int index){
        cardList[index].SetActive(false);
    }
}
