using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunDialogue : MonoBehaviour {

    public DialogueChain chainToRun;

    public int index = 0;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(chainToRun != null) {
            StartCoroutine(WaitForKeyDown(KeyCode.Space));
        }
	}

    IEnumerator WaitForKeyDown(KeyCode keyCode) {
        while (!Input.GetKeyDown(keyCode)) {
            Debug.Log(chainToRun.chainEvents[index].cEventType);
            yield return null;
        }
    }
}
