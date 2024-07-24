using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultBloodPack : MonoBehaviour
{
    void Start()
    {
        int i = 0;
        Dictionary<string, int> after, gap;
        after = GameManager.Instance.bloodPackList.after;
        gap = GameManager.Instance.bloodPackList.gap;
        string[] sexes = {"Male", "Female"};
        string[] bloodTypes = {"A", "B", "AB", "O"};
        string[] rhs = {"+", "-"};
        foreach(string sex in sexes){
            foreach(string bloodType in bloodTypes){
                foreach(string rh in rhs){
                    string key = sex+bloodType+rh;
                    if(after.ContainsKey(key)){
                        GameObject TMPObj = transform.GetChild(i).gameObject;
                        TextMeshProUGUI TMP = TMPObj.GetComponent<TextMeshProUGUI>();
                        TMP.text = sex + "/" + bloodType + "(" + rh + ")" + " : " + after[key].ToString() + " (+" + gap[key].ToString() + ")";
                        i++;
                    }
                }
            }
        }
    }
}
