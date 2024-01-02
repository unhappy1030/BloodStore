using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(InteractObjInfo))]
public class InteractObjInfoInspctor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

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
            case CameraControlType.ChangeCam:
                var changingCam = serializedObject.FindProperty("_changeVertualCam");
                bool isExist;

                GUILayout.Label("Change Camera");
                EditorGUILayout.PropertyField(changingCam);

                isExist = (changingCam.objectReferenceValue != null);

                EditorGUI.BeginDisabledGroup(isExist);
                EditorGUI.indentLevel++;
                GUILayout.Label("Testing...");
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(changingCam);
                EditorGUI.EndDisabledGroup();

                break;
                /*
                            case ChagingCameraType.BlendListCamera:
                                var subCamList = serializedObject.FindProperty("_subCamList");

                                GUILayout.Label("Blend List Camera");
                                EditorGUILayout.PropertyField(subCamList, true);
                                break;
                */
        }
    }
}