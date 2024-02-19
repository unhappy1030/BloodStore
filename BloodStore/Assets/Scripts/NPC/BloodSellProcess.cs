using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSellProcess : MonoBehaviour
{
    public bool isBloodSelected;
    public bool isFiltered;
    public bool isBloodSellFinish;

    public BoxCollider2D selectBloodPackObj;
    public BoxCollider2D filteringObj;

    public BloodPackUITest bloodPackUITest; // assign at inspector

    void Start(){
        isBloodSelected = false;
        isFiltered = false;
        isBloodSellFinish = false;
    }

    public void ResetAllBloodSellStatus(){
        isBloodSelected = false;
        isFiltered = false;
        isBloodSelected = false;
    }

    // button
    public void SelectBlood(){
        // GameManager.Instance.bloodPackList.SubtractBloodPack(bloodPackUITest.sex, bloodPackUITest.bloodType, bloodPackUITest.rh, 1);

        selectBloodPackObj.enabled = false;
        filteringObj.enabled = true;
        
        isBloodSelected = true;
    } 

    public void FilterBlood(){
        filteringObj.enabled = false;

        isFiltered = true;
    }
    
    // button
    public void EndBloodProcess(){
        selectBloodPackObj.enabled = true;
        filteringObj.enabled = false;

        isBloodSellFinish = true;
    }
}
