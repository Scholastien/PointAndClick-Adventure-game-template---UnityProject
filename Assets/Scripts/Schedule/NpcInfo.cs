using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScheduleSystem;


[System.Serializable]
public class NpcSchedule
{
    [HideInInspector]
    public string name;
    public CharacterFunction character;
    public Schedule schedule;
}

[System.Serializable]
public class NpcScheduleChanges
{
    [HideInInspector]
    public string name;

    public ScheduleChanges scheduleChanges;

    public NpcScheduleChanges(string newname, ScheduleChanges changes)
    {
        name = newname;
        scheduleChanges = changes;
    }

    public NpcScheduleChanges()
    {

    }
}

[System.Serializable]
public class NpcSerializedSchedule
{
    [HideInInspector]
    public string name;

    public List<int> scheduleChangesSerialized;

    public void SetName(string newname)
    {
        name = newname;
    }

    public NpcSerializedSchedule(string newname)
    {
        name = newname;
        scheduleChangesSerialized = new List<int>();
    }

    public NpcSerializedSchedule(ScheduleChangesCollection changesCollection, NpcScheduleChanges npcSchedule)
    {

    }

}



