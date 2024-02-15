using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOnButton : MonoBehaviour
{
    public GameObject UI;
    public void OnUI(){
        UI.SetActive(true);
    }
}
