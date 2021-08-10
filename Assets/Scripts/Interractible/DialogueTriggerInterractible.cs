using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerInterractible : Interractible
{
    [Header("Properties")]
    public DialogueChain ChainToRun;


    // Use this for initialization
    new void Start()
    {
        base.Start();
    }
    new void LateUpdate()
    {
        base.LateUpdate();
    }

    public void StartDialoque()
    {
        if (!disable)
        {
            if (ChainToRun != null)
                GameManager.instance.managers.dialogueManager.RunDialogue(ChainToRun);
            else
                Debug.Log("ChainToRun in this DialogueTrigger <color=red>undefined</color> at \n" + displayName + "(<color=blue>" + nameIdentifier + "</color>)");
            //EventManager.TriggerEvent("StartEndTransitionAnimation");
        }
    }
}
