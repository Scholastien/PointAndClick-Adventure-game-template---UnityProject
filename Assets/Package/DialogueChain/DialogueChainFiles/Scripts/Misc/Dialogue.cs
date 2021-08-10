using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Dialogue
{
    public static string DialogueSyntaxFix(string text)
    {
        //Makes a temporary string and sets it equal to the original text.
        string newText = text;

        if (newText != string.Empty)
        {
            //Replaces all instances of "/playerName" in the text to that of your player's name. Make sure to customize this in the DialogueChainPreferences.
            //newText = newText.Replace("/playerName", DialogueChainPreferences.GetCharacterNameWithFuntion(CharacterFunction.MainCharacter));
            //string value = LocalisationSystem.GetLocalisedValue(newText);
            //newText = LocalisationSystem.ReplaceVariables(value);

            newText = LanguageManager.instance.GetTranslation(newText);
        }

        return newText;

    }

    public static string ReplaceCharacterFunction(string key)
    {
        switch (key.ToLower())
        {
            case "[playername]":
                return DialogueChainPreferences.GetCharacterNameWithFuntion(CharacterFunction.MainCharacter);
            case "[test]":
                return DialogueChainPreferences.GetCharacterNameWithFuntion(CharacterFunction.test);
            default:
                Debug.Log("Character not named");
                return "Character_Name";
        }
    }
}
