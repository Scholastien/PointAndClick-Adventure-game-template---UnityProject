using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Room", menuName = "LewdOwl/Navigation/Room")]
public class Room : ScriptableObject {

    [Header("Identifier")]
    [Tooltip("Name : Work like an ID, must be unique!")]
    public new string name;
    [Tooltip("Display name")]
    public string displayName;

    [Header("Display Settings")]
    [Tooltip("Icon used by the NavigationUI")]
    public Sprite navIcon;

    public bool locked;

    public List<DaytimeSpriteVariation> daytimeSpriteVariation;

    public Room(Room room) {
        name = room.name;
        displayName = room.displayName;
        navIcon = room.navIcon;
        daytimeSpriteVariation = room.daytimeSpriteVariation;
    }

    public Sprite give_DaytimeSprite(TimeOfTheDay currentTime)
    {
        foreach(DaytimeSpriteVariation daytimeSprite in daytimeSpriteVariation)
        {
            if (daytimeSprite.IsActive(currentTime))
            {
                //if(GameManager.instance.managers.navigationManager.Debug_NavigationLog)
                //    Debug.Log("Picking a BG for this room from : \n" +"<color=green>" + daytimeSprite.begin + "</color> \t to   <color=green>" + daytimeSprite.end + "</color> ");
                return daytimeSprite.sprite;
            }
        }
        return null;
    }

    public void GetNavIcon(TimeOfTheDay currentTime)
    {
        navIcon = give_DaytimeSprite(currentTime);
    }
}

[System.Serializable]
public class DaytimeSpriteVariation
{
    public TimeOfTheDay begin;
    public TimeOfTheDay end;

    public Sprite sprite;


    //Check if the daytimeSprite is active with current time
    public bool IsActive(TimeOfTheDay currentTime)
    {
        int start, finish, x;

        start = (int)begin;
        finish = (int)end;
        x = (int)currentTime;

        // 7AM 8AM 9AM 10AM 11AM 12PM .... 10PM 11PM 12AM 1AM 2AM
        //  ▼   ▼   ▼    ▼   ▼    ▼         ▼    ▼    ▼    ▼   ▼
        //          ▲                                 ▲
        //        start                             finish 
        //          x    x   x    x  .....  x    x    x
        //
        //                        OR
        //        finish                            start 
        //  x   x   x                                 x    x   x
        //               Possible position of x

        bool a = (x <= finish);
        bool b = (x >= start);

        if (start <= finish)
            return (a && b);
        else
            return (a && !b) || (!a && b);
    }
}
