using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogueButtonNextBehaviour : DialogueButtonBehaviour
{

    new void Start()
    {
        base.Start();
    }

    private new void Update() {
        base.Update();
        CheckCEvent();
    }
    public void CheckCEvent() {
        if (GameManager.instance.managers.dialogueManager.ChainToRun != null) {
            if (!isWriting && isRunning) {
                if (GameManager.instance.managers.dialogueManager.GetCurrentEventType() != ChainEventType.Dialogue &&
                    GameManager.instance.managers.dialogueManager.GetCurrentEventType() != ChainEventType.SubDialogue)
                {
                    text.enabled = false;
                    button.interactable = false;
                } else {
                    text.enabled = true;
                    button.interactable = true;
                }
            }
        }
    }

    public void nextSentence() {
        if (!isWriting && !pressed) {
            pressed = true;
            GameObject myEventSystem = GameObject.Find("EventSystem");
            myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
            GameManager.instance.managers.dialogueManager.NextEvent();
        }
    }
    



}
