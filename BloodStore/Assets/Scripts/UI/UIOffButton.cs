using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactiveButton : MonoBehaviour
{
    public GameObject UI;
    public void OffUI(){
        UI.SetActive(false);
    }
}
