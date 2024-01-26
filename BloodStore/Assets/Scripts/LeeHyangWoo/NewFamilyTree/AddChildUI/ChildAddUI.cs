using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildAddUI : MonoBehaviour
{
    public Group group;
    void Start()
    {
        gameObject.SetActive(false);
    }
    public void Active(){
        gameObject.SetActive(true);
    }
    public void DeActive(){
        gameObject.SetActive(false);
    }
    public void SetGroup(Group group){
        this.group = group;
    }
}
