using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Cinemachine;
using System;
using Unity.VisualScripting;

public enum InteractType
{
    None,
    CameraControl,
    StartDialogue,
    FamilyTree,
    UIOnOff,
    SceneLoad,
    GameExit
}

public enum CameraControlType
{
    ChangeCamera,
    CameraEffect
}

public enum CameraType
{
    VirtualCamera,
    TargetGroupCamera,
    BlendListCamera
}

public enum FamilyTreeType{
    Group,
    EmptyNode,
    Node,
    ChildButton
}

[System.Serializable]
public class BlendInfo
{
    public float hold;
    public CinemachineBlendDefinition.Style blendIn;
    public float blendTime;
}

[System.Serializable]
public class VirtualCameraInfo{
    public List<GameObject> targets;
    public bool doseUseBound;
    public Collider2D bound;
    public float lensOthoSize;
    public BlendInfo blendInfo;
}

[System.Serializable]
public class BlendListSubCameraInfo{
    public CameraType cameraType;
    public VirtualCameraInfo virtualCam;
}

[System.Serializable]
public class BlendListCameraInfo{
    public List<BlendListSubCameraInfo> subCams;
    public bool doseUseBound;
    public Collider2D bound;
    public BlendInfo blendInfo;
}

public class InteractObjInfo : MonoBehaviour
{
    [SerializeField] public InteractType _interactType;

    // CameraControlType
    [SerializeField] public CameraControlType _cameraMovementType;

    // - ChangeCam
    [SerializeField] public CameraType _cameraType;

    [SerializeField] public VirtualCameraInfo _virtualCam;
    [SerializeField] public BlendListCameraInfo _blendListCam;

    // StartDialogue
    [SerializeField] public string _nodeName;

    // Family Tree
    public FamilyTreeType _familyTreeType;

    // UI On Off
    [SerializeField] public GameObject _ui;

    // SceneLoad
    [SerializeField] public bool _isFade;
    [SerializeField] public string _sceneName;
    
    public void SetVirtualCameraInfo(GameObject target, bool doseUseBound, Collider2D bound, float lensOthoSize, float hold, CinemachineBlendDefinition.Style blendIn, float blendTime){
        _interactType = InteractType.CameraControl;
        _cameraMovementType = CameraControlType.ChangeCamera;
        _cameraType = CameraType.VirtualCamera;

        if(_virtualCam == null){
            _virtualCam = new();
        }
        
        if(_virtualCam.targets == null){
            _virtualCam.targets = new();
        }

        _virtualCam.targets.Clear();
        _virtualCam.targets.Add(target);

        _virtualCam.doseUseBound = doseUseBound;
        
        if(doseUseBound){
            _virtualCam.bound = bound;
        }

        _virtualCam.lensOthoSize = lensOthoSize;

        if(_virtualCam.blendInfo == null){
            _virtualCam.blendInfo = new();
        }

        _virtualCam.blendInfo.hold = hold;
        _virtualCam.blendInfo.blendIn = blendIn;
        _virtualCam.blendInfo.blendTime = blendTime;
    }

    public void SetTargetCameraInfo(List<GameObject> targets, bool doseUseBound, Collider2D bound, float hold, CinemachineBlendDefinition.Style blendIn, float blendTime){
        _interactType = InteractType.CameraControl;
        _cameraMovementType = CameraControlType.ChangeCamera;
        _cameraType = CameraType.TargetGroupCamera;

        if(_virtualCam == null)
            _virtualCam = new();
        
        if(_virtualCam.targets == null)
            _virtualCam.targets = new();

        _virtualCam.doseUseBound = doseUseBound;
        
        if(doseUseBound){
            _virtualCam.bound = bound;
        }

        bool isAvailableTarget = AssignCameraTarget(targets);

        if(!isAvailableTarget){
            Debug.Log("It is not available form of targets...");
        }

        if(_virtualCam.blendInfo == null)
            _virtualCam.blendInfo = new();
        
        _virtualCam.blendInfo.hold = hold;
        _virtualCam.blendInfo.blendIn = blendIn;
        _virtualCam.blendInfo.blendTime = blendTime;
    }

    bool AssignCameraTarget(List<GameObject> targets){
        int nullCount = 0;
        _cameraType = CameraType.TargetGroupCamera;

        if(targets.Count == 0)
            return false;
        
        for(int i=0; i<targets.Count; i++){
            if(targets[i] == null)
                nullCount++;
        }

        if(nullCount == targets.Count)
            return false;

        _virtualCam.targets = new(targets);
        return true;
    }

}

