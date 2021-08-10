using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "new LocationCollection", menuName = "LewdOwl/Navigation/LocationCollection")]
public class LocationCollection : ScriptableObject
{

    [Tooltip("number ID in locations (start at 0)")]
    public int defaultLocationID = 1;
    public List<Location> locations;

    public Room SetFirstRoom()
    {
        Room room = GameManager.instance.managers.variablesManager.locationCollection.locations[defaultLocationID]
            .rooms[GameManager.instance.managers.variablesManager.locationCollection.locations[defaultLocationID].defaultRoomID];
        GameManager.instance.managers.variablesManager.gameSavedData.GameInfo.currentRoom = room.name;
        return room;
    }

    public int GetLocationID(string roomName)
    {
        Room room = GetRoom(roomName);
        Location location = GetLocation(room);
        for (int i = 0; i < locations.Count; i++)
        {
            if (locations[i] == location)
                return i;
        }
        Debug.Log("Location not found");
        return 0;
    }

    public int GetRoomID(Location location, string roomName)
    {
        for (int i = 0; i < location.rooms.Count; i++)
        {
            if (location.rooms[i].name == roomName)
                return i;
        }
        return 0;
    }

    public Location GetLocation(Room currentRoom)
    {
        if (currentRoom != null)
        {
            foreach (Location location in locations)
            {
                foreach (Room room in location.rooms)
                {
                    if (currentRoom == room)
                        return location;
                }
            }
        }
        return locations[defaultLocationID];
    }

    public Room GetRoom(string roomName)
    {
        foreach (Location location in locations)
        {
            if (location != null)
            {
                foreach (Room room in location.rooms)
                {
                    if (roomName == room.name)
                    {
                        return room;
                    }
                }
            }
            else
            {
                Debug.LogError("Location doesn't exist in the current LocationCollection. \n Please verify that all field in the locationCollection.Locations are filled!");
            }
        }
        Debug.LogError("RoomName : " + roomName + " doesn't exist in the current LocationCollection");
        return GetLocation(locations[0].rooms[0]).rooms[0];
    }

    public void LoadDataFromSaveSystem(List<bool> list)
    {
        List<Room> allRooms = new List<Room>();
        foreach (Location location in GameManager.instance.managers.variablesManager.locationCollection.locations)
        {
            foreach (Room room in location.rooms)
            {
                allRooms.Add(room);
            }
        }

        for (int i = 0; i < list.Count; i++)
        {
            allRooms[i].locked = list[i];
        }
    }

    public void UpdateBoolValues()
    {
        List<Room> allRooms = new List<Room>();
        foreach (Location location in GameManager.instance.managers.variablesManager.locationCollection.locations)
        {
            foreach (Room room in location.rooms)
            {
                allRooms.Add(room);
            }
        }
        
        for (int i = 0; i < allRooms.Count; i++)
        {
            GameManager.instance.managers.variablesManager.gameSavedData.locationLockState[i] = allRooms[i].locked;
        }
    }
}




