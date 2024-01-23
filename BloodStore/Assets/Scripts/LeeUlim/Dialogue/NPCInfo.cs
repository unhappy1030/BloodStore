using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using System.Threading;

[CreateAssetMenu(fileName ="NPCInfo", menuName = "Scriptable Object/NPCInfo")]
public class NPCInfo : ScriptableObject
{
    public string npcName;
    [Tooltip("0 : Normal / 1 : Happy / 2 : Sad / 3 : Angry")]
    public List<Sprite> sprites;
    public int startDay;
    public List<DayDialogue> dayDialogues;
    public List<CondDialogue> condDialogues;

    public bool AbleNPC(int day){
        if(startDay > day){
            return false;
        }
        return true;
    }

    public List<DayDialogue> GetDayDialogues(WhereNodeStart where, WhenNodeStart when, int day){
        List<DayDialogue> list = new();
        
        int index = 0;
        foreach(DayDialogue dialogue in dayDialogues){
            if(dialogue.where == where && dialogue.when == when){
                if(dialogue.day == day){
                    list.Add(dialogue);
                    index++;
                }
            }
        }

        return list;
    }

    public List<CondDialogue> GetCondDialogues(WhereNodeStart where, WhenNodeStart when, int condition, int day){
        List<CondDialogue> list = new();
        
        int index = 0;
        foreach(CondDialogue dialogue in condDialogues){
            if(dialogue.where == where && dialogue.when == when){
                if(dialogue.condition == condition 
                    && day >= dialogue.waitUntil 
                    && day <= dialogue.deadline)
                {
                    list.Add(dialogue);
                    index++;
                }
            }
        }

        return list;
    }
}

[Serializable]
public class DayDialogue{
    public WhereNodeStart where;
    public WhenNodeStart when;
    
    public int day;
    public int priority;

    public string dialogueName;
}


[Serializable]
public class CondDialogue{
    public WhereNodeStart where;
    public WhenNodeStart when;
    
    public int condition;
    public int waitUntil;
    public int deadline;
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

