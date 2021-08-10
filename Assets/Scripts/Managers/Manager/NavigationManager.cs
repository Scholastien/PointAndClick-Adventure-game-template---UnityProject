using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NavigationManager : MonoBehaviour
{
    public static NavigationManager instance = null;

    [Header("UI")]
    public bool needToOpenTheNavBar = false;

    [Header("Debugger")]
    public bool Debug_NavigationLog;

    [Header("Current Data")]
    public Room currentRoom;
    public Location currentLocation;
    public string previousRoomName;
    public int previousRoom;
    public int previousLocation;

    [Header("Processed Values")]
    public float fadeDuration = 0.1f;
    [Range(0, 1)]
    public float fadeValue = 0f;

    // Private variables
    private LocationCollection locationCollection;
    private bool start;
    private bool loadNextRoom = false;


    public bool needToOpen = false;


    public void Init()
    {
        if (currentRoom != null)
        {
            if (Debug_NavigationLog)
                Debug.Log("<color=#cc66ff>" + "Setting PreviousRoom" + "</color>:" + currentRoom.name);
            SetRoom(GetGameData().GameInfo.currentRoom);
            UpdatePreviousPosition();
            start = false;
            if (GameManager.instance.gameState != GameState.MainMenu)
                SendLocationMusicToAM();
        }
    }

    #region Event subscription

    private void OnEnable()
    {
        EventManager.onNewValues += FadeWhileLoading;
        EventManager.onFadeIn += FadeWhileLoading;
        EventManager.onQuestUpdate += QuestUpdated;
        EventManager.isLoadedFromSave += OpenUI;
        EventManager.onEndOfDialogue += OpenNavBar;
    }

    private void OnDisable()
    {
        EventManager.onNewValues -= FadeWhileLoading;
        EventManager.onFadeIn -= FadeWhileLoading;
        EventManager.onQuestUpdate -= QuestUpdated;
        EventManager.isLoadedFromSave -= OpenUI;
        EventManager.onEndOfDialogue += OpenNavBar;
    }

    void OpenUI()
    {
        //Debug.LogError("OpenUI");
        needToOpen = true;
    }

    void OpenNavBar()
    {
        //Debug.LogError("Opening the navBar");
        needToOpenTheNavBar = true;
    }

    #endregion

    #region MonoBehaviour




    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    // Use this for initialization
    void Start()
    {
        start = true;
        fadeValue = 0f;
        locationCollection = GameManager.instance.managers.variablesManager.locationCollection;
    }


    // Update is called once per frame
    void Update()
    {

        if (start)
        {
            Init();
        }

        currentRoom = locationCollection.GetRoom(GetGameData().GameInfo.currentRoom);
        currentLocation = locationCollection.GetLocation(currentRoom);
    }

    #endregion

    void FadeWhileLoading()
    {
        StartCoroutine(FadeIn());
    }

    void QuestUpdated()
    {
        loadNextRoom = true;
    }


    public GameData GetGameData()
    {
        return GameManager.instance.managers.variablesManager.gameSavedData;
    }

    public void UpdatePreviousPosition()
    {
        previousRoomName = GetGameData().GameInfo.currentRoom;
        previousLocation = locationCollection.GetLocationID(previousRoomName);
        previousRoom = locationCollection.GetRoomID(locationCollection.locations[previousLocation], previousRoomName);

        //GameManager.instance.managers.variablesManager.currentRoom = locationCollection.GetRoom(GetGameData().playerInfo.currentRoom);
        //GameManager.instance.managers.variablesManager.currentLocation = locationCollection.GetLocation(currentRoom);
    }

    public Room GetRoom(string roomName)
    {
        return locationCollection.GetRoom(roomName);
    }

    public void SetRoom(string newRoom)
    {
        UpdatePreviousPosition();
        //StartCoroutine(FadeToNewRoom(newRoom));

        ChangeRoom(newRoom);
    }

    public IEnumerator FadeToNewRoom(string newRoom = null)
    {
        //yield return null;
        // update a public float value from 0 to 1 and vice versa

        //fadeout

        // loop over 1 second
        for (float i = 0; i <= fadeDuration; i += Time.deltaTime)
        {
            // set color with i as alpha
            fadeValue = i / (fadeDuration);
            yield return null;
        }
        fadeValue = 1f;

        yield return new WaitUntil(() => loadNextRoom == true);

        if (newRoom != null)
            //ChangeRoom(newRoom);
            //fadein
            // loop over 1 second backwards
            for (float i = fadeDuration; i >= 0; i -= Time.deltaTime)
            {
                // set color with i as alpha
                // min value = i
                // max value = fadeDuration / 2
                // ratio value
                fadeValue = i / (fadeDuration);
                yield return null;
            }
        fadeValue = 0f;


        loadNextRoom = false;
    }

    public IEnumerator FadeIn()
    {
        float temp_duration = 1f;

        //yield return null;
        // update a public float value from 0 to 1 and vice versa

        // loop over 1 second backwards
        for (float i = temp_duration; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            // min value = i
            // max value = fadeDuration / 2
            // ratio value
            fadeValue = i / (temp_duration);
            yield return null;
        }
        fadeValue = 0f;


    }

    public void ChangeRoom(string newRoom)
    {
        GetGameData().GameInfo.currentRoom = newRoom;
        if (Debug_NavigationLog)
            Debug.Log("set room to: " + newRoom);
        currentRoom = locationCollection.GetRoom(GetGameData().GameInfo.currentRoom);
        currentLocation = locationCollection.GetLocation(currentRoom);
        currentLocation = locationCollection.GetLocation(currentRoom);
        if (Debug_NavigationLog)
        {
            Debug.Log("currentLocation : " + currentLocation.displayName);
            Debug.Log("currentRoom : " + currentRoom.displayName);
        }
        if (!start)
            SendLocationMusicToAM();
    }

    public void SetRoom(int locationId, int roomId)
    {
        UpdatePreviousPosition();
        GetGameData().GameInfo.currentRoom = locationCollection.locations[locationId].rooms[roomId].name;

        if (Debug_NavigationLog)
            Debug.Log("<color=##9999ff>" + "SetRoom" + "</color>:   <color=yellow> \n"
                + GetPositionID(currentRoom.name).Item1 + "</color> -> " + "<color=yellow> " + GetPositionID(currentRoom.name).Item2 + " </color>");

        currentRoom = locationCollection.GetRoom(GetGameData().GameInfo.currentRoom);
        currentLocation = locationCollection.GetLocation(currentRoom);
        currentLocation = locationCollection.GetLocation(currentRoom);
        SendLocationMusicToAM();
    }

    public void GoToPreviousRoom()
    {
        if (Debug_NavigationLog)
            Debug.Log("<color=#cc66ff>" + "Go to PreviousRoom" + "</color>:   <color=yellow> \n"
                + GetLocationDisplayName(previousLocation) + "</color> -> " + "<color=yellow> " + previousRoomName + " </color>");
        SetRoom(previousLocation, previousRoom);

        currentRoom = locationCollection.GetRoom(GetGameData().GameInfo.currentRoom);
        currentLocation = locationCollection.GetLocation(currentRoom);
        currentLocation = locationCollection.GetLocation(currentRoom);
        SendLocationMusicToAM();
    }


    #region Getter

    public Tuple<int, int> GetPositionID(string roomName)
    {
        Tuple<int, int> result = new Tuple<int, int>(0, 0);
        for (int i = 0; i < locationCollection.locations.Count; i++)
        {
            for (int j = 0; j < locationCollection.locations[i].rooms.Count; j++)
            {
                if (locationCollection.locations[i].rooms[j].name == roomName)
                {
                    result = new Tuple<int, int>(j, i);

                }
            }
        }

        return result;
    }

    public string GetLocationDisplayName(int locationId)
    {
        return locationCollection.locations[locationId].displayName;
    }


    public bool IsNavigationBarRequiered()
    {
        int i = 0;
        foreach (Room room in currentLocation.rooms)
        {
            if (!room.locked)
                i++;
        }
        if (i > 1)
            return true;
        else
            return false;
    }

    #endregion

    #region Audio
    public void SendLocationMusicToAM()
    {
        currentLocation = locationCollection.GetLocation(currentRoom);
        currentLocation.locationSound.audioType = AudioType.Music;
        currentLocation.locationSound.loop = true;
        currentLocation.locationSound.clipName = currentLocation.displayName;

        if (AudioManager.instance.Debug_AudioLog)
            Debug.Log("<color=yellow>SendLocationMusicToAM </color>: " + currentLocation.displayName);

        AudioManager.instance.currentLocationSound = currentLocation.locationSound;

        AudioManager.instance.GetLocationMusic(currentLocation.locationSound);
    }
    #endregion
}
