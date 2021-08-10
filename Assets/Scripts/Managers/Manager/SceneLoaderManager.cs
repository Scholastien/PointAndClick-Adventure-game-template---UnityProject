using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    MainMenu,
    NewGame,
    Navigation,
    Activity,
    Dialogue,
    Pause,
    Minigame
}

public class ScenesDictionnaries
{
    public Dictionary<string, string> essentials;
    public Dictionary<string, string> mainMenu;
    public Dictionary<string, string> newGame;
    public Dictionary<string, string> navigation;
    public Dictionary<string, string> activities;
    public Dictionary<string, string> dialogue;
    public Dictionary<string, string> pause;
    public Dictionary<string, string> miniGames;

    public ScenesDictionnaries()
    {
        essentials = new Dictionary<string, string> {
            {"Characters",      "Scenes/Characters" }
        };
        newGame = new Dictionary<string, string> {
            {"NewGame",         "Scenes/NewGame" }
        };
        mainMenu = new Dictionary<string, string> {
            {"MainMenu",        "Scenes/MainMenu" }
        };
        navigation = new Dictionary<string, string> {
            {"Navigation",      "Scenes/Navigation" }
        };
        activities = new Dictionary<string, string> {
            {"Activity",        "Scenes/Activity" }
        };
        dialogue = new Dictionary<string, string> {
            {"Dialogue",        "Scenes/Dialogue" }
        };
        pause = new Dictionary<string, string> {
            {"Pause",           "Scenes/Pause" }
        };
        miniGames = new Dictionary<string, string> {
            {"Mc's computer",     "Scenes/Minigames/Mc's computer" },
            {"Shop",     "Scenes/Minigames/Shop" },
            {"Bike 3D",         "Scenes/Minigames/Bike 3D" },
            {"BikeRidingMinigame",         "Scenes/Minigames/BikeRidingMinigame" },
            {"QNA DNA test",         "Scenes/Minigames/QNA DNA test" }
        };

    }

}

[System.Serializable]
public class SceneDesc
{
    public string sceneName;
    public string scenePath;

    public SceneDesc(string name, string path)
    {
        sceneName = name;
        scenePath = path;
    }
}

[System.Serializable]
public class LoadedScene
{

    [HideInInspector]
    public string name;
    [HideInInspector]
    public GameState gameState;
    public List<SceneDesc> sceneDesc;

    public LoadedScene(int enumIndex)
    {
        gameState = (GameState)Enum.ToObject(typeof(GameState), enumIndex);
        name = gameState.ToString();
        sceneDesc = new List<SceneDesc>();
    }

    public void AddNewScene(string name, string path)
    {
        sceneDesc.Add(new SceneDesc(name, path));
    }
}

[System.Serializable]
public class LoadedNavigationScene
{
    public string sceneName;
    public string scenePath;

    public LoadedNavigationScene(string _sceneName, string _scenePath)
    {
        sceneName = _sceneName;
        scenePath = _scenePath;
    }

    public void UnloadThisNavigationScene()
    {
        if (CheckIfSceneLoaded(sceneName))
            SceneManager.UnloadSceneAsync(sceneName);
    }

    public void ReloadThisNavigationScene()
    {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }

    public bool CheckIfSceneLoaded(string name)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name == name)
            {
                return true;
            }
        }
        return false;
    }
}

public class SceneLoaderManager : MonoBehaviour
{

    public ScenesDictionnaries sceneDictionnary;

    public bool debug_SceneLoaderLog = false;

    public Dictionary<GameState, List<Scene>> loadedScenes;

    public List<LoadedScene> sceneLoaded;

    private GameState previousGameState;

    private string previousNavigationScene = "";

