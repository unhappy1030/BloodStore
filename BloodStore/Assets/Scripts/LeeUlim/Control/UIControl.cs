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
        pauseButton.SetActive(false);
        pausePanel.SetActive(false);
        settingPanel.SetActive(false);
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
        if (SceneManager.GetActiveScene().name == GameManager.Instance.startSceneName)
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
            ChangeUIStatusEnum();
            ControlAlwaysOnCanvasUI();
        }
    }

    void ChangeUIStatusEnum()
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
                pauseButton.SetActive(false);
                pausePanel.SetActive(false);
                settingPanel.SetActive(false);
                break;

            case AlwaysOnUIStatus.PauseButtonOn:
                pauseButton.SetActive(true);
                pausePanel.SetActive(false);
                settingPanel.SetActive(false);
                break;

            case AlwaysOnUIStatus.Pause:
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
}
