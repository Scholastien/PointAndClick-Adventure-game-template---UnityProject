using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueButtonHideBehaviour : DialogueButtonBehaviour {

    DialogueSpawnBehaviour dialogueSpawn;
    string parentName;
    Image image;
    Text btn_txt;

    new void Start()
    {
        base.Start();
        dialogueSpawn = (DialogueSpawnBehaviour)FindObjectOfType(typeof(DialogueSpawnBehaviour));
        parentName = gameObject.transform.parent.name;
        image = gameObject.GetComponent<Image>();
        text = gameObject.GetComponentInChildren<Text>();
    }

    private new void Update() {
        base.Update();

        if(parentName == "BackButton")
        {
            text.enabled = dialogueSpawn.hide;
            if(image != null)
                image.enabled = text.enabled;
        }

        if(parentName == "SceneBackground")
        {
            image.enabled = dialogueSpawn.hide;
        }
    }

    public void HideUI() {
        if (!GameManager.instance.managers.dialogueManager.hiddenUi) {
            //dialogueSpawn = (DialogueSpawnBehaviour)FindObjectOfType(typeof(DialogueSpawnBehaviour));
            GameManager.instance.managers.dialogueManager.skip = false;
            dialogueSpawn.hide = true;
            GameManager.instance.managers.dialogueManager.hiddenUi = true;
            dialogueSpawn.dialogueUi.SetActive(false);
        } else {
            //Dialogue SpawnBehaviour dialogueSpawn = (DialogueSpawnBehaviour)FindObjectOfType(typeof(DialogueSpawnBehaviour));
            GameManager.instance.managers.dialogueManager.skip = false;
            dialogueSpawn.hide = false;
            dialogueSpawn.showLog = false;
            GameManager.instance.managers.dialogueManager.hiddenUi = false;
            dialogueSpawn.dialogueUi.SetActive(true);
        }
        UnselectCurrent();
    }

}
