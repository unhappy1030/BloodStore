using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSelectedCardBackButton : MonoBehaviour
{
    public NodeSO nodeSO;
    public GameObject selectableCardUI;
    public void SetSelectedCard(){
        SelectCardUI UI = selectableCardUI.GetComponent<SelectCardUI>();
        if(!nodeSO.node.empty){
            UI.ActiveSelectedCard();
        }
    }
}
