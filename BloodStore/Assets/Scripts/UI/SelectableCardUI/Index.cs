using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Index : MonoBehaviour
{
    private int index;
    public bool buttonCheck;
    public void SetIndex(int index){
        this.index = index;
        buttonCheck = true;
    }
    public int GetIndex(){
        return index;
    }
}
