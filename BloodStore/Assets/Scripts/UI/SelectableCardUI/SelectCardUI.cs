using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SelectCardUI : MonoBehaviour
{
    public Index index;
    public GameObject view;
    public GameObject selectedCard;
    void Start(){
        if (view.activeSelf){
            view.SetActive(false);
        }
        if(selectedCard.activeSelf){
            selectedCard.SetActive(false);
        }
    }
    public void ActiveSelectableCards(){
        index.buttonCheck = false;
        view.SetActive(true);
    }
    public void DeActiveSelectableCards(){
        view.SetActive(false);
    }
    public void ActiveSelectedCard(){
        selectedCard.SetActive(true);
    }
    public void DeActiveSelectedCard(){
        selectedCard.SetActive(false);
    }
}
