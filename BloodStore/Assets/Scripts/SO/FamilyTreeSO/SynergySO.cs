using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Synergy{
    public List<string> synergyName;
    public Dictionary<string, int> synergyDict;
    private readonly float[] weights = { 0.2f, 0.2f, 0.2f, 0.1f, 0.1f, 0.05f };
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
    public int GetRandomSynergyCode()
    {
        float totalWeight = 0f;

        foreach (float weight in weights)
        {
            totalWeight += weight;
        }
        float randomValue = Random.Range(0, totalWeight);

        // 랜덤 값에 따른 숫자 선택
        float cumulativeWeight = 0f;
        for (int i = 0; i < weights.Length; i++)
        {
            cumulativeWeight += weights[i];
            if (randomValue < cumulativeWeight)
            {
                return i;
            }
        }
        return weights.Length - 1;
    }
}


[CreateAssetMenu(fileName = "SynergySO", menuName = "Scriptable Object/SynergySO")]
public class SynergySO : ScriptableObject
{
    public Synergy synergy = new();
}
