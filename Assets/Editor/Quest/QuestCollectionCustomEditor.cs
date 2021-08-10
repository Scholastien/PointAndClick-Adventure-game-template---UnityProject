using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

public class AssetHandler
{
    [OnOpenAsset]
    public static bool OpenEditor(int instanceId, int line)
    {
        QuestCollection obj = EditorUtility.InstanceIDToObject(instanceId) as QuestCollection;
        if(obj != null)
        {
            QuestCollectionEditorWindow.Open(obj);
            return true;
        }
        return false;
    }
}

[CustomEditor(typeof(QuestCollection))]
public class QuestCollectionCustomEditor : Editor
{
    QuestCollection obj;

    public void OnEnable()
    {
        obj = (QuestCollection)target;
    }

    public override void OnInspectorGUI()
    {
        if(GUILayout.Button("Open Editor"))
        {
            QuestCollectionEditorWindow.Open(obj);
        }
    }
}
