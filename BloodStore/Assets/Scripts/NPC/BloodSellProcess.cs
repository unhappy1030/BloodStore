using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSellProcess : MonoBehaviour
{
    public Color newColor; // assign at inspector

    public bool isBloodSelected;
    public bool isFiltered;
    public bool isBloodSellFinish;
    public bool isFix;

    public GameObject filteringCanvas; // assign at inspector
    public GameObject notAbleFilterCanvas; // assign at inspector

    public Collider2D selectBloodPackObj;
    public Collider2D filteringObj;

    public MoneyControl moneyControl;
    public BloodPackUITest bloodPackUITest; // assign at inspector
    public BloodPackCount bloodPackCount; // assign at inspector
    

    void Start(){
        notAbleFilterCanvas.SetActive(false);
        selectBloodPackObj.enabled = true;
        filteringObj.enabled = false;
        
        isBloodSelected = false;
        isFiltered = false;
        isBloodSellFinish = false;
        isFix = false;
    }

    public void ResetAllBloodSellStatus(){
        isBloodSelected = false;
        isFiltered = false;
        isBloodSelected = false;
    }

    // button
    public void SelectBlood(){
        GameManager.Instance.bloodPackList.SubtractBloodPack(bloodPackUITest.sex, bloodPackUITest.bloodType, bloodPackUITest.rh, 1);

        selectBloodPackObj.enabled = false;
        CheckFilterAvailable();
        if(isFix)
        {
            filteringObj.enabled = false;
        }
        else
        {
            filteringObj.enabled = true;
        }
        
        isBloodSelected = true;

        bloodPackCount.SetCount();
    } 

    public void CheckFilterAvailable(){
        InteractObjInfo filterInfo = filteringObj.GetComponent<InteractObjInfo>();
        if(filterInfo == null){
            Debug.Log("There is no InteractObjInfo in Filtering Machine...");
            return;
        }

        if(GameManager.Instance.filterDurability <= 0)
        {
            filterInfo._ui = notAbleFilterCanvas;
        }
        else
        {
            filterInfo._ui = filteringCanvas;
        }
    }

    public void FilterBlood(){
        float damage = Random.Range(2f, 5f);
        GameManager.Instance.filterDurability -= damage;
        if(GameManager.Instance.filterDurability < 0){
            GameManager.Instance.filterDurability = 0;
        }
        
        moneyControl.CalculateMoney(-10);

        filteringObj.enabled = false;
        isFiltered = true;
    }
    
    // button
    public void EndBloodProcess(){
        selectBloodPackObj.enabled = true;
        filteringObj.enabled = false;

        isBloodSellFinish = true;
    }

    public void FixMachine(){
        moneyControl.CalculateMoney(-200);
        isFix = true;
        filteringObj.enabled = false;
        
        SpriteRenderer filterSprite = filteringObj.gameObject.GetComponent<SpriteRenderer>();
        if(filterSprite == null){
            Debug.Log("There is no SpriteRenderer in Filtering machine...");
            return;
        }
        
        filterSprite.color = newColor;
    }

    private void OnDestroy()
    {
        if(isFix){
            GameManager.Instance.filterDurability = 100;
            isFix = false;
        }
    }
}
