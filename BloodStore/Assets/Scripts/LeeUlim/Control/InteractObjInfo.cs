using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Cinemachine;
using System;

// ���콺 ��ȣ�ۿ� ����
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
    public GameObject target;
    public BlendInfo blendInfo;
}

[System.Serializable]
public class TargetGroupCameraInfo{
    public List<GameObject> targets;
    public BlendInfo blendInfo;
}

[System.Serializable]
public class BlendListSubCameraInfo{
    public CameraType cameraType;
    public VirtualCameraInfo virtualCam;
    public TargetGroupCameraInfo targetGroupCam;
    public BlendInfo blendInfo;
}

[System.Serializable]
public class BlendListCameraInfo{
    public List<BlendListSubCameraInfo> subCams;
    public BlendInfo blendInfo;
}

public class InteractObjInfo : MonoBehaviour
{
    [SerializeField] public InteractType _interactType;

    // CameraContorlType
    [SerializeField] public CameraControlType _cameraMovementType;

    // - ChangeCam
    [SerializeField] public CameraType _cameraType;

    //      : Virtual Camera
    [SerializeField] public VirtualCameraInfo _vertualCam;
    [SerializeField]public TargetGroupCameraInfo _targetGroupCam;
    [SerializeField]public BlendListCameraInfo _blendListCam;

    // SceneLoad
    [SerializeField] public bool _isFade;
    [SerializeField] public string _sceneName;
}
