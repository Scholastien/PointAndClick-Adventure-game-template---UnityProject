using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerUI : MonoBehaviour {

    public bool hide = false;

    public DialogueContainer dialogueContainer;

    public void Update()
    {
        dialogueContainer.dialogueBoxImage.gameObject.SetActive(!hide);

        if (Input.GetKeyDown(KeyCode.H))
        {
            DialogueManager.instance.blockNext = !hide;
            hide = !hide;
        }

        if (Input.GetButton("Submit") && hide && TransitionManager.instance.IsAnimCompleted())
        {
            hide = !hide;
            StartCoroutine(UpdateBlockOneFrameLater());
        }

    }

    private IEnumerator UpdateBlockOneFrameLater()
    {
        yield return new WaitForSeconds(0.1f);
        DialogueManager.instance.blockNext = hide;
    }

    // inputs

}
