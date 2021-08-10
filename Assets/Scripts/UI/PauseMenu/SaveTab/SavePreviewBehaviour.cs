using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.IO;


[RequireComponent(typeof(CanvasGroup))]
public class SavePreviewBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{

    public SaveLoadWindowController saveLoadWindow;
    public EditSaveName editSaveName;

    public int ID;
    public GameData save;

    public Image roomPreview;

    public bool Is_quickSave = false;

    public bool inside = false;


    [Header("TextMeshPro")]
    // New TMP text
    public TextMeshProUGUI SaveIndex;
    public TextMeshProUGUI SaveName;
    public TextMeshProUGUI PlayerLocationAndTime;
    public TextMeshProUGUI Money;

    private bool needPreviewRefresh = false;

    #region MonoBehaviour
    // Use this for initialization
    void Start()
    {
        Display();
        inside = false;
    }

    private void Update()
    {
        if (needPreviewRefresh)
        {
            needPreviewRefresh = false;
            Display();
        }

        // If in and not already dragging
        GetMouseInputForDragging();
    }
    #endregion

    public void Display()
    {
        UI_SavedData savedData;
        if (!Is_quickSave)
        {
            save = GameManager.instance.managers.saveManager.saves[ID];
        }
        else
        {

            // Ask Savemanager last save

            save = GameManager.instance.managers.saveManager.quickSave;
        }

        savedData = new UI_SavedData(save, ID);
        if (editSaveName != null)
        {
            editSaveName.saveID = ID;
            editSaveName.savePreview = this;
            editSaveName.saveLoadWindow = saveLoadWindow;

        }

        VariablesManager vm = GameManager.instance.managers.variablesManager;


        HandleSavePreview(save);


        SaveIndex.text = savedData.savedTypeOrIndex;
        if (SaveName != null)
            SaveName.text = savedData.savedCustomName == "" ? savedData.savedNameWithTimestamp : savedData.savedCustomName;
        PlayerLocationAndTime.text = savedData.savedPlayerLocationAndTime;
        Money.text = savedData.savedMoney;
    }

    #region OnClickBehavioursToOpenDisclaimers

    public void OverwriteSave()
    {
        // Old Pause menu callback
        //PauseMenuUIBehaviour manager = FindObjectOfType<PauseMenuUIBehaviour>();
        //manager.interactionSaveIndex = ID;
        //manager.OverwriteSaveDisclaimer();

        // New callback using the SaveLoadWindowController
        saveLoadWindow.saveID = ID;
        saveLoadWindow.OverwriteSaveDisclaimer(save.saveType, ID);
        needPreviewRefresh = true;
    }
    public void LoadSave()
    {
        // Old Pause menu callback
        //PauseMenuUIBehaviour manager = FindObjectOfType<PauseMenuUIBehaviour>();
        //manager.interactionSaveIndex = ID;
        //manager.LoadSaveDisclaimer();

        // New callback using the SaveLoadWindowController
        saveLoadWindow.saveID = ID;
        saveLoadWindow.LoadSaveDisclaimer(save.saveType, ID);
        needPreviewRefresh = true;
    }
    public void RemoveSave()
    {
        // Old Pause menu callback
        //PauseMenuUIBehaviour manager = FindObjectOfType<PauseMenuUIBehaviour>();
        //manager.interactionSaveIndex = ID;
        //manager.RemoveSaveDisclaimer();

        // New callback using the SaveLoadWindowController
        saveLoadWindow.saveID = ID;
        saveLoadWindow.RemoveSaveDisclaimer(save.saveType, ID);
        needPreviewRefresh = true;
    }

    #endregion

    #region ImagePreview

    public void HandleSavePreview(GameData save)
    {
        VariablesManager vm = GameManager.instance.managers.variablesManager;
        Room savedRoom = vm.locationCollection.GetRoom(save.GameInfo.currentRoom);


        if (System.IO.File.Exists(save.imgPreview_Path))
        {
            //Debug.LogWarning("FileExist");

            //pathPrefix = Application.dataPath;
            //+ saveFolderName = "/Saves";
            //+ saveFileName

            // Resources works only when files are located in the ressource folder
            Texture2D tex = LoadTexture2D(save);
            //FileStream file = File.Open(save.imgPreview_Path, FileMode.Open);

            //System.Drawing.Image.FromStream


            roomPreview.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100);
        }
        else
        {
            //Debug.LogWarning("! FileExist : " + save.imgPreview_Path);
            savedRoom.GetNavIcon((TimeOfTheDay)save.GameInfo.intTime);
            roomPreview.sprite = savedRoom.navIcon; // tmp sprite waiting for screenshot
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


    #region DragAndDrop

    // When click:
    // create a clone of the object
    // original alpha = 0.2
    // Clone should follow the mouse while left click is pressed
    public void SetTransparency()
    {
        if (this == saveLoadWindow.savePreviewToDrag)
        {
            gameObject.GetComponent<CanvasGroup>().alpha = 0.4f;
        }
        else
        {
            gameObject.GetComponent<CanvasGroup>().alpha = 1f;
        }
    }


    public void GetMouseInputForDragging()
    {
        // if inside and button pressed
        if (Input.GetMouseButton(0))
        {
            if (inside || this == saveLoadWindow.savePreviewToDrag)
            {
                //Debug.LogError("held");
                // Start the clone follow coroutine
                saveLoadWindow.StartDragging(this);
            }
        }
        else
        {
            // check if the saveloadwindowCtrl is following THIS savepreview.
            // if yes, then stop the follow coroutine,
            // else do nothing
            if (this == saveLoadWindow.savePreviewToDrag)
            {
                saveLoadWindow.StopDragging();
            }
        }

        SetTransparency();
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        // in only if on draggable savepreviewImg
        if (pointerEventData.pointerEnter == roomPreview.gameObject)
        {
            inside = true;

            //Debug.LogError("Entering image ? " + (pointerEventData.pointerEnter == roomPreview.gameObject));
        }
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        // in only if on draggable savepreviewImg
        if (pointerEventData.pointerEnter == roomPreview.gameObject)
        {
            inside = true;

            //Debug.LogError("holdingdown image ? " + (pointerEventData.pointerEnter == roomPreview.gameObject));
        }
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        // in only if on draggable savepreviewImg
        inside = false;

        //Debug.LogError("holdingdown image ? " + (pointerEventData.pointerEnter == roomPreview.gameObject));

    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        //out
        inside = false;
    }


    #endregion


}
