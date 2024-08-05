using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Synergy{
    public List<string> synergyName;
    public Dictionary<string, int> synergyDict;
    public Synergy(){
        synergyName = new List<string>{
            "Business",
            "Healthiness",
            "Weakness",
            "Doctor",
            "Religion",
            "Albino"
        };
        synergyDict = new();
        foreach(string syn in synergyName){
            synergyDict.Add(syn, 0);
        }
    }
}


[CreateAssetMenu(fileName = "SynergySO", menuName = "Scriptable Object/SynergySO")]
public class SynergySO : ScriptableObject
{
    public Synergy synergy = new();
}
