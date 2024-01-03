using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Cinemachine;

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
    ChangeCam
}

[System.Serializable]
public class ChangingCameraInfo{
    public GameObject target;
    public float backCamHold;
    public CinemachineBlendDefinition.Style blendIn;
    public float blendTime;
}

public class InteractObjInfo : MonoBehaviour
{
    [SerializeField] public InteractType _interactType;

    // CameraContorlType
    [SerializeField] public CameraControlType _cameraMovementType;

    // - ChangeCam
    [SerializeField] public bool _useCurrentCamAsStart;
    [SerializeField] public GameObject _startCam;
    [SerializeField] public List<ChangingCameraInfo> _changingCamList;

    // SceneLoad
    [SerializeField] public bool _isFade;
    [SerializeField] public string _sceneName;
}
