using System;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;


// 
public class SaveAndLoad : MonoBehaviour
{
    public bool justLoaded = false;



    // Bool to debug in the inspector during runtime
    public bool debug_SaveLog;


    // I save a list of GameData
    // that list is the ability to multisave
    public List<GameData> saves;

    public GameData quickSave;

    // save template is a save template, pretty explicit
    public GameData saveTemplate;

    public string PlayIntro = "PlayIntro Activity";

    private string saveFileName = "/gamesaves.save"; // we don't care
    private string quickSaveFileName = "/quicksave.save"; // we don't care

    private string saveFolderName = "/Saves";

    // I only use it to link data between playercontroller/gamedata
    private PlayerController playerController;

    private string pathPrefix;

    //private SaveScreenShot saveScreenShot;

    #region MonoBehaviour

    // Use this for initialization
    void Start()
    {
        pathPrefix = Application.dataPath;

        //saveScreenShot = new SaveScreenShot(gameObject.GetComponent<Camera>());

        CreateSaveFolder();
        LoadFile();

        Init();
        StartCoroutine(AutoSaveAfterDelay());

        saveTemplate = new GameData();
    }



    #endregion
    // Set the variable manager with gameData ready for new game
    void Init()
    {

        GameManager.instance.managers.variablesManager.charactersCollection.Init();

        saveTemplate.GameInfo = new PlayerInfo();
        saveTemplate.GameInfo.currentRoom = GameManager.instance.managers.variablesManager.locationCollection.SetFirstRoom().name;

        GameManager.instance.managers.variablesManager.questCollection.DisableAll();
        saveTemplate.activeQuest = new List<int>();
        saveTemplate.activeQuest.Add(GameManager.instance.managers.variablesManager.questCollection.starterQuest);
        saveTemplate.progressionTab = new List<bool> ();
        for (int i = 0; i < GameManager.instance.managers.variablesManager.triggerCollection.chainTriggers.Count; i++)
        {
            saveTemplate.progressionTab.Add(false);
        }
        GameManager.instance.managers.variablesManager.triggerCollection.LoadDataFromSaveSystem(saveTemplate.progressionTab);

        saveTemplate.mailsState = new List<SerializedMail>();
        saveTemplate.mailsState = GameManager.instance.managers.variablesManager.mailCollection.InitializeMailListForNewGame();

        saveTemplate.locationLockState = new List<bool>();
        foreach (Location location in GameManager.instance.managers.variablesManager.locationCollection.locations)
        {
            foreach (Room room in location.rooms)
            {
                saveTemplate.locationLockState.Add(false);
            }
        }
        GameManager.instance.managers.variablesManager.locationCollection.LoadDataFromSaveSystem(saveTemplate.locationLockState);


        saveTemplate.npcSerializedInfos = new List<NpcSerializedSchedule>();
        foreach (CharacterFunction type in Enum.GetValues(typeof(CharacterFunction)))
        {
            //Debug.Log(type.ToString());
            saveTemplate.npcSerializedInfos.Add(new NpcSerializedSchedule(type.ToString()));
        }

        GameManager.instance.managers.variablesManager.gameSavedData = new GameData(saveTemplate);



    }

    #region NewGame

    public void NewGame()
    {
        Init();

        GameManager.instance.charactersToRename = new List<Character>{
            new Character("Max", CharacterFunction.MainCharacter)
        };


        GameManager.instance.gameState = GameState.NewGame;

        GameManager.instance.managers.variablesManager.gameSavedData.GameInfo.mainCharacter = new MainCharacter();

        GameManager.instance.StartCoroutine(WaitForNames());



    }

    // Prepare GameData For new Game
    public void ResetForNewGame()
    {
        GameManager.instance.managers.variablesManager.gameSavedData = new GameData();
        Init();
        GameObject.FindObjectOfType<PlayerController>().LinkPlayerControllerToGameManager();
    }

    public IEnumerator WaitForNames()
    {
        yield return new WaitUntil(() => !GameManager.instance.naming);
        yield return new WaitUntil(() => GameManager.instance.naming);

        while (GameManager.instance.naming)
        {
            yield return null;
            FindObjectOfType<PlayerController>().LinkPlayerControllerToGameManager();
            GameManager.instance.LinkData(true);
        }

        Debug.Log("Start new game");
        Debug.Log(PlayIntro);

        // Loading screen


        GameManager.instance.gameState = GameState.Dialogue;

        // Start dialogue chain here

        //Debug.LogError("Start intro dialog");
        if (!GameManager.instance.managers.dialogueManager.isRunning)
        {
            DialogueChain dc = GameManager.instance.IntroChain;
            GameManager.instance.managers.dialogueManager.NewGame_RunDialogue(dc);
        }

        yield return new WaitUntil(() => !DialogueManager.instance.isRunning);
        yield return new WaitUntil(() => DialogueManager.instance.isRunning);
        yield return new WaitUntil(() => !DialogueManager.instance.isRunning);


        ResetFromLoad();

    }

