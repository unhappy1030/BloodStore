using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectButton : MonoBehaviour
{
    public Index index;
    public NodeSO nodeSO;
    public GameObject selectedCard;
    public SelectableCardSO cardSO;
    public void OnButtonClick()
    {
        SelectCardUI UI = GetComponentInParent<SelectCardUI>();
        UI.DeActiveSelectableCards();
        nodeSO.SetNode(cardSO.cards[index.GetIndex()]);
        SelectedCardDisplay cardDisplay = selectedCard.GetComponent<SelectedCardDisplay>();
        cardDisplay.SetCardData(nodeSO.node, index.GetIndex());
        UI.ActiveSelectedCard();
    }

}