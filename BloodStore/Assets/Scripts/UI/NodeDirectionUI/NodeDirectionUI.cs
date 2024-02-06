using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeDirectionUI : MonoBehaviour
{
    public GameObject upUI, leftUI, rightUI;
    public List<GameObject> downUI;
    public GameObject downGroup;
    void Start(){
        SetAllDeactive();
    }
    public void UpdateDirection(int[] direction){
        SetAllDeactive();
        if(direction[0] == 1){
            upUI.SetActive(true);
        }
        if(direction[1] == 1){
            leftUI.SetActive(true);
        }
        if(direction[2] == 1){
            rightUI.SetActive(true);
        }
        if(direction[3] != 0){
            if(direction[3] == 1){
                downUI[0].SetActive(true);
            }
            else{
                foreach(GameObject down in downUI){
                    down.SetActive(true);
                }
                if(downUI.Count < direction[3]){
                    Downs downs = downGroup.GetComponent<Downs>();
                    for(int i = downUI.Count + 1; i <= direction[3]; i++){
                        downUI.Add(downs.AddDown(i));
                    }
                }
                
            }
        }
    }
    public void SetAllDeactive(){
        if(upUI.activeSelf){
            upUI.SetActive(false);
        }
        if(leftUI.activeSelf){
            leftUI.SetActive(false);
        }
        if(rightUI.activeSelf){
            rightUI.SetActive(false);
        }
        foreach(GameObject down in downUI){
            if(down.activeSelf){
                down.SetActive(false);
            }
        }
    }
}
