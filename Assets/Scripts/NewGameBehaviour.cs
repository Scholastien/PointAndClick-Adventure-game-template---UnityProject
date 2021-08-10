using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewGameBehaviour : MonoBehaviour
{

    public Image CharacterBackground;
    public Text Instructions;
    public InputField inputField;
    public Text PlaceHolderText;
    public Text InputText;
    public Button Validate;

    public List<Character> newCharacters;

    public int currentIndex = 0;
    public List<bool> named;

    // Use this for initialization
    void Awake()
    {

        GameManager.instance.naming = true;
        newCharacters = new List<Character>();
        if (GameManager.instance.charactersToRename.Count > 0)
        {
            newCharacters = GameManager.instance.charactersToRename;
            foreach (Character character in newCharacters)
            {
                named.Add(false);
            }

            StartCoroutine(WaitForNaming());
        }
        //else
        //{
        //    GameManager.instance.naming = false;
        //    GameManager.instance.gameState = GameState.Navigation;
        //}
    }


    public string GetCorrectName(CharacterFunction characterFunction)
    {
        switch (characterFunction)
        {
            case CharacterFunction.MainCharacter:
                return "the main character's";
            case CharacterFunction.test:
                return "your [test]'s";
            default:
                return "";
        }
    }

    public void SpawnRenameWindow()
    {
        string instruction = "Please enter " + GetCorrectName(newCharacters[currentIndex].function) + " name :";

        //Spawn window
        CharacterBackground.gameObject.name = "Rename - " + newCharacters[currentIndex].function;
        Instructions.text = LanguageManager.instance.GetTranslation(instruction);
        PlaceHolderText.text = newCharacters[currentIndex].name;
        inputField.text = "";
    }

    public void StartRenaming()
    {
        string newName = "";

        if (CheckValidity(InputText.text))
        {

            if (InputText.text == "")
            {
                newName = newCharacters[currentIndex].name;
            }
            else
            {
                newName = InputText.text;
            }

            if (newCharacters[currentIndex].function != CharacterFunction.MainCharacter)
                GameManager.instance.managers.variablesManager.gameSavedData.GameInfo.RenameCharacter(new Character(newName, newCharacters[currentIndex].function));
            else
                GameManager.instance.managers.variablesManager.gameSavedData.GameInfo.mainCharacter.name = newName;
            named[currentIndex] = true;
        }
        else
        {
            // Wrong name format behaviour
        }

    }

    public bool CheckValidity(string textToCheck)
    {
        foreach (char c in textToCheck)
        {
            if (c == ' ')
                return false;
        }
        return true;
    }

    public IEnumerator WaitForNaming()
    {
        while (currentIndex < newCharacters.Count)
        {
            if (currentIndex < newCharacters.Count - 1)
            {
                SpawnRenameWindow();

                yield return new WaitUntil(() => named[currentIndex]);

                currentIndex++;
            }
            else
            {
                SpawnRenameWindow();

                yield return new WaitUntil(() => named[currentIndex]);

                // Close renaming because end
                GameManager.instance.naming = false;
                GameManager.instance.charactersToRename = new List<Character>();
            }
        }
    }


    public void UnselectUI()
    {
        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);

    }




}
