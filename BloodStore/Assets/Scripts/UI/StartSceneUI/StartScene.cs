using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : MonoBehaviour
{
    public float time = 4.5f;
    public CameraControl cameraControl; // assign at inspector
    public GameObject startCanvas;
    public GameObject gameCanvas;

    void Start()
    {
        startCanvas.SetActive(false);
        gameCanvas.SetActive(false);
        StartCoroutine(WaitUntilIntro(time));
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
        GameManager.Instance.bloodPackList = GameManager.Instance.bloodPackList.LoadNew();
        GameManager.Instance.StartCoroutine(GameManager.Instance.FadeOutAndLoadScene(sceneName, 0.25f));
    }

    // button
    public void StartCurrentGame(string sceneName){
        GameManager.Instance.pairList = GameManager.Instance.pairList.Load();
        GameManager.Instance.bloodPackList = GameManager.Instance.bloodPackList.Load();
        GameManager.Instance.StartCoroutine(GameManager.Instance.FadeOutAndLoadScene(sceneName, 0.25f));
    }

    public void Setting(){

    }
}
