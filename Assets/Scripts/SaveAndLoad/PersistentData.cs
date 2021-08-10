using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;


/// <summary>
/// Class that holds every persistent data saved
/// </summary>
[System.Serializable]
public class PersistentSave
{
    [Header("Graphic")]
    public bool fullScreen;
    [Header("Text")]
    public Language language;
    [Range(0.1f, 0.5f)]
    public float textSpeed;
    [Header("AudioMixer")]
    public MixerVolumes savedMixerVolumes;
    [Header("Gallery")]
    public List<bool> serializedGallery;

}

// Persistent Save/Load system
/// <summary>
/// Save data like settings, gallery, or whatever you want automaticaly
/// I wanted to make a class like that because i have a game idea for a dokidoki kinda experience for a nsfw game
/// not creepy, but it use the "return to main menu" at it's full potential
/// making the player think that they need to restart a new game, but in fact they already progressed in it
/// </summary>
public class PersistentData : MonoBehaviour {

    public static PersistentData instance = null;

    public bool needToSave;

    public PersistentSave persistantSaves;


    private string saveFileName = "/persistentData.save";
    private string saveFolderName = "/Persistent";
    private string pathPrefix;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        if(instance != this)
        {
            Destroy(this);
        }
    }

    // Use this for initialization
    void Start () {
        pathPrefix = Application.persistentDataPath;
        CreateSaveFolder();
        Load();

        Init();
    }

    private void Update()
    {
        if (needToSave) {
            Save();
            needToSave = false;
        }
        
    }

    public void Init()
    {
        AudioManager.instance.mixerVolumes = persistantSaves.savedMixerVolumes;
    }
    
    // new save
    public void Save()
    {
        persistantSaves.serializedGallery = VariablesManager.instance.galleryCollection.SerializeGallery();

        // Add a new save to the save list
        PersistentSave save = persistantSaves;

        // Make a file with it
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(pathPrefix + saveFolderName + saveFileName);
        bf.Serialize(file, save);
        file.Close();
    }

    // Load the save list to this object
    public void Load()
    {
        if (File.Exists(pathPrefix + saveFolderName + saveFileName))
        {

            bool deleteFile = false;

            // Resetting data
            GameManager.instance.managers.variablesManager.gameSavedData = new GameData();

            // Read the file
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(pathPrefix + saveFolderName + saveFileName, FileMode.Open);

            PersistentSave save = new PersistentSave();

            try
            {
                var temp_saveRead = bf.Deserialize(file);
                if (save.GetType() == temp_saveRead.GetType())
                {
                    //Debug.Log(temp_saveRead.GetType());
                    save = (PersistentSave)temp_saveRead;
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
                persistantSaves = save;
                VariablesManager.instance.galleryCollection.DeserializeGallery(persistantSaves.serializedGallery);
                file.Close();
            }

            if (deleteFile)
            {
                Debug.Log("You can delete");
                //File.Delete(pathPrefix + saveFolderName + saveFileName);
            }

            
        }
        else
        {
            Debug.Log(saveFolderName + saveFileName + " doesn't exist");
        }
    }

    private void CreateSaveFolder()
    {
        if (!Directory.Exists(pathPrefix + saveFolderName))
        {
            Directory.CreateDirectory(pathPrefix + saveFolderName);
        }
    }
}
