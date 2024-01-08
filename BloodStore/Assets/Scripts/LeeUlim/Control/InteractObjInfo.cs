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

// [System.Serializable]
// public class TargetGroupCameraInfo{
//     public List<GameObject> targets;
//     public bool doseUseBound;
//     public Collider2D bound;
//     public BlendInfo blendInfo;
// }

[System.Serializable]
public class BlendListSubCameraInfo{
    public CameraType cameraType;
    public VirtualCameraInfo virtualCam;
    // public TargetGroupCameraInfo targetGroupCam;
}

[System.Serializable]
public class BlendListCameraInfo{
    public List<BlendListSubCameraInfo> subCams;
    // public List<VirtualCameraInfo> subCams;
    public bool doseUseBound;
    public Collider2D bound;
    public BlendInfo blendInfo;
}

public class InteractObjInfo : MonoBehaviour
{
    [SerializeField] public InteractType _interactType;

    // CameraContorlType
    [SerializeField] public CameraControlType _cameraMovementType;

    // - ChangeCam
    [SerializeField] public CameraType _cameraType;

    [SerializeField] public VirtualCameraInfo _vertualCam;
    // [SerializeField]public TargetGroupCameraInfo _targetGroupCam;
    [SerializeField]public BlendListCameraInfo _blendListCam;

    // SceneLoad
    [SerializeField] public bool _isFade;
    [SerializeField] public string _sceneName;
}
