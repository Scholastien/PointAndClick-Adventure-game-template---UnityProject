using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    [CreateAssetMenu(fileName = "new TimeObjective", menuName = "LewdOwl/QuestSystem/Objective/TimeObjective")]
    [System.Serializable]
    public class TimeObjective : AbstractObjective
    {
        [Header("Properties")]
        public TimeOfTheDay targetTime;

        private TimeOfTheDay currentTime;

        public override void CheckProgress()
        {
            if (currentTime == targetTime)
                isComplete = true;
            else
                isComplete = false;

        }


        public override void UpdateProgress()
        {
            currentTime = GameManager.instance.managers.timeManager.currentTime;
        }
    }
}