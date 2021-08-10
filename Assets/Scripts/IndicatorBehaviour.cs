using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorBehaviour : MonoBehaviour {

    public GameObject skipIndicator;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        skipIndicator.SetActive(GameManager.instance.managers.dialogueManager.skip);
	}
}
