using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SaveType
{
    defaultSave,
    quickSave,
    autoSave
}

[System.Serializable]
public class UI_SavedData
{
    public int id;

    public string savedTypeOrIndex;
    public string savedNameWithTimestamp;
    public string savedCustomName;
    public string savedPlayerLocationAndTime;
    public string savedMoney;
    public string savedPreviewImgPath;

    public Sprite tmp_SpritePreview;

    public UI_SavedData(GameData gameData, int index)
    {
        id = index;


        savedTypeOrIndex = gameData.saveType == SaveType.defaultSave ? gameData.name : gameData.saveType.ToString();
        savedNameWithTimestamp = gameData.timestampSave;
        savedCustomName = gameData.customName;

        VariablesManager vm = GameManager.instance.managers.variablesManager;
        Room savedRoom = vm.locationCollection.GetRoom(gameData.GameInfo.currentRoom);
        Location savedLocation = vm.locationCollection.GetLocation(savedRoom);
        savedPlayerLocationAndTime = savedLocation.displayName + ", " + (DayOfTheWeek)gameData.GameInfo.intDay + ", " + (TimeOfTheDay)gameData.GameInfo.intTime;

        savedMoney = gameData.GameInfo.mainCharacter.money.ToString() + "$";
        savedPreviewImgPath = gameData.imgPreview_Path;



        savedRoom.GetNavIcon((TimeOfTheDay)gameData.GameInfo.intTime);
        tmp_SpritePreview = savedRoom.navIcon; // tmp sprite waiting for screenshot


    }
}

// Serializable class that hold every variable on the current playing session
[System.Serializable]
public class GameData
{
    [Header("Raw data")]
    public SaveType saveType;
    public string name = "save";

    public int saveID;
    public string timestampSave;
    public string customName;
    public string imgPreview_Path;

    public PlayerInfo GameInfo;

    public List<bool> progressionTab;
    public List<int> activeQuest;
    public List<bool> locationLockState;
    public List<SerializedMail> mailsState;
    public List<string> inventory;
    public List<NpcSerializedSchedule> npcSerializedInfos;



    public GameData(bool forceCharactercreation = true)
    {
        GameInfo = new PlayerInfo(forceCharactercreation);

        progressionTab = new List<bool>();
        activeQuest = new List<int>();
        locationLockState = new List<bool>();
        mailsState = new List<SerializedMail>();
        inventory = new List<string>();
        npcSerializedInfos = new List<NpcSerializedSchedule>();
    }

    public UI_SavedData GetUiData(int index)
    {
        return new UI_SavedData(this, index);
    }

    public List<NpcSerializedSchedule> GetNpcSerializedSchedule()
    {
        return npcSerializedInfos;
    }
    /// <summary>
    /// DONT FORGET TO WRITE YOUR VARIABLE YOU WANT TO SAVE HERE YOU MORON
    /// </summary>
    /// <param name="gameData"></param>
    public GameData(GameData gameData)
    {
        saveType = gameData.saveType;
        name = gameData.name;
        saveID = gameData.saveID;

        timestampSave = gameData.timestampSave;
        customName = gameData.customName;
        imgPreview_Path = gameData.imgPreview_Path;

        GameInfo = new PlayerInfo(gameData.GameInfo);

        progressionTab = new List<bool>(gameData.progressionTab);
        activeQuest = new List<int>(gameData.activeQuest);
        locationLockState = new List<bool>(gameData.locationLockState);
        mailsState = new List<SerializedMail>(gameData.mailsState);
        inventory = new List<string>(gameData.inventory);
        npcSerializedInfos = new List<NpcSerializedSchedule>(gameData.npcSerializedInfos);
    }
}

