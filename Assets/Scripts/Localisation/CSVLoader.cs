using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class CSVLoader
{

    private string pathPrefix;
    public string folderPath = "Localisation/";

    public string localisationFileName = "localisation";
    private string charactersFileName = "characters";



    public string localisationFile;
    public string characterFile;



    private char LineSeperator = '\n';
    private char surround = '"';
    private string[] fieldSeperator = { "\",\"" };

    public void LoadCSV()
    {
        pathPrefix = Application.dataPath;

        CreateLocalisationFolder();


        string gamefolderFilePath = pathPrefix + "/" + folderPath + charactersFileName + ".csv";


        localisationFile = Resources.Load<TextAsset>(folderPath + localisationFileName).text;


        characterFile = GetCharacterFile(charactersFileName, gamefolderFilePath);

        //characterFile = Resources.Load<TextAsset>(charactersFileName).text;

    }


    #region File Management

    // Check if a file exist
    public bool isFileExisting(string path)
    {
        return File.Exists(path);
    }


    // Make a copy of the file inside the game folder
    public void CopyFile(string textToCopy, string path)
    {
        StreamWriter sw = new StreamWriter(path);

        sw.Write(textToCopy);
        sw.Close();
    }

    // Get the value of the copied file
    public string GetCharacterFile(string resourceFilePath, string gamefolderFilePath)
    {
        string result = "";

        if (isFileExisting(gamefolderFilePath))
        {
            try
            {
                result = File.ReadAllText(gamefolderFilePath);
            }
            catch
            {

            }
        }
        else
        {
            result = Resources.Load<TextAsset>(folderPath + charactersFileName).text;
            CopyFile(result, gamefolderFilePath);

        }

        // Verify if the result from the gamefolder is valid
        // if not, put the default file in result;


#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif

        return result;
    }


    #endregion



    #region Data Manipulation

    public Dictionary<string, string> GetDictionaryValue(string attributeID, bool localisation = true)
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();

        string[] lines = localisationFile.Split(LineSeperator);

        if (localisation)
            lines = localisationFile.Split(LineSeperator); // Line separator is "\"\n\""
        else
            lines = characterFile.Split(LineSeperator); // Line separator is "\"\n\""

        // validating lines
        //lines = CombineLinesIfNeeded(lines);



        int attributeIndex = -1; // get the header "key", "eng", "fr"

        string[] headers = lines[0].Split(fieldSeperator, StringSplitOptions.None);

        for (int i = 0; i < headers.Length; i++)
        {


            if (headers[i].Contains(attributeID))
            {
                attributeIndex = i;
                break;
            }
        }

        Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];



            string[] fields = CSVParser.Split(line);

            for (int f = 0; f < fields.Length; f++)
            {
                //Debug.Log(fields[f]);
                //Debug.Log("Vanilla : |" + fields[f] + "   " + fields[f].Length);
                fields[f] = fields[f].TrimStart(' ', surround);
                fields[f] = fields[f].TrimEnd('\0', ' ', '\n', surround);
                fields[f] = RemoveLastCharacter('\n', fields[f]);
                fields[f] = RemoveLastCharacter(surround, fields[f]);
                //Debug.Log("Trimmed : " + fields[f]);
            }

            if (fields.Length > attributeIndex)
            {
                var key = fields[0];

                if (dictionary.ContainsKey(key))
                {
                    continue;
                }

                var value = fields[attributeIndex];

                dictionary.Add(key, value);
            }
        }
        return dictionary;
    }


    public string RemoveLastCharacter(char charToRemove, string text)
    {
        //Debug.Log("Last character : "+ text[text.Length - 1]);
        char[] charsToTrim = { charToRemove, ' ' };
        text.TrimEnd(charsToTrim);

        if (text.Length > 3)
        {
            if (text[text.Length - 1] == charToRemove)
            {
                text = text.Substring(0, text.Length - 1);
                text = RemoveLastCharacter(charToRemove, text);
            }
        }
        return text;
    }
    #endregion


#if UNITY_EDITOR
    #region Editor

    public void Add(string key, string value, string path = "Assets/Resources/Localisation/characters.csv")
    {
        if (key != "")
        {

            //Debug.LogError("value contain return: " + (value.Contains("\n")|| value.Contains("\r")));

            value = RemoveLastCharacter('\n', value);

            //                             "  r "key","value",""  "
            string appended = string.Format("\n\"{0}\",\"{1}\",\"\"", key, value);

            //Debug.LogError("adding : " + appended);

            File.AppendAllText(path, appended);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            LoadCSV();

            if (path == "Assets/Resources/Localisation/characters.csv")
            {
                // Assets/Resources/Localisation

                string gamefolderFilePath = pathPrefix + "/" + folderPath + charactersFileName + ".csv";
                File.AppendAllText(gamefolderFilePath, appended);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

        }

        
    }


    public void Remove(string key)
    {
        string[] lines = localisationFile.Split(LineSeperator);

        string[] keys = new string[lines.Length];

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];

            keys[i] = line.Split(fieldSeperator, StringSplitOptions.None)[0];
        }
        int index = -1;

        for (int i = 0; i < keys.Length; i++)
        {
            if (keys[i].Contains(key))
            {
                index = i;
                break;
            }
        }
        if (index > -1)
        {
            string[] newLines;
            newLines = lines.Where(w => w != lines[index]).ToArray();

            string replaced = string.Join(LineSeperator.ToString(), newLines);

            File.WriteAllText("Assets/Resources/" + "localisation.csv", replaced);



        }
        AssetDatabase.Refresh();
        LoadCSV();
    }

    public void Edit(string key, string value)
    {
        Remove(key);
        Add(key, value);
    }

    #endregion
#endif

    private void CreateLocalisationFolder()
    {
        if (!Directory.Exists(pathPrefix + "/Localisation"))
        {
            Directory.CreateDirectory(pathPrefix + "/Localisation");
        }
    }

}
