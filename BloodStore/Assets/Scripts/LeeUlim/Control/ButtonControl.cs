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

    public void SceneLoad(string sceneName)
    {
        GameManager.Instance.StartCoroutine(GameManager.Instance.FadeOutAndLoadScene(sceneName, 0.05f));
    }


    // ---< AlwaysOnCanvas >---

    public void GotoPause()
    {
        uiControl.status = UIControl.AlwaysOnUIStatus.Pause;
        uiControl.ControlAlwaysOnCanvasUI();
    }

    public void GoToContinue()
    {
        uiControl.status = UIControl.AlwaysOnUIStatus.PauseButtonOn;
        uiControl.ControlAlwaysOnCanvasUI();
    }

    
    public void GoToSetting()
    {
        uiControl.status = UIControl.AlwaysOnUIStatus.Setting;
        uiControl.ControlAlwaysOnCanvasUI();
    }

}
