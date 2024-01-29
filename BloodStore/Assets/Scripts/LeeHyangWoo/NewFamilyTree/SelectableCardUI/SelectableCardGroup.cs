using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableCardGroup : MonoBehaviour
{
    public SelectableCardSO cardSO;
    void Start(){
        int cardDisplayNum = transform.childCount;
        cardSO.MakeData(cardDisplayNum);
        SetCardDisplayData(cardDisplayNum);
    }
    void SetCardDisplayData(int cardDisplayNum){
        for(int i = 0; i < cardDisplayNum; i++){
            Transform cardDisplayTransfrom = transform.GetChild(i);
            SelectableCardDisplay cardDisplay = cardDisplayTransfrom.GetComponent<SelectableCardDisplay>();
            cardDisplay.SetCardData(cardSO.cards[i], i);
        }
    }
}
