using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSellProcess : MonoBehaviour
{
    public bool isBloodSelected;
    public bool isFiltered;
    public bool isBloodSellFinish;

    public BloodPackUITest bloodPackUITest; // assign at inspector

    void Start(){
        isBloodSelected = false;
        isFiltered = false;
        isBloodSellFinish = false;
    }

    // button
    public void ResetAllBloodSellStatus(){
        isBloodSelected = false;
        isFiltered = false;
        isBloodSelected = false;
    }

    // button
    public void SelecteBlood(){
        GameManager.Instance.bloodPackList.SubtractBloodPack(bloodPackUITest.sex, bloodPackUITest.bloodType, bloodPackUITest.rh, 1);
        isBloodSelected = true;
    } 

    public void FilterBlood(){   
        isFiltered = true;
    }
    
    // button
    public void EndBloodProcess(){
        isBloodSellFinish = true;
    }
}
