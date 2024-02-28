using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    public GameObject tutorialCanvas; // assign at inspector
    public List<GameObject> tutorialUI; // assign at inspector
    public GameObject leftButton; // assign at inspector
    public GameObject rightButton; // assign at inspector
    public GameObject endButton; // assign at inspector

    public int uiIndex = 0;

    public bool isTutorialFinish;

    public TutorialControl tutorialControl;

    void Start(){
        uiIndex = 0;

        tutorialCanvas.SetActive(false);

        foreach(GameObject ui in tutorialUI){
            ui.SetActive(false);
        }
        tutorialUI[0].SetActive(true);
        leftButton.SetActive(false);
        rightButton.SetActive(true);
        endButton.SetActive(false);

        if(tutorialControl.tutorialEndStatus.ContainsKey(SceneManager.GetActiveScene().name) 
            && !tutorialControl.tutorialEndStatus[SceneManager.GetActiveScene().name])
        {
            isTutorialFinish = false;
            tutorialCanvas.SetActive(true);
        }
        else
        {
            isTutorialFinish = true;
            tutorialCanvas.SetActive(false);
        }
    }

    // // left button
    // public void LeftButton(){
    //     if(uiIndex - 1 < 0){
    //         Debug.Log("UI Index cannot be negative...");
    //         return;
    //     }

    //     uiIndex--;
    // }

    // // right button
    // public void RightButton(){
    //     if(uiIndex + 1 >= tutorialUI.Count){
    //         Debug.Log("UI Index cannot be more than tutorial UI Count...");
    //         return;
    //     }

    //     uiIndex++;
    // }

    public void EndButton(){
        isTutorialFinish = true;
        tutorialControl.ChangeTutorialStatus(SceneManager.GetActiveScene().name, true);
        tutorialControl.CheckTutorialStatus();
        tutorialCanvas.SetActive(false);
    }

    // both buttons
    public void ShowNextOrBackUI(bool isNext){
        tutorialUI[uiIndex].SetActive(false);

        uiIndex = (isNext)? uiIndex + 1 : uiIndex - 1;
        Debug.Log("UI Index : " + uiIndex);

        tutorialUI[uiIndex].SetActive(true);

        leftButton.SetActive(uiIndex > 0);
        rightButton.SetActive(uiIndex < tutorialUI.Count - 1);
        endButton.SetActive(uiIndex == tutorialUI.Count - 1);
    }
}
