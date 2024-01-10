using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SelectableCardViewButton : MonoBehaviour
{
    public void OnButtonClick()
    {
        SelectCardUI UI = GetComponentInParent<SelectCardUI>();
        UI.ActiveSelectableCards();
        UI.DeActiveSelectedCard();
    }
}
