using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScheduleSystem;

public class NpcScheduleBehaviour : MonoBehaviour
{

    public CharacterFunction character;
    [HideInInspector]
    public CharacterInterractible characterInterractible;
    public CharacterInterractible currentCharacterInterractible;

    [HideInInspector]
    public CharacterInterractible previousCharacterInterractible;

    [HideInInspector]
    public Room CharacterInterractibleRoom;

    [HideInInspector]
    public TimeOfTheDay time;

    [HideInInspector]
    public DayOfTheWeek day;

    public Activity currentActivity;
    //[HideInInspector]
    public Room ActivityRoom;

    public Schedule schedule;

    public ScheduleChanges scheduleChanges;

    [HideInInspector]
    public ScheduleManager sm;

    [HideInInspector]
    public bool needRefresh;

    // Use this for initialization
    void Start()
    {
        sm = GameManager.instance.managers.scheduleManager;
        needRefresh = true;

        sm.npcsSchedulebehaviour.Add(this);

    }

    // Update is called once per frame
    void Update()
    {

        scheduleChanges = sm.GetCharacterScheduleChanges(character);
        schedule = sm.GetCharacterSchedule(character);

        //if (currentActivity != DetermineCurrentActivity() && ((time != sm.tm.currentTime) || (day != sm.tm.currentDay)))
        //{
        //    needRefresh = true;
        //    currentActivity = DetermineCurrentActivity();
        //}

        //if ((time != sm.tm.currentTime) || (day != sm.tm.currentDay) || needRefresh)
        //{
        //    time = sm.tm.currentTime;
        //    day = sm.tm.currentDay;
        //    if (currentCharacterInterractible != null)
        //        if (IsScheduled(currentCharacterInterractible))
        //            MakeInterractible(currentCharacterInterractible);
        //    needRefresh = false;
        //}

        currentActivity = DetermineCurrentActivity();

        //if (needRefresh)
        //{
        //    if (currentCharacterInterractible != null)
        //    {
        //        currentCharacterInterractible.image.sprite = sm.transparentInterractible;
        //    }
        //    MakeInterractible();
        //}
    }

    public Activity DetermineCurrentActivity()
    {
        if(currentActivity != null)
            GameManager.instance.managers.variablesManager.gameSavedData.GameInfo.ModifyCharacterLocation(character, currentActivity.room, true);
        return schedule.GetActivityAtDayAndTime(scheduleChanges, sm.tm.currentDay, sm.tm.currentTime);
    }

    public void NextActivity(Activity activity)
    {
        ScheduleManager.instance.SetNextActivity(activity);
    }

    public void MakeInterractible(CharacterInterractible _characterInterractible)
    {

        //if (previousCharacterInterractible != null)
        //    previousCharacterInterractible.hide = true;
        if (currentCharacterInterractible == null)
        {

        }
        else if (currentCharacterInterractible.activity != currentActivity)
        {
            _characterInterractible.image.sprite = currentActivity.GetSprite();
            currentCharacterInterractible.activity = currentActivity;
            if (currentActivity.dialogueToStart != null)
            {
                currentCharacterInterractible.ChainToRun = currentActivity.dialogueToStart;
            }
            _characterInterractible.npcScheduleBehaviour = this;
        }

        //if (currentCharacterInterractible.room == currentActivity.room)
        //{
        //    if (currentCharacterInterractible.hide)
        //    {
        //        currentCharacterInterractible.hide = false;
        //        currentCharacterInterractible.image.sprite = currentActivity.GetSprite();
        //        if (currentActivity.dialogueToStart != null)
        //        {
        //            currentCharacterInterractible.ChainToRun = currentActivity.dialogueToStart;
        //        }
        //        needRefresh = false;
        //    }
        //}
        //else
        //{
        //    currentCharacterInterractible.hide = true;
        //}
    }

    public bool IsScheduled(CharacterInterractible _characterInterractible)
    {
        if (_characterInterractible != null && currentActivity != null)
            if (_characterInterractible.room == currentActivity.room)
                return true;
            else
                return false;
        else
            return false;
    }
}
