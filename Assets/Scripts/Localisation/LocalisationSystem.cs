using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;


public enum Language
{
    English,
    French,
}

public class LocalisationSystem : MonoBehaviour
{
    public static LocalisationSystem instance = null;

    public static Language language;

    private static Dictionary<string, string> localizedENG;
    private static Dictionary<string, string> localizedFR;


    private static Dictionary<string, string> characterENG;
    private static Dictionary<string, string> characterFR;

    public static bool isInit = false;

    private static CSVLoader CsvLoader;


    public List<string> ExcludedCharacterKey = new List<string>() { "[playerName]" };



    #region MonoBehaviour

    private void Awake()
    {
        //Check if instance already exists
        if (instance == null)
            //if not, set instance to this
            instance = this;
        //If instance already exists and it's not this:
        else if (instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);


        Init();
    }

    private void Start()
    {
        language = LanguageManager.instance.language;
    }

    #endregion


    #region Loader

    public static void Init()
    {
        CsvLoader = new CSVLoader();
        CsvLoader.LoadCSV();

        UpdateDictionary();

        isInit = true;
    }

    // Whenever editing, update afterward
    public static void UpdateDictionary()
    {

        localizedENG = CsvLoader.GetDictionaryValue("eng");
        localizedFR = CsvLoader.GetDictionaryValue("fr");


        characterENG = CsvLoader.GetDictionaryValue("eng", false);
        characterFR = CsvLoader.GetDictionaryValue("fr", false);

    }

    #endregion


    #region Static Getter

    #region Direct Get

    public static Dictionary<string, string> GetDictionaryForEditor()
    {
        if (!isInit) { Init(); }
        return localizedENG;
    }

    public static string GetLocalisedValue(string key)
    {
        string value = key.Trim('\n', '\r', '\"');
        key = value;

        if (!isInit)
        {
            Init();
        }

        bool exist = false;

        switch (language)
        {
            case Language.English:
                exist = localizedENG.TryGetValue(key, out value);
                break;
            case Language.French:
                exist = localizedFR.TryGetValue(key, out value);
                break;
        }

        if (!exist)
        {
            value = key;

#if UNITY_EDITOR
            CsvLoader.LoadCSV();
            CsvLoader.Add(key, key, "Assets/Resources/" + CsvLoader.folderPath + CsvLoader.localisationFileName + ".csv");
            CsvLoader.LoadCSV();
            UpdateDictionary();
#endif

        }

        //value = ReplaceVariables(value);


        return value;
    }

    public static bool IsKeyExcluded(string key)
    {
        return instance.ExcludedCharacterKey.Contains(key);
    }


    #endregion



    #region Processed Get

    // Get a character identifier in a string (value)
    public static string GetCharacterIdentifier(string value)
    {
        string foundMatch = "No match";
        //foundMatch = Regex.Match(value, @"\[.*?\]").Value;



        try
        {
            //Debug.Log("foudmatch " + Regex.Match(value, @"\[.*?\]").Value);
            foundMatch = Regex.Match(value, @"\[.*?\]").Value;
        }
        catch (Exception e)
        {
            if (e.GetType() == typeof(ArgumentNullException))
            {
                // Create a new formatted line for the value we just got
                //Debug.Log("Try harder plz ");

            }
            else
            {
                //Debug.Log("another error");
            }
        }


        return foundMatch;
    }





    // Replace a variable by a string
    public static string ReplaceVariables(string value)
    {
        if (!isInit)
        {
            Init();
        }

        string key = GetCharacterIdentifier(value);

        // Check if the key doesn't match the exception list (only apply to names)
        if (!IsKeyExcluded(key))
        {
            // doesn't match => continue your job
            // replace with Dictionary values
            value = ReplaceVariableWithKey(value, key);
        }
        else
        {
            // match => Replace by the DialogueChainPreferences.GetCharacterNameWithFuntion(CharacterFunction.MainCharacter));
            // replace by data from the variable manager
            string name = Dialogue.ReplaceCharacterFunction(key);
            value = value.Replace(key, name);
        }


        if (Regex.Match(value, @"\[.*?\]").Value != string.Empty)
        {
            Debug.LogError("value : " + value);
            value = ReplaceVariables(value);
        }

        return value;
    }


    public static string ReplaceVariableWithKey(string value, string key)
    {
        if (!isInit)
        {
            Init();
        }
        bool success = false;
        string characterFunction = key;
        if (key != string.Empty)
        {
            //string errorMsg = "Get Key" + key + "\t => ";
            //bool test = characterENG.TryGetValue(key, out characterFunction);
            //string color = "";
            //if (test)
            //{
            //    color = "<color=green> Succeed </color>";
            //}
            //else
            //{
            //    color = "<color=red> Failed </color>";
            //}
            //string appended = "\n";




            switch (language)
            {
                case Language.English:
                    success = characterENG.TryGetValue(key, out characterFunction);
                    break;
                case Language.French:
                    success = characterFR.TryGetValue(key, out characterFunction);
                    break;
            }

            if (success)
            {
                value = value.Replace(key, characterFunction);
            }
            else
            {


                characterFunction = key.Trim('[', ']');
                value = value.Replace(key, "{" + characterFunction + "}");
                //appended = string.Format("\n\"{0}\",\"{1}\",\"\"", key, characterFunction);



#if UNITY_EDITOR
                CsvLoader.LoadCSV();
                CsvLoader.Add(key, characterFunction);
                CsvLoader.LoadCSV();
                UpdateDictionary();
                var keyColl = characterENG.Keys;
                foreach (string s in keyColl)
                {
                    Debug.LogErrorFormat("Key = {0}", s);
                }
#endif



            }



            //Debug.LogError(errorMsg + color + appended);

        }




        return value;
    }





    #endregion


    #endregion



    #region Edition

    public static void Add(string key, string value)
    {
        if (CsvLoader == null)
        {
            CsvLoader = new CSVLoader();
        }

        if (value.Contains("\""))
        {
            value.Replace('"', '\"');
        }

#if UNITY_EDITOR

        CsvLoader.LoadCSV();
        CsvLoader.Add(key, value);
        CsvLoader.LoadCSV();

#endif

        UpdateDictionary();
    }

    public static void Remove(string key)
    {
        if (CsvLoader == null)
        {
            CsvLoader = new CSVLoader();
        }

#if UNITY_EDITOR
        CsvLoader.LoadCSV();
        CsvLoader.Remove(key);
        CsvLoader.LoadCSV();

#endif
        UpdateDictionary();
    }

    public static void Replace(string key, string value)
    {
        if (CsvLoader == null)
        {
            CsvLoader = new CSVLoader();
        }

        if (value.Contains("\""))
        {
            value.Replace('"', '\"');
        }

#if UNITY_EDITOR
        CsvLoader.LoadCSV();
        CsvLoader.Edit(key, value);
        CsvLoader.LoadCSV();

#endif

        UpdateDictionary();
    }

    #endregion

}
