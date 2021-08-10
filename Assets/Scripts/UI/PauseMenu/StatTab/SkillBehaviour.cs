using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SkillBehaviour : MonoBehaviour {

    private Slider slider;

    public ChainIntType dataToTrack;

    // Use this for initialization
    void Start () {
        slider = gameObject.GetComponent<Slider>();
	}
	
	// Update is called once per frame
	void Update () {
        slider.value = DialogueChainPreferences.GetChainInt(dataToTrack);
	}
}
