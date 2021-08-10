using QuestSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NotificationReward))]
public class NotificationRewardEditor : Editor
{

    NotificationReward nr;

    public void OnEnable()
    {
        nr = (NotificationReward)target;
    }


    public override void OnInspectorGUI()
    {
        GUILayout.BeginHorizontal();
        //base.OnInspectorGUI();
        EditorGUILayout.LabelField("Notification Type", GUILayout.Width(130));
        nr.notificationType = (NotificationType)EditorGUILayout.EnumPopup(nr.notificationType);
        GUILayout.EndHorizontal();

        switch (nr.notificationType)
        {
            case NotificationType.Blanck:
                break;
            case NotificationType.StatChanged:
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Signed Value", GUILayout.Width(130));
                nr.statSigned = (StatSigned)EditorGUILayout.EnumPopup(nr.statSigned);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Value", GUILayout.Width(130));
                nr.IntValue = EditorGUILayout.IntField(nr.IntValue);
                GUILayout.EndHorizontal();
                switch (nr.statSigned)
                {
                    case StatSigned.none:
                        nr.StringValue = "";
                        break;
                    case StatSigned.plus:
                        nr.StringValue = "+";
                        break;
                    case StatSigned.minus:
                        nr.StringValue = "-";
                        break;
                    default:
                        break;
                }
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Variable Name", GUILayout.Width(130));
                nr.Verb = EditorGUILayout.TextField(nr.Verb);
                GUILayout.EndHorizontal();
                break;
            case NotificationType.LocationUnlocked:
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Location Name", GUILayout.Width(130));
                nr.StringValue = EditorGUILayout.TextField(nr.StringValue);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Obtained or Removed", GUILayout.Width(130));
                nr.locationState = (LocationState)EditorGUILayout.EnumPopup(nr.locationState);
                nr.Verb = nr.locationState.ToString();
                GUILayout.EndHorizontal();
                break;
            case NotificationType.CharacterMet:
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Character Name", GUILayout.Width(130));
                nr.StringValue = EditorGUILayout.TextField(nr.StringValue);
                GUILayout.EndHorizontal();
                break;
            case NotificationType.NewQuest:
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Quest Name", GUILayout.Width(130));
                nr.StringValue = EditorGUILayout.TextField(nr.StringValue);
                GUILayout.EndHorizontal();
                break;
            case NotificationType.NewObjective:
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Objective Name", GUILayout.Width(130));
                nr.StringValue = EditorGUILayout.TextField(nr.StringValue);
                GUILayout.EndHorizontal();
                break;
            case NotificationType.Item:
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Item", GUILayout.Width(130));
                nr.StringValue = EditorGUILayout.TextField(nr.StringValue);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Obtained or Removed", GUILayout.Width(130));
                nr.itemState = (ItemState)EditorGUILayout.EnumPopup(nr.itemState);
                nr.Verb = nr.itemState.ToString();
                GUILayout.EndHorizontal();
                break;
            default:
                break;
        }
    }
}
