using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LocaliserUI : MonoBehaviour
{
    public bool DebugLog;

    // TMP
    // Text

    public LocalisedString localisedString;
    //public LocalisedString localisedFunction;


    private void Start()
    {
        Text TextUI = GetComponent<Text>();
        TextMeshProUGUI TmpUI = GetComponent<TextMeshProUGUI>();

        if (TextUI != null)
            localisedString = new LocalisedString(TextUI.text);
        else
            localisedString = new LocalisedString(TmpUI.text);

        //Debug.Log("Key : " + key);
        string value = LanguageManager.instance.GetTranslation(localisedString.key);
        //string function = LocalisationSystem.GetLocalisedVariable(localisedFunction.key);
        UpdateUIText(value, TextUI);
        UpdateUIText(value, TmpUI);

    }

    public void UpdateUIText(string value, Text textField = null)
    {
        if (textField != null)
            textField.text = value;
    }

    public void UpdateUIText(string value, TextMeshProUGUI TmpField = null)
    {
        if (TmpField != null)
            TmpField.text = value;
    }
}
