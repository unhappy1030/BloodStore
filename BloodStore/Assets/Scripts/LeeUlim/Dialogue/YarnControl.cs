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

}
