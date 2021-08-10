using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class PauseMenuUIBehaviour : MonoBehaviour {



    public SaveTabBehaviour saveTab;
    public TaskTabBehaviour taskTab;
    public InformationBehaviour statsTab;
    public InventoryTabBehaviour inventoryTab;
    public OptionTabBehaviour optionTab;
    public GalleryTabBehaviour galleryTab;



    public int interactionSaveIndex;
    public DisclaimerType disclaimerType;
    public DisclaimerBehaviour disclaimer;

    public GameObject firstSelected;

    private void Awake() {
        GameManager.instance.gameObject.GetComponentInChildren<EventSystem>().SetSelectedGameObject(firstSelected);
    }


    // Use this for initialization
    void Start () {

        statsTab.UpdateInformations();

        saveTab.saves = GameManager.instance.managers.saveManager.saves;


        saveTab.savesBuffer = new List<GameObject>();

        saveTab.SpawnSavePreview();

        taskTab.SpawnTask();

        galleryTab.SpawnRevelentItems();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    

    public void GoToMainMenu() {
        disclaimerType = DisclaimerType.ReturnToMainMenu;
        disclaimer.gameObject.SetActive(true);
        disclaimer.DisplayDisclaimerWindow(interactionSaveIndex);
    }

    public void OverwriteSaveDisclaimer() {
        disclaimerType = DisclaimerType.OverwriteSave;
        disclaimer.gameObject.SetActive(true);
        disclaimer.DisplayDisclaimerWindow(interactionSaveIndex);
    }
    public void LoadSaveDisclaimer() {
        disclaimerType = DisclaimerType.LoadSave;
        disclaimer.gameObject.SetActive(true);
        disclaimer.DisplayDisclaimerWindow(interactionSaveIndex);
    }
    public void RemoveSaveDisclaimer() {
        disclaimerType = DisclaimerType.RemoveSave;
        disclaimer.gameObject.SetActive(true);
        disclaimer.DisplayDisclaimerWindow(interactionSaveIndex);
    }
    public void RestoreDefaultDisclaimer()
    {
        disclaimerType = DisclaimerType.RestoreDefault;
        disclaimer.gameObject.SetActive(true);
        disclaimer.DisplayDisclaimerWindow(interactionSaveIndex);
    }

    public void OverwriteSave() {
        GameManager.instance.managers.saveManager.SaveAt(interactionSaveIndex);

        saveTab.saves = GameManager.instance.managers.saveManager.saves;
        saveTab.ResetSaveList();
        saveTab.SpawnSavePreview();
    }
    public void LoadSave() {
        GameManager.instance.managers.saveManager.LoadGame(interactionSaveIndex);

        saveTab.saves = GameManager.instance.managers.saveManager.saves;
        saveTab.ResetSaveList();
        saveTab.SpawnSavePreview();
    }
    public void RemoveSave() {
        Debug.Log(interactionSaveIndex + " Remove");
        GameManager.instance.managers.saveManager.RemoveSave(interactionSaveIndex);

        saveTab.saves = GameManager.instance.managers.saveManager.saves;
        saveTab.ResetSaveList();
        saveTab.SpawnSavePreview();
    }

    public void AddSave() {
        GameManager.instance.managers.saveManager.AddSave();

        saveTab.saves = GameManager.instance.managers.saveManager.saves;
        saveTab.ResetSaveList();
        saveTab.SpawnSavePreview();
    }

    public void RestoreDefaultSettings()
    {
        Debug.Log("Restore");
        GameManager.instance.managers.optionManager.mixerVolumes = new MixerVolumes();
        optionTab.fullScreen.isOn = false;
        GameManager.instance.managers.optionManager.fullScreen = false;
        optionTab.SetSliders();
        optionTab.SendSliderValue();
        optionTab.Save();
    }
}
