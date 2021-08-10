using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Location", menuName = "LewdOwl/Navigation/Location")]
public class Location : ScriptableObject
{
    [Header("Display Settings")]
    [Tooltip("Display name")]
    public string displayName;
    [Header("Identifier")]
    [Tooltip("Name : Work like an ID, must be unique!")]
    public new string name;
    [Tooltip("Path in the asset folder")]
    public string scenePath;
    [Tooltip("Name in the asset folder")]
    public string sceneName;

    [Header("Properties")]
    [Tooltip("number ID in rooms (start at 0)")]
    public int defaultRoomID = 0;

    public List<Room> rooms;

    public Sound locationSound;
}