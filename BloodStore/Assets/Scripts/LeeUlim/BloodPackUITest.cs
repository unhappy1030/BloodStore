using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodPackUITest : MonoBehaviour
{
    public string sex;
    public string[] bloodType;

    public List<ChangeSelected> togles;

    void Start(){
        togles = new(GetComponentsInChildren<ChangeSelected>());
    }

    public void CollectTogleInfo(){
        foreach(ChangeSelected change in togles){
            ;
        }
    }
}
