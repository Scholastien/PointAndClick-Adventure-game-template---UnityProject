using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class contain every variable used by the UI.
/// It will allow the UI to catch data from here in order to have a coherent display
/// Most of the value should be change using the EventManager.
/// 
/// </summary>
[System.Serializable]
public class GUI_properties
{
    

    [Header("Info Bar")]
    public Sub_MenuBehaviour.SubMenuState SubMenuState = Sub_MenuBehaviour.SubMenuState.Closed;
    public string LocationName;
    public string RoomName;
    public int CurrentEnergy;
    public string Day;
    public string Time;
    public int TimeValue;
    public bool ShowWaitButton;
    public string Money;
    [Header("Nav Bar")]
    public List<NavIcon> navIcons;
}

[System.Serializable]
public class NavIcon
{
    public Sprite Picture;
    public string RoomName;
}
