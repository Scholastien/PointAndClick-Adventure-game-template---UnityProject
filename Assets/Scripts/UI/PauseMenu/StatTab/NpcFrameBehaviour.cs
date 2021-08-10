using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcFrameBehaviour : MonoBehaviour
{

    public Text characterName;
    public Text description;
    public Text scenarioHint;
    public Text currentLocation;

    public Slider relationshipSlider;




    public Npc npcToDisplay;

    // Use this for initialization
    void Start()
    {
        characterName.text = npcToDisplay.name;
        description.text = npcToDisplay.description;
        scenarioHint. text = npcToDisplay.extraInfo;
        currentLocation. text = npcToDisplay.currentLocation;

        relationshipSlider.value = npcToDisplay.relationshipPoint;
}
    

}