    [Header("NavigationScene")]
    public LoadedNavigationScene loadedNavigationScene = null;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < Enum.GetNames(typeof(GameState)).Length; i++)
        {
            sceneLoaded.Add(new LoadedScene(i));
        }
        sceneDictionnary = new ScenesDictionnaries();
        LoadEssentials();
        previousGameState = GameState.MainMenu;
        LoadSceneWithGameState(GameManager.instance.gameState);
    }

    // Update is called once per frame
    void Update()
    {


        loadedScenes = LoadedScenes();

        if (previousGameState != GameManager.instance.gameState && TransitionManager.instance.IsAnimCompleted())
            LoadSceneWithGameState(GameManager.instance.gameState);

    }

    #region LoadScene

    // Load the essentials Scenes : {"Characters",      "Scenes/Characters" }
    void LoadEssentials()
    {
        foreach (KeyValuePair<string, string> entry in sceneDictionnary.essentials)
        {
            SceneManager.LoadScene(entry.Value);
        }
    }

    // Load a scene with a game state after transition manager is Waiting for loading
    public void LoadSceneWithGameState(GameState gameState, bool overwriteTransition = false)
    {

        // Delay this with the scene transition anim, and the Type of transition
        StartCoroutine(LoadSceneAfterTransitionManager(gameState, overwriteTransition));



    }

    // Verify that a scene is not already loaded before loading it
    void LoadSceneOnce(string path, Dictionary<string, string> dictionnary)
    {

        foreach (KeyValuePair<string, string> entry in dictionnary)
        {
            if (path == entry.Value && !CheckIfSceneLoaded(entry.Key))
            {
                SceneManager.LoadScene(path, LoadSceneMode.Additive);
                GetGameStateLoadedScenes(GameManager.instance.gameState).sceneDesc.Add(new SceneDesc(entry.Key, entry.Value));
            }
        }
    }

    // Used by the NavigationController to load a specific navigation scene. (McHouse, TownMap, ...)
    public void LoadNavigationScene(string scenename, string scenepath)
    {

        if (debug_SceneLoaderLog)
            Debug.Log("<color=purple> LoadNavigationScene  </color> \tscenename : \t" + scenename + "\n    \t\t\tscenepath : \t" + scenepath);

        if (loadedNavigationScene == null)
        {
            loadedNavigationScene = new LoadedNavigationScene(scenename, scenepath);
        }
        else if (scenename != loadedNavigationScene.sceneName)
        {
            loadedNavigationScene.UnloadThisNavigationScene();
            loadedNavigationScene = new LoadedNavigationScene(scenename, scenepath);
        }


        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName(previousNavigationScene))
        {
            UnloadSceneWithName(previousNavigationScene);
        }
        if (!CheckIfSceneLoaded(scenename))
            SceneManager.LoadSceneAsync(scenepath, LoadSceneMode.Additive);
        previousNavigationScene = scenename;
    }

    #endregion

    #region UnloadScene

    void UnloadSceneWithGameState(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Dialogue:
                UnloadParticularState(GameState.NewGame);
                UnloadParticularState(GameState.MainMenu);
                break;
            case GameState.MainMenu:
                UnloadAllLoadedScenes();
                break;
            case GameState.NewGame:
                UnloadParticularState(GameState.Navigation);
                UnloadParticularState(GameState.MainMenu);
                UnloadParticularState(GameState.Pause);
                UnloadParticularState(GameState.Dialogue);
                break;
            case GameState.Navigation:
                UnloadParticularState(GameState.MainMenu);
                UnloadParticularState(GameState.Pause);
                UnloadParticularState(GameState.Dialogue);
                UnloadParticularState(GameState.NewGame);
                UnloadParticularState(GameState.Minigame);
                break;
            case GameState.Activity:
                UnloadParticularState(GameState.MainMenu);
                break;
            case GameState.Pause:
                UnloadParticularState(GameState.MainMenu);
                break;
            case GameState.Minigame:
                UnloadParticularState(GameState.MainMenu);
                UnloadParticularState(GameState.Navigation);
                UnloadParticularState(GameState.NewGame);
                UnloadParticularState(GameState.Pause);
                break;
        }
    }

    void UnloadAllLoadedScenes()
    {
        foreach (GameState state in Enum.GetValues(typeof(GameState)))
        {
            UnloadParticularState(state);
        }
    }

    void UnloadParticularState(GameState state)
    {
        foreach (SceneDesc sceneDesc in GetGameStateLoadedScenes(state).sceneDesc)
        {

            UnloadSceneWithName(sceneDesc.sceneName);
        }
        GetGameStateLoadedScenes(state).sceneDesc = new List<SceneDesc>();
    }

    public void UnloadSceneWithName(string scenename)
    {
        if (debug_SceneLoaderLog)
            Debug.Log("<color=blue>Trying to unload </color> " + scenename);
        SceneManager.UnloadSceneAsync(scenename);
    }

    #endregion

    #region Coroutine

    IEnumerator LoadSceneAfterTransitionManager(GameState gameState, bool overwriteTransition = false)
    {
        if (!overwriteTransition)
        {
            TransitionManager.instance.StartCoroutine(TransitionManager.instance.StartTransition(gameState));

            yield return new WaitForSeconds(TransitionManager.instance.transitionTimeStart);
            //previousGameState = gameState;
            yield return new WaitUntil(() => TransitionManager.instance.maskAnimationState == AnimationState.Waiting);
        }
        UnloadSceneWithGameState(gameState);
        switch (gameState)
        {
            case GameState.MainMenu:
                LoadSceneOnce(sceneDictionnary.mainMenu["MainMenu"], sceneDictionnary.mainMenu);
                break;
            case GameState.NewGame:
                LoadSceneOnce(sceneDictionnary.newGame["NewGame"], sceneDictionnary.newGame);
                break;
            case GameState.Navigation:
                LoadSceneOnce(sceneDictionnary.navigation["Navigation"], sceneDictionnary.navigation);
                break;
            case GameState.Activity:
                LoadSceneOnce(sceneDictionnary.navigation["Navigation"], sceneDictionnary.navigation);
                LoadSceneOnce(sceneDictionnary.activities["Activity"], sceneDictionnary.activities);
                break;
            case GameState.Dialogue:
                LoadSceneOnce(sceneDictionnary.dialogue["Dialogue"], sceneDictionnary.dialogue);
                break;
            case GameState.Pause:
                LoadSceneOnce(sceneDictionnary.navigation["Navigation"], sceneDictionnary.navigation);
                LoadSceneOnce(sceneDictionnary.pause["Pause"], sceneDictionnary.pause);
                break;
            case GameState.Minigame:
                // LoadSceneOnce(string path, Dictionary<string, string> dictionnary)
                // path = Global minigame name in GameManager
                string name = GameManager.instance.managers.minigameManager.currentMinigame.Name;

                Debug.Log(name);

                LoadSceneOnce(sceneDictionnary.miniGames[name], sceneDictionnary.miniGames);
                break;
        }
        previousGameState = gameState;

        EventManager.TriggerEvent("StartEndTransitionAnimation");

    }

    #endregion

    #region Getter

    LoadedScene GetGameStateLoadedScenes(GameState gameState)
    {
        foreach (LoadedScene scenes in sceneLoaded)
        {
            if (scenes.gameState == gameState)
            {
                return scenes;
            }
        }
        return sceneLoaded[0];
    }

    public bool CheckIfSceneLoaded(string name)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name == name)
            {
                return true;
            }
        }
        return false;
    }

    public Scene SearchForScene(string sceneName)
    {
        Scene scene = new Scene();
        if (SceneManager.sceneCount > 0)
        {
            for (int n = 0; n < SceneManager.sceneCount; ++n)
            {
                scene = SceneManager.GetSceneAt(n);
                if (scene.name == sceneName)
                {
                    return scene;
                }

            }
        }
        else
        {
            Debug.LogError("<color=red>Wrong Scene, can't find it!</color>");
        }
        return scene;
    }

    private Dictionary<GameState, List<Scene>> LoadedScenes()
    {
        Dictionary<GameState, List<Scene>> loadedScene = new Dictionary<GameState, List<Scene>>() {
            {GameState.MainMenu, new List<Scene>() },
            {GameState.Navigation, new List<Scene>() },
            {GameState.Activity, new List<Scene>() },
            {GameState.Pause, new List<Scene>() }
        };
        List<Scene> mainMenuList = new List<Scene>();
        foreach (KeyValuePair<string, string> entry in sceneDictionnary.mainMenu)
        {
            if (SceneManager.GetSceneByName(entry.Key).isLoaded)
                mainMenuList.Add(SceneManager.GetSceneByName(entry.Key));
        }
        List<Scene> navigationList = new List<Scene>();
        foreach (KeyValuePair<string, string> entry in sceneDictionnary.navigation)
        {
            if (SceneManager.GetSceneByName(entry.Key).isLoaded)
                navigationList.Add(SceneManager.GetSceneByName(entry.Key));
        }
        List<Scene> activitiesList = new List<Scene>();
        foreach (KeyValuePair<string, string> entry in sceneDictionnary.activities)
        {
            if (SceneManager.GetSceneByName(entry.Key).isLoaded)
                activitiesList.Add(SceneManager.GetSceneByName(entry.Key));
        }
        List<Scene> pauseList = new List<Scene>();
        foreach (KeyValuePair<string, string> entry in sceneDictionnary.pause)
        {
            if (SceneManager.GetSceneByName(entry.Key).isLoaded)
                pauseList.Add(SceneManager.GetSceneByName(entry.Key));
        }

        loadedScene[GameState.MainMenu] = mainMenuList;
        loadedScene[GameState.Navigation] = navigationList;
        loadedScene[GameState.Activity] = activitiesList;
        loadedScene[GameState.Pause] = pauseList;

        return loadedScene;
    }

    public GameState GetPreviousGameState()
    {
        return previousGameState;
    }

    #endregion

    #region PublicMethod

    public void BackToMainMenu()
    {
        GameManager.instance.returnMainMenu = true;
        GameManager.instance.gameState = GameState.MainMenu;
    }

    #endregion
}
