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
            "Albino",
            "None"
        };
}

[CreateAssetMenu(fileName = "SynergySO", menuName = "Scriptable Object/SynergySO")]
public class SynergySO : ScriptableObject
{
    public List<int> synergyList;
    public List<int> synergyEffect;
    private void OnEnable() {
        synergyList = new List<int>{0, 0, 0, 0, 0, 0, 0};
        synergyEffect = new List<int>{0, 0, 0, 0, 0, 0, 0};
    }
    public void SetSynergyList(TreePair root){
        if(!root.pair.male.empty){
            synergyList[root.pair.male.synergyCode]++;
        }
        if(!root.pair.female.empty){
            synergyList[root.pair.female.synergyCode]++;
        }
        if(root.pair.childNum != 0){
            foreach(TreePair now in root.children){
                SetSynergyList(now);
            }
        }
    }
    public void SetSynergyList(List<SerializePair> serializePairList){
        synergyList = new List<int>{0, 0, 0, 0, 0, 0, 0};
        foreach(SerializePair serializePair in serializePairList){
            if(!serializePair.male.empty){
                synergyList[serializePair.male.synergyCode]++;
            }
            if(!serializePair.female.empty){
                synergyList[serializePair.female.synergyCode]++;
            }
        }
    }
    public void SetSynergyEffect(){
        synergyEffect = new List<int>{0, 0, 0, 0, 0, 0, 0};
        if(synergyList[5] >= 20){
            synergyEffect[5] = 10;
        }
        else if(synergyEffect[5] >= 10){
            synergyEffect[5] = 5;
        }
        for(int i = 0; i < synergyList.Count - 2; i++){
            if(synergyList[i] + synergyEffect[5] >= 20){
                synergyEffect[i] = 3;
            }
            else if(synergyList[i] + synergyEffect[5] >= 10){
                synergyEffect[i] = 2;
            }
            else if(synergyList[i] + synergyEffect[5] >= 5){
                synergyEffect[i] = 1;
            }
        }
    }
}
