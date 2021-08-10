using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class DisclaimerBehaviour : MonoBehaviour {

    public PauseMenuUIBehaviour uiBehaviour;

    public GameObject MainMenu_Window;
    public GameObject RestoreDefault_Window;
    public WindowFeeder OverwriteSave_Window;
    public WindowFeeder LoadSave_Window;
    public WindowFeeder RemoveSave_Window;



    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public void DisplayDisclaimerWindow(int id) {
        switch (uiBehaviour.disclaimerType)
        {
            case DisclaimerType.ReturnToMainMenu:
                MainMenu_Window.SetActive(true);
                break;
            case DisclaimerType.RestoreDefault:
                RestoreDefault_Window.SetActive(true);
                break;
            case DisclaimerType.OverwriteSave:
                OverwriteSave_Window.disclaimerWindow.SetActive(true);
                RemplaceInfo(OverwriteSave_Window, id);
                break;
            case DisclaimerType.LoadSave:
                LoadSave_Window.disclaimerWindow.SetActive(true);
                RemplaceInfo(LoadSave_Window, id);
                break;
            case DisclaimerType.RemoveSave:
                RemoveSave_Window.disclaimerWindow.SetActive(true);
                RemplaceInfo(RemoveSave_Window, id);
                break;
        }
    }

    public void RemplaceInfo(WindowFeeder windowDislaimer, int id) {

        GameData selectedSave = GameManager.instance.managers.saveManager.saves[id];

        VariablesManager vm = GameManager.instance.managers.variablesManager;

        windowDislaimer.saveName.text = selectedSave.name;

        windowDislaimer.date.text = selectedSave.timestampSave;

        Room savedRoom = vm.locationCollection.GetRoom(selectedSave.GameInfo.currentRoom);
        Location savedLocation = vm.locationCollection.GetLocation(savedRoom);
        windowDislaimer.imgPreview.sprite = savedRoom.navIcon;
        windowDislaimer.location.text = savedLocation.displayName;

        windowDislaimer.money.text = selectedSave.GameInfo.mainCharacter.money.ToString();
    }
}
