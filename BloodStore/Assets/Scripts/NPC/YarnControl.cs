using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yarn.Unity;

public class YarnControl : MonoBehaviour
{
    public DialogueRunner dialogueRunner;
    public InMemoryVariableStorage variableStorage;

    public Image lineViewUIImg;

    public MoneyControl moneyControl;
    public DialogueControl dialogueControl;
    public CameraControl cameraControl;

    public int targetIndex;
    public string nodeName;
    public bool isSell;
    public static float sellInfo = 0;
    public static float sellPrice = 0;
    // public static bool isSelect;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        targetIndex = 0;
        isSell = false;
        lineViewUIImg.gameObject.SetActive(false);
        lineViewUIImg.sprite = null;
    }

    private void OnSceneUnloaded(Scene scene){
        lineViewUIImg.gameObject.SetActive(false);
        lineViewUIImg.sprite = null;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    // add SetActive false event in 'On Node Complete' of Dialogue Runner
    [YarnCommand("ChangeUIImg")]
    public void ChangeUIImg(int index){
        Sprite sprite = dialogueControl.GetSpriteForDialogueView(index);
        lineViewUIImg.sprite = sprite;
        lineViewUIImg.gameObject.SetActive(true);
    }

    [YarnFunction("UpdateMoney")]
    public static float UpdateMoney(){
        float money = GameManager.Instance.money;        
        return money;
    }

    [YarnFunction("GetSellPrice")]
    public static float GetSellPrice(){
        return sellPrice;
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
    // [YarnCommand("SetCamTargetIndex")]
    // public void SetCamTargetIndex(int _targetIndex){
    //     targetIndex = _targetIndex;
    // }

    // must use
    [YarnCommand("SetNodeName")]
    public void SetNodeName(string _nodeName){
        nodeName = _nodeName;
    }

    // only for sell
    [YarnCommand("SetIsSell")]
    public void SetIsSell(bool _isTrue){
        isSell = _isTrue;
    }

    [YarnFunction("GetBloodTaste")]
    public static string GetBloodTaste(){
        string taste = "";
        taste = NPCInteract.tasteStr;

        if(taste == null){
            taste = "Null...";
        }

        return taste;
    }

    // [YarnCommand("WaitUntilSell")]
    // public static IEnumerator WaitUntilSell(){
    //     isSelect = false;
    //     yield return new WaitUntil(() => isSelect);
    //     isSelect = false;
    // }

    
    [YarnCommand("WaitForFadeIn")]
    public static IEnumerator WaitForFadeIn(){
        yield return GameManager.Instance.FadeInUI(GameManager.Instance.blackPanel, 0.5f);
    }
    
    
    [YarnCommand("WaitForFadeOut")]
    public static IEnumerator WaitForFadeOut(){
        yield return GameManager.Instance.FadeOutUI(GameManager.Instance.blackPanel, 0.7f);
    }

    [YarnCommand("WaitUntilVirtualCamBlend_EaseInOut")]
    public static IEnumerator WaitUntilVirtualCamBlend_EaseInOut(int _targetIndex){
        GameManager.Instance.CreateVirtualCamera(CameraControl.targetsForYarn[_targetIndex], false, null, 5.4f, 0.25f, Cinemachine.CinemachineBlendDefinition.Style.EaseInOut, 1f);
        yield return new WaitUntil(() => CameraControl.isFinish);
    }

    [YarnCommand("VirtualCamBlend_Cut")]
    public void VirtualCamBlend_Cut(int _targetIndex){
        GameManager.Instance.CreateVirtualCamera(CameraControl.targetsForYarn[_targetIndex], false, null, 5.4f, 0f, Cinemachine.CinemachineBlendDefinition.Style.Cut, 0f);
    }
    

    // button
    // public void ChangeIsSelect(){
    //     isSelect = true;
    // }

    // must use
    [YarnFunction("GetSellInfo")]
    public static float GetSellInfo(){
        return sellInfo;
    }

}
