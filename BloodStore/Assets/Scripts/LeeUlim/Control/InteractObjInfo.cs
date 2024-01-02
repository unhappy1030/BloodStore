using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Cinemachine;

// 마우스 상호작용 모음
public enum InteractType
{
    None,
    CameraControl,
    SceneLoad,
    GameExit
}

public enum CameraControlType
{
    ChangeCam,
    // BlendListCamera,
}

public class InteractObjInfo : MonoBehaviour
{
    [HideInInspector] [SerializeField] public InteractType _interactType;

    // CameraType
    [HideInInspector] [SerializeField] public CameraControlType _cameraMovementType;

    // - Vertual
    [HideInInspector] [SerializeField] public GameObject _changeVertualCam;

    // - BlendList
    // [HideInInspector] [SerializeField] public List<GameObject> _subCamList;

    // SceneLoad
    [HideInInspector] [SerializeField] public bool _isFade;
    [HideInInspector] [SerializeField] public string _sceneName;
}
