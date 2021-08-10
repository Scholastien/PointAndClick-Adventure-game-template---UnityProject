using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunChain : MonoBehaviour {
    public DialogueChain chainToRun;

    public RunDialogue runDialogue;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    public void chainRun() {
        runDialogue.chainToRun = chainToRun;
        //chainToRun.StartChain();
    }
}