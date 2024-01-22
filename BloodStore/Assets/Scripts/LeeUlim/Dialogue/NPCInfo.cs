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
    public List<DialogueFrame> dialogues;

    public bool ableNPC(int day){
        if(startDay > day){
            return false;
        }
        return true;
    }

    public List<DialogueFrame> GetAllDialogues(WhereNodeStart where, WhenNodeStart when, bool isDay, int num){
        List<DialogueFrame> list = new();
        
        int index = 0;
        foreach(DialogueFrame dialogue in dialogues){
            if(dialogue.where == where && dialogue.when == when){
                if(dialogue.isDay == isDay && dialogue.num == num){
                    list.Add(new());
                    list[index] = dialogue;
                    list[index].npcName = npcName; // assign npc Name here
                }
            }
        }

        return list;
    }
    
    public int GetDialoguesCount(WhereNodeStart where, WhenNodeStart when, bool isDay, int num){
        int count = 0;
        
        if(dialogues != null){
            foreach(DialogueFrame dialogue in dialogues){
                if(dialogue.where == where && dialogue.when == when){
                    if(dialogue.isDay == isDay && dialogue.num == num){
                        count++;
                    }
                }
            }
        }

        return count;
    }

}

[Serializable]
public class DialogueFrame{
    [HideInInspector] public string npcName; // not assign at inspector
    public WhereNodeStart where;
    public WhenNodeStart when;
    
    public bool isDay;
    [Tooltip("If isDay is true, num means day, or condition.")]
    public int num;
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

