using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SceneReferenceInEditor;

public class MinigameLauncherNode : Node
{
    private SerializedProperty sceneRefProperty;

    private void OnEnable()
    {
        baseHeight = 68;
        baseWidth = 300;
        originalWindowTitle = "Minigame Launcher";
        //sceneRefProperty = serializedObject.FindProperty("SceneReference");

    }

    public override void OnInspectorGUI()
    {
        
        //var oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(cEvent.scenePath);

        //serializedObject.Update();

        //EditorGUI.BeginChangeCheck();
        //var newScene = EditorGUILayout.ObjectField("scene", oldScene, typeof(SceneAsset), false) as SceneAsset;

        //if (EditorGUI.EndChangeCheck())
        //{
        //    var newPath = AssetDatabase.GetAssetPath(newScene);
        //    var scenePathProperty = serializedObject.FindProperty("scenePath");
        //    scenePathProperty.stringValue = newPath;

        //    cEvent.scenePath = newPath;
        //    cEvent.minigameSceneRef = new SceneReference(newScene);
        //}
        //serializedObject.ApplyModifiedProperties();
    }


    public override void DrawWindow(DialogueChain chain)
    {
        base.DrawWindow(chain);
        HandleTitle();

        //var picker = target as ScenePicker;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.EndHorizontal();

        //EditorGUILayout.PropertyField(sceneRefProperty, new GUIContent("minigame scene"));
        EditorGUILayout.LabelField("minigame scene reference", GUILayout.Width(185));
        //cEvent.sceneAsset = (SceneAsset)EditorGUILayout.ObjectField(cEvent.sceneAsset, typeof(SceneAsset), false, GUILayout.Width(150));

        var oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(cEvent.scenePath) as SceneAsset;

        EditorGUI.BeginChangeCheck();
        var newScene = EditorGUILayout.ObjectField("scene", oldScene, typeof(SceneAsset), false) as SceneAsset;

        if (EditorGUI.EndChangeCheck())
        {
            var newPath = AssetDatabase.GetAssetPath(newScene);
            //var scenePathProperty = serializedObject.FindProperty("scenePath");
            //scenePathProperty.stringValue = newPath;

            cEvent.scenePath = newPath;
            cEvent.minigameSceneRef = new SceneReference(newScene);
        }

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.EndHorizontal();


        windowRect.height = height;
        cEvent.windowRect = windowRect;
    }


    public override void DrawCurves()
    {
        base.DrawCurves();
    }

    public override void DrawNodeCurve(Rect start, Rect end, float sTanMod, float eTanMod, Color color, bool rightLeftConnect)
    {
        base.DrawNodeCurve(start, end, sTanMod, eTanMod, color, rightLeftConnect);
    }
}
