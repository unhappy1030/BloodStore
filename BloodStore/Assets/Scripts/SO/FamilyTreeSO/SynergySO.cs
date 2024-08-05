using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SynergyName{
    public static readonly List<string> names  = new List<string>{
            "Business",
            "Healthiness",
            "Weakness",
            "Doctor",
            "Religion",
            "Albino"
        };
}

[System.Serializable]
public class Synergy{
    public Dictionary<string, int> synergyDict;
    public Synergy(){
        synergyDict = new();
        foreach(string syn in SynergyName.names){
            synergyDict.Add(syn, 0);
        }
    }
}


[CreateAssetMenu(fileName = "SynergySO", menuName = "Scriptable Object/SynergySO")]
public class SynergySO : ScriptableObject
{
    public Synergy synergy = new();
}
