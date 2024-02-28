using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialControl : MonoBehaviour
{
    public Dictionary<string, bool> tutorialStatus;

    public void Start()
    {
        tutorialStatus = new()
        {
            { "FamilyTree", false },
            { "ResultFamilyTree", false },
            { "Store", false },
            { "ResultStore", false }
        };
    }

    public void ResetTutorialStatus(bool isTutorial){
        List<string> Keys = new(tutorialStatus.Keys);

        for(int i=0; i<tutorialStatus.Count; i++){
            tutorialStatus[Keys[i]] = !isTutorial;
        }
    }

    public void ChangeTutorialStatus(string sceneName, bool status){
        if(!tutorialStatus.ContainsKey(sceneName)){
            Debug.Log("There is no such key in doesTutorials...");
            return;
        }

        tutorialStatus[sceneName] = status;
    }

    public bool CheckTutorialStatus(){
        int count = 0;
        foreach(bool isFinish in tutorialStatus.Values){
            if(isFinish){
                count++;
            }
        }

        if(count == tutorialStatus.Count){
            GameManager.Instance.isFirstTurotial = false;
            return true;
        }

        return false;
    }
}
