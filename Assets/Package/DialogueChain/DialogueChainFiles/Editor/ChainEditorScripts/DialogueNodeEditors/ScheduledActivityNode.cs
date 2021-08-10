using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ScheduleSystem;

public class ScheduledActivityNode : Node {

    private void OnEnable()
    {
        baseHeight = 68;
        baseWidth = 300;
        originalWindowTitle = "ScheduleActivity Set";
    }

    public override void DrawWindow(DialogueChain chain)
    {
        base.DrawWindow(chain);
        HandleTitle();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add ScheduleActivity"))
        {
            cEvent.scheduledActivities.Add(null);
        }
        EditorGUILayout.EndHorizontal();

        for (int i = 0; i < cEvent.scheduledActivities.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            height += 20;

            EditorGUILayout.LabelField("Set", GUILayout.Width(25));
            cEvent.scheduledActivities[i] = (ScheduledActivity)EditorGUILayout.ObjectField(cEvent.scheduledActivities[i], typeof(ScheduledActivity), false, GUILayout.Width(230));
            if (GUILayout.Button("-", GUILayout.Width(20)))
            {
                cEvent.scheduledActivities.RemoveAt(i);
                return;
            }
            EditorGUILayout.EndHorizontal();
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
