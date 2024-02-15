using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextDay : MonoBehaviour
{
    public GameObject packingUI;

    public void SetPackingUI(){
        PackingUI UI = packingUI.GetComponent<PackingUI>();
        UI.SetFirstValue();
    }
}
