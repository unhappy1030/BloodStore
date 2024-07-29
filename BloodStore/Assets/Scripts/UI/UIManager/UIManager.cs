using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public List<GameObject> DefaultOnUI;
    public List<GameObject> DefaultOffUI;

    void Start(){
        foreach(GameObject onUI in DefaultOnUI){
            if(!onUI.activeSelf){
                onUI.SetActive(true);
            }
        }
        foreach(GameObject offUI in DefaultOffUI){
            if(offUI.activeSelf){
                offUI.SetActive(false);
            }
        }
    }
}
