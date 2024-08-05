using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SynergySO", menuName = "Scriptable Object/SynergySO")]
public class SynergySO : ScriptableObject
{
    public List<string> synergy;
    public Dictionary<string, int> data;
    public void OnEnable(){
        synergy = new List<string>{
            "Business",
            "Religion",
            "Healthiness",
            "Weakness",
            "Doctor",
            "Albino"
        };
        data = new();
        foreach(string syn in synergy){
            data.Add(syn, 0);
        }
    }
}
