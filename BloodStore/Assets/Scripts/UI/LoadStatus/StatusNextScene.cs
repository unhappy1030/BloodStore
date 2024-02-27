using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusNextScene : MonoBehaviour
{
    public void NextSceneLoad(){
        if(GameManager.Instance.lastSceneName == "ResultStore"){
            GameManager.Instance.StartCoroutine(GameManager.Instance.FadeOutAndLoadScene("FamilyTree", 0.05f));
        }
        else if(GameManager.Instance.lastSceneName == "ResultFamilyTree"){
            GameManager.Instance.StartCoroutine(GameManager.Instance.FadeOutAndLoadScene("Store", 0.05f));
        }
    }
}