    #endregion


    #region SaveAndLoad


    #region ReadFile

    // Load the save list to this object
    public void LoadFile()
    {
        if (File.Exists(pathPrefix + saveFolderName + saveFileName))
        {

            bool deleteFile = false;

            // Resetting data
            GameManager.instance.managers.variablesManager.gameSavedData = new GameData();

            // Read the file
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(pathPrefix + saveFolderName + saveFileName, FileMode.Open);

            List<GameData> save = new List<GameData>();

            try
            {
                var temp_saveRead = bf.Deserialize(file);
                if (save.GetType() == temp_saveRead.GetType())
                {
                    //Debug.Log(temp_saveRead.GetType());
                    save = (List<GameData>)temp_saveRead;
                }
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                deleteFile = true;
                throw;
            }
            finally
            {
                saves = save;
                AssignCorrectValuesToSaves(saves);
                file.Close();
            }


            if (deleteFile)
            {
                Debug.Log("You can delete");
            }
        }
        else
        {
            Debug.Log(saveFolderName + saveFileName + " doesn't exist");
        }


    }


    #endregion

    #region Save

    // Save current state
    public void Save()
    {

        AssignCorrectValuesToSaves(saves);




        // Make a file with it
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(pathPrefix + saveFolderName + saveFileName);
        bf.Serialize(file, saves);
        file.Close();
    }


    // Add a new save
    public void AddSave()
    {
        // Add a new save to the save list
        List<GameData> save = saves;
        GameData gameData = CreateGameDataToSave();
        gameData.saveID = save.Count;

        VariablesManager.instance.gameSavedData.saveID = gameData.saveID;

        save.Add(gameData);



        AssignCorrectValuesToSaves(save);
        NavigationUIBehaviour navigationUIBehaviour = FindObjectOfType<NavigationUIBehaviour>();


        // Make a file with it
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(pathPrefix + saveFolderName + saveFileName);
        bf.Serialize(file, save);
        file.Close();
    }

    // Add a new save
    public void SaveGame(List<GameData> saveList)
    {
        // Add a new save to the save list
        List<GameData> save = saveList;
        GameData gameData = CreateGameDataToSave();

        gameData.saveID = save.Count;

        VariablesManager.instance.gameSavedData.saveID = gameData.saveID;
        save.Add(gameData);


        AssignCorrectValuesToSaves(save);
        NavigationUIBehaviour navigationUIBehaviour = FindObjectOfType<NavigationUIBehaviour>();

        // Make a file with it
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(pathPrefix + saveFolderName + saveFileName);
        bf.Serialize(file, save);
        file.Close();
    }

    // overwrite a save
    public void SaveAt(int index)
    {
        // remplace a new save in the save list with index
        List<GameData> save = saves;


        GameData gameData = CreateGameDataToSave();

        gameData.saveID = save[index].saveID;
        gameData.imgPreview_Path = save[index].imgPreview_Path;

        save.Insert(index, gameData);


        save.RemoveAt(index + 1);

        gameData.saveID = index;

        VariablesManager.instance.gameSavedData.saveID = gameData.saveID;

        AssignCorrectValuesToSaves(save);

        NavigationUIBehaviour navigationUIBehaviour = FindObjectOfType<NavigationUIBehaviour>();

        // Make a file with it
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(pathPrefix + saveFolderName + saveFileName);
        bf.Serialize(file, save);
        file.Close();


        EventManager.Instance.IsLoaded();
    }

    public void SaveNameUpdated(string newName, int id)
    {
        // Add a new save to the save list
        List<GameData> save = saves;

        save[id].customName = newName;

        AssignCorrectValuesToSaves(save);

        // Make a file with it
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(pathPrefix + saveFolderName + saveFileName);
        bf.Serialize(file, save);
        file.Close();
    }


    #endregion

    #region QuickSave

    public void CreateQuickSave()
    {
        quickSave = CreateGameDataToSave();
        List<GameData> saveList = new List<GameData>();
        saveList.Add(quickSave);

        // Make a file with it
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(pathPrefix + saveFolderName + quickSaveFileName);
        bf.Serialize(file, saveList);
        file.Close();
    }

