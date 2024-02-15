using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildAddUI : MonoBehaviour
{
    public Group group;
    public GameObject A;
    public GameObject B;
    public GameObject C;
    void Start()
    {
        gameObject.SetActive(false);
    }
    public void Active(Group group){
        this.group = group;
        ValueButton aValue = A.GetComponent<ValueButton>();
        ValueButton bValue = B.GetComponent<ValueButton>();
        ValueButton cValue = C.GetComponent<ValueButton>();
        aValue.SetValueRandom();
        aValue.CheckCost();
        bValue.SetValueRandom();
        bValue.CheckCost();
        cValue.SetValueRandom();
        cValue.CheckCost();
        gameObject.SetActive(true);
    }
    public void DeActive(){
        gameObject.SetActive(false);
    }
}
