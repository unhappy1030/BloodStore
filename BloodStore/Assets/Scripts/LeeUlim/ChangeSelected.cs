using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class ChangeSelected : MonoBehaviour
{
    public List<GameObject> childs;
    public List<bool> isActivated;
    public int index;
    public bool notInteractied = true;

    private void Start()
    {
        for(int i=0; i<childs.Count; i++){
            isActivated = new(){
                false
            };
        }
        
    }

    public void Select(int index){
        // all reset
        foreach(GameObject child in childs){
            TextMeshProUGUI text = child.GetComponent<TextMeshProUGUI>();
            text.color = Color.black;
        }
        
        for(int i=0; i<isActivated.Count; i++){
            isActivated[i] = false;
        }

        if(this.index == index){
            TextMeshProUGUI indexText = childs[index].GetComponent<TextMeshProUGUI>();
            indexText.color = Color.red;
            isActivated[index] = true;

            this.index =index;
        }
    }
}
