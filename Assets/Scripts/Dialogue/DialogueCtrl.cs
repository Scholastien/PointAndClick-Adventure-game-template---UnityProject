using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

public class DialogueCtrl
{

    public DialogueChain dialogueChain;

    public DialogueCtrl(DialogueChain dialChain)
    {
        dialogueChain = dialChain;
        StartDialogue();
    }

    public void StartDialogue()
    {
        //Debug.Log("start Dialoque from dialogue ctrl");
        if (!GameManager.instance.managers.dialogueManager.isRunning)
            dialogueChain.StartChain();
    }

    public void Next()
    {
        dialogueChain.GetNextEvent();

    }



}
