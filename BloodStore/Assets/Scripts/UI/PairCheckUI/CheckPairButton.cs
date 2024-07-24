using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPairButton : MonoBehaviour
{
    public void ConfirmClick(){
        CheckPair checkPair = GetComponentInParent<CheckPair>();
        EmptyDisplay emptyDisplay = checkPair.emptyDisplay;
        emptyDisplay.ChangeConfirmed();
        checkPair.gameObject.SetActive(false);
    }
}
