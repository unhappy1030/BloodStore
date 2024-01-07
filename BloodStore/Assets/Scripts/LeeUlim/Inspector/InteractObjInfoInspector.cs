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
    int count = 1;
    int targetCount = 1;
    bool isOpen = true;
    bool isOtherOpen = false;

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
                EditorGUI.indentLevel += 2;
                // Virtual & TargetGroup
                if(cameraType == CameraType.VirtualCamera || cameraType == CameraType.TargetGroupCamera)
                    DrawInspectorForVirtualAndTargetCamera(interactObjInfo._vertualCam, cameraType);
                // Blend List Camera
                else if(cameraType == CameraType.BlendListCamera)
                    // EditorGUILayout.PropertyField(serializedObject.FindProperty("_blendListCam"));
                    DrawInspectorForBlendListCamera();
                EditorGUI.indentLevel -= 2;
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

            if(virtualCamera == null)
                virtualCamera = new();

            if(virtualCamera.targets == null || virtualCamera.targets.Count <= 0){
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
                else{
                    targetCount = EditorGUILayout.IntField(new GUIContent("Count"), virtualCamera.targets.Count);
                    if(targetCount > virtualCamera.targets.Count)
                        for(int i=virtualCamera.targets.Count; i<targetCount; i++)
                            virtualCamera.targets.Add(null);
                    else if(targetCount < virtualCamera.targets.Count)
                        for(int i=targetCount; i<virtualCamera.targets.Count; i++)
                            virtualCamera.targets.RemoveAt(i);
                    
                    EditorGUILayout.LabelField("Terget Obj", EditorStyles.boldLabel);
                    EditorGUI.indentLevel+=2;
                    for(int i=0; i<virtualCamera.targets.Count; i++)
                        virtualCamera.targets[i] = EditorGUILayout.ObjectField(new GUIContent(""), virtualCamera.targets[i], typeof(GameObject), true) as GameObject;
                    EditorGUI.indentLevel-=2;
                }
            }
            
            virtualCamera.doseUseBound = EditorGUILayout.Toggle(new GUIContent("Use Bound"), virtualCamera.doseUseBound);
            
            EditorGUI.indentLevel += 2;
            if(virtualCamera.doseUseBound)
                virtualCamera.bound = EditorGUILayout.ObjectField(new GUIContent(""), virtualCamera.bound, typeof(Collider2D), true) as Collider2D;

            EditorGUI.indentLevel -= 2;
            // Vertual Camera
            if(cameraType == CameraType.VirtualCamera)
                virtualCamera.lensOthoSize = EditorGUILayout.FloatField(new GUIContent("Lens Otho Size"), virtualCamera.lensOthoSize);

            EditorGUI.indentLevel--;
        }
        
        // Blend Info
        GUILayout.Space(10);
        EditorGUILayout.LabelField("Blend Info", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        if(virtualCamera.blendInfo == null)
            virtualCamera.blendInfo = new();

        virtualCamera.blendInfo.hold = EditorGUILayout.FloatField(new GUIContent("Hold"), virtualCamera.blendInfo.hold);
        virtualCamera.blendInfo.blendIn = (CinemachineBlendDefinition.Style)EditorGUILayout.EnumPopup(new GUIContent("Blend In"), virtualCamera.blendInfo.blendIn);
        if(virtualCamera.blendInfo.blendIn != CinemachineBlendDefinition.Style.Cut)
            virtualCamera.blendInfo.blendTime = EditorGUILayout.FloatField(new GUIContent("Blend Time"), virtualCamera.blendInfo.blendTime);
        EditorGUI.indentLevel--;
    }


    void DrawInspectorForBlendListCamera(){
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("_blendListCam.subCams"), new GUIContent("Sub Cams"));
        isOtherOpen = EditorGUILayout.Foldout(isOtherOpen, "Blend List Camera", true);

        if(isOtherOpen){
            if(interactObjInfo._blendListCam.subCams.Count <= 0)
            {
                interactObjInfo._blendListCam.subCams = new()
                {
                    null
                };
                Debug.Log(interactObjInfo._blendListCam.subCams.Count);
            }
            else
            {
                count = EditorGUILayout.IntField(new GUIContent("SubCams Count"), interactObjInfo._blendListCam.subCams.Count);
                
                if(count <=0)
                    count =1;
                else{
                    if(count > interactObjInfo._blendListCam.subCams.Count)
                        for(int i=interactObjInfo._blendListCam.subCams.Count; i<count; i++)
                                interactObjInfo._blendListCam.subCams.Add(new());
                    else if(count < interactObjInfo._blendListCam.subCams.Count)
                        for(int j=count; j<interactObjInfo._blendListCam.subCams.Count; j++)
                            interactObjInfo._blendListCam.subCams.RemoveAt(j);
                }
                
                

                EditorGUILayout.LabelField("SubCams", EditorStyles.boldLabel);
                for(int i=0; i<interactObjInfo._blendListCam.subCams.Count; i++){
                    EditorGUI.indentLevel += 2;
                    interactObjInfo._blendListCam.subCams[i].cameraType = (CameraType)EditorGUILayout.EnumPopup(interactObjInfo._blendListCam.subCams[i].cameraType);
                    EditorGUI.indentLevel += 2;
                    DrawInspectorForVirtualAndTargetCamera(interactObjInfo._blendListCam.subCams[i].virtualCam, interactObjInfo._blendListCam.subCams[i].cameraType);
                    EditorGUI.indentLevel -= 2;
                    GUILayout.Space(5);
                    EditorGUI.indentLevel -= 2;
                }
            }
        }

        interactObjInfo._blendListCam.doseUseBound = EditorGUILayout.Toggle(new GUIContent("Use Bound"), interactObjInfo._blendListCam.doseUseBound);
        
        EditorGUI.indentLevel++;
        if(interactObjInfo._blendListCam.doseUseBound)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_blendListCam.bound"), new GUIContent(""));
        EditorGUI.indentLevel--;

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