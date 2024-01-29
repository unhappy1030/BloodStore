using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableCardGroup : MonoBehaviour
{
    public SelectableCardSO cardSO;
    public List<GameObject> CardList;
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
            CardList.Add(cardDisplay.gameObject);
        }
    }
    public void SetOffCard(int index){
        CardList[index].SetActive(false);
    }
}
