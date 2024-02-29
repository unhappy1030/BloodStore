using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialControl : MonoBehaviour
{
    public bool isAllTutorialFinish;
    public Dictionary<string, bool> tutorialEndStatus;

    public void Start()
    {
        isAllTutorialFinish = true;

        tutorialEndStatus = new()
        {
            { "FamilyTree", true },
            { "ResultFamilyTree", true },
            { "Store", true },
            { "ResultStore", true }
        };
    }

    public void ResetTutorialStatus(bool isTrue){
        List<string> Keys = new(tutorialEndStatus.Keys);

        for(int i=0; i<tutorialEndStatus.Count; i++){
            tutorialEndStatus[Keys[i]] = isTrue;
        }
    }

    public void ChangeTutorialStatus(string sceneName, bool status){
        if(!tutorialEndStatus.ContainsKey(sceneName)){
            Debug.Log("There is no such key in doesTutorials...");
            return;
        }

        tutorialEndStatus[sceneName] = status;
    }

    public void CheckTutorialEnd(){
        int count = 0;
        foreach(bool isFinish in tutorialEndStatus.Values){
            if(isFinish){
                count++;
            }
        }

        if(count == tutorialEndStatus.Count){
            isAllTutorialFinish = true;
        }
    }
}
