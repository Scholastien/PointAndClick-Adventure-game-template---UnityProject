using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DialogueChain))]
public class DialogueChainEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DialogueChain chain = (DialogueChain)target;

        GUI.enabled = false;
        EditorGUILayout.ObjectField("Script", MonoScript.FromScriptableObject(chain), typeof(ScriptableObject), false);
        GUI.enabled = true;

        GUILayout.BeginHorizontal();
        GUILayout.Label("Halt Movement", GUILayout.Width(200));
        chain.haltMovement = EditorGUILayout.Toggle(chain.haltMovement);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Default Show Portraits", GUILayout.Width(200));
        chain.defaultShowImages = EditorGUILayout.Toggle(chain.defaultShowImages);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Default Show Names", GUILayout.Width(200));
        chain.defaultShowNames = EditorGUILayout.Toggle(chain.defaultShowNames);
        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();
        GUILayout.Label("SubDialogue (false = default)", GUILayout.Width(200));
        chain.isSubDialogueChain = EditorGUILayout.Toggle(chain.isSubDialogueChain);
        GUILayout.EndHorizontal();


        if (chain.defaultShowImages)
        {
            chain.defaultSprite = (Sprite)EditorGUILayout.ObjectField("Default Portrait", chain.defaultSprite, typeof(Sprite), false);
        }

        if (chain.defaultShowNames)
        {
            chain.defaultSpeaker = EditorGUILayout.TextField("Default Speaker Name", chain.defaultSpeaker);
        }

        chain.defaultContainerType = (ContainerType)EditorGUILayout.EnumPopup("Default Container", chain.defaultContainerType);
        chain.defaultTextDelay = EditorGUILayout.FloatField("Default Text Delay", chain.defaultTextDelay);

        if (GUILayout.Button("Open Node Editor"))
        {
            DialogueChainEditorWindow editor = EditorWindow.GetWindow<DialogueChainEditorWindow>();
            editor.dialogueChain = chain;
            editor.dialogueChain.Awake();

            // Secure dialogue node that use a non-existant CharacterFunction
            editor.dialogueChain.SecureOutOfRangeCharacterFunction();


            editor.OnDestroy();
            editor.LoadCurrentQuest();
        }

        EditorUtility.SetDirty(chain);
    }
}





