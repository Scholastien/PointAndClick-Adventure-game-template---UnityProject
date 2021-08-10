using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;



// Overlay : Show edit icon
// When mouseover, show the italic placeholder
// While editing show bold edit text
// When user press Enter, save the new name by taking its index and not showing the disclaimer window
// > /!\ Don't save the current game

public class EditSaveName : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool enableEditing = false;
    public SavePreviewBehaviour savePreview;
    public SaveLoadWindowController saveLoadWindow;

    [Header("Data")]
    public string currentName ="test";
    public int saveID;
    public bool inside = false;

    [Header("Holder")]
    public Image Holder;              // contain the sprite change
    public Color idle;
    public Color mouseover;
    public Color click;

    [Header("Editing")]
    public TextMeshProUGUI saveName;
    public TMP_InputField inputField;
    public bool editing = false;


    #region AddListener behaviour
    public void OnEnable()
    {
        if (enableEditing)
            inputField.onEndEdit.AddListener(UpdateTextDisplayed);
    }
    public void OnDisable()
    {
        if (enableEditing)
            inputField.onEndEdit.RemoveListener(UpdateTextDisplayed);

        GameManager.instance.naming = false;
    }

    private void UpdateTextDisplayed(string newName)
    {
        if (newName != string.Empty)
        {
            //Debug.LogError("new name : " + newName);
            saveName.text = newName;
        }
        // hide input and show default text
        saveName.gameObject.SetActive(true);
        inputField.gameObject.SetActive(false);

        Holder.color = idle;
        editing = false;

        GameManager.instance.naming = false;


        // Send the new name to The save system
        GameManager.instance.managers.saveManager.SaveNameUpdated(newName, saveID);
    }
    #endregion

    #region MonoBehaviour
    public void Awake()
    {
        if (GameManager.instance.gameState != GameState.MainMenu)
        {
            enableEditing = true;
        }
    }

    public void Start()
    {
        if (enableEditing)
        {
            saveName.gameObject.SetActive(true);
            inputField.gameObject.SetActive(false);
            Holder.color = idle;

            GameData save = GameManager.instance.managers.saveManager.saves[saveID];

            if(save.customName == string.Empty)
            {
                currentName = save.timestampSave;
            }
            else
            {
                currentName = save.customName;
            }

            saveName.text = currentName;
            inputField.placeholder.GetComponent<TextMeshProUGUI>().text = currentName;
        }
    }


    public void Update()
    {
        if (enableEditing)
        {
            if (inside)
            {
                MouseoverBehaviour();
            }
            else if (Input.GetMouseButtonDown(0) && !editing && !saveLoadWindow.isMouseOverAnotherEditName()) // unselect
            {
                GameManager.instance.naming = false;
            }
        }

        
    }

    #endregion

    #region Mouse Input
    public void MouseoverBehaviour()
    {
        if (!editing)
        {
            Holder.color = mouseover;
        }

        if (Input.GetMouseButtonDown(0))  // if click inside
        {
            GameManager.instance.naming = true;

            editing = true;



            saveName.gameObject.SetActive(false);
            inputField.gameObject.SetActive(true);
            Holder.color = click;

            EventSystem.current.SetSelectedGameObject(inputField.gameObject, null);
            inputField.OnPointerClick(new PointerEventData(EventSystem.current));
        }
    }
    #endregion

    #region OnPointer Interface
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (enableEditing && !GameManager.instance.naming)
        {

            inside = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (enableEditing && !GameManager.instance.naming)
        {
            inside = false;
            if (!editing)
            {
                Holder.color = idle;
            }
        }
    }
    #endregion


}
