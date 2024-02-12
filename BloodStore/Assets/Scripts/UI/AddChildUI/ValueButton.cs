using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValueButton : MonoBehaviour
{
    public float weight;

    public float probability;

    public float cost;

    public GameObject addChildUI;

    public Color disabledColor; // 비활성화 될 때의 색상
    public bool costCheck = false; // 조건 충족 여부

    void Start()
    {
        SetValueRandom();
    }
    public void SetValueRandom(){
        weight = Random.Range(0.8f, 0.9f);
        probability = Random.Range(0.75f, 1f);
        cost = Random.Range(10, 16);
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
            ChildAddUI UI = addChildUI.GetComponent<ChildAddUI>();
            Group group = UI.group;
            group.pairTree.AddChildByValue(weight, probability);
            UI.DeActive();
            group.button.SetActive(false);
            group.buttonOff.SetActive(true);
        }
    }
}
