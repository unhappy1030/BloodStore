using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SetCameraTarget : MonoBehaviour
{
    public List<GameObject> test;
    public List<GameObject> targets = new();
    InteractObjInfo interactObjInfo;

    private void Start()
    {
        interactObjInfo = GetComponent<InteractObjInfo>();
        SetTarget(test);
    }
    
    public bool AssignCameraTarget(InteractObjInfo objInfo){
        int nullCount = 0;
        objInfo._cameraType = CameraType.TargetGroupCamera;

        if(targets.Count == 0)
            return false;
        
        for(int i=0; i<targets.Count; i++){
            if(targets[i] == null)
                nullCount++;
        }

        if(nullCount == targets.Count)
            return false;

        objInfo._vertualCam.targets = new(targets);
        return true;
    }

    // set tempList -> out of my boundary
    public void SetTarget(List<GameObject> list){
        targets = new(list);
    }
}
