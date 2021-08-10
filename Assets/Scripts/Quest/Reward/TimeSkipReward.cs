using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    [CreateAssetMenu(fileName = "new TimeSkipReward", menuName = "LewdOwl/QuestSystem/Reward/TimeSkipReward")]
    public class TimeSkipReward : AbstractReward
    {
        public bool skipTimeOfTheDay;
        public TimeOfTheDay newTimeOfTheDay;
        public bool skipDayOfTheWeek;
        public DayOfTheWeek newDayOfTheWeek;

        public override void RunReward()
        {
            if (skipTimeOfTheDay)
            {
                GameManager.instance.managers.timeManager.GoToTime((int)newTimeOfTheDay);
            }
            if (skipDayOfTheWeek)
            {
                GameManager.instance.managers.timeManager.GoToDay((int)newDayOfTheWeek);
            }
            given = true;
        }
    }
}