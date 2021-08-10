using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueButtonSkipBehaviour : DialogueButtonBehaviour {

    [HideInInspector]
    public DialogueSpawnBehaviour dialogueSpawn;

    new void Start()
    {
        base.Start();
        dialogueSpawn = (DialogueSpawnBehaviour)FindObjectOfType(typeof(DialogueSpawnBehaviour));
    }

    private new void Update() {
        base.Update();
        dialogueSpawn.skip = GameManager.instance.managers.dialogueManager.skip;

        //Debug.Log("isRunning=" + isRunning +
        //    "\t cEventType=" + GameManager.instance.managers.dialogueManager.ChainToRun.currentEvent.cEventType   +
        //    "\n isWriting=" + isWriting +
        //    "\t dialogueManager.skip=" + GameManager.instance.managers.dialogueManager.skip);
        
    }

    private void FixedUpdate() {
        if (GameManager.instance.managers.dialogueManager.skip
            && GameManager.instance.managers.dialogueManager.isRunning
            && !GameManager.instance.managers.dialogueManager.isWriting
            && GameManager.instance.managers.dialogueManager.ChainToRun.currentEvent.cEventType != ChainEventType.UserInput) {

            GameManager.instance.managers.dialogueManager.Skip();
        }
    }

    public void Skip() {
        if (dialogueSpawn.skip) {
            GameManager.instance.managers.dialogueManager.skip = false;
        } else {

            GameObject myEventSystem = GameObject.Find("EventSystem");
            myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
            GameManager.instance.managers.dialogueManager.skip = true;
        }

    }

    public IEnumerator SpamMaxWritingSpeed() {
        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
        // while current event is not a choice and button or input pressed
        do {

            if (GameManager.instance.managers.dialogueManager.ChainToRun.currentEvent.dialogueContainer != ContainerType.Input)
                GameManager.instance.managers.dialogueManager.NextEvent();
            else {
                while (isWriting && isRunning) {
                        yield return new WaitForEndOfFrame();
                }
            }



            
            

        } while (dialogueSpawn.skip);
    }
}
