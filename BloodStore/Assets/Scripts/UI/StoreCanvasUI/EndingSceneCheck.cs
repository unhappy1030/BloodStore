using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingSceneCheck : MonoBehaviour
{
    public List<string> EndingScenes;

    public void LoadScene(){
        if(GameManager.Instance.day == 30){
            GameManager.Instance.StartCoroutine(GameManager.Instance.FadeOutAndLoadScene(EndingScenes[0], 1f));
        }
        else{
            GameManager.Instance.StartCoroutine(GameManager.Instance.FadeOutAndLoadScene("ResultStore", 1f));
        }
    }
}
