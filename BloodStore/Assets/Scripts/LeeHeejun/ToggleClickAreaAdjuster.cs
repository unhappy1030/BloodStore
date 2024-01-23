using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Toggle))]
public class ToggleClickAreaAdjuster : MonoBehaviour
{
    [SerializeField] private TMP_Text textMeshPro;
    [SerializeField] private Image clickAreaImage;

    private Toggle toggle;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
    }

    private void Start()
    {
        AdjustClickArea();
    }

    private void AdjustClickArea()
    {
        if (textMeshPro == null || clickAreaImage == null)
        {
            Debug.LogWarning("TextMeshPro or ClickAreaImage is not set.");
            return;
        }

        // Adjust the size of the click area image to the size of the text.
        clickAreaImage.rectTransform.sizeDelta = textMeshPro.rectTransform.sizeDelta;

        // Make the click area image invisible.
        clickAreaImage.color = new Color(0, 0, 0, 0);

        // Enable raycast target to make it clickable.
        clickAreaImage.raycastTarget = true;
    }
}