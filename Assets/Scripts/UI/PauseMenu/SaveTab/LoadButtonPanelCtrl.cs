using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SaveLoadWindowController))]
public class LoadButtonPanelCtrl : MonoBehaviour
{

    private SaveLoadWindowController windowController;

    public GameObject load_Btn;

    private List<GameData> saves;

    // Use this for initialization
    void OnEnable()
    {

        windowController = gameObject.GetComponent<SaveLoadWindowController>();
        //windowController.SpawnAllSavesPreview();

        //saves = GetCurrentSaves();
        //for (int i = 0; i < saves.Count; i++)
        //{
        //    InstantiateLoadButton(i);
        //}
    }


    private List<GameData> GetCurrentSaves()
    {
        return GameManager.instance.managers.saveManager.saves;
    }

    private void InstantiateLoadButton(int saveIndex)
    {
        GameObject newButton = Instantiate(load_Btn, gameObject.transform);
        newButton.name = "Btn_" + saves[saveIndex].name;
        newButton.GetComponentInChildren<LoadButtonBehaviour>().index = saveIndex;
        newButton.GetComponentInChildren<LoadButtonBehaviour>().label = saves[saveIndex].name;
        newButton.GetComponentInChildren<LoadButtonBehaviour>().GetDataFromSave(saves[saveIndex]);


    }
}
