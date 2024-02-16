using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ValueButton : MonoBehaviour
{
    public float weight;

    public float probability;

    public float cost;

    public GameObject addChildUI;

    public TextMeshProUGUI contentLabel;

    public Color disabledColor; // 비활성화 될 때의 색상
    public bool costCheck = false; // 조건 충족 여부

    public void SetValueRandom(float[] values, string content){
        weight = values[0];
        probability = values[1];
        cost = values[2];
        contentLabel.text = content;
    }
    public void CheckCost(){
        Button button = this.GetComponent<Button>();
        if(cost > GameManager.Instance.money){
            button.image.color = disabledColor;
            costCheck = false;
        }
        else{
            button.image.color = Color.white;
            costCheck = true;
        }
    }
    public void AddChildByUI(){
        if(costCheck){
            MoneyControl moneyControl = GameManager.Instance.gameObject.GetComponent<MoneyControl>();
            moneyControl.CalculateMoney(cost * -1);
            ChildAddUI UI = addChildUI.GetComponent<ChildAddUI>();
            Group group = UI.group;
            group.pairTree.AddChildByValue(weight, probability);
            UI.DeActive();
            group.button.SetActive(false);
            group.buttonOff.SetActive(true);
        }
    }
}
