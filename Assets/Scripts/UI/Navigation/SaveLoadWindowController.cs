using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;




// Should spawn every saves in the save folder
// Allocate to each of its spawns an ID linked to the save manager
// Is called right after the Sub_menuBehaviour ask to display the saveload window

// Each spawned save preview should have a reference to this object, that way we ca use this controller to ask for a disclaimer if needed
public class SaveLoadWindowController : ClosableWindowBehaviour
{
    //public Sub_MenuBehaviour sub_MenuBehaviour;
    public DisclaimerMenuBehaviour disclaimerMenu;

    public int saveID = 0;

    public Transform scrollListContent;

    public GameObject SavePreviewPrefab;
    public Scrollbar SaveScrollbar;

    public ScrollRect scrollRect;

    public List<GameData> saves;

    public bool SkipDisclaimerOnLoad = false;


    [Header("Quick save")]
    public SavePreviewBehaviour quickSavePreview;


    [Header("Name Editing")]
    public List<EditSaveName> editingList;

    //[HideInInspector]
    public List<GameObject> savesBuffer;
    //public GameObject AddedNewSave;


    [Header("DragDrop")]
    public bool dragging = false;
    public bool moved = false;
    public SavePreviewBehaviour savePreviewToDrag;
    public IEnumerator drag;


    private void OnEnable()
    {
        SpawnAllSavesPreview();
    }


    public void Start()
    {
        scrollRect = GetComponentInChildren<ScrollRect>();
    }






    #region Spawner

    public void ResetSpawnedList()
    {
        foreach (Transform child in scrollListContent)
            Destroy(child.gameObject);
        savesBuffer = new List<GameObject>();
    }

    public void SpawnAllSavesPreview()
    {
        ResetSpawnedList();

        DisplayQuickSave();

        saves = GameManager.instance.managers.saveManager.saves;
        GameData lastSave = new GameData();
        editingList = new List<EditSaveName>();
        for (int i = 0; i < saves.Count; i++)
        {
            GameObject savePreview = Instantiate(SavePreviewPrefab, scrollListContent);
            savePreview.name = "Save Preview " + (i + 1);

            SavePreviewBehaviour savePreviewBehaviour = savePreview.GetComponent<SavePreviewBehaviour>();
            savePreviewBehaviour.ID = i;
            savePreviewBehaviour.saveLoadWindow = this;
            savePreviewBehaviour.save = saves[i];
            lastSave = saves[i];
            savesBuffer.Add(savePreview);
            savePreviewBehaviour.Display();

            editingList.Add(savePreviewBehaviour.editSaveName);
        }
        //AddedNewSave.transform.SetAsLastSibling();

        SaveScrollbar.value = 0f;

    }

    public void DisplayQuickSave()
    {
        if (quickSavePreview != null)
            quickSavePreview.Display();
    }


    #endregion

    #region NameEdition

    public bool isMouseOverAnotherEditName()
    {
        foreach (EditSaveName edit in editingList)
        {
            if (edit.inside)
            {
                return true;
            }
        }
        return false;
    }

    #endregion


    #region Disclaimers

    internal void OverwriteSaveDisclaimer(SaveType saveType, int index)
    {

        disclaimerMenu.RemplaceInfo(DisclaimerType.OverwriteSave, saveType, index);
    }

    internal void LoadSaveDisclaimer(SaveType saveType, int index)
    {
        if (!SkipDisclaimerOnLoad)
            disclaimerMenu.RemplaceInfo(DisclaimerType.LoadSave, saveType, index);
        else
            GameManager.instance.managers.saveManager.LoadGame(saveID);
    }

    internal void RemoveSaveDisclaimer(SaveType saveType, int index)
    {

        disclaimerMenu.RemplaceInfo(DisclaimerType.RemoveSave, saveType, index);
    }
    #endregion


    #region OnclickBehavioursConfirmation

    public void AddSave()
    {
        GameManager.instance.managers.saveManager.AddSave();

        SpawnAllSavesPreview();
    }

    public void Confirm_SaveGame()
    {
        GameManager.instance.managers.saveManager.SaveAt(saveID);
        SpawnAllSavesPreview();
    }

    public void Confirm_LoadSave()
    {
        GameManager.instance.managers.saveManager.LoadGame(saveID);
        SpawnAllSavesPreview();
    }

    public void Confirm_RemoveSave()
    {
        GameManager.instance.managers.saveManager.RemoveSave(saveID);
        SpawnAllSavesPreview();
    }




    public void OverwriteQuickSave()
    {
        GameManager.instance.managers.saveManager.CreateQuickSave();
        DisplayQuickSave();

    }

    public void LoadQuickSave()
    {
        GameManager.instance.managers.saveManager.LoadQuickSave();
        DisplayQuickSave();
    }

    #endregion


    #region SaveDragDrop

