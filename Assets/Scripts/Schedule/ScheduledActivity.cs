using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleSystem
{

    [System.Serializable]
    public class SerializableScheduledActivity
    {
        [HideInInspector]
        public string name;
        public ScheduledActivity scheduledActivity;

        public SerializableScheduledActivity(ScheduledActivity s_Activity)
        {
            scheduledActivity = s_Activity;
            name = s_Activity.display_name;
        }

    }

    // Scriptable object used to plan an activity for a character
    [System.Serializable, CreateAssetMenu(fileName = "new ScheduledActivity", menuName = "LewdOwl/ScheduleSystem/ScheduledActivity")]
    public class ScheduledActivity : Activity
    {
        public Activity previousActivity;

        [Header("Properties Everyday and Time interval")]

        public DayOfTheWeek dayStart;
        public DayOfTheWeek dayEnd;
        public TimeOfTheDay timeStart;
        public TimeOfTheDay timeEnd;


        public bool IsDayActive(DayOfTheWeek day)
        {
            bool between = (((int)day >= (int)dayStart)
                         && ((int)day <= (int)dayEnd));

            bool notBetween = !(((int)day > (int)dayEnd)
                        &&
                            ((int)day < (int)dayStart));

            if (dayStart <= dayEnd)
            {
                if(GameManager.instance.managers.timeManager.Debug_TimeLog)
                    Debug.Log(between + " : IS Day Active");
                return between;
            }
            else
            {
                if (GameManager.instance.managers.timeManager.Debug_TimeLog)
                    Debug.Log(notBetween + " : IS Day Active");
                return notBetween;
            }
        }

        public bool IsTimeActive(TimeOfTheDay time)
        {

            bool between =( ((int)time >= (int)timeStart)
                         && ((int)time <= (int)timeEnd) );

            bool notBetween = !(((int)time > (int)timeEnd )
                        &&
                            ((int)time < (int)timeStart));



            if(timeStart <= timeEnd)
            {
                if (GameManager.instance.managers.timeManager.Debug_TimeLog)
                    Debug.Log(between + " : IS Time Active");
                return between;
            }
            else
            {
                if (GameManager.instance.managers.timeManager.Debug_TimeLog)
                    Debug.Log(notBetween + " : IS Time Active");
                return notBetween;
            }
        }
    }

}