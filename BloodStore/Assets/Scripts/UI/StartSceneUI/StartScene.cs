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
        if(SceneManager.GetActiveScene().name == "Start")
        {
            askTutorialCanvas.SetActive(false);
            startCanvas.SetActive(false);
            gameCanvas.SetActive(false);

            GameManager.Instance.ableToFade = true;
            
            StartCoroutine(WaitUntilIntro(time));
        }
        else
        {
            startCanvas.SetActive(true);
            gameCanvas.SetActive(false);
            
            GameManager.Instance.ableToFade = true;
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
        GameManager.Instance.saveData.SetInitialValue();
        GameManager.Instance.saveData.Save();
        GameManager.Instance.pairList = GameManager.Instance.pairList.LoadNew();
        GameManager.Instance.pairList.Save();
        GameManager.Instance.bloodPackList = GameManager.Instance.bloodPackList.LoadNew();
        GameManager.Instance.bloodPackList.Save();
        GameManager.Instance.loadfileName = "";
        GameManager.Instance.StartCoroutine(GameManager.Instance.FadeOutAndLoadScene(sceneName, 1f));
    }

    // button
    public void ChoosePlayTutorial(bool isPlayTutorial){
        if(isPlayTutorial)
        {
            GameManager.Instance.isTurotial = true;
            tutorialControl.isAllTutorialFinish = false;
            tutorialControl.ResetTutorialStatus(false);
        }
        else
        {
            GameManager.Instance.isTurotial = false;
            tutorialControl.isAllTutorialFinish = true;
            tutorialControl.ResetTutorialStatus(true);
        }
        
        GameManager.Instance.isFirstPlay = false;
    }


    // button
    public void SetTutorialStatus(bool isTutorial){
        tutorialControl.ResetTutorialStatus(!isTutorial);
    }

    // button
    public void AskAndStartNewGame(string sceneName){
        if(GameManager.Instance.isFirstPlay)
        {
            askTutorialCanvas.SetActive(true);
        }
        else
        {
            tutorialControl.ResetTutorialStatus(true);

            StartNewGame(sceneName);
        }
    }

    public void ExitGame(){
        Application.Quit();
        Debug.Log("Quit Game...");
    }

    public void Setting(){

    }
}
