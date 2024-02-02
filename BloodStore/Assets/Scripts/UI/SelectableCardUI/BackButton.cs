using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : MonoBehaviour
{
    public NodeSO nodeSO;
    public GameObject selectableCardUI;
    public void OffUI(){
        SelectCardUI UI = selectableCardUI.GetComponent<SelectCardUI>();
        UI.DeActiveSelectableCards();
        if(!nodeSO.node.empty){
            UI.ActiveSelectedCard();
        }
    }
}
