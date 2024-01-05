using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Cinemachine;
using Unity.IO.LowLevel.Unsafe;


[CustomEditor(typeof(InteractObjInfo))]
public class InteractObjInfoInspctor : Editor
{
    InteractObjInfo interactObjInfo;
    // int count = 1;
    bool isOpen = true;

    private void OnEnable()
    {
        interactObjInfo = (InteractObjInfo)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var interactType = (InteractType)serializedObject.FindProperty("_interactType").enumValueIndex;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_interactType"));

        var sceneNameProperty = serializedObject.FindProperty("_sceneName");

        EditorGUI.indentLevel++;

        switch (interactType)
        {
            case InteractType.None:
            break;

            case InteractType.CameraControl:
                CameraMoveCase();
            break;

            case InteractType.SceneLoad:
                GUILayout.Space(10);
                GUILayout.Label("Scene Load");
                int selectedIndex = System.Array.IndexOf(GameManager.Instance.sceneNamesInBuild, sceneNameProperty.stringValue);
                selectedIndex = EditorGUILayout.Popup("Scene Names in Builds", selectedIndex, GameManager.Instance.sceneNamesInBuild);

                if (selectedIndex >= 0 && selectedIndex < GameManager.Instance.sceneNamesInBuild.Length)
                    sceneNameProperty.stringValue = GameManager.Instance.sceneNamesInBuild[selectedIndex];

                EditorGUILayout.PropertyField(serializedObject.FindProperty("_isFade"));
            break;
        }

        EditorGUI.indentLevel--;

        serializedObject.ApplyModifiedProperties();
    }

    void CameraMoveCase()
    {
        GUILayout.Space(10);
        GUILayout.Label("Camera Move");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_cameraMovementType"));

        var cameraMovementType = (CameraControlType)serializedObject.FindProperty("_cameraMovementType").enumValueIndex;

        GUILayout.Space(10);
        switch (cameraMovementType)
        {
            case CameraControlType.ChangeCamera:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_cameraType"));
                var cameraType = (CameraType)serializedObject.FindProperty("_cameraType").enumValueIndex;

                // Virtual & TargetGroup
                if(cameraType == CameraType.VirtualCamera || cameraType == CameraType.TargetGroupCamera)
                    DrawInspectorForVirtualAndTargetCamera(interactObjInfo._vertualCam, cameraType);
                // Blend List Camera
                else if(cameraType == CameraType.BlendListCamera)
                    DrawInspectorForBlendListCamera();
                
            break;
        }
    }

    void DrawInspectorForVirtualAndTargetCamera(VirtualCameraInfo virtualCamera, CameraType cameraType){
        // Vertual Camera
        if(cameraType == CameraType.VirtualCamera)
            isOpen = EditorGUILayout.Foldout(isOpen, "Virtual Camera", true);
        // Target Group Camera
        else
            isOpen = EditorGUILayout.Foldout(isOpen, "Target Group Camera", true);
        
        if(isOpen){
            EditorGUI.indentLevel++;

            if(virtualCamera.targets.Count <= 0){
                virtualCamera.targets = new()
                {
                    null
                };
            }else{
                // VirtualCamera
                if(cameraType == CameraType.VirtualCamera){
                    GameObject newObj = EditorGUILayout.ObjectField(new GUIContent("Target"), virtualCamera.targets[0], typeof(GameObject), true) as GameObject;
                    virtualCamera.targets[0] = newObj;
                }
                // TargetGroupCamera
                else
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_vertualCam.targets"), new GUIContent("Target"));
                
            }
            
            virtualCamera.doseUseBound = EditorGUILayout.Toggle(new GUIContent("Use Bound"), virtualCamera.doseUseBound);
            
            if(virtualCamera.doseUseBound){
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_vertualCam.bound"), new GUIContent("Bound Object"));
                EditorGUI.indentLevel--;
            }

            // Vertual Camera
            if(cameraType == CameraType.VirtualCamera)
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_vertualCam.lensOthoSize"), new GUIContent("Lens Otho Size"));

            EditorGUI.indentLevel--;
        }
        
        // Blend Info
        GUILayout.Space(10);
        EditorGUILayout.LabelField("Blend Info", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_vertualCam.blendInfo.hold"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_vertualCam.blendInfo.blendIn"));
        if(virtualCamera.blendInfo.blendIn != CinemachineBlendDefinition.Style.Cut)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_vertualCam.blendInfo.blendTime"));

        EditorGUI.indentLevel--;
    }

    void DrawInspectorForBlendListCamera(){
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_blendListCam.subCams"), new GUIContent("Sub Cams"));
        // isOpen = EditorGUILayout.Foldout(isOpen, "Blend List Camera", true);

        // if(isOpen){
        //     if(interactObjInfo._blendListCam.subCams.Count <= 0)
        //     {
        //         interactObjInfo._blendListCam.subCams = new()
        //         {
        //             null
        //         };
        //         Debug.Log(interactObjInfo._blendListCam.subCams.Count);
        //     }
        //     else
        //     {
        //         count = EditorGUILayout.IntField("SubCams Count", count);

        //         if(count <=0)
        //             count =1;
        //         else{
        //             for(int i=interactObjInfo._blendListCam.subCams.Count; i<count; i++){
        //                 interactObjInfo._blendListCam.subCams.Add(new());
        //             }
        //         }

        //         EditorGUILayout.LabelField("SubCams", EditorStyles.boldLabel);
        //         for(int i=0; i<interactObjInfo._blendListCam.subCams.Count; i++){
        //             EditorGUI.indentLevel++;
        //             GUILayout.Space(5);
        //             DrawInspectorForVirtualAndTargetCamera(interactObjInfo._blendListCam.subCams[i].virtualCam, interactObjInfo._blendListCam.subCams[i].cameraType);
        //             EditorGUI.indentLevel--;
        //         }
        //     }
        // }

        interactObjInfo._blendListCam.doseUseBound = EditorGUILayout.Toggle(new GUIContent("Use Bound"), interactObjInfo._blendListCam.doseUseBound);
        
        if(interactObjInfo._blendListCam.doseUseBound){
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_blendListCam.bound"), new GUIContent("Bound Object"));
            EditorGUI.indentLevel--;
        }

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Blend Info", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_blendListCam.blendInfo.hold"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_blendListCam.blendInfo.blendIn"));
        if(interactObjInfo._blendListCam.blendInfo.blendIn != CinemachineBlendDefinition.Style.Cut)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_blendListCam.blendInfo.blendTime"));

        EditorGUI.indentLevel--;
    }
}