using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerInfo
{

    public MainCharacter mainCharacter;

    public List<Npc> Npcs;

    //Everything here is used with dialogue chains. Customize the script DialogueChainPreferences to reflect your own variables.

    // current location of the player

    // current room of the player
    public string currentRoom;

    public int intTime;
    public int intDay;




    public List<Sprite> playerDialogueAvatars;

    // Data setter for new game -> Set a new player
    public PlayerInfo(bool forceCharactercreation = true)
    {

        mainCharacter = new MainCharacter();

        intTime = 0;
        intDay = 0;
        if (forceCharactercreation)
            CreateCharacters();

    }

    // Load a player from save file
    public PlayerInfo(PlayerInfo playerInfo)
    {
        mainCharacter = new MainCharacter(playerInfo.mainCharacter);

        Npcs = playerInfo.Npcs;

        currentRoom = playerInfo.currentRoom;


        intTime = playerInfo.intTime;
        intDay = playerInfo.intDay;




    }



    #region Npcs
    // Here we're gonna create a new character object for each character function (exept main character)
    public void CreateCharacters()
    {
        Array arrayEnum = Enum.GetValues(typeof(CharacterFunction));

        Npcs = new List<Npc>();

        foreach (CharacterFunction enumValue in arrayEnum)
        {

            Debug.LogWarning(enumValue.ToString() + " " + GameManager.instance.managers.variablesManager.charactersCollection.characterNames.Count);

            if (enumValue != CharacterFunction.MainCharacter)
            {
                string name = GameManager.instance.managers.variablesManager.charactersCollection.GetCharacterName(enumValue);
                Npcs.Add(new Npc(name, enumValue));
            }
        }
    }

    public void RenameCharacter(Character newCharacter)
    {
        foreach (Character character in Npcs)
        {
            if (character.function == newCharacter.function)
            {
                character.name = newCharacter.name;
            }
        }
    }

    public string GetCharacterNameWithFunction(CharacterFunction characterFunction)
    {
        foreach (Character character in Npcs)
        {
            if (character.function == characterFunction)
            {
                return character.name;
            }
        }
        return mainCharacter.name;
    }

    public Npc GetNpcWithFunction(CharacterFunction characterFunction)
    {
        foreach (Npc character in Npcs)
        {
            if (character.function == characterFunction)
            {
                return character;
            }
        }
        return Npcs[0];
    }

    public string GetCharacterNameWithString(string characterFunction)
    {
        foreach (Character character in Npcs)
        {
            if (character.function.ToString() == characterFunction)
            {
                return character.name;
            }
        }
        return mainCharacter.name;
    }

    public void MetCharacter(CharacterFunction characterFunction)
    {
        foreach (Npc npc in Npcs)
        {
            if (npc.function == characterFunction)
            {
                npc.alreadyMet = true;
            }
        }
    }

    public void ModifyCharacterLocation(CharacterFunction characterFunction, Room room, bool display)
    {
        if (room != null)
        {
            foreach (Character character in Npcs)
            {
                if (character.function == characterFunction)
                {
                    if (display)
                        character.currentLocation = room.displayName;
                    else
                        character.currentLocation = String.Empty;
                }
            }
        }
    }

    public int GetCharacterRelationshipValue(CharacterFunction characterFunction)
    {
        foreach (Npc npc in Npcs)
        {
            if (npc.function == characterFunction)
            {
                return npc.relationshipPoint;
            }
        }

        return -1;
    }

    public void SetCharacterRelationshipValue(CharacterFunction characterFunction, int value)
    {
        foreach (Npc npc in Npcs)
        {
            if (npc.function == characterFunction)
            {
                npc.relationshipPoint = value;
            }
        }
    }

    public void OperateOnRelationshipPoint(CharacterFunction characterFunction, int value)
    {
        foreach (Npc npc in Npcs)
        {
            if (npc.function == characterFunction)
            {
                npc.relationshipPoint += value;
            }
        }
    }
    #endregion
}
