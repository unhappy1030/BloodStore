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

    // ---< 씬 로드 & 종료 시 수행 >---
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    // 씬 로드
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 시작 화면에서는 상호작용x
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

    // 씬 종료
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
        // esc 입력
        if (Input.GetKeyDown(KeyCode.Escape) && status != AlwaysOnUIStatus.Nothing)
        {
            ChangeUIStatusEnum();
            ControlAlwaysOnCanvasUI();
        }
    }

    // esc 누른 경우에 [status 값 바뀜] -> 그 외의 경우는 직접 값을 조정
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

    // ---< 버튼 상호작용 UI >---

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
