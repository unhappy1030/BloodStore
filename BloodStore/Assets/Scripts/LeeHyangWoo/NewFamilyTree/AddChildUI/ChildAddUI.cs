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
    public void Active(Group group){
        this.group = group;
        gameObject.SetActive(true);
    }
    public void DeActive(){
        gameObject.SetActive(false);
    }
}
