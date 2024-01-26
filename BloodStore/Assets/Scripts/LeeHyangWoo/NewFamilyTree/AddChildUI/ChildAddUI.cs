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
        A.GetComponent<ValueButton>().SetValueRandom();
        B.GetComponent<ValueButton>().SetValueRandom();
        C.GetComponent<ValueButton>().SetValueRandom();
        gameObject.SetActive(true);
    }
    public void DeActive(){
        gameObject.SetActive(false);
    }
}
