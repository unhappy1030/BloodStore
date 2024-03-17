using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yarn.Unity;

public class GameManager : MonoBehaviour
{
    public string[] sceneNamesInBuild; // *** Inspector

    public string startSceneName = "Start"; // set start Scene name

    public int sellCount = 0;
    public float totalPoint = 0;
    public float currentAveragePoint = 0;
    public float filterDurability = 100;

    public float money = 0;
    public int day = 0;
    public string loadfileName = "";
    public string lastSceneName = "";
    public bool isFading = false;
    public bool isSceneLoadEnd = false;
    public bool ableToFade = false;
    bool wasFade = false;
    public bool isFirstPlay = true;
    public bool isTurotial = false;

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
    public ResultStore resultStore;
    public BloodSellProcess bloodSellProcess;

    public ResultMoney resultMoney;

    public StartScene startScene;

    public Pairs pairList;
    public BloodPacks bloodPackList;
    public SaveData saveData;
    public ImageLoad imageLoad;

    public TutorialControl tutorialControl;
    public Tutorial tutorial;

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
        resultStore = FindObjectOfType<ResultStore>();
        resultMoney = FindObjectOfType<ResultMoney>();
        bloodSellProcess = FindObjectOfType<BloodSellProcess>();
        tutorial = FindObjectOfType<Tutorial>();
        startScene = FindObjectOfType<StartScene>();

