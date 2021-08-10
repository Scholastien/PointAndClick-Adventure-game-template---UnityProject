using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadButtonBehaviour : MonoBehaviour {

    public int index;
    public string label;

    public Text saveName;
    public Text locationAndMoney;
    public Text date;

    public Image preview;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ClickLoadGame() {
        GameManager.instance.managers.saveManager.GetComponent<SaveAndLoad>().LoadGame(index);
    }

    internal void GetDataFromSave(GameData gameData)
    {
        GameData save = GameManager.instance.managers.saveManager.saves[index];
        Room savedRoom = GameManager.instance.managers.variablesManager.locationCollection.GetRoom(gameData.GameInfo.currentRoom);
        preview.sprite = savedRoom.navIcon;
        locationAndMoney.text = savedRoom.displayName + " - " + save.GameInfo.mainCharacter.money.ToString() + "$";
        date.text = gameData.timestampSave;
        saveName.text = gameData.name;
    }
}
