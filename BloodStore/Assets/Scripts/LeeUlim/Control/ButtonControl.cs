using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonControl : MonoBehaviour
{
    UIControl uiControl;

    private void Awake()
    {
        uiControl = GameManager.Instance.GetComponentInChildren<UIControl>();
    }

    // << 버튼 상호작용 >>

    // [씬 이동]
    public void SceneLoad(string sceneName)
    {
        GameManager.Instance.StartCoroutine(GameManager.Instance.FadeOutAndLoadScene(sceneName, 0.05f));
    }


    // ---< AlwaysOnCanvas 버튼 상호작용 >---

    public void GotoPause()
    {
        uiControl.status = UIControl.AlwaysOnUIStatus.Pause;
        uiControl.ControlAlwaysOnCanvasUI();
        Debug.Log("일시정지...");
    }

    public void GoToContinue()
    {
        uiControl.status = UIControl.AlwaysOnUIStatus.PauseButtonOn;
        uiControl.ControlAlwaysOnCanvasUI();
        Debug.Log("게임을 계속 진행합니다...");
    }

    
    public void GoToSetting()
    {
        uiControl.status = UIControl.AlwaysOnUIStatus.Setting;
        uiControl.ControlAlwaysOnCanvasUI();
        Debug.Log("설정 화면을 띄웁니다...");
    }

}
