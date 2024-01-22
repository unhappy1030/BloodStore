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

    public void GetDialogues(){
        foreach(DialogueFrame dialogueInfo in dialogues){
            
        }
    }

    public int GetDayCount(int day){
        int count = 0;
        
        if(dialogues != null){
            foreach(DialogueFrame dialogueInfo in dialogues){
                if(dialogueInfo.isDay && dialogueInfo.num == day){
                    count++;
                }
            }
        }

        return count;
    }

    public int GetConditionCount(int condition){
        int count = 0;

        if(dialogues != null){
            foreach(DialogueFrame dialogueInfo in dialogues){
                if(!dialogueInfo.isDay && dialogueInfo.num == condition){
                    count++;
                }
            }
        }

        return count;
    }
}

[Serializable]
public class DialogueFrame{
    public WhereNodeStart where;
    public WhenNodeStart when;
    
    public bool isDay;
    [Tooltip("If isDay is true, num means day, or condition.")]
    public int num;
    public int priority;
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

