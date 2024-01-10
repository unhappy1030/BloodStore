using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yarn.Unity;

public class GameManager : MonoBehaviour
{
    public string[] sceneNamesInBuild; // *** Inspector

    public string startSceneName = "Start"; // set start Scene name

    bool wasFade = false;

    public Image whitePanel;
    public Image blackPanel;

    public CameraControl cameraControl; // *** warning : must be in Scene and set "CameraControl" tag
    public NPCInteract npcInteract;
    public MouseRayCast mouseRayCast;
    public DialogueRunner dialogueRunner;
    public InMemoryVariableStorage variableStorage;


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
        // ---< Singleton >---
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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // find CameraContorl object
        GameObject cameraControlObj = GameObject.FindWithTag("CameraControl");
        if(cameraControlObj != null)
            cameraControl = cameraControlObj.GetComponent<CameraControl>();
        
        else
        {
            cameraControlObj = new("CameraContorl");
            cameraControl = cameraControlObj.AddComponent<CameraControl>();
        }

        // find yarnControl
        npcInteract = FindObjectOfType<NPCInteract>(true);

        mouseRayCast = GetComponent<MouseRayCast>();

        dialogueRunner = GetComponentInChildren<DialogueRunner>();
        variableStorage = GetComponentInChildren<InMemoryVariableStorage>();

        // Fade in
        if (wasFade)
            StartCoroutine(FadeInUI(blackPanel, 0.01f));
    }

    void OnSceneUnloaded(Scene currentScene)
    {

    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }


    public void SceneLoad(string sceneName)
    {
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.Log(sceneName + "");
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.Log("There is no scene in build...");
        }
    }


    public IEnumerator FadeOutAndLoadScene(string sceneName, float speed)
    {
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.Log(sceneName + " load...");
            yield return StartCoroutine(FadeOutUI(blackPanel, speed));
            wasFade = true;
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.Log("There is no scene in build...");
        }
    }


    public IEnumerator FadeOutUI(Image _Image, float _fadeSpeed)
    {
        // Debug.Log("Fade out...");
        Color t_color = _Image.color;
        t_color.a = 0;
        
        _Image.gameObject.SetActive(true);

        while (t_color.a < 1)
        {
            t_color.a += _fadeSpeed;
            _Image.color = t_color;
            yield return null;
        }

        wasFade = true;
    }

    public IEnumerator FadeInUI(Image _Image, float _fadeSpeed)
    {
        // Debug.Log("Fade in...");
        Color t_color = _Image.color;
        t_color.a = 1;
        _Image.gameObject.SetActive(true);

        while (t_color.a > 0)
        {
            t_color.a -= _fadeSpeed;
            _Image.color = t_color;
            yield return null;
        }

        _Image.gameObject.SetActive(false);

        wasFade = false;
    }

    public IEnumerator FadeOutSprite(SpriteRenderer _Sprite, float _fadeSpeed){
        // Debug.Log("Fade out...");
        Color t_color = _Sprite.color;
        t_color.a = 0;
        
        _Sprite.gameObject.SetActive(true);

        while (t_color.a < 1)
        {
            t_color.a += _fadeSpeed;
            _Sprite.color = t_color;
            yield return null;
        }
        
    }

    public IEnumerator FadeInSprite(SpriteRenderer _Sprite, float _fadeSpeed){
        // Debug.Log("Fade in...");
        Color t_color = _Sprite.color;
        t_color.a = 1;
        
        _Sprite.gameObject.SetActive(true);

        while (t_color.a > 0)
        {
            t_color.a -= _fadeSpeed;
            _Sprite.color = t_color;
            yield return null;
        }

        _Sprite.gameObject.SetActive(false);

        wasFade = false;
    }


}
