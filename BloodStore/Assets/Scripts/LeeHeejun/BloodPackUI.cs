using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class BloodPackUI : MonoBehaviour, IPointerClickHandler
{
    public BloodPackManager bloodPackManager;
    public Color defaultColor = Color.white;
    public Color selectedColor = Color.green;
    private bool isSelected = false;
    public TMP_Text[] texts;

    void Start()
    {
        texts = GetComponentsInChildren<TMP_Text>();
        bloodPackManager = GameObject.Find("BloodPackManager").GetComponent<BloodPackManager>();

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isSelected = !isSelected;
        Color targetColor = isSelected ? selectedColor : defaultColor;

        if (isSelected)
        {
            bloodPackManager.SelectBloodPack(this);
            PrintInfo();
        }
        else
        {
            bloodPackManager.DeselectBloodPack(this);  // 선택이 해제되었으므로 BloodPackManager에서 제거
        }

        foreach (TMP_Text text in texts)
        {
            text.color = targetColor;
        }
    }

    private void PrintInfo()
    {
        foreach (TMP_Text text in texts)
        {
            Debug.Log(text.text);
        }
    }
}
