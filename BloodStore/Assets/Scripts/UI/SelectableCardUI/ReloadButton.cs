using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadButton : MonoBehaviour
{
    public GameObject selectableCardGroupUI;

    public void Reload(){
        SelectableCardGroup selectableCardGroup = selectableCardGroupUI.GetComponent<SelectableCardGroup>();
        selectableCardGroup.SetCardAll();
    }
}
