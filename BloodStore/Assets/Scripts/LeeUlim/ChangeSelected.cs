using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using TreeEditor;
using UnityEngine.UI;

public class ChangeSelected : MonoBehaviour
{
    public List<GameObject> childs;
    public List<bool> isActivated;
    public int index;
    public bool notSelected;

    private void Start()
    {
        notSelected = true;

        isActivated = new();
        
        for(int i=0; i<childs.Count; i++){
            isActivated.Add(false);
        }
        
    }

    public void Select(int index){
        // all reset
        foreach(GameObject child in childs){
            TextMeshProUGUI text = child.GetComponentInChildren<TextMeshProUGUI>();
            text.color = Color.black;
        }
        
        for(int i=0; i<isActivated.Count; i++){
            isActivated[i] = false;
        }

        if(notSelected || this.index != index){
            TextMeshProUGUI indexText = childs[index].GetComponentInChildren<TextMeshProUGUI>();
            indexText.color = Color.red;
            isActivated[index] = true;

            this.index =index;
            notSelected = false;
            return;
        }

        notSelected = true;
    }
}
