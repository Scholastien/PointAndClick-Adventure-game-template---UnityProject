using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class QuickSavePreviewBehaviour : MonoBehaviour
{
    private VariablesManager vm;

    [Header("Data")]
    public GameData save;
    public int saveID;

    [Header("GameObject Scene reference")]
    public GameObject SavePreview;
    public Image Img_SavePreview;
    public Button Btn_Save;
    public Button Btn_Load;

    private void OnEnable()
    {
        if (GameManager.instance.managers.saveManager.saves.Count != 0)
        {
            SavePreview.SetActive(true);
            vm = VariablesManager.instance;
            int savesCount = GameManager.instance.managers.saveManager.saves.Count;

            if (savesCount <= vm.gameSavedData.saveID)
            {
                vm.gameSavedData.saveID = 0;
                saveID = 0;
            }
            else
            {
                saveID = vm.gameSavedData.saveID;
            }
            save = GameManager.instance.managers.saveManager.saves[saveID];
            Display();
            AssignFunctionToButtonPress();

        }
        else
        {
            SavePreview.SetActive(false);
        }
    }


    public void Display()
    {


        UI_SavedData savedData = new UI_SavedData(save, saveID);

        VariablesManager vm = GameManager.instance.managers.variablesManager;


        HandleSavePreview(save);
    }

    #region ButtonPressFunctions

    public void AssignFunctionToButtonPress()
    {
        Btn_Save.onClick.AddListener(Save);
        Btn_Load.onClick.AddListener(Load);
    }


    public void Save()
    {
        GameManager.instance.managers.saveManager.SaveAt(saveID);
        Display();
    }

    public void Load()
    {
        GameManager.instance.managers.saveManager.LoadGame(saveID);
    }

    #endregion


    #region ImagePreview

    public void HandleSavePreview(GameData save)
    {
        VariablesManager vm = GameManager.instance.managers.variablesManager;
        Room savedRoom = vm.locationCollection.GetRoom(save.GameInfo.currentRoom);


        if (System.IO.File.Exists(save.imgPreview_Path))
        {
            Texture2D tex = LoadTexture2D(save);

            Img_SavePreview.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100);
        }
        else
        {
            //Debug.LogWarning("! FileExist : " + save.imgPreview_Path);
            savedRoom.GetNavIcon((TimeOfTheDay)save.GameInfo.intTime);
            Img_SavePreview.sprite = savedRoom.navIcon; // tmp sprite waiting for screenshot
        }
    }

    public Texture2D LoadTexture2D(GameData save)
    {
        // refresh editor view
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
        byte[] bytes;
        bytes = System.IO.File.ReadAllBytes(save.imgPreview_Path);
        Texture2D loadedTexture = new Texture2D(1, 1);
        loadedTexture.LoadImage(bytes);
        return loadedTexture;
    }
    #endregion
}
