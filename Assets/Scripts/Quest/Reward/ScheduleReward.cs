using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScheduleSystem;

namespace QuestSystem
{
    [CreateAssetMenu(fileName = "new ScheduleReward", menuName = "LewdOwl/QuestSystem/Reward/ScheduleReward")]
    public class ScheduleReward : AbstractReward
    {
        public ScheduledActivity scheduledActivity;

        public override void RunReward()
        {
            ScheduleChanges scheduleChanges = GameManager.instance.managers.scheduleManager.GetCharacterScheduleChanges(scheduledActivity.characterName);
            scheduleChanges.AddNewScheduledActivityOrRemoveIt(scheduledActivity);
            GameManager.instance.managers.scheduleManager.UpdateScheduleChange();
            given = true;
        }
    }
}