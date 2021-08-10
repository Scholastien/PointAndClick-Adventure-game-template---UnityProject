using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum DisclaimerType
{
    ReturnToMainMenu,
    OverwriteSave,
    LoadSave,
    RemoveSave,
    RestoreDefault,
    QuitGame
}

[System.Serializable]
public class WindowFeeder
{
    public GameObject disclaimerWindow;

    public Image imgPreview;

    public TextMeshProUGUI saveName;
    public TextMeshProUGUI date;

    public TextMeshProUGUI location;
    public TextMeshProUGUI money;

    public WindowFeeder()
    {

    }
}

public class DisclaimerMenuBehaviour : MonoBehaviour
{
    public GameObject DisclaimersHolder;

    [Header("Disclaimer windows")]
    public DisclaimerPopupBehaviour ReturnToMainMenu;
    public DisclaimerPopupBehaviour OverwriteSave;
    public DisclaimerPopupBehaviour LoadSave;
    public DisclaimerPopupBehaviour RemoveSave;
    public DisclaimerPopupBehaviour RestoreDefault;
    public DisclaimerPopupBehaviour QuitGame;

    public void Start()
    {
        CloseDisclaimer();
    }

    #region Display
    public void CloseDisclaimer()
    {
        // Disable all children
        foreach (Transform child in DisclaimersHolder.transform)
            child.gameObject.SetActive(false);

        // Disable holder
        DisclaimersHolder.SetActive(false);
    }

    private void EnableHolder()
    {
        DisclaimersHolder.SetActive(true);
    }

    private void DisplayDisclaimerWindow(DisclaimerType disclaimerType)
    {
        EnableHolder();
        switch (disclaimerType)
        {
            case DisclaimerType.ReturnToMainMenu:
                // Show Return to main menu window disclaimer
                ReturnToMainMenu.gameObject.SetActive(true);
                OverwriteSave.gameObject.SetActive(false);
                LoadSave.gameObject.SetActive(false);
                RemoveSave.gameObject.SetActive(false);
                RestoreDefault.gameObject.SetActive(false);
                QuitGame.gameObject.SetActive(false);
                break;
            case DisclaimerType.OverwriteSave:
                // Show overwrite save window disclaimer
                ReturnToMainMenu.gameObject.SetActive(false);
                OverwriteSave.gameObject.SetActive(true);
                LoadSave.gameObject.SetActive(false);
                RemoveSave.gameObject.SetActive(false);
                RestoreDefault.gameObject.SetActive(false);
                QuitGame.gameObject.SetActive(false);
                break;
            case DisclaimerType.LoadSave:
                // Show load save window disclaimer
                ReturnToMainMenu.gameObject.SetActive(false);
                OverwriteSave.gameObject.SetActive(false);
                LoadSave.gameObject.SetActive(true);
                RemoveSave.gameObject.SetActive(false);
                RestoreDefault.gameObject.SetActive(false);
                QuitGame.gameObject.SetActive(false);
                break;
            case DisclaimerType.RemoveSave:
                // Show remove save window disclaimer
                ReturnToMainMenu.gameObject.SetActive(false);
                OverwriteSave.gameObject.SetActive(false);
                LoadSave.gameObject.SetActive(false);
                RemoveSave.gameObject.SetActive(true);
                RestoreDefault.gameObject.SetActive(false);
                QuitGame.gameObject.SetActive(false);
                break;
            case DisclaimerType.RestoreDefault:
                // Show restore settings window disclaimer
                ReturnToMainMenu.gameObject.SetActive(false);
                OverwriteSave.gameObject.SetActive(false);
                LoadSave.gameObject.SetActive(false);
                RemoveSave.gameObject.SetActive(false);
                RestoreDefault.gameObject.SetActive(true);
                QuitGame.gameObject.SetActive(false);
                break;
            case DisclaimerType.QuitGame:
                // Show restore settings window disclaimer
                ReturnToMainMenu.gameObject.SetActive(false);
                OverwriteSave.gameObject.SetActive(false);
                LoadSave.gameObject.SetActive(false);
                RemoveSave.gameObject.SetActive(false);
                RestoreDefault.gameObject.SetActive(false);
                QuitGame.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }
    #endregion

    #region DataManipulation

    public void RemplaceInfo(DisclaimerType disclaimerType, SaveType saveType, int id)
    {
        GameData selectedSave;
        if (saveType == SaveType.defaultSave)
            selectedSave = GameManager.instance.managers.saveManager.saves[id];
        else if (saveType == SaveType.quickSave)
            selectedSave = GameManager.instance.managers.saveManager.quickSave;
        else
            selectedSave = GameManager.instance.managers.saveManager.quickSave; // remplace with auto save


        VariablesManager vm = GameManager.instance.managers.variablesManager;
        Room savedRoom = vm.locationCollection.GetRoom(selectedSave.GameInfo.currentRoom);
        Location savedLocation = vm.locationCollection.GetLocation(savedRoom);

        switch (disclaimerType)
        {
            case DisclaimerType.OverwriteSave:
                OverwriteSave.RemplaceInfo(new UI_SavedData(selectedSave, id));
                break;
            case DisclaimerType.LoadSave:
                LoadSave.RemplaceInfo(new UI_SavedData(selectedSave, id));
                break;
            case DisclaimerType.RemoveSave:
                RemoveSave.RemplaceInfo(new UI_SavedData(selectedSave, id));
                break;
            default:
                break;
        }

        DisplayDisclaimerWindow(disclaimerType);

        //windowDislaimer.saveName.text = selectedSave.name;

        //windowDislaimer.date.text = selectedSave.timestampSave;

        //windowDislaimer.imgPreview.sprite = savedRoom.navIcon;
        //windowDislaimer.location.text = savedLocation.displayName;

        //windowDislaimer.money.text = selectedSave.GameInfo.mainCharacter.money.ToString();
    }

    #endregion

    #region ClickBehaviour

    public void Disclaimer_ReturnMainMenu()
    {
        DisplayDisclaimerWindow(DisclaimerType.ReturnToMainMenu);
    }

    public void Confirmation_ReturnMainMenu()
    {
        GameManager.instance.managers.sceneManager.BackToMainMenu();
    }

    public void Disclaimer_QuitGame()
    {
        DisplayDisclaimerWindow(DisclaimerType.QuitGame);
    }

    public void Confirmation_QuitGame()
    {
        GameManager.instance.QuitApplication();
    }

    public void Disclaimer_RestoreDefaultSettings()
    {
        DisplayDisclaimerWindow(DisclaimerType.RestoreDefault);
    }

    public void Confirmation_RestoreDefaultSettings(OptionTabBehaviour optionTabBehaviour)
    {
        optionTabBehaviour.RestoreDefault();
    }

    #endregion
}
