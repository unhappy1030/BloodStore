using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 혈액을 판매하는 모든 과정 총괄
/// </summary>
public class BloodSellProcessManager : MonoBehaviour
{
    public Color fixingStatusColor; // assign at inspector

    public bool isBloodSelected;
    public bool isFiltered;
    public bool isBloodSellFinish;
    public bool isMachineFixing;

    public GameObject filterBloodUI; // assign at inspector
    public GameObject notAbleFilterUI; // assign at inspector

    public Collider2D selectBloodPlaceCollider;
    public Collider2D filterMachineCollider;

    public MoneyControl moneyControl;
    public BloodPackUITest bloodPackUITest; // assign at inspector
    public BloodPackCount bloodPackCount; // assign at inspector
    

    void Start(){
        notAbleFilterUI.SetActive(false);
        selectBloodPlaceCollider.enabled = true;
        filterMachineCollider.enabled = false;
        
        isBloodSelected = false;
        isFiltered = false;
        isBloodSellFinish = false;
        isMachineFixing = false;
    }

    /// <summary>
    /// 혈액 판매 상태를 초기화, NPCInteract에서 사용
    /// </summary>
    public void ResetAllBloodSellStatus(){
        isBloodSelected = false;
        isFiltered = false;
        isBloodSelected = false;
    }

    // button
    /// <summary>
    /// StoreCanvas -> SelectBloodPackUI -> SelectButton
    /// 혈액팩을 고른 후 선택.
    /// 혈액팩 개수를 차감하고 콜라이더를 컨트롤함
    /// </summary>
    public void SelectBloodBTN(){
        GameManager.Instance.bloodPackList.SubtractBloodPack(bloodPackUITest.sex, bloodPackUITest.bloodType, bloodPackUITest.rh, 1);

        selectBloodPlaceCollider.enabled = false;
        CheckFilterAvailable();
        if(isMachineFixing)
        {
            filterMachineCollider.enabled = false;
        }
        else
        {
            filterMachineCollider.enabled = true;
        }
        
        isBloodSelected = true;

        bloodPackCount.SetCount();
        bloodPackUITest.ChangeCount();
    } 

    /// <summary>
    /// 필터 기계의 내구도에 따라 필터가 가능한지 확인 후 이에 해당하는 UI를 필터 기계에 설정.
    /// </summary>
    public void CheckFilterAvailable(){
        InteractObjInfo filterInfo = filterMachineCollider.GetComponent<InteractObjInfo>();
        if(filterInfo == null){
            Debug.Log("There is no InteractObjInfo in Filtering Machine...");
            return;
        }

        if(GameManager.Instance.filterDurability <= 0)
        {
            filterInfo._ui = notAbleFilterUI;
        }
        else
        {
            filterInfo._ui = filterBloodUI;
        }
    }

    /// <summary>
    /// 혈액 필터링 후 기계 내구도 차감
    /// </summary>
    public void FilterBlood(){
        float damage = Random.Range(2f, 5f);
        GameManager.Instance.filterDurability -= damage;
        if(GameManager.Instance.filterDurability < 0){
            GameManager.Instance.filterDurability = 0;
        }
        
        moneyControl.CalculateMoney(-10);

        filterMachineCollider.enabled = false;
        isFiltered = true;
    }
    
    // button
    /// <summary>
    /// StoreCanvas -> FinishSellProcessButton
    /// 혈액 선택 과정을 마치고 콜라이더 초기화
    /// </summary>
    public void EndBloodProcessBTN(){
        selectBloodPlaceCollider.enabled = true;
        filterMachineCollider.enabled = false;

        isBloodSellFinish = true;
    }

    // button
    /// <summary>
    /// StoreCanvas -> NotAbleFilterUI -> FixButton
    /// 기계 수리. isMachineFixing을 true로 변경한 후 onDestroy함수 내에서 내구도 변경
    /// </summary>
    public void FixMachineBTN(){
        moneyControl.CalculateMoney(-200);
        isMachineFixing = true;
        filterMachineCollider.enabled = false;
        
        SpriteRenderer filterSprite = filterMachineCollider.gameObject.GetComponent<SpriteRenderer>();
        if(filterSprite == null){
            Debug.Log("There is no SpriteRenderer in Filtering machine...");
            return;
        }
        
        filterSprite.color = fixingStatusColor;
    }

    /// <summary>
    ///  기계 수리를 한 경우 기계 내구도 100으로 변경
    /// </summary>
    private void OnDestroy()
    {
        if(isMachineFixing){
            GameManager.Instance.filterDurability = 100;
            isMachineFixing = false;
        }
    }
}
