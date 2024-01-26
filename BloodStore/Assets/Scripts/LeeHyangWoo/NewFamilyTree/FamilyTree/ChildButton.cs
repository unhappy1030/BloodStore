using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildButton : MonoBehaviour
{
    public Group group;
    public GameObject addChildUI;
    public void OnAddChildUI(){
        ChildAddUI UI = addChildUI.GetComponent<ChildAddUI>();
        UI.Active(group);
    }
}
