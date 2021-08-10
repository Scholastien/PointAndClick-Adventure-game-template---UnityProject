using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class VariableModifierNode : Node
{
    private void OnEnable()
    {
        baseHeight = 47;
        baseWidth = 350;
        originalWindowTitle = "Variable Adjustment";
    }

    public override void DrawWindow(DialogueChain chain)
    {
        base.DrawWindow(chain);
        HandleTitle();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add"))
        {
            cEvent.variableModifiers.Add(new VariableModifier());
        }
        if (GUILayout.Button("Remove"))
        {
            if (cEvent.variableModifiers.Count > 0)
            {
                cEvent.variableModifiers.Remove(cEvent.variableModifiers[cEvent.variableModifiers.Count - 1]);
            }
        }
        EditorGUILayout.EndHorizontal();

        if (cEvent.variableModifiers.Count == 0)
        {
            cEvent.variableModifiers.Add(new VariableModifier());
        }
        
        for (int i = 0; i < cEvent.variableModifiers.Count; i++)
        {
            height += 18;

            EditorGUILayout.BeginHorizontal();
            cEvent.variableModifiers[i].intNeeded = (ChainIntType)EditorGUILayout.EnumPopup(cEvent.variableModifiers[i].intNeeded, GUILayout.Width(117));
            EditorGUILayout.LabelField("", GUILayout.Width(1));
            cEvent.variableModifiers[i].equator = EditorGUILayout.Popup(cEvent.variableModifiers[i].equator, new string[3] { "+", "-", "=" }, GUILayout.Width(30));
            
            cEvent.variableModifiers[i].valueType = EditorGUILayout.Popup(cEvent.variableModifiers[i].valueType, new string[2] { "number", "variable" }, GUILayout.Width(60));
            EditorGUILayout.LabelField("", GUILayout.Width(1));

            switch(cEvent.variableModifiers[i].valueType){
                case 0:
                    cEvent.variableModifiers[i].value = EditorGUILayout.IntField(cEvent.variableModifiers[i].value, GUILayout.Width(110));
                    break;
                case 1:
                    cEvent.variableModifiers[i].valueToOperate = (ChainIntType)EditorGUILayout.EnumPopup(cEvent.variableModifiers[i].valueToOperate, GUILayout.Width(110));
                    break;
                default:
                    Debug.Log("Value non assigned");
                    break;
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