    public void LoadQuickSave()
    {
        if (File.Exists(pathPrefix + saveFolderName + quickSaveFileName))
        {
            // Resetting data
            GameManager.instance.managers.variablesManager.gameSavedData = new GameData();

            // Read the file
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(pathPrefix + saveFolderName + saveFileName, FileMode.Open);
            List<GameData> save = (List<GameData>)bf.Deserialize(file);


            // Putting Data to the Variable manager
            if (quickSave == null)
            {
                GameManager.instance.managers.variablesManager.gameSavedData = quickSave;
                GameManager.instance.saveIndex = 0;
            }
            else
            {
                GameManager.instance.managers.variablesManager.gameSavedData = quickSave;
            }
            GameManager.instance.managers.variablesManager.triggerCollection.LoadDataFromSaveSystem(GameManager.instance.managers.variablesManager.gameSavedData.progressionTab);
            GameManager.instance.managers.variablesManager.locationCollection.LoadDataFromSaveSystem(GameManager.instance.managers.variablesManager.gameSavedData.locationLockState);
            GameManager.instance.managers.variablesManager.mailCollection.DeserializeMails(GameManager.instance.managers.variablesManager.gameSavedData.mailsState);
            GameManager.instance.managers.navigationManager.Init();

            if (debug_SaveLog)
                Debug.Log("Game Loaded from : " + pathPrefix + saveFolderName + saveFileName);
            file.Close();
        }
        else
        {
            if (debug_SaveLog)
                Debug.Log(saveFolderName + saveFileName + " doesn't exist");
        }
        GameManager.instance.LinkData(true);
        ResetFromLoad();

        GameManager.instance.gameState = GameState.NewGame;
    }


    #endregion

    #region AutoSave


    // Autosave coroutine, called at the start and saves by an interval
    public IEnumerator AutoSaveAfterDelay()
    {
        // autosave only during navigation mode
        if (GameManager.instance.gameState == GameState.Navigation)
        {

            // Update autosave GameData
            quickSave = CreateGameDataToSave();
            quickSave.name = "Quick Save";


            // Make a file with it
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(pathPrefix + saveFolderName + quickSaveFileName);
            bf.Serialize(file, quickSave);
            file.Close();

            // autosave every 60 seconds
            yield return new WaitForSeconds(60f);
        }
        else
        {
            yield return null;
        }

        StartCoroutine(AutoSaveAfterDelay());
    }


    #endregion

    #region Load


    // Load game in the save list
    public void LoadGame(int index, GameData savedData = null)
    {


        if (File.Exists(pathPrefix + saveFolderName + saveFileName))
        {
            // Resetting data
            GameManager.instance.managers.variablesManager.gameSavedData = new GameData();

            // Read the file
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(pathPrefix + saveFolderName + saveFileName, FileMode.Open);
            List<GameData> save = (List<GameData>)bf.Deserialize(file);

            GameData gameData = new GameData();
            // Putting Data to the Variable manager
            if (savedData == null)
            {
                gameData = save[index];
                GameManager.instance.saveIndex = index;

                Debug.Log("save[" + index + "] : " + (TimeOfTheDay)save[index].GameInfo.intTime + " " + (DayOfTheWeek)save[index].GameInfo.intDay);
            }
            else
            {
                Debug.Log(" savedData null");
                gameData = savedData;
            }

            GameManager.instance.managers.variablesManager.gameSavedData = new GameData(gameData);


            GameManager.instance.managers.variablesManager.triggerCollection.LoadDataFromSaveSystem(GameManager.instance.managers.variablesManager.gameSavedData.progressionTab);
            GameManager.instance.managers.variablesManager.locationCollection.LoadDataFromSaveSystem(GameManager.instance.managers.variablesManager.gameSavedData.locationLockState);
            GameManager.instance.managers.variablesManager.mailCollection.DeserializeMails(GameManager.instance.managers.variablesManager.gameSavedData.mailsState);

            GameManager.instance.managers.navigationManager.Init();

            if (debug_SaveLog)
                Debug.Log("Game Loaded from : " + pathPrefix + saveFolderName + saveFileName);
            file.Close();
        }
        else
        {
            if (debug_SaveLog)
                Debug.Log(saveFolderName + saveFileName + " doesn't exist");
        }

        GameManager.instance.LinkData(true);
        ResetFromLoad();

        //GameManager.instance.gameState = GameState.NewGame;
        //Debug.LogWarning("Newgame");

        justLoaded = true;
        EventManager.Instance.IsLoaded();

    }


