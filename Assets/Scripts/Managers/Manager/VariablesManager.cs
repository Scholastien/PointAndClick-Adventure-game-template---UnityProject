using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScheduleSystem;

/// <summary>
/// Variable manager holds every variable in the game:
///       - player infos that need to be saved
///       - Collections (scriptable objects) that have const data in the game, such as locations and items
/// </summary>

public class VariablesManager : MonoBehaviour {

    public static VariablesManager instance = null;


    public GUI_properties GUI_Properties;

    public GameData gameSavedData;
    

    // Each location have a "All_typeofcollection" field that holds those data
    // the field that we want to modify is a List
    [Header("Collections")]
    public QuestCollection questCollection; // Can't be done because it requires IDs
    public ItemCollection itemCollection; //Done
    public LocationCollection locationCollection; // Can't be done because it requires IDs
    public ScheduleChangesCollection scheduleChangesCollection;
    public TriggerCollection triggerCollection;  //Done
    public GalleryCollection galleryCollection; // Done
    public MailCollection mailCollection; // Done
    public CharactersCollection charactersCollection;

    [Header("Current Data")]
    private Room currentRoom;
    private Location currentLocation;
    public string previousRoomName;
    public int previousRoom;
    public int previousLocation;

    // Private variables
    private NavigationManager nm;


    #region MonoBehaviour

    private void OnEnable()
    {
        EventManager.isLoadedFromSave += GetNewValues;
    }

    private void OnDisable()
    {
        EventManager.isLoadedFromSave -= GetNewValues;
    }


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(this);
        }


        // Init in Awake so it's called before everything
        // Each Collection should have a Init function
        // that function load every object (item, location, quest) in the games folder
        // and copy it in the collection
        triggerCollection.Init();
        itemCollection.Init();
        scheduleChangesCollection.Init();
        galleryCollection.Init();
        questCollection.Init();
        mailCollection.Init();
        gameSavedData = new GameData(false);
    }

    // Use this for initialization
    void Start () {
        
        nm = GameManager.instance.managers.navigationManager;
        // Save the data that after loading in awake()
        //PersistentData.instance.Save();
        questCollection.Init();
    }
	
	// Update is called once per frame
	void LateUpdate () {

        currentRoom = locationCollection.GetRoom(gameSavedData.GameInfo.currentRoom);
        currentLocation = locationCollection.GetLocation(currentRoom);
        triggerCollection.UpdateBoolValues();
        locationCollection.UpdateBoolValues();
        mailCollection.UpdateBoolValues();
    }
    #endregion


    #region LoadedFromSave

    void GetNewValues()
    {
        int id = GameManager.instance.saveIndex;

        gameSavedData = new GameData(GameManager.instance.managers.saveManager.saves[id]);

        EventManager.Instance.UpdatedValues();
    }

    #endregion



    public GameData GetGameData() {
        GameData result = new GameData(gameSavedData);
        return result;
    }

    public void SetRoom(int locationId, int roomId) {

        previousRoom = roomId;
        previousLocation = locationId;
        gameSavedData.GameInfo.currentRoom = locationCollection.locations[locationId].rooms[roomId].name;
    }

    public void GoToPreviousRoom()
    {
        SetRoom(previousLocation, previousRoom);
    }

    public Tuple<int, int> GetPositionID(string roomName)
    {
        Tuple<int, int> result = new Tuple<int, int>(0, 0);
        for (int i = 0; i < locationCollection.locations.Count; i++)
        {
            for (int j = 0; j < locationCollection.locations[i].rooms.Count; j++)
            {
                if (locationCollection.locations[i].rooms[j].name == roomName)
                {
                    result = new Tuple<int, int>(i, j);
                }
            }
        }

        return result;
    }

    public string GetLocationDisplayName(int locationId)
    {
        return locationCollection.locations[locationId].displayName;
    }
}


