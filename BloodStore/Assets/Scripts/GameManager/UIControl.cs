using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    public GameObject alwaysOnCanvas;
    public GameObject fadeInOutCanvas;
    public GameObject pauseButton;
    public GameObject pausePanel;
    public GameObject settingPanel;
    public GameObject tutorialCheckUI;

    public TutorialControl tutorialControl; // assign at inspector

    public static bool isPause;

    public enum AlwaysOnUIStatus
    {
        Nothing,
        PauseButtonOn,
        Pause,
        Setting
    }

    public AlwaysOnUIStatus status = AlwaysOnUIStatus.Nothing;

    private void Awake()
    {
        isPause = false;

        pauseButton.SetActive(false);
        pausePanel.SetActive(false);
        settingPanel.SetActive(false);
        tutorialCheckUI.SetActive(false);
        alwaysOnCanvas.SetActive(true);
        fadeInOutCanvas.SetActive(true);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == GameManager.Instance.startSceneName || SceneManager.GetActiveScene().name == "StartMenu")
        {
            status = AlwaysOnUIStatus.Nothing;
            ControlAlwaysOnCanvasUI();
        }
        else
        {
            status = AlwaysOnUIStatus.PauseButtonOn;
            ControlAlwaysOnCanvasUI();
        }
    }

    void OnSceneUnloaded(Scene currentScene)
    {

    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && status != AlwaysOnUIStatus.Nothing)
        {
            ChangeAlwaysOnCanvasUIStatusEnum();
            ControlAlwaysOnCanvasUI();
        }   
    }

    void ChangeAlwaysOnCanvasUIStatusEnum()
    {
        switch (status)
        {
            case AlwaysOnUIStatus.Setting:
            case AlwaysOnUIStatus.PauseButtonOn:
                status = AlwaysOnUIStatus.Pause;
                break;

            case AlwaysOnUIStatus.Pause:
                status = AlwaysOnUIStatus.PauseButtonOn;
                break;
        }
    }

    // [AlwaysOnCanvas]
    public void ControlAlwaysOnCanvasUI()
    {
        switch (status)
        {
            case AlwaysOnUIStatus.Nothing:
                EscapePasue();
                pauseButton.SetActive(false);
                pausePanel.SetActive(false);
                settingPanel.SetActive(false);
                break;

            case AlwaysOnUIStatus.PauseButtonOn:
                EscapePasue();
                pauseButton.SetActive(true);
                pausePanel.SetActive(false);
                settingPanel.SetActive(false);
                break;

            case AlwaysOnUIStatus.Pause:
                Pause();
                pauseButton.SetActive(false);
                pausePanel.SetActive(true);
                settingPanel.SetActive(false);
                break;

            case AlwaysOnUIStatus.Setting:
                pauseButton.SetActive(false);
                pausePanel.SetActive(false);
                settingPanel.SetActive(true);
                break;
        }

    }

    // public void ActiveTutorialCheckUI(bool isActive){
    //     tutorialCheckUI.SetActive(isActive);
    // }
    
    void Pause(){
        Time.timeScale = 0f;
        isPause = true;
    }

    void EscapePasue(){
        Time.timeScale = 1f;
        isPause = false;
    } 

    // button
    public void GotoStart(string sceneName){
        if((GameManager.Instance.isTurotial && tutorialControl.isAllTutorialFinish) || !GameManager.Instance.isTurotial)
        {
            GameManager.Instance.StartCoroutine(GameManager.Instance.FadeOutAndLoadScene(sceneName, 1f));
        }
        else
        {
            tutorialCheckUI.SetActive(true);
        }
    }

    // button
    public void GotoStartAfterTutorialNotice(string sceneName){
        tutorialControl.ResetTutorialStatus(true);
        GameManager.Instance.isTurotial = false;
        tutorialCheckUI.SetActive(false);
        GameManager.Instance.StartCoroutine(GameManager.Instance.FadeOutAndLoadScene(sceneName, 1f));
    }

    public void GotoPause()
    {
        status = UIControl.AlwaysOnUIStatus.Pause;
        ControlAlwaysOnCanvasUI();
    }

    public void GoToContinue()
    {
        status = UIControl.AlwaysOnUIStatus.PauseButtonOn;
        ControlAlwaysOnCanvasUI();
    }

    
    public void GoToSetting()
    {
        status = UIControl.AlwaysOnUIStatus.Setting;
        ControlAlwaysOnCanvasUI();
    }

}