    #endregion

    #region Remove

    // Remove a save
    public void RemoveSave(int index)
    {
        // remplace a new save in the save list with index 
        List<GameData> save = saves;

        save.RemoveAt(index);

        if (save.Count > VariablesManager.instance.gameSavedData.saveID)
        {
            VariablesManager.instance.gameSavedData.saveID = 0;
        }


        AssignCorrectValuesToSaves(save);

        // Make a file with it
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(pathPrefix + saveFolderName + saveFileName);
        bf.Serialize(file, save);
        file.Close();

        saves = save;
    }

    #endregion

    #region Functions

    private void CreateSaveFolder()
    {
        if (!Directory.Exists(pathPrefix + saveFolderName))
        {
            Directory.CreateDirectory(pathPrefix + saveFolderName);
        }
    }

    public GameData CreateGameDataToSave()
    {
        GameData gameData = new GameData(GameManager.instance.managers.variablesManager.GetGameData());
        RegisterLastSaveDate(gameData);


        return gameData;

    }

    // TODO : take in count custom names
    public void AssignCorrectValuesToSaves(List<GameData> gameDatas)
    {
        int i = 1;
        foreach (GameData gamedata in gameDatas)
        {
            gamedata.name = "Save " + i;
            gamedata.saveID = i - 1;
            i++;
        }


    }

    public void RegisterLastSaveDate(GameData gameData)
    {
        string customFmt = "H:mm  -  d MMM yyyy";
        DateTime date = DateTime.Now;
        gameData.timestampSave = date.ToString(customFmt);
    }

    public void ResetFromLoad()
    {
        if (GameManager.instance.gameState != GameState.Navigation)
        {
            GameManager.instance.gameState = GameState.Navigation;
        }

    }

    #endregion


    #endregion


}





// public class SaveScreenShot
// {
//     public Camera camera;

//     public int resWidht = 1920 / 10;
//     public int resHeight = 1080 / 10;

//     public SaveScreenShot(Camera camera)
//     {
//         this.camera = camera;
//     }

//     public void TakeScreenShot(NavigationUIBehaviour navigationUIBehaviour, GameData gameData, string saveFolderName)
//     {
//         if (navigationUIBehaviour != null)
//         {
//             navigationUIBehaviour.ChangeTargetDisplay(1); // Go on display 2

//             //RemovePreview(gameData.imgPreview_Path);


//             // Take screenshot here
//             navigationUIBehaviour.StartCoroutine(CreatePictureFile(navigationUIBehaviour, gameData, saveFolderName));
//         }
//     }

//     public IEnumerator CreatePictureFile(NavigationUIBehaviour navigationUIBehaviour, GameData gameData, string saveFolderName)
//     {

//         RemovePreview(gameData.imgPreview_Path);

//         do
//         {
//             yield return null;
//         } while (File.Exists(gameData.imgPreview_Path));


//         string fileName = Application.dataPath + saveFolderName + "/" + ScreenshotName(gameData.name) + ".png";
//         gameData.imgPreview_Path = fileName;

//         // We should only read the screen after all rendering is complete
//         yield return new WaitForEndOfFrame();

//         // Create a texture the size of the screen, RGB24 format
//         int width = Screen.width;
//         int height = Screen.height;
//         var tex = new Texture2D(width, height, TextureFormat.RGB24, false);

//         // Read screen contents into the texture
//         tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
//         tex.Apply();

//         // Encode texture into PNG
//         byte[] bytes = tex.EncodeToPNG();

//         GameObject.Destroy(tex);




//         BinaryFormatter bf = new BinaryFormatter();
//         System.IO.File.WriteAllBytes(fileName, bytes);


//         Debug.LogError("New file :" + gameData.imgPreview_Path);

//         //ScreenCapture.CaptureScreenshot(fileName + "screencapt");


//         yield return new WaitForEndOfFrame();
//         // refresh editor view
// #if UNITY_EDITOR
//         UnityEditor.AssetDatabase.Refresh();
// #endif


//         navigationUIBehaviour.ChangeTargetDisplay(0); // Return on display 1
//     }


//     public string ScreenshotName(string name)
//     {
//         return name;
//     }

//     public void RemovePreview(string path)
//     {
//         if (File.Exists(path))
//         {
//             //Debug.LogError("Remove :" + path);
//             File.Delete(path);
//         }



