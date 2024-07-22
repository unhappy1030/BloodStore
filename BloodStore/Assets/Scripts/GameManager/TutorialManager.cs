using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
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
        StartCoroutine(WaitUntilAllsettingsdone());
    }

    IEnumerator WaitUntilAllsettingsdone(){
        uiIndex = 0;

        tutorialCanvas.SetActive(false);

        foreach(GameObject ui in tutorialUI){
            ui.SetActive(false);
        }
        tutorialUI[0].SetActive(true);
        leftButton.SetActive(false);
        rightButton.SetActive(true);
        endButton.SetActive(false);
        
        if(GameManager.Instance.isSceneLoadEnd)
            yield return new WaitUntil(() => !GameManager.Instance.isSceneLoadEnd);
        yield return new WaitUntil(() => GameManager.Instance.isSceneLoadEnd);

        yield return new WaitForSeconds(0.5f);

        if(GameManager.Instance.isTurotial
            && tutorialControl.tutorialEndStatus.ContainsKey(SceneManager.GetActiveScene().name) 
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

    // button
    public void EndButton(){
        isTutorialFinish = true;
        tutorialControl.ChangeTutorialStatus(SceneManager.GetActiveScene().name, true);
        tutorialControl.CheckTutorialEnd();
        
        // if(tutorialControl.isAllTutorialFinish){
        //     GameManager.Instance.isTurotial = false;
        // }

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
