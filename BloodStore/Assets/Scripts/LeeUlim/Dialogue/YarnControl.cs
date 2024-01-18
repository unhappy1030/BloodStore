using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class YarnControl : MonoBehaviour
{
    public DialogueRunner dialogueRunner;
    public InMemoryVariableStorage variableStorage;
    public MoneyControl moneyControl;

    public int targetIndex;

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

    [YarnCommand("GetTargetIndex")]
    public void GetTargetIndex(){
        variableStorage.TryGetValue("$cameraTarget", out targetIndex);
    }
}
