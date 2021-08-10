using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class QuestCollectionEditorWindow : ExtendedEditorWindow
{
   public static void Open(QuestCollection questCollection)
    {
        QuestCollectionEditorWindow window = GetWindow<QuestCollectionEditorWindow>("Quest Collection Editor");
        window.serializedObject = new SerializedObject(questCollection);
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("test");


        currentProperty = serializedObject.FindProperty("allQuest");
        DrawProperties(currentProperty, true);

        //EditorGUILayout.PropertyField(currentProperty);
        GUILayout.EndHorizontal();
;
        //EditorGUILayout.PropertyField(currentProperty);
        //foreach(SerializedProperty p in currentProperty)
        //{

        //    EditorGUILayout.PropertyField(p);
        //}
    }
}
