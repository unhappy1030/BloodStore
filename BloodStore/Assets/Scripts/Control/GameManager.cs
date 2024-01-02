using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public string[] sceneNamesInBuild; // **** Inspector 창에서 써 놓은 씬 이름

    public string startSceneName = "Start"; // 여기서 설정한 이름의 씬을 시작화면 씬으로 인식함

    bool wasFade = false;

    public Image whitePanel;
    public Image blackPanel;


    // ---< 싱글톤 >---
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
        // ---< 싱글톤 >---
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

    // ---< 씬 로드 & 종료 시 수행 >---
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    // 씬 로드
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (wasFade)
            StartCoroutine(FadeIn(blackPanel, 0.01f));
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


    // [씬 로드]
    public void SceneLoad(string sceneName)
    {
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.Log(sceneName + " 씬을 불러옵니다...");
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.Log("없는 씬입니다...");
        }
    }


    // [fade out 후 씬 로드]
    public IEnumerator FadeOutAndLoadScene(string sceneName, float speed)
    {
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.Log(sceneName + " 씬을 불러옵니다...");
            yield return StartCoroutine(FadeOut(blackPanel, speed));
            wasFade = true;
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.Log("없는 씬입니다...");
        }
    }


    // ---< 코루틴 >---
    // fadeSpeed는 0.005f ~ 0.05f 사이의 값이 적절

    // [fade out] -> 드러남
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

    // [fade in] -> 사라짐
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
