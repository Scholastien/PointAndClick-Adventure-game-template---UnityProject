using System.Collections.Generic;
using UnityEngine;
using InventorySystem;
using System;
//using UnityEditor;

public static class DialogueChainPreferences
{
    //New features in v1.1:

    //Costmetic change. Max character count for dialogue boxes. Box will turn red when there are more characters than this. You can still use it when it's red, it's just a warning for you. Zero = unlimited.
    public static int maxCharCount = 0;

    //Cosmetic change for Item node, instead of experience, you can call it whatever you want.
    public static string experienceString = "Experience";


    //For the next three options: For each input button, this will add something before the text. Pick one you'd like, or make your own.
    //If you have more input buttons than strings in the array, it will only use the first entry for the extra buttons.

    //Number your input buttons. Add more numbers if you'll ever have more than this many options in an input box.
    public static string[] characterBeforeInputText = new string[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

    //Un-comment the next line and comment the above line to have a hyphen before each option. Change the hyphen to an asterisk or whatever you'd like.
    //public static string[] characterBeforeInputText = new string[] { "-" };

    //Un-comment the next line and comment the above string arrays to have nothing added before each user input option.
    //public static string[] characterBeforeInputText = new string[] { "" };



    //End of new features in v1.1



    //The input commands in Unity's "Edit/Project Settings/Input" that you want to use to advance dialogue. You can have multiple, separate with a comma.
    public static string[] inputsToAdvanceDialogue = new string[] { "Submit" };

    //The input commands in Unity's "Edit/Project Settings/Input" that you want to use to make the typing dialogue text complete itself.
    public static string[] inputsToAdvanceDialogueQuickly = new string[] { "Submit" };


    //Default Values for newly created dialogue chains.
    public static bool defaultShowSpeakerImage = true;
    public static bool defaultShowSpeakerNameBox = true;
    public static ContainerType defaultContainerType = 0;


    //The pathways to the folders with your chains and chain triggers. These pathways must be in the Resources folder and these strings assume the root is the
    //Resources folder. The pathways need to be in the Resources folder for the built in saving and loading method to work, or you can design your own saving/loading method.
    //Defaults = "ChainResources/Chains"  and  "ChainResources/ChainTriggers"
    //DO NOT PUT "Resources/ChainResources/ChainTriggers" or "Assets/Resources/ChainResources/ChainTriggers. Only the pathway after Resources.
    public static string chainAssetPathway = "ChainResources/Chains";
    public static string triggerAssetPathway = "ChainResources/Triggers";

    //This array will always be added to a new chain's list of speakers. Add names that often appear in dialogue for convenience.
    //Example: new string[] {"Tom", "Susie", "Mr. Hats"};
    //If you use a variable for your player name, you sould keep "Player" in the list. Any speaker set to Player will get the player's name automatically.
    public static CharacterFunction defaultSpeakerList;


    //return your project's bool that halts character movement so when set to true the player can't control their characte.
    public static bool GetHaltMovement()
    {
        return GameManager.instance.haltMovement;
    }
    //change the variable to your project's haltmovent bool. This allows the dialogue chain to control the halt movement variable.
    public static void SetHaltMovement(bool setTo)
    {
        GameManager.instance.haltMovement = setTo;
    }

    public static CharacterFunction GetCharacterFunctionWithName(string characterFunctionString)
    {
        foreach (CharacterFunction cf in Enum.GetValues(typeof(CharacterFunction)))
        {
            if (cf.ToString() == characterFunctionString)
            {
                //Debug.LogError(cf.ToString() + "  ==  " + characterFunctionString);
                return cf;
            }
        }
        return CharacterFunction.MainCharacter;
    }


    //return the variable that stores the player's name. Or just return the player's name if you're not using a variable.
    //This is only used if you put "Player" as a speaker in dialogue. It retrieves the player's name with this method.
    //If you're not using a variable to store the player's name, I still suggest returning the player name you choose here, and using "Player" as the speaker.
    //That way if you decide to change the player's name halfway through your game, your dialogue will update if you return the new name here.
    public static string GetCharacterNameWithFuntion(CharacterFunction characterFunction)
    {
        return GameManager.instance.managers.variablesManager.gameSavedData.GameInfo.GetCharacterNameWithFunction(characterFunction);

        //return GameManager.instance.managers.variablesManager.gameSavedData.GameInfo.mainCharacter.name;

        //return TempGameController.instance.player.GetComponent<TempPlayerInfo>().playerName;
    }
    public static string GetCharacterNameWithString(string characterFunctionString)
    {
        if (DialogueManager.instance.debug_ConversationLog)
            Debug.Log("Trying to get this string : " + characterFunctionString);
        return GameManager.instance.managers.variablesManager.gameSavedData.GameInfo.GetCharacterNameWithString(characterFunctionString);

        //return GameManager.instance.managers.variablesManager.gameSavedData.GameInfo.mainCharacter.name;

        //return TempGameController.instance.player.GetComponent<TempPlayerInfo>().playerName;
    }

    //New feature allows you to change the string from "experience" to whatever you want. It's still called experience here, but you can make it add to whatever variable you want. See the top of this script.
    //change the variable that stores the player's experience so it can be changed. 
    //If you're not going to use experience changing nodes, you should leave the method but delete its contents.
    public static void AddToPlayerExperience(int amount)
    {
        GameManager.instance.managers.variablesManager.gameSavedData.GameInfo.mainCharacter.experience += amount;
    }


    //If you are using variables to display your player's avatar during dialogue. I suggest making a list so that you can easily choose different expressions.
    //If you only have one avatar, but using a variable to choose it, you'll still need a list, but it can have only one element.
    //return your list of player avatar expressions here.
    public static List<Sprite> GetPlayerDialogueAvatars()
    {
        return GameManager.instance.managers.variablesManager.gameSavedData.GameInfo.playerDialogueAvatars;
    }

    //You'll have to customize these methods with your specific integers if you want to use Dialogue Chains to adjust numbers or use their values as a requirement.
    //First: Add your possible ints to the enum ChainIntType (at the end of this script). Feel free to clear the enum of its original values.
    //Second: Adjust the switch statements to reflect your own integer variables. In the SetChainInt method, have each variable += amount
    public static int GetChainInt(ChainIntType chainInt)
    {
        switch (chainInt)
        {
            case ChainIntType.RidingSkill:

                return GameManager.instance.managers.variablesManager.gameSavedData.GameInfo.mainCharacter.GetSkillValue(SkillType.Riding);
            case ChainIntType.Money:
                return GameManager.instance.managers.variablesManager.gameSavedData.GameInfo.mainCharacter.money;
            case ChainIntType.testRelationship:
                return GameManager.instance.managers.variablesManager.gameSavedData.GameInfo.GetCharacterRelationshipValue(CharacterFunction.test);
            case ChainIntType.intDay:
                return GameManager.instance.managers.variablesManager.gameSavedData.GameInfo.intDay;
            case ChainIntType.intTime:
                return GameManager.instance.managers.variablesManager.gameSavedData.GameInfo.intTime;
            case ChainIntType.CurrentEnergy:
                return GameManager.instance.managers.variablesManager.gameSavedData.GameInfo.mainCharacter.currentEnergy;
            case ChainIntType.MaxEnergy:
                return GameManager.instance.managers.variablesManager.gameSavedData.GameInfo.mainCharacter.GetSkillValue(SkillType.Energy);
            default:
                return 0;
        }
    }

    // 
    public static void AddToChainInt(ChainIntType chainInt, int amount)
    {
        int value = 0;

        switch (chainInt)
        {
            case ChainIntType.RidingSkill:
                value = GameManager.instance.managers.variablesManager.gameSavedData.GameInfo.mainCharacter.GetSkillValue(SkillType.Riding);
                value += amount;
                GameManager.instance.managers.variablesManager.gameSavedData.GameInfo.mainCharacter.SetSkillValue(SkillType.Riding, value);
                break;
            case ChainIntType.Money:
                GameManager.instance.managers.variablesManager.gameSavedData.GameInfo.mainCharacter.money += amount;
                break;
            case ChainIntType.testRelationship:


                value = GameManager.instance.managers.variablesManager.gameSavedData.GameInfo.GetCharacterRelationshipValue(CharacterFunction.test);
                value += amount;
                GameManager.instance.managers.variablesManager.gameSavedData.GameInfo.SetCharacterRelationshipValue(CharacterFunction.test, value);
                break;
            case ChainIntType.intDay:


                // New day behaviour
                VariablesManager.instance.gameSavedData.GameInfo.mainCharacter.FillEnergy();

                GameManager.instance.managers.variablesManager.gameSavedData.GameInfo.intDay += amount;
                break;
            case ChainIntType.intTime:
                GameManager.instance.managers.variablesManager.gameSavedData.GameInfo.intTime += amount;
                break;
            default:
                break;
        }
    }

    public static void SetChainInt(ChainIntType chainInt, int value)
    {
        switch (chainInt)
        {
            case ChainIntType.RidingSkill:
                GameManager.instance.managers.variablesManager.gameSavedData.GameInfo.mainCharacter.SetSkillValue(SkillType.Riding, value);
                break;
            case ChainIntType.Money:
                GameManager.instance.managers.variablesManager.gameSavedData.GameInfo.mainCharacter.money = value;
                break;
            case ChainIntType.intTime:
                GameManager.instance.managers.variablesManager.gameSavedData.GameInfo.intTime = value;
                break;
            case ChainIntType.intDay:
                GameManager.instance.managers.variablesManager.gameSavedData.GameInfo.intDay = value;
                break;
            case ChainIntType.testRelationship:
                GameManager.instance.managers.variablesManager.gameSavedData.GameInfo.SetCharacterRelationshipValue(CharacterFunction.test, value);
                break;
            case ChainIntType.CurrentEnergy:
                GameManager.instance.managers.variablesManager.gameSavedData.GameInfo.mainCharacter.currentEnergy = value;
                break;
            case ChainIntType.MaxEnergy:
                GameManager.instance.managers.variablesManager.gameSavedData.GameInfo.mainCharacter.SetSkillValue(SkillType.Energy, value);
                break;
            default:
                break;
        }
    }



    //Below is where you can fit your item management system into dialogue chains. To use your items with dialogue chains, if they are scriptable objects,
    //their class must either be called or inherit from "Item".

    //Dialogue chains uses scriptable objects for Items by default. If your project doesn't use scriptable objects for items, change itemsAreScriptableObjects to false.
    public static bool itemsAreScriptableObjects = true;

    //Customize these three methods if you're using scriptable objects for items. Ignore them if you're not.
    //The methods must only have an Item passed to them.
    public static void AddToInventory(Item item)
    {
        GameManager.instance.managers.inventoryManager.PickItem(item);
        //Dictionary<Item, int> inventory = TempGameController.instance.inventory;

        //if (inventory.ContainsKey(item))
        //{
        //    inventory[item]++;
        //}
        //else
        //{
        //    inventory.Add(item, 1);
        //}
    }
    public static void RemoveFromInventory(Item item)
    {
        GameManager.instance.managers.inventoryManager.RemoveItem(item);

        //Dictionary<Item, int> inventory = TempGameController.instance.inventory;

        //if (inventory.ContainsKey(item))
        //{
        //    inventory[item]--;
        //    if (inventory[item] <= 0)
        //    {
        //        inventory.Remove(item);
        //    }
        //}
    }
    public static bool InventoryContains(Item item)
    {
        if (GameManager.instance.managers.inventoryManager.ItemOccurence(item) == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }


    //Customize these three methods if your project doesn't use scriptable objects for items. Ignore them if it does.
    //the methods must only have a string passed to them.
    public static void AddToInventory(string itemString)
    {
        Dictionary<string, int> inventoryNotScriptable = TempGameController.instance.inventoryNotScriptable;

        if (inventoryNotScriptable.ContainsKey(itemString))
        {
            inventoryNotScriptable[itemString]++;
        }
        else
        {
            inventoryNotScriptable.Add(itemString, 1);
        }
        return;
    }
    public static void RemoveFromInventory(string itemString)
    {
        Dictionary<string, int> inventoryNotScriptable = TempGameController.instance.inventoryNotScriptable;

        if (inventoryNotScriptable.ContainsKey(itemString))
        {
            inventoryNotScriptable[itemString]--;
            if (inventoryNotScriptable[itemString] <= 0)
            {
                inventoryNotScriptable.Remove(itemString);
            }
        }
        return;
    }
    public static bool InventoryContainsString(string itemString)
    {
        if (!itemsAreScriptableObjects)
        {
            return TempGameController.instance.inventoryNotScriptable.ContainsKey(itemString);
        }
        else
        {
            return false;
        }
    }
}



//These are the container names you can choose from when you create a dialogue node. Assign Dialogue Container prefabs to each of these on the Dialogue Controller.
//Add new containers by adding to the ContainerType enum. You can rename the ones already here.
public enum ContainerType
{
    //!!Important!! The index number of this enum is used. Keep the index the same if you've alread created dialogue chains that use one of these values.
    Main = 0,
    Center = 1,
    GameText = 2,

    //Containers for user inputs (questions the player answers with UI buttons) must have the word "input" or "Input" in the enum name
    Input = 3,
    InputExpandUp = 4,

    // Custom container with specific behaviours
    Thinking = 5,
    Narrator = 6
}

//These are the dialogue box image names. You can choose from these when you create a dialogue node. Assign sprites to each of these on the Dialogue Controller.
//Add new images for the dialogue boxes by adding to the BoxImages enum. You can rename the ones already here.
//Your custom box images might need to adjust their "Pixels per Unit" to get the correct sizing. This is found in the inspector under Sprite Mode while focused on the image.
public enum BoxImage
{
    //!!Important!! The index number of this enum is used. Keep the index the same if you've alread created dialogue chains that use one of these values.
    Default = 0,
    Secondary = 1,
    Extra = 2,
}

//ChainIntType contains each of the integers you wish to have available to adjust or use as a requirement in your dialogue chains.
//You must customize the GetChainInt and SetChainInt methods above for this to be useful.
//Add your own variables and feel free to change the ones already here.
public enum ChainIntType
{
    //!!Important!! The index number of this enum is used. Keep the index the same if you've alread created dialogue chains that use one of these values.
    RidingSkill = 0,
    Money = 1,
    intTime = 2,
    intDay = 3,
    testRelationship = 4,
    CurrentEnergy = 5,
    MaxEnergy = 6
}