using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleSystem
{
    // Scriptable object used to make a day schedule template for a character
    [CreateAssetMenu(fileName = "new DaySchedule", menuName = "LewdOwl/ScheduleSystem/DaySchedule")]
    public class DaySchedule : ScriptableObject
    {

        #region FullHour
        //public List<SerializableActivity> am7,
        //                                    am8,
        //                                    am9,
        //                                    am10,
        //                                    am11,
        //                                    pm12,
        //                                    pm1,
        //                                    pm2,
        //                                    pm3,
        //                                    pm4,
        //                                    pm5,
        //                                    pm6,
        //                                    pm7,
        //                                    pm8,
        //                                    pm9,
        //                                    pm10,
        //                                    pm11,
        //                                    am12,
        //                                    am1,
        //                                    am2;
        //public List<SerializableActivity> GetActivitiesAtHour(TimeOfTheDay daytime)
        //{
        //    switch (daytime)
        //    {
        //        case TimeOfTheDay.SevenAM:
        //            return am7;
        //        case TimeOfTheDay.EightAM:
        //            return am8;
        //        case TimeOfTheDay.NineAM:
        //            return am9;
        //        case TimeOfTheDay.TenAM:
        //            return am10;
        //        case TimeOfTheDay.ElevenAM:
        //            return am11;
        //        case TimeOfTheDay.TwelvePM:
        //            return pm12;
        //        case TimeOfTheDay.OnePM:
        //            return pm1;
        //        case TimeOfTheDay.TwoPM:
        //            return pm2;
        //        case TimeOfTheDay.ThreePM:
        //            return pm3;
        //        case TimeOfTheDay.FourPM:
        //            return pm4;
        //        case TimeOfTheDay.FivePM:
        //            return pm5;
        //        case TimeOfTheDay.SixPM:
        //            return pm6;
        //        case TimeOfTheDay.SevenPM:
        //            return pm7;
        //        case TimeOfTheDay.EightPM:
        //            return pm8;
        //        case TimeOfTheDay.NinePM:
        //            return pm9;
        //        case TimeOfTheDay.TenPM:
        //            return pm10;
        //        case TimeOfTheDay.ElevenPM:
        //            return pm11;
        //        case TimeOfTheDay.TwelveAM:
        //            return am12;
        //        case TimeOfTheDay.OneAM:
        //            return am1;
        //        case TimeOfTheDay.TwoAM:
        //            return am2;
        //        default:
        //            return am1;

        //    }
        //}
        #endregion

        #region AbstractHour
        public List<SerializableActivity>   DawnActivities,
                                            MorningActivities,
                                            NoonActivities,
                                            AfternoonActivities,
                                            DuskActivities,
                                            EveningActivities,
                                            NightActivities,
                                            LateNightActivities;


        public List<SerializableActivity> GetActivitiesAtHour(TimeOfTheDay daytime)
        {
            switch (daytime)
            {
                case TimeOfTheDay.Dawn:
                    return DawnActivities;
                case TimeOfTheDay.Morning:
                    return MorningActivities;
                case TimeOfTheDay.Noon:
                    return NoonActivities;
                case TimeOfTheDay.Afternoon:
                    return AfternoonActivities;
                case TimeOfTheDay.Dusk:
                    return DuskActivities;
                case TimeOfTheDay.Evening:
                    return EveningActivities;
                case TimeOfTheDay.Night:
                    return NightActivities;
                case TimeOfTheDay.LateNight:
                    return LateNightActivities;
                default:
                    return MorningActivities;

            }
        }

        #endregion


    }
}