using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueButton : MonoBehaviour
{
    public float weight;

    public float probability;

    public float cost;

    public GameObject addChildUI;

    void Start()
    {
        SetValueRandom();
    }
    void SetValueRandom(){
        weight = Random.Range(0.5f, 0.9f);
        probability = Random.Range(0.75f, 1f);
        cost = 0;
    }
    public void AddChildByUI(Group group){
        group.pairTree.AddChildByValue(weight, probability);
        ChildAddUI UI = addChildUI.GetComponent<ChildAddUI>();
        UI.DeActive();
        group.button.SetActive(false);
        group.buttonOff.SetActive(true);
    }
}
