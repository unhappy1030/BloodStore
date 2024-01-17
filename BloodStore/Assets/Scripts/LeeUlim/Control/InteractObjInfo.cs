using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Cinemachine;
using System;

public enum InteractType
{
    None,
    CameraControl,
    NpcInteraction,
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

    // NpcInteraction
    [SerializeField] public string _nodeName;

    // SceneLoad
    [SerializeField] public bool _isFade;
    [SerializeField] public string _sceneName;
    
    public void SetTargetCameraInfo(List<GameObject> targets, float hold, CinemachineBlendDefinition.Style blendIn, float blendTime){
        _interactType = InteractType.CameraControl;
        _cameraMovementType = CameraControlType.ChangeCamera;
        _cameraType = CameraType.TargetGroupCamera;

        if(_virtualCam == null)
            _virtualCam = new();
        
        if(_virtualCam.targets == null)
            _virtualCam.targets = new();
        
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

