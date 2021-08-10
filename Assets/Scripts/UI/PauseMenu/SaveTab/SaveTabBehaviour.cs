using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveTabBehaviour : MonoBehaviour {
    public List<GameData> saves;
    [HideInInspector]
    public List<GameObject> savesBuffer;
    public GameObject SaveScrollableList;
    public GameObject SavePreviewPrefab;
    public GameObject AddNewSave;
    public Scrollbar SaveScrollbar;

    public void SpawnSavePreview() {
        saves = GameManager.instance.managers.saveManager.saves;
        GameData lastSave = new GameData();
        for (int i = 0; i < saves.Count; i++) {
            GameObject savePreview = Instantiate(SavePreviewPrefab, SaveScrollableList.transform);
            savePreview.GetComponent<SavePreviewBehaviour>().ID = i;
            savePreview.GetComponent<SavePreviewBehaviour>().save = saves[i];
            lastSave = saves[i];
            savesBuffer.Add(savePreview);
        }
        AddNewSave.transform.SetAsLastSibling();

        SaveScrollbar.value = 0f;
    }
    public void ResetSaveList() {
        foreach (GameObject go in savesBuffer) {
            Destroy(go);
        }
        savesBuffer = new List<GameObject>();
    }
}
