using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_ConversationSampleBehaviour : MonoBehaviour
{
    public TextMeshProUGUI characterName;
    public TMP_FontAsset dialogueFont;
    public Transform linesParent;
    public List<TextMeshProUGUI> dialogueLines;

    public void Awake()
    {
    }

    //public string speaker;
    //[TextArea(3, 10)]
    //public List<string> dialogueLines;

    public void CreateDialogueSample(ConversationLog conversationLog)
    {
        ClearDialogueLines();

        characterName.text = LanguageManager.instance.GetTranslation(conversationLog.speaker);
        //CharacterFunction cf = DialogueChainPreferences.GetCharacterFunctionWithName(conversationLog.characterFunction_string);
        characterName.color = VariablesManager.instance.charactersCollection.GetCharacterColor(conversationLog.characterFunction_string);

        foreach (string dialogueLine in conversationLog.dialogueLines)
        {
            CreateDialogueLine(dialogueLine);
        }
    }

    private void CreateDialogueLine(string dialogueText)
    {
        GameObject go = Instantiate(characterName.gameObject);
        go.name = "dialogue line";
        go.transform.parent = linesParent;
        go.transform.localScale = new Vector3(1, 1, 1);
        TextMeshProUGUI tmp = go.GetComponent<TextMeshProUGUI>();
        tmp.font = dialogueFont;
        tmp.text = "- " + LanguageManager.instance.GetTranslation(dialogueText);

        MakeLayoutElementOnText(go);
    }

    public void MakeLayoutElementOnText(GameObject go)
    {
        LayoutElement layoutElement = go.AddComponent<LayoutElement>();
        layoutElement.minHeight = 45;
        layoutElement.preferredWidth = 700;
    }

    public void ClearDialogueLines()
    {
        foreach(Transform child in linesParent)
        {
            Destroy(child.gameObject);
            //child.gameObject.SetActive(false);
        }
        dialogueLines = new List<TextMeshProUGUI>();
    }
}
