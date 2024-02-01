using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BloodPackUITest : MonoBehaviour
{
    public string sex;
    public string rh;
    public string bloodType;

    public GameObject selectButton;
    public TextMeshProUGUI countText;

    List<string> sexString = new() {"Male", "Female"};
    List<string> rhString = new() {"+", "-"};
    List<string> bloodString = new() {"A", "B", "AB", "O"};

    public List<ChangeSelected> togles;
    Dictionary<string, int> counts;
    BloodPacks bloodPacks;

    void Start(){
        bloodPacks = GameManager.Instance.bloodPackList;
        bloodPacks.UpdateCategory();

        counts = new(){
            {"Male", bloodPacks.categoryNum["Male"]},
            {"Famale", bloodPacks.categoryNum["Male"]},
            {"+", bloodPacks.categoryNum["Male"]},
            {"-", bloodPacks.categoryNum["Male"]},
            {"A", bloodPacks.categoryNum["Male"]},
            {"B", bloodPacks.categoryNum["Male"]},
            {"AB", bloodPacks.categoryNum["Male"]},
            {"O", bloodPacks.categoryNum["Male"]}
        };
        
        sex = "";
        rh = "";
        bloodType = "";

        GetTotalCount();

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

    public void ChangeCount(){
        CollectTogleInfo();
        GetTotalCount();
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

    public void GetTotalCount(){
        int count = 0;
        int flag = 0;

        countText.text = count.ToString();
    }
}
