using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectButton : MonoBehaviour
{
    public Index index;
    public NodeSO nodeSO;
    public GameObject selectedCard;
    public GameObject selectableCardGroup;
    public SelectableCardSO cardSO;
    public void OnButtonClick()
    {
        if(index.buttonCheck){
            if(cardSO.cards[index.GetIndex()].cost < GameManager.Instance.money){ // 중매비 체크
                GameManager.Instance.money -= cardSO.cards[index.GetIndex()].cost;
                SelectCardUI UI = GetComponentInParent<SelectCardUI>();
                SelectableCardGroup group = selectableCardGroup.GetComponent<SelectableCardGroup>();
                UI.DeActiveSelectableCards();
                nodeSO.SetNode(cardSO.cards[index.GetIndex()]);
                group.SetOffCard(index.GetIndex());
                SelectedCardDisplay cardDisplay = selectedCard.GetComponent<SelectedCardDisplay>();
                cardDisplay.SetCardData(nodeSO.node, index.GetIndex());
                UI.ActiveSelectedCard();
            }
            
        }
    }
}
