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

    public float money = 0;
    public int day = 0;

    public bool isFading = false; 
    bool wasFade = false;

    public Image whitePanel;
    public Image blackPanel;

    public CameraControl cameraControl; // *** warning : must be in Scene and set "CameraControl" tag
    public NPCInteract npcInteract;
    public DialogueControl dialogueControl;
    public NodeInteraction nodeInteraction;
    public MouseRayCast mouseRayCast;
    public MoneyControl moneyControl;
    public YarnControl yarnControl;
    public DialogueRunner dialogueRunner;
    public InMemoryVariableStorage variableStorage;

    public Pairs pairList;
    public BloodPacks bloodPackList;

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

        npcInteract = FindObjectOfType<NPCInteract>();
        nodeInteraction = FindObjectOfType<NodeInteraction>();

        mouseRayCast = GetComponent<MouseRayCast>();
        moneyControl = GetComponent<MoneyControl>();
        dialogueControl = GetComponent<DialogueControl>();
        pairList = GetComponent<Pairs>();
        bloodPackList = GetComponent<BloodPacks>();

        yarnControl = GetComponentInChildren<YarnControl>();
        dialogueRunner = GetComponentInChildren<DialogueRunner>();
        variableStorage = GetComponentInChildren<InMemoryVariableStorage>();
        
        // assign scripts 
        if(npcInteract != null){
            npcInteract.dialogueRunner = dialogueRunner;
            npcInteract.cameraControl = cameraControl;
            npcInteract.yarnControl = yarnControl;
            npcInteract.dialogueControl = dialogueControl;
        }

        if(nodeInteraction != null){
            if(cameraControl != null)
            {
                nodeInteraction.cameraControl = cameraControl;
            }
            nodeInteraction.dialogueRunner = dialogueRunner;
        }

        mouseRayCast.cameraControl = cameraControl;
        mouseRayCast.npcInteract = npcInteract;

        if(nodeInteraction != null){
            mouseRayCast.nodeInteraction = nodeInteraction;
        }

        mouseRayCast.dialogueRunner = dialogueRunner;

        yarnControl.moneyControl = moneyControl;
        yarnControl.dialogueRunner = dialogueRunner;
        yarnControl.variableStorage = variableStorage;
        yarnControl.dialogueControl = dialogueControl;
        
        whitePanel.gameObject.SetActive(false);
        blackPanel.gameObject.SetActive(false);
        
        // Fade in
        if (wasFade){
            StartCoroutine(FadeInUI(blackPanel, 0.01f));
        }
    }

    void OnSceneUnloaded(Scene currentScene)
    {
        if(dialogueRunner.IsDialogueRunning){
            dialogueRunner.Stop();
            Debug.Log("Stop all dialogue...");
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }


    // Scene Load
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
            yield return StartCoroutine(FadeOutUI(blackPanel, speed));
            wasFade = true;
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.Log("There is no scene in build...");
        }
    }


    // Fade in & out
    public IEnumerator FadeOutUI(Image _Image, float _fadeSpeed)
    {
        isFading = true;
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

        isFading = false;
        wasFade = true;
    }

    public IEnumerator FadeInUI(Image _Image, float _fadeSpeed)
    {
        isFading = true;
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

        isFading = false;
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
