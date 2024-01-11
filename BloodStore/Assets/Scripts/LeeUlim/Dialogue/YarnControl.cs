using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class YarnControl : MonoBehaviour
{
    DialogueRunner dialogueRunner;
    InMemoryVariableStorage variableStorage;
    MoneyControl moneyControl;

    void Start(){
        dialogueRunner = GameManager.Instance.dialogueRunner;
        variableStorage = GameManager.Instance.variableStorage;
        moneyControl = GameManager.Instance.moneyControl;
    }

    [YarnFunction("UpdateMoney")]
    public static float UpdateMoney(){
        float money = GameManager.Instance.money;        
        return money;
    }


    [YarnCommand("CalculateMoney")]
    public void CalculateMoney(float amount){
        float money = moneyControl.CalculateMoney(amount);
        variableStorage.SetValue("$money", money);
    }
}
