using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SelectCardUI : MonoBehaviour
{
    public Index index;
    public GameObject viewUI;
    public GameObject selectableCardGroupUI;
    public GameObject selectedCardUI;
    void Start(){
        if (viewUI.activeSelf){
            viewUI.SetActive(false);
        }
        if(selectedCardUI.activeSelf){
            selectedCardUI.SetActive(false);
        }
    }
    public void ActiveSelectableCards(){
        index.buttonCheck = false;
        SelectableCardGroup selectableCardGroup = selectableCardGroupUI.GetComponent<SelectableCardGroup>();
        selectableCardGroup.UpdateCardData();
        viewUI.SetActive(true);
    }
    public void DeActiveSelectableCards(){
        viewUI.SetActive(false);
    }
    public void ActiveSelectedCard(){
        selectedCardUI.SetActive(true);
    }
    public void DeActiveSelectedCard(){
        selectedCardUI.SetActive(false);
    }
}
