using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public PlayerInfo playerInfo;

	// Use this for initialization
	void Start () {

        LinkPlayerControllerToGameManager();
    }
	
	// Update is called once per frame
	void Update () {
        // PULL only when player is in navigation state

        // push each time a new event occur
    }

    // Link player data to GameManager
    public void LinkPlayerControllerToGameManager() {


        GameData save = GameManager.instance.managers.variablesManager.gameSavedData;

        

        playerInfo = save.GameInfo;
    }
    
    public string GetNpcName(CharacterFunction function)
    {
        foreach(Character character in playerInfo.Npcs)
        {
            if (character.function == function)
            {
                return character.name;
            }
        }
        return "";
    }
}
