using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadButton : MonoBehaviour
{
    public GameObject selectableCardGroupUI;

    public void Reload(){
        if(GameManager.Instance.money > 5){
            MoneyControl moneyControl = GameManager.Instance.gameObject.GetComponent<MoneyControl>();
            moneyControl.CalculateMoney(-5);
            SelectableCardGroup selectableCardGroup = selectableCardGroupUI.GetComponent<SelectableCardGroup>();
            selectableCardGroup.SetCardAll();
        }
    }
}
