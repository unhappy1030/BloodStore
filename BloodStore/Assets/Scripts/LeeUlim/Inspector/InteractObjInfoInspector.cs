using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(InteractObjInfo))]
public class InteractObjInfoInspctor : Editor
{
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
            case CameraControlType.ChangeCam:
                var useCurrnetCamAsStart = serializedObject.FindProperty("_useCurrentCamAsStart");
                var startCam = serializedObject.FindProperty("_startCam");
                var subCamList = serializedObject.FindProperty("_changingCamList");

                GUILayout.Label("Change Camera");

                EditorGUILayout.PropertyField(useCurrnetCamAsStart);

                if(!useCurrnetCamAsStart.boolValue)
                    EditorGUILayout.PropertyField(startCam);
                
                EditorGUILayout.PropertyField(subCamList, true);
                

                break;
        }
    }
}