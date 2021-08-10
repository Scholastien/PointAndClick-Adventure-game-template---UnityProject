using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(LocalisedString))]
public class LocalisedStringDrawer : PropertyDrawer
{
    bool dropdown;
    float height;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (dropdown)
        {
            return height += 25;
        }
        return 20;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //base.OnGUI(position, property, label);

        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        position.width -= 34;
        position.height = 18;


        Rect valueRect = new Rect(position);
        valueRect.x += 15;
        valueRect.y -= 15;

        Rect foldButtonRect = new Rect(position);
        foldButtonRect.width = 15;

        dropdown = EditorGUI.Foldout(foldButtonRect, dropdown, "");

        position.x += 15;
        position.width -= 15;

        SerializedProperty key = property.FindPropertyRelative("key");
        key.stringValue = EditorGUI.TextField(position, key.stringValue);

        position.x += position.width + 2;
        position.width = 17;
        position.height = 17;

        Texture searchIcon = (Texture)Resources.Load("EditorIcon/search");
        GUIContent searchContent = new GUIContent(searchIcon, "Search in database");

        if(GUI.Button(position, searchContent))
        {
            TextLocaliserSearchWindow.Open();
        }

        position.x += position.width + 2;

        Texture storeIcon = (Texture)Resources.Load("EditorIcon/download");
        GUIContent storeContent = new GUIContent(storeIcon, "Add to database");


        if (GUI.Button(position, storeContent))
        {
            TextLocaliserEditWindow.Open(key.stringValue);
        }

        if (dropdown)
        {
            var value = LocalisationSystem.GetLocalisedValue(key.stringValue);
            GUIStyle style = GUI.skin.box;
            height = style.CalcHeight(new GUIContent(value), valueRect.width);

            valueRect.height = height;
            valueRect.y += 31;
            EditorGUI.LabelField(valueRect, value, EditorStyles.wordWrappedLabel);
        }



        //GUI.Label(new Rect(10, 40, 100, 40), GUI.tooltip);

        EditorGUI.EndProperty();

    }
}
