using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    public float time = 4.5f;
    public CameraControl cameraControl; // assign at inspector
    public GameObject startCanvas;
    public GameObject gameCanvas;

    void Start()
    {
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


    public void Setting(){

    }
}
