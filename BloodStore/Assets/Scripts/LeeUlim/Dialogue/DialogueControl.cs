using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueControl : MonoBehaviour
{
    public List<NPCInfo> npcInfos;

    void GetDialgues(WhereNodeStart where, WhenNodeStart when){
        foreach(NPCInfo npcInfo in npcInfos){
            foreach(DialogueFrame dialogue in npcInfo.dialogues){
                if(dialogue.where == where && dialogue.when == when){
                    
                }
            }
        }
    }
}
