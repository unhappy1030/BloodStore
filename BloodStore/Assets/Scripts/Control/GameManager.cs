using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public string[] sceneNamesInBuild; // **** Inspector â���� �� ���� �� �̸�

    public string startSceneName = "Start"; // ���⼭ ������ �̸��� ���� ����ȭ�� ������ �ν���

    bool wasFade = false;

    public Image whitePanel;
    public Image blackPanel;


    // ---< �̱��� >---
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (_instance == null)
                    Debug.Log("no singleton obj");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        // ---< �̱��� >---
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // ---< �� �ε� & ���� �� ���� >---
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    // �� �ε�
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (wasFade)
            StartCoroutine(FadeIn(blackPanel, 0.01f));
    }

    // �� ����
    void OnSceneUnloaded(Scene currentScene)
    {

    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }


    // [�� �ε�]
    public void SceneLoad(string sceneName)
    {
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.Log(sceneName + " ���� �ҷ��ɴϴ�...");
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.Log("���� ���Դϴ�...");
        }
    }


    // [fade out �� �� �ε�]
    public IEnumerator FadeOutAndLoadScene(string sceneName, float speed)
    {
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.Log(sceneName + " ���� �ҷ��ɴϴ�...");
            yield return StartCoroutine(FadeOut(blackPanel, speed));
            wasFade = true;
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.Log("���� ���Դϴ�...");
        }
    }


    // ---< �ڷ�ƾ >---
    // fadeSpeed�� 0.005f ~ 0.05f ������ ���� ����

    // [fade out] -> �巯��
    public IEnumerator FadeOut(Image _Image, float _fadeSpeed)
    {
        Debug.Log("Fade out...");
        _Image.gameObject.SetActive(true);
        Color t_color = _Image.color;
        t_color.a = 0;

        while (t_color.a < 1)
        {
            t_color.a += _fadeSpeed;
            _Image.color = t_color;
            yield return null;
        }

        wasFade = true;
    }

    // [fade in] -> �����
    public IEnumerator FadeIn(Image _Image, float _fadeSpeed)
    {
        Debug.Log("Fade in...");
        Color t_color = _Image.color;
        t_color.a = 1;

        while (t_color.a > 0)
        {
            t_color.a -= _fadeSpeed;
            _Image.color = t_color;
            yield return null;
        }

        _Image.gameObject.SetActive(false);

        wasFade = false;
    }

}
