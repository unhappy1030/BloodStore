using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackingUIButtons : MonoBehaviour
{
    public GameObject packingUI;
    public void Confirm(){
        PackingUI UI = packingUI.GetComponent<PackingUI>();
        UI.PackingConfirm();
        ButtonControl buttonControl = this.GetComponent<ButtonControl>();
        buttonControl.DayIncrease();
        buttonControl.SceneLoad("ResultFamilyTree");
    }
    public void CountUp(){
        PackingUI UI = packingUI.GetComponent<PackingUI>();
        UI.SetCountUp();
    }
    public void CountDown(){
        PackingUI UI = packingUI.GetComponent<PackingUI>();
        UI.SetCountDown();
    }
}
