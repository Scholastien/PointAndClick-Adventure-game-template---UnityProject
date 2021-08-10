using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image), typeof(Button))]
public class MapButtonBehaviour : MonoBehaviour {

    public NavigationController navCtrl;

    [Header("Processed Data")]
    public Room RoomToGo;

    [Header("Display")]
    public Sprite townIcon;
    public Sprite bedIcon;



    [Header("Raw data")]
    public Image icon;
    [Range(0,100)]
    public float transparency;

    public Room townRoom;
    public Room bedRoom;

    private Button button;
    private Image image;

    private MouseoverBehaviour mouseoverBehaviour;

	// Use this for initialization
	void Start () {
        mouseoverBehaviour = gameObject.GetComponent<MouseoverBehaviour>();
        button = gameObject.GetComponent<Button>();
        image = gameObject.GetComponent<Image>();

    }
	
	// Update is called once per frame
	void Update () {

        DetermineButtonBehaviour();

        DisableMap();

    }

    public void GoToMap() {
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
        GameManager.instance.managers.navigationManager.SetRoom(RoomToGo.name);
    }

    public void DisableMap()
    {
        button = gameObject.GetComponent<Button>();
        image = gameObject.GetComponent<Image>();

        button.enabled = !RoomToGo.locked;
        image.enabled = !RoomToGo.locked;
    }

    public void DetermineButtonBehaviour()
    {
        if(navCtrl.currentRoom != townRoom)
        {
            icon.sprite = townIcon;
            RoomToGo = townRoom;
            mouseoverBehaviour.displayName = "Town map";
        }else
        {
            icon.sprite = bedIcon;
            RoomToGo = bedRoom;
            mouseoverBehaviour.displayName = "My bedroom";
        }
    }
}
