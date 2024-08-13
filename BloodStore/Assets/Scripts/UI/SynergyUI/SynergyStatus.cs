using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SynergyStatus : MonoBehaviour
{
    public List<TextMeshProUGUI> SynergyTextList;
    public SynergySO synergySO;
    void Start()
    {
        UpdateText();
    }
    public void UpdateText(){
        Debug.Log("SynergyTextList Size : " + SynergyTextList.Count);
        Debug.Log("SynergyList Size : " + synergySO.synergyList.Count);
        for(int i = 0; i < 6; i++){
            SynergyTextList[i].text = synergySO.synergyList[i].ToString();
        }
    }
}
