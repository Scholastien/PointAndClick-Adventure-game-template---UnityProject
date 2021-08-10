using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUIBehaviour : MonoBehaviour {

    public Text skillName;
    public Text extraInfo;

    public Slider slider;


    public Skill skillToDisplay;

    // Use this for initialization
    void Start () {
        skillName.text = skillToDisplay.SkillName;
        extraInfo.text = skillToDisplay.informations;
        slider.value = skillToDisplay.value;

    }
}
