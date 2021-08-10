using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SceneReferenceInEditor;

public class LocationNode : Node
{
    private SerializedProperty sceneRefProperty;

    private void OnEnable()
    {
        baseHeight = 55;
        baseWidth = 300;
        originalWindowTitle = "Set new Location";
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


        //EditorGUILayout.PropertyField(sceneRefProperty, new GUIContent("minigame scene"));
        //EditorGUILayout.LabelField("Location", GUILayout.Width(185));
        //cEvent.sceneAsset = (SceneAsset)EditorGUILayout.ObjectField(cEvent.sceneAsset, typeof(SceneAsset), false, GUILayout.Width(150));

        //var oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(cEvent.scenePath) as SceneAsset;

        EditorGUI.BeginChangeCheck();
        Location newLocation = EditorGUILayout.ObjectField("Location", cEvent.location, typeof(Location), false) as Location;

        if (EditorGUI.EndChangeCheck())
        {
            cEvent.location = newLocation;
        }


        cEvent.useCustomRoom = EditorGUILayout.Foldout(cEvent.useCustomRoom, "Overwrite default location destination's Room");


        if (cEvent.useCustomRoom)
        {

            height += 20;

            EditorGUI.BeginChangeCheck();
            Room newRoom = EditorGUILayout.ObjectField("Room", cEvent.room, typeof(Room), false) as Room;

            if (EditorGUI.EndChangeCheck())
            {
                cEvent.room = newRoom;
            }

        }


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
