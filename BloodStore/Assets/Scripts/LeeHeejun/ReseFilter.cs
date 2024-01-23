using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetFilter : MonoBehaviour
{
    public Button resetButton;
    public ToggleGroups toggleGroupsScript;

    void Start()
    {
        resetButton.onClick.AddListener(DeactivateAllToggles);
    }

    void DeactivateAllToggles()
    {
        toggleGroupsScript.DeactivateAllToggles();
    }
}
