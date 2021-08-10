using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleSystem
{

    // Class that contain the base schedule for a character
    [System.Serializable]
    public class Schedule
    {
        public DaySchedule monday,
                            tuesday,
                            wednesday,
                            thursday,
                            friday,
                            saturday,
                            sunday;

        public DayOfTheWeek currentDay;
        public TimeOfTheDay currentTime;
        public Activity currentActivity;

        #region Getter

        public DaySchedule GetDayScheduleWithDay(DayOfTheWeek day)
        {
            switch (day)
            {
                case DayOfTheWeek.Monday:
                    return monday;
                case DayOfTheWeek.Tuesday:
                    return tuesday;
                case DayOfTheWeek.Wednesday:
                    return wednesday;
                case DayOfTheWeek.Thursday:
                    return thursday;
                case DayOfTheWeek.Friday:
                    return friday;
                case DayOfTheWeek.Saturday:
                    return saturday;
                case DayOfTheWeek.Sunday:
                    return sunday;
                default:
                    return monday;
            }
        }

        public Activity GetActivityAtDayAndTime(ScheduleChanges scheduleChanges, DayOfTheWeek day, TimeOfTheDay time)
        {
            List<SerializableScheduledActivity> scheduledActivitiesAtHourAndDay = scheduleChanges.GetScheduledActivitiesAtHourAndDay(day, time);


            if (scheduledActivitiesAtHourAndDay.Count > 1)
            {

                currentDay = day;
                currentTime = time;
                Debug.Log("More than one ScheduledActivity are planned for this day (" + day + ") and time (" + time + ").");
                currentActivity = scheduledActivitiesAtHourAndDay[(int)Random.Range(0, scheduledActivitiesAtHourAndDay.Count)].scheduledActivity;
            }
            else if (scheduledActivitiesAtHourAndDay.Count == 1)
            {

                currentDay = day;
                currentTime = time;
                currentActivity = scheduledActivitiesAtHourAndDay[0].scheduledActivity;

            }
            else if ((day != currentDay || time != currentTime) || (IsCurrentActivityScheduled() && scheduledActivitiesAtHourAndDay.Count == 0))
            {
                currentDay = day;
                currentTime = time;
                List<SerializableActivity> activitiesAtHour = GetDayScheduleWithDay(day).GetActivitiesAtHour(time);
                currentActivity = activitiesAtHour[(int)Random.Range(0, activitiesAtHour.Count)].activity;
            }
            return currentActivity;
        }

        public bool IsCurrentActivityScheduled()
        {

            if (currentActivity != null)
                if (typeof(ScheduledActivity) == currentActivity.GetType())
                    return true;
            return false;

        }

        #endregion
    }

    //Class that contain every changes made to that schedule. Will be used for the save system
    [System.Serializable]
    public class ScheduleChanges
    {
        public List<SerializableScheduledActivity> serializableScheduledActivities;

        public ScheduleChanges()
        {
            serializableScheduledActivities = new List<SerializableScheduledActivity>();
        }

        #region ActivityArrayManipulation

        /// <summary>
        /// Add a new Scheduled activity if it doesn't exist
        /// </summary>
        /// <param name="s_Activity"></param>
        public void AddNewScheduledActivityOrRemoveIt(ScheduledActivity s_Activity)
        {
            bool exist = false;

            foreach (SerializableScheduledActivity activity in serializableScheduledActivities)
            {
                if (activity.scheduledActivity == s_Activity)
                {
                    exist = true;
                }
            }
            if (!exist)
                serializableScheduledActivities.Add(new SerializableScheduledActivity(s_Activity));
            else
                RemoveScheduledActivity(s_Activity);
        }


        public void RemplaceScheduledActivityWithNext(Activity s_Activity)
        {
            //serializableScheduledActivities.Add(new SerializableScheduledActivity(s_Activity));

            SerializableScheduledActivity current = new SerializableScheduledActivity((ScheduledActivity)s_Activity);

            for (int i = 0; i < serializableScheduledActivities.Count; i++)
            {
                if (s_Activity == serializableScheduledActivities[i].scheduledActivity)
                {
                    serializableScheduledActivities[i] = new SerializableScheduledActivity(current.scheduledActivity.nextActivity);
                }
            }
        }



        public void RemoveScheduledActivity(Activity activity)
        {
            for (int i = 0; i < serializableScheduledActivities.Count; i++)
            {
                if (activity == serializableScheduledActivities[i].scheduledActivity)
                {
                    serializableScheduledActivities.RemoveAt(i);
                    break;
                }
            }
        }


        #endregion


        #region Crawler

        public List<SerializableScheduledActivity> GetScheduledActivitiesAtHourAndDay(DayOfTheWeek day, TimeOfTheDay daytime)
        {
            List<SerializableScheduledActivity> result = new List<SerializableScheduledActivity>();

            foreach (SerializableScheduledActivity activity in serializableScheduledActivities)
            {
                if (activity.scheduledActivity.IsDayActive(day) && activity.scheduledActivity.IsTimeActive(daytime))
                {
                    result.Add(activity);
                }
            }
            return result;
        }

        public SerializableScheduledActivity GetScheduledActivityAtHourAndDay(DayOfTheWeek day, TimeOfTheDay daytime)
        {
            foreach (SerializableScheduledActivity activity in serializableScheduledActivities)
            {
                if (activity.scheduledActivity.IsDayActive(day) && activity.scheduledActivity.IsTimeActive(daytime))
                {
                    return activity;
                }
            }
            return null;
        }


        #endregion



    }





}