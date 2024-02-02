using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodPackUITest : MonoBehaviour
{
    public string sex;
    public string rh;
    public string bloodType;

    public GameObject selectButton;

    List<string> sexString = new() {"Male", "Female"};
    List<string> rhString = new() {"+", "-"};
    List<string> bloodString = new() {"A", "B", "AB", "O"};

    public List<ChangeSelected> togles;

    void Start(){
        selectButton.SetActive(false);
    }

    private void Update()
    {
        if(!togles[0].notSelected || !togles[1].notSelected || !togles[2].notSelected){
            selectButton.SetActive(true);
        }else{
            selectButton.SetActive(false);
        }
    }

    public void CollectTogleInfo(){
        if(togles[0].notSelected)
        {
            sex = "";
        }
        else
        {
            sex = sexString[togles[0].index];
        }

        if(togles[1].notSelected)
        {
            rh = "";
        }
        else
        {
            rh = rhString[togles[1].index];
        }

        if(togles[2].notSelected)
        {
            bloodType = "";
        }
        else
        {
            bloodType = bloodString[togles[2].index];
        }

        // Debug.Log(sex + " "+ rh + " " + bloodType);
    }
}
