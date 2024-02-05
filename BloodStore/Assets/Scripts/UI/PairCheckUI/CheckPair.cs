using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPair : MonoBehaviour
{
    EmptyDisplay emptyDisplay;
    public void ConfirmCheck(EmptyDisplay emptyDisplay){
        this.emptyDisplay = emptyDisplay;
        gameObject.SetActive(true);
    }
    public void ConfirmClick(){
        emptyDisplay = GetComponentInParent<CheckPair>().emptyDisplay;
        emptyDisplay.ChangeConfirmed();
        gameObject.SetActive(false);
    }
    public void BackClick(){
        gameObject.SetActive(false);
    }
}
