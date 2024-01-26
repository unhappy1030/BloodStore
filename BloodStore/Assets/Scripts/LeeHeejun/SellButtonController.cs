using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SellButtonController : MonoBehaviour
{
    public BloodPackManager bloodPackManager;  // BloodPackManager에 대한 참조

    private Button sellButton;  // 판매 버튼

    private void Awake()
    {
        sellButton = GetComponent<Button>();
        sellButton.interactable = false;  // 처음에는 버튼을 비활성화 시킵니다.
    }

    private void Update()
    {
        sellButton.interactable = bloodPackManager.SelectedBloodPacks.Count > 0;
    }

}