        mouseRayCast = GetComponent<MouseRayCast>();
        moneyControl = GetComponent<MoneyControl>();
        dialogueControl = GetComponent<DialogueControl>();
        pairList = GetComponent<Pairs>();
        bloodPackList = GetComponent<BloodPacks>();
        saveData = GetComponent<SaveData>();
        imageLoad = GetComponent<ImageLoad>();
        tutorialControl = GetComponent<TutorialControl>();

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
            nodeInteraction.dialogueControl = dialogueControl;
            nodeInteraction.yarnControl = yarnControl;
        }

        if(resultStore != null){
            resultStore.moneyControl = moneyControl;
        }

        if(resultMoney != null){
            resultMoney.moneyControl = moneyControl;
        }

        if(bloodSellProcess != null){
            bloodSellProcess.moneyControl = moneyControl;
        }
        
        mouseRayCast.cameraControl = cameraControl;

        if(nodeInteraction != null){
            mouseRayCast.nodeInteraction = nodeInteraction;
        }

        mouseRayCast.dialogueRunner = dialogueRunner;

        if(tutorial != null){
            tutorial.tutorialControl = tutorialControl;
        }
        
        if(startScene != null){
            startScene.tutorialControl = tutorialControl;
        }

        yarnControl.moneyControl = moneyControl;
        yarnControl.dialogueRunner = dialogueRunner;
        yarnControl.variableStorage = variableStorage;
        yarnControl.dialogueControl = dialogueControl;
        
        whitePanel.gameObject.SetActive(false);
        blackPanel.gameObject.SetActive(false);
        
        StartCoroutine(WaitUntilAbleSceneLoad());
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

    public void StartDialogue(string nodeName){
        if(!dialogueRunner.IsDialogueRunning)
        {
            if(dialogueRunner.NodeExists(nodeName))
                dialogueRunner.StartDialogue(nodeName);
            else
                Debug.Log(nodeName + " is not Exist...");
        }
        else
        {
            Debug.Log("Other Dialogue is running...");
        }
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

    public IEnumerator FadeOutAndLoadScene(string sceneName, float second)
    {
        isFading = true;

        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            yield return StartCoroutine(FadeInUI(blackPanel, second));
            wasFade = true;
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.Log("There is no scene in build...");
        }
        
        isFading = false;
    }

    IEnumerator FadeInAndLoadScene(){
        yield return FadeOutUI(blackPanel, 1f);
        isSceneLoadEnd = true;
    }
    
    public IEnumerator WaitUntilAbleSceneLoad(){
        isSceneLoadEnd = false;
        blackPanel.gameObject.SetActive(true);
        
        yield return new WaitUntil(() => ableToFade);

        if(wasFade)
        {
            StartCoroutine(FadeInAndLoadScene());
        }
        else
        {
            blackPanel.gameObject.SetActive(false);
            isSceneLoadEnd = true;
        }

        ableToFade = false;
    }

    public void CreateVirtualCamera(GameObject target, bool doesUSeBound, Collider2D bound, float lensOthoSize, float hold, Cinemachine.CinemachineBlendDefinition.Style style, float blendTime){
        InteractObjInfo interactObjInfo = cameraControl.gameObject.GetComponent<InteractObjInfo>();
        if(interactObjInfo == null){
            interactObjInfo = cameraControl.gameObject.AddComponent<InteractObjInfo>();
        }

        interactObjInfo.SetVirtualCameraInfo(target, doesUSeBound, bound, lensOthoSize, hold, style, blendTime);
        cameraControl.ChangeCam(interactObjInfo);
    }

    public void CreateTargetCamera(List<GameObject> targets, bool doesUSeBound, Collider2D bound, float hold, Cinemachine.CinemachineBlendDefinition.Style style, float blendTime){
        InteractObjInfo interactObjInfo = cameraControl.gameObject.GetComponent<InteractObjInfo>();
        if(interactObjInfo == null){
            interactObjInfo = cameraControl.gameObject.AddComponent<InteractObjInfo>();
        }

        if(targets == null || targets.Count == 0){
            Debug.Log("Target List is empty...");
            return;
        }

        interactObjInfo.SetTargetCameraInfo(targets, doesUSeBound, bound, hold, style, blendTime);
        cameraControl.ChangeCam(interactObjInfo);
    }

    // Fade in & out
    public IEnumerator FadeInUI(Image _Image, float _second)
    {
        if(_second == 0){
            Debug.Log("Second cannot be 0...");
            yield break;
        }

        if(Time.timeScale == 0){
            Time.timeScale = 1;
        }

        isFading = true;
        Color t_color = _Image.color;
        t_color.a = 0;
        Debug.Log("Fade In Start : " + DateTime.Now.ToString(("yyyy-MM-dd HH:mm:ss.fff")));
        _Image.gameObject.SetActive(true);

        while (t_color.a < 1)
        {
            t_color.a += Time.deltaTime/_second;
            _Image.color = t_color;
            yield return null;
        }
        
        Debug.Log("Fade In Finish : " + DateTime.Now.ToString(("yyyy-MM-dd HH:mm:ss.fff")));
        isFading = false;
        wasFade = true;
    }

    float CalculateFadeInAlpha(float x, float s){
        float y;
        y = (x/s)*(x/s)*(x/s)*(x/s);
        return y;
    }

    public IEnumerator FadeOutUI(Image _Image, float _second)
    {
        if(_second == 0){
            Debug.Log("Second cannot be 0...");
            yield break;
        }

        isFading = true;
        Color t_color = _Image.color;
        t_color.a = 1;
        _Image.gameObject.SetActive(true);
        Debug.Log("Fade Out Start : "+DateTime.Now.ToString(("yyyy-MM-dd HH:mm:ss.fff")));
        float x = 1;

        while (t_color.a > 0)
        {   
            x -= Time.deltaTime/_second;
            t_color.a = CalculateFadeOutAlpha(x, _second);
            _Image.color = t_color;
            yield return null;
        }
        
        Debug.Log("Fade Out Finish : " + DateTime.Now.ToString(("yyyy-MM-dd HH:mm:ss.fff")));
        _Image.gameObject.SetActive(false);

        isFading = false;
        wasFade = false;
    }

    float CalculateFadeOutAlpha(float x, float s){
        float y;
        y = -1 * (x/s-1)*(x/s-1)*(x/s-1)*(x/s-1) + 1;
        return y;
    }

    public IEnumerator FadeInSprite(SpriteRenderer _Sprite, float _second){
        // Debug.Log("Fade in...");
        if(_second == 0){
            Debug.Log("Second cannot be 0...");
            yield break;
        }

        Color t_color = _Sprite.color;
        t_color.a = 0;
        
        _Sprite.gameObject.SetActive(true);

        while (t_color.a < 1)
        {
            t_color.a += Time.deltaTime/_second;
            _Sprite.color = t_color;
            yield return null;
        }
    }

    public IEnumerator FadeOutSprite(SpriteRenderer _Sprite, float _second){
        // Debug.Log("Fade out...");
        if(_second == 0){
            Debug.Log("Second cannot be 0...");
            yield break;
        }

        Color t_color = _Sprite.color;
        t_color.a = 1;
        
        _Sprite.gameObject.SetActive(true);

        while (t_color.a > 0)
        {
            t_color.a -= Time.deltaTime/_second;
            _Sprite.color = t_color;
            yield return null;
        }

        _Sprite.gameObject.SetActive(false);

        wasFade = false;
    }


}
