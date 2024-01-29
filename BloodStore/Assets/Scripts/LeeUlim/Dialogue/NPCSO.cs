using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="NPCSO", menuName = "Scriptable Object/NPCSO")]
public class NPCSO : ScriptableObject
{
    public string npcName;
    public List<Sprite> sprites;
    public int startDay;

    public List<ConditionInfo> conditionInfos;

    public bool AbleNPC(int day){
        if(startDay > day){
            return false;
        }
        
        return true;
    }

    public List<Dialogue> GetDialogues(WhereNodeStart where, WhenNodeStart when, int day, int condition){
        List<Dialogue> list = new();

        int index = 0;
        foreach(ConditionInfo conditionInfo in conditionInfos){
            if(conditionInfo.condition != condition){
                continue;
            }

            foreach(Dialogue dialogue in conditionInfo.dialogues){
                if(dialogue.where == where && dialogue.when == when){
                    if(dialogue.day == day){
                        list.Add(dialogue);
                        index++;
                    }
                }
            }
        }

        return list;
    }
}

[Serializable]
public class ConditionInfo{
    public int condition;
    public int waitUntil;
    public int deadLine;
    public List<Dialogue> dialogues;
}

[Serializable]
public class Dialogue{
    public WhereNodeStart where;
    public WhenNodeStart when;

    public int day;
    public int priority;
    public string dialogueName;
}

[Serializable]
public enum WhereNodeStart{
    Store,
    Edit
}

[Serializable]
public enum WhenNodeStart{
    Click,
    SceneLoad
}
