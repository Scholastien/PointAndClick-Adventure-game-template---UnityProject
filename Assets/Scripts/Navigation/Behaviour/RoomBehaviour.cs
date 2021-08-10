using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour {

    public Room room;

	// Use this for initialization
	void Start () {
        Init();
	}
	
	// Update is called once per frame
	void Update () {
	}

    public int GetMyRoomID() {
        int id = 0;

        for( int i = 0; i <  gameObject.transform.parent.childCount; i++) {
            if (gameObject.transform.parent.GetChild(i) == gameObject.transform) {

                id = i;
            }
        }
        return id;
    }

    public void Init() {
        room = 
            gameObject.GetComponentInParent<LocationCanvasBehaviour>()
                .currentLocation.rooms[GetMyRoomID()];
    }
}
