using UnityEngine;
using System.Collections;
using ScheduleSystem;
using System.Collections.Generic;       //Allows us to use Lists. 

[System.Serializable]
public class Managers
{
    public SceneLoaderManager sceneManager;
    public VariablesManager variablesManager;
    public SaveAndLoad saveManager;
    public QuestManager questManager;
    public InventoryManager inventoryManager;
    public TimeManager timeManager;
    public DialogueManager dialogueManager;
    public NavigationManager navigationManager;
    public ScheduleManager scheduleManager;
    public InterractableManager interractableManager;
    public AudioManager audioManager;
    public OptionManager optionManager;
    public PersistentData persistentData;
    //public LanguageManager languageManager;
    public LocalisationSystem LocalisationSystem;
    public VideoPlayerManager videoManager;
    public NotificationManager notificationManager;
    public MinigameManager minigameManager;
    public EventManager eventManager;

}

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.

    public int saveIndex = 0;

    public GameState gameState;
    public Managers managers;

    [Header("New Game")]
    public DialogueChain IntroChain;
    public bool naming = false;
    public List<Character> charactersToRename;

    [Header("Gallery")]
    public bool galleryOpenned = false;

    private PlayerController playerCtrl;

    [Header("DialogueCtrl")]
    public bool haltMovement;

    [Header("Main Menu")]
    public bool returnMainMenu = false;




    //private BoardManager boardScript;                       //Store a reference to our BoardManager which will set up the level.
    //private int level = 3;                                  //Current level number, expressed in game as "Day 1".

    //Awake is always called before any Start functions
    void Awake()
    {

        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);


        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        //Get a component reference to the attached BoardManager script
        //boardScript = GetComponent();

        //Call the InitGame function to initialize the first level 
        InitGame();
    }

    //Initializes the game for each level.
    void InitGame()
    {
        //Call the SetupScene function of the BoardManager script, pass it current level number.
        //boardScript.SetupScene(level);
    }



    //Update is called every frame.
    void Update()
    {
        managers.navigationManager.enabled = (GameState.MainMenu != gameState);
        managers.questManager.enabled = !(GameState.NewGame == gameState || GameState.MainMenu == gameState);
    }
    

    public void LinkData(bool loaded)
    {
        playerCtrl = FindObjectOfType<PlayerController>();

        instance.managers.scheduleManager.npcSerializedInfos = instance.managers.variablesManager.gameSavedData.GetNpcSerializedSchedule();

        instance.managers.questManager.currentQuests = instance.managers.questManager.questCollection.InitCurrentQuest(instance.managers.questManager.currentQuests, loaded);

        instance.managers.questManager.Init(loaded);

        instance.managers.inventoryManager.Init();



        playerCtrl.LinkPlayerControllerToGameManager();

    }




    public void OnApplicationQuit()
    {
        //Debug.Log("display quit window");
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}