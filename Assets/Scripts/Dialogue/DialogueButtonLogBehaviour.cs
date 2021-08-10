using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueButtonLogBehaviour : DialogueButtonBehaviour, IPointerExitHandler
{

    [HideInInspector]
    public DialogueSpawnBehaviour dialogueSpawn;
    public DialogueButtonHideBehaviour hideBehaviour;

    new void Start()
    {
        base.Start();
        dialogueSpawn = (DialogueSpawnBehaviour)FindObjectOfType(typeof(DialogueSpawnBehaviour));
    }

    private new void Update()
    {
        base.Update();
        if (dialogueSpawn.showLog != false)
        {
            gameObject.GetComponent<Button>().Select();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (dialogueSpawn.showLog == false)
        {
            GameObject myEventSystem = GameObject.Find("EventSystem");
            myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
        }
    }

    public void ShowLog()
    {
        if (dialogueSpawn.showLog == false)
        {
            if (!GameManager.instance.managers.dialogueManager.hiddenUi)
                hideBehaviour.HideUI();
            dialogueSpawn.logScrollList.GetComponent<LogScrollListBehaviour>().SpawnLogHistory();
            dialogueSpawn.showLog = true;
        }
        else
        {
            dialogueSpawn.showLog = false;
        }

    }
    
}
