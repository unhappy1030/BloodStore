using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadBloodPack : MonoBehaviour
{
    void Start(){
        int i = 0;
        Dictionary<string, int> data;
        GameManager.Instance.bloodPackList.Deserialize();
        data = GameManager.Instance.bloodPackList.ShowAllDic();
        string[] sexes = {"Male", "Female"};
        string[] bloodTypes = {"A", "B", "AB", "O"};
        string[] rhs = {"+", "-"};
        foreach(string sex in sexes){
            foreach(string bloodType in bloodTypes){
                foreach(string rh in rhs){
                    string key = sex+bloodType+rh;
                    if(data.ContainsKey(key)){
                        GameObject TMPObj = transform.GetChild(i).gameObject;
                        TextMeshProUGUI TMP = TMPObj.GetComponent<TextMeshProUGUI>();
                        TMP.text = sex + "/" + bloodType + "(" + rh + ")" + " : " + data[key].ToString();
                        i++;
                    }
                }
            }
        }
    }
}