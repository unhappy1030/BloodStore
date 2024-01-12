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

    [SerializeField] public VirtualCameraInfo _vertualCam;
    [SerializeField] public BlendListCameraInfo _blendListCam;

    // NpcInteraction
    [SerializeField] public string _nodeName;

    // SceneLoad
    [SerializeField] public bool _isFade;
    [SerializeField] public string _sceneName;
    public void SetBlendData(){
        _interactType = InteractType.CameraControl;
        _cameraMovementType = CameraControlType.ChangeCamera;
        _cameraType = CameraType.TargetGroupCamera;

        if (_vertualCam == null) {
            _vertualCam = new VirtualCameraInfo();
        }

        if (_vertualCam.blendInfo == null) {
            _vertualCam.blendInfo = new BlendInfo();
        }
        
        _vertualCam.blendInfo.hold = 0.25f;
        _vertualCam.blendInfo.blendIn = CinemachineBlendDefinition.Style.EaseInOut;
        _vertualCam.blendInfo.blendTime = 0.25f;
    }

}
