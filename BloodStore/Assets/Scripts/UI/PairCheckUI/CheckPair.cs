using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPair : MonoBehaviour
{
    public EmptyDisplay emptyDisplay;
    public void ConfirmCheck(EmptyDisplay emptyDisplay){
        this.emptyDisplay = emptyDisplay;
        gameObject.SetActive(true);
    }
}
