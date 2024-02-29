using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllDeadEndingUI : MonoBehaviour
{
    void Start()
    {
        if(GameManager.Instance.pairList.CheckAllDead()){
            gameObject.SetActive(true);
        }
        else{
            gameObject.SetActive(false);
        }
    }

    public void LoadEnding(){
        GameManager.Instance.StartCoroutine(GameManager.Instance.FadeOutAndLoadScene("AllDeadEnding", 1f));
    }
}
