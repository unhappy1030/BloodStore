using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class YarnControl : MonoBehaviour
{
    DialogueRunner dialogueRunner;

    void Start(){
        dialogueRunner = GameManager.Instance.dialogueRunner;
    }

    public void StartDialogue(InteractObjInfo interactObjInfo){
        string nodeName = interactObjInfo._nodeName;
        
        if(!dialogueRunner.IsDialogueRunning){
            if(dialogueRunner.NodeExists(nodeName))
                dialogueRunner.StartDialogue(nodeName);
            else
                Debug.Log(nodeName + " is not Exist...");
        }else
            Debug.Log("Other Dialogue is running...");
    }
}
