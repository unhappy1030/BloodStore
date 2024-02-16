using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSellProcess : MonoBehaviour
{
    public bool isBloodSelected;
    public bool isFiltered;
    public bool isBloodSellFinish;

    public GameObject selectBloodPackObj;

    public GameObject filteringObj;
    public GameObject filteringCanvas;

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

        InteractObjInfo selectBloodObjInfo = selectBloodPackObj.GetComponent<InteractObjInfo>();
        if(selectBloodObjInfo != null){
            Destroy(selectBloodObjInfo);
        }

        InteractObjInfo filterObjInfo = filteringObj.AddComponent<InteractObjInfo>();
        filterObjInfo._interactType = InteractType.UIOnOff;
        filterObjInfo._ui = filteringCanvas;
        
        isBloodSelected = true;
    } 

    public void FilterBlood(){
        InteractObjInfo filterObjInfo = filteringObj.GetComponent<InteractObjInfo>();
        
        if(filterObjInfo != null){
            Destroy(filterObjInfo);
        }

        isFiltered = true;
    }
    
    // button
    public void EndBloodProcess(){
        isBloodSellFinish = true;
    }
}
