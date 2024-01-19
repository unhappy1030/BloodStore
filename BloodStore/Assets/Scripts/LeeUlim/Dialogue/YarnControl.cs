using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Yarn.Unity;

public class YarnControl : MonoBehaviour
{
    public DialogueRunner dialogueRunner;
    public InMemoryVariableStorage variableStorage;
    public MoneyControl moneyControl;

    public int targetIndex;
    public string nodeName;
    public static float sellInfo = 0;
    public bool isSell = false;

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

    // ---< Sell blood >---

    // must use
    [YarnCommand("SellBlood")]
    public void SellBlood(bool _isSell){
        isSell = _isSell;
    }

    // must use
    [YarnCommand("SetCamTargetIndex")]
    public void SetCamTargetIndex(int _targetIndex){
        targetIndex = _targetIndex;
    }

    // must use
    [YarnCommand("SetNodeName")]
    public void SetNodeName(string _nodeName){
        nodeName = _nodeName;
    }

    // // must use
    // [YarnFunction("GetSellInfo")]
    // public static float GetSellInfo(){
    //     return sellInfo;
    // }

}