//         // refresh editor view
// #if UNITY_EDITOR
//         UnityEditor.AssetDatabase.Refresh();
// #endif
//     }


//     #region SavePreviewConflict

//     public string GetFilePathWithFileName(string fileName, string saveFolderName)
//     {
//         // Application.dataPath + saveFolderName + "/"  <= front
//         // ".png";                                      <= end
//         string front = Application.dataPath + saveFolderName + "/";
//         string end = ".png";
//         return front + fileName + end;
//     }

//     public string GetFileNameWithFilePath(string filePath, string saveFolderName)
//     {
//         // Application.dataPath + saveFolderName + "/"  <= front
//         // ".png";                                      <= end
//         string front = Application.dataPath + saveFolderName + "/";
//         string end = ".png";
//         string fileName = filePath.Remove(0, front.Length);

//         // 0                 9
//         // 0 1 2 3 4 5 . p n g
//         // 

//         fileName.Substring(0, fileName.Count)

//         return fileName.Remove(conflict.name.Length);
//     }


//     public void RenameFile(GameData gameData, string newName)
//     {
//         try
//         {
//             if (!File.Exists(newName))
//             {
//                 File.Move(gameData.imgPreview_Path, newName);
//             }
//         }
//         catch
//         {
//             Debug.LogErrorFormat("Couldn't move the file \t {0} \n into a new file \t {1}", gameData.imgPreview_Path, newName);
//         }

//         gameData.imgPreview_Path = newName;

//         // refresh editor view
// #if UNITY_EDITOR
//         UnityEditor.AssetDatabase.Refresh();
// #endif

//     }

//     public void RenameConflictFiles(IEnumerable<GameData> conflicts, string frontPath)
//     {
//         int i = 0;
//         if (conflicts.Count() != 0)
//         {
//             foreach (GameData conflict in conflicts)
//             {
//                 string fileName = conflict.imgPreview_Path.Remove(0, frontPath.Length);
//                 string temp_fileName = "temp_" + i + "_" + fileName.Remove(conflict.name.Length);
//                 i++;
//                 RenameFile(conflict, temp_fileName);
//             }
//         }
//     }




//     public void RemakeSavePreview(List<GameData> saves, string saveFolderName)
//     {
//         string front = Application.dataPath + saveFolderName + "/";
//         string end = ".png";
//         foreach (GameData save in saves)
//         {
//             string fileName = save.imgPreview_Path.Remove(0, front.Length);
//             if (needRemake(save, saveFolderName))
//             {
//                 string predictedPath = front + save.name + end;

//                 IEnumerable<GameData> conflicts = GetConflictingSave(saves, predictedPath);
//                 RenameConflictFiles(conflicts, front);

//             }
//         }
//     }

//     public bool needRemake(GameData gameData, string saveFolderName)
//     {

//         // Application.dataPath + saveFolderName + "/"  <= front
//         // ".png";                                      <= end
//         string front = Application.dataPath + saveFolderName + "/";

//         string fileName = gameData.imgPreview_Path.Remove(0, front.Length);
//         fileName = fileName.Remove(gameData.name.Length);

//         return fileName != gameData.name;
//     }

//     public IEnumerable<GameData> GetConflictingSave(List<GameData> saves, string predictedPath)
//     {
//         IEnumerable<GameData> conflicting = saves.Where(c => c.imgPreview_Path == predictedPath);
//         return conflicting;
//     }


//     public void CopySavePreview(GameData gameData, string saveFolderName)
//     {
//         string front = Application.dataPath + saveFolderName + "/";
//         string end = ".png";

//         string fileName = gameData.imgPreview_Path.Remove(0, front.Length);
//         fileName = fileName.Remove(gameData.name.Length);


//         if (needRemake(gameData, saveFolderName))
//         {
//             string newPath = front + gameData.name + end;

//             Debug.LogError(gameData.name + " => " + fileName);
//             try
//             {
//                 if (!File.Exists(newPath))
//                 {
//                     File.Copy(gameData.imgPreview_Path, newPath);
//                 }
//             }
//             catch
//             {
//                 Debug.LogErrorFormat("Couldn't copy the file \t {0} \n into a new file \t {1}", gameData.imgPreview_Path, newPath);
//             }

//             RemovePreview(gameData.imgPreview_Path);
//         }


//         // refresh editor view
// #if UNITY_EDITOR
//         UnityEditor.AssetDatabase.Refresh();
// #endif


//     }

//     #endregion

// }