    void SetValuesForDragging(bool start)
    {
        List<GameObject> temp = new List<GameObject>();
        foreach (GameObject go in savesBuffer)
        {
            if (go != null)
            {
                temp.Add(go);
            }
        }
        savesBuffer = temp;

        if (start)
        {
            // Set vertical scrolling to false
            scrollRect.vertical = false;
            dragging = true;
        }
        else
        {
            // Set vertical scrolling to true
            scrollRect.vertical = true;
            dragging = false;

        }
    }


    public void StartDragging(SavePreviewBehaviour savePreview)
    {
        // Set a local variable, saying that the user is dragging a savePreview

        // if he's not dragging, then we can continue

        // Take the image and wrap it inside a container that use the UI themed background

        if (!dragging)
        {
            if (drag != null)
            {
                StopCoroutine(drag);
            }
            SetValuesForDragging(true);
            savePreviewToDrag = savePreview;

            drag = UpdatePreviewPosition();
            StartCoroutine(drag);
        }
    }

    public void StopDragging()
    {
        SetValuesForDragging(false);
        savePreviewToDrag = null;
        StopCoroutine(drag);
        savesBuffer = new List<GameObject>();

        SaveChangesIfNeeded();
        SpawnAllSavesPreview();
    }

    public void SaveChangesIfNeeded()
    {

        // Compare New list with saved list
        // If a change occured, save it with the save manager

        SaveAndLoad savesManager = GameManager.instance.managers.saveManager;


        if (moved)
        {
            Debug.LogError("Save Changes");
            moved = false;
            savesManager.Save();
        }
    }



    public IEnumerator UpdatePreviewPosition()
    {
        RectTransform rt = GetComponent<RectTransform>();

        // Make the preview copy follow the mouse position on each frame


        while (dragging)
        {


            yield return null;
            //Debug.LogError("Holding through coroutine");

            CalculateMousePositionOnTheList(rt);
        }
    }

    public void CalculateMousePositionOnTheList(RectTransform window)
    {
        float offset = window.sizeDelta.y / 2; // for a pivot of 0.5
        float positionY = (Screen.height / 2) + window.anchoredPosition.y;
        //                 1080 / 2         +   -307.75
        //                 540 - 307.75
        //                  232.25

        positionY = window.position.y;

        Vector3 mousePos_world = Input.mousePosition;

        // Convert the world mouse position into a comprehensible position relative to the scroll list.
        //Debug.LogError(mousePos_world);


        //Debug.LogErrorFormat("up : {0} \n down : {1}", upSave.position.y, downSave.position.y);

        MoveSavePreview();

        if (mousePos_world.y > positionY + offset)
        {
            //Debug.LogErrorFormat("scroll up");
            if (scrollRect.verticalNormalizedPosition < 1)
            {
                scrollRect.verticalNormalizedPosition += Time.deltaTime;
            }
        }
        else if (mousePos_world.y < positionY - offset)
        {
            //Debug.LogErrorFormat("scroll down");
            if (scrollRect.verticalNormalizedPosition > 0)
            {
                scrollRect.verticalNormalizedPosition -= Time.deltaTime;
            }
        }
    }


    void MoveSavePreview()
    {
        Vector3 mousePos_world = Input.mousePosition;

        RectTransform upSave = GetUpSave();
        RectTransform downSave = GetDownSave();

        int curr = savesBuffer.IndexOf(savePreviewToDrag.gameObject);

        //Debug.LogErrorFormat("current index {0}", curr);

        if (mousePos_world.y > upSave.position.y)
        {
            //Debug.LogError("Insert up");
            if (curr > 0)
            {
                savesBuffer.Move(curr, curr - 1);
                saves.Move(curr, curr - 1);
                savePreviewToDrag.transform.SetSiblingIndex(curr - 1);
                moved = true;
            }
        }
        else if (mousePos_world.y < downSave.position.y)
        {
            //Debug.LogError("Insert down");
            if (curr + 1 < savesBuffer.Count)
            {
                savesBuffer.Move(curr, curr + 1);
                saves.Move(curr, curr + 1);
                savePreviewToDrag.transform.SetSiblingIndex(curr + 1);
                moved = true;
            }
        }
    }





    RectTransform GetUpSave()
    {
        int index = savesBuffer.IndexOf(savePreviewToDrag.gameObject);

        if (index - 1 >= 0)
            return savesBuffer[index - 1].GetComponent<RectTransform>();
        else
            return savePreviewToDrag.GetComponent<RectTransform>();
    }

    RectTransform GetDownSave()
    {
        int index = savesBuffer.IndexOf(savePreviewToDrag.gameObject);

        //         5
        // 0 1 2 3 4
        if (index + 1 < savesBuffer.Count)
            return savesBuffer[index + 1].GetComponent<RectTransform>();
        else
            return savePreviewToDrag.GetComponent<RectTransform>();
    }

    #endregion

}
