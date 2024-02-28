using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    public float time = 4.5f;
    public CameraControl cameraControl; // assign at inspector
    public GameObject startCanvas; // assign at inspector
    public GameObject gameCanvas; // assign at inspector
    public GameObject askTutorialCanvas; // assign at inspector

    public TutorialControl tutorialControl;

    void Start()
    {
        askTutorialCanvas.SetActive(false);

        if(SceneManager.GetActiveScene().name == "Start")
        {
            startCanvas.SetActive(false);
            gameCanvas.SetActive(false);
            StartCoroutine(WaitUntilIntro(time));
        }
        else
        {
            startCanvas.SetActive(true);
            gameCanvas.SetActive(false);
        }
    }

    IEnumerator WaitUntilIntro(float time){
        yield return new WaitForSeconds(time);
        startCanvas.SetActive(true);
    }

    // button
    public void ActiveGameCanvas(bool isGame){
        if(isGame)
        {
            startCanvas.SetActive(false);
            gameCanvas.SetActive(true);
        }
        else
        {
            gameCanvas.SetActive(false);
            startCanvas.SetActive(true);
        }
    }

    // button
    public void StartNewGame(string sceneName){
        GameManager.Instance.pairList = GameManager.Instance.pairList.LoadNew();
        GameManager.Instance.pairList.Save();
        GameManager.Instance.bloodPackList = GameManager.Instance.bloodPackList.LoadNew();
        GameManager.Instance.bloodPackList.Save();
        GameManager.Instance.loadfileName = "";
        GameManager.Instance.StartCoroutine(GameManager.Instance.FadeOutAndLoadScene(sceneName, 1f));
    }

    // button
    public void SetTutorialStatus(bool isTutorial){
        tutorialControl.ResetTutorialStatus(isTutorial);
    }

    // button
    public void AskAndStartNewGame(string sceneName){
        if(GameManager.Instance.isFirstTurotial)
        {
            askTutorialCanvas.SetActive(true);
        }
        else
        {
            tutorialControl.ResetTutorialStatus(false);

            GameManager.Instance.pairList = GameManager.Instance.pairList.LoadNew();
            GameManager.Instance.pairList.Save();
            GameManager.Instance.bloodPackList = GameManager.Instance.bloodPackList.LoadNew();
            GameManager.Instance.bloodPackList.Save();
            GameManager.Instance.loadfileName = "";
            GameManager.Instance.StartCoroutine(GameManager.Instance.FadeOutAndLoadScene(sceneName, 1f));
        }
    }


    public void Setting(){

    }
}
