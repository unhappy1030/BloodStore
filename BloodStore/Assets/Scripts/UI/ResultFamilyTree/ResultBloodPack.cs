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
        foreach(string key in after.Keys){
            GameObject TMPObj = transform.GetChild(i).gameObject;
            TextMeshProUGUI TMP = TMPObj.GetComponent<TextMeshProUGUI>();
            TMP.text = key + " : " + after[key].ToString() + " (+" + gap[key].ToString() + ")";
            i++;
        }
    }

}
