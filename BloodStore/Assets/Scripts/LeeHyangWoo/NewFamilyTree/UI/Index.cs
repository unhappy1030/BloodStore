using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Index : MonoBehaviour
{
    private int index;
    public void SetIndex(int index){
        this.index = index;
    }
    public int GetIndex(){
        return index;
    }
}
