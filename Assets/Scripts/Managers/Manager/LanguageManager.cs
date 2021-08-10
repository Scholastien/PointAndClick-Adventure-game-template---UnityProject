using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// All Text UI that contain the translatable script would be suscribed to the language manager language state via the event system

public class LanguageManager : MonoBehaviour
{

    public static LanguageManager instance = null;

    public Language language;

    private void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Use this for initialization
    void Start()
    {
        CSVLoader loader = new CSVLoader();
        loader.LoadCSV();
    }


    #region Setter

    public void SetLanguage(Language language)
    {
        this.language = language;
        LocalisationSystem.language = language;

        // Signal language has been changed
    }

    #endregion


    #region Getter

    public string GetTranslation(string value)
    {
        char[] excludedChar = new char[] { '\r', '\n', '\"' };

        value = value.Trim(excludedChar);


        value = LocalisationSystem.GetLocalisedValue(value);
        value = LocalisationSystem.ReplaceVariables(value);

        value = value.Trim(excludedChar);

        return value;
    }

    #endregion

}

