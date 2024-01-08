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
    public DialogueRunner dialogueRunner;
    public MouseRayCast mouseRayCast;


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

        // Find yarnControl
        npcInteract = FindObjectOfType<NPCInteract>();
        if(npcInteract == null){
            GameObject temp = new("NPCInteracttionInfo");
            npcInteract = temp.AddComponent<NPCInteract>();
        }

        mouseRayCast = GetComponent<MouseRayCast>();

        dialogueRunner = GetComponentInChildren<DialogueRunner>();

        // Fade in
        if (wasFade)
            StartCoroutine(FadeIn(blackPanel, 0.01f));
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
            yield return StartCoroutine(FadeOut(blackPanel, speed));
            wasFade = true;
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.Log("There is no scene in build...");
        }
    }


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
