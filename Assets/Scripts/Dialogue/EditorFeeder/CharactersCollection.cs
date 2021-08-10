using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new CharactersCollection", menuName = "LewdOwl/Dialogue/CharactersCollection")]
public class CharactersCollection : ScriptableObject
{
    public Color mainCharacterColor;


    public List<CharacterName> characterNames;


    public void Init()
    {
        Array characterArray = Enum.GetValues(typeof(CharacterFunction));

        foreach (CharacterFunction cf in characterArray)
        {
            if (cf != CharacterFunction.MainCharacter)
            {
                if (CharacterExist(cf))
                {
                    if (GetCharacterName(cf) == "")
                    {
                        GetCharacterNameObj(cf).name = GetCharacterNameObj(cf).characterFunction.ToString();
                    }
                }
                else
                {
                    characterNames.Add(new CharacterName(cf));
                }
            }
        }
    }

    public CharacterName GetCharacterNameObj(CharacterFunction characterFunction)
    {
        foreach (CharacterName cr in characterNames)
        {
            if (cr.characterFunction == characterFunction)
            {
                return cr;
            }
        }
        return null;
    }


    public string GetCharacterName(CharacterFunction characterFunction)
    {
        foreach (CharacterName cr in characterNames)
        {
            if (cr.characterFunction == characterFunction)
            {
                
                return cr.name;
            }
        }
        return "";
    }

    public bool CharacterExist(CharacterFunction characterFunction)
    {
        foreach (CharacterName cr in characterNames)
        {
            if (cr.characterFunction == characterFunction)
            {
                return true;
            }
        }
        return false;
    }


    public Color GetCharacterColor(string characterFunctionString)
    {
        // nameof(MyEnum.EnumValue);
        Color color = new Color();
        string[] speakersArray = Enum.GetNames(typeof(CharacterFunction));
        foreach (string speaker in speakersArray)
        {
            //Debug.LogError(speaker + "  ==  " + characterFunctionString);
            if (speaker == characterFunctionString)
            {
                CharacterFunction cf = DialogueChainPreferences.GetCharacterFunctionWithName(characterFunctionString);
                color = cf == CharacterFunction.MainCharacter? mainCharacterColor : GetCharacterNameObj(cf).displayColor;
                return color;
            }
        }
        //Debug.LogError(color.r + "," + color.g + "," + color.b + "," + color.a);
        color = mainCharacterColor;
        return color;
    }

}

[System.Serializable]
public class CharacterName
{
    public string name;
    public CharacterFunction characterFunction;
    public Color displayColor;

    public CharacterName(CharacterFunction _characterFunction)
    {
        characterFunction = _characterFunction;
        name = _characterFunction.ToString();
    }
}
