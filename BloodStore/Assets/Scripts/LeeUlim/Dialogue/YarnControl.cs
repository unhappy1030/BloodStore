using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class YarnControl : MonoBehaviour
{
    public DialogueRunner dialogueRunner;
    public InMemoryVariableStorage variableStorage;
    public MoneyControl moneyControl;
    public DialogueControl dialogueControl;

    public int targetIndex;
    public string nodeName;
    public static float sellInfo = 0;
    public bool isSell;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        targetIndex = 0;
        isSell = false;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
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

    [YarnCommand("SetCondition")]
    public void SetCondition(string npcName, int condition){
        dialogueControl.SetCondition(npcName, condition);
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

    [YarnFunction("GetBloodTaste")]
    public static string GetBloodTaste(){
        string taste = "";
        taste = NPCInteract.tasteStr;
        return taste;
    }

    // // must use
    // [YarnFunction("GetSellInfo")]
    // public static float GetSellInfo(){
    //     return sellInfo;
    // }

}
