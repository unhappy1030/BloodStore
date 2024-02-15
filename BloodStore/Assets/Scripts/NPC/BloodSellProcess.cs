using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSellProcess : MonoBehaviour
{
    public bool isBloodSelected;
    public bool isFiltered;
    public bool isBloodSellFinish;

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
