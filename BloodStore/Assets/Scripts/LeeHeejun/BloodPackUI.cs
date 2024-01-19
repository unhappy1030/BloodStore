using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class BloodPackUI : MonoBehaviour, IPointerClickHandler
{
    public Color defaultColor = Color.white;
    public Color selectedColor = Color.green;
    private bool isSelected = false;
    public TMP_Text[] texts;

    void Start()
    {
        texts = GetComponentsInChildren<TMP_Text>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isSelected = !isSelected;
        Color targetColor = isSelected ? selectedColor : defaultColor;

        foreach (TMP_Text text in texts)
        {
            text.color = targetColor;
        }

        if (isSelected)
        {
            PrintInfo();
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