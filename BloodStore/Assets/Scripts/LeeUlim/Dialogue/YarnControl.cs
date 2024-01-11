using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class YarnControl : MonoBehaviour
{
    DialogueRunner dialogueRunner;
    InMemoryVariableStorage variableStorage;

    void Start(){
        dialogueRunner = GameManager.Instance.dialogueRunner;
        variableStorage = GameManager.Instance.variableStorage;
    }
}
