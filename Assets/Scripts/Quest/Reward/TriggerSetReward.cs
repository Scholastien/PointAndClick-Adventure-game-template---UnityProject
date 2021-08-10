using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace QuestSystem
{
    [CreateAssetMenu(fileName = "new TriggerSet", menuName = "LewdOwl/QuestSystem/Reward/TriggerSet")]
    public class TriggerSetReward : AbstractReward
    {
        public ChainTrigger chainTrigger;

        public List<ChainTrigger> multipleTriggers;

        public bool value = true;

        public override void RunReward()
        {
            if (chainTrigger != null)
                chainTrigger.triggered = value;

            if (multipleTriggers.Count != 0)
            {
                foreach (ChainTrigger element in multipleTriggers)
                {
                    element.triggered = value;
                }
            }
            given = true;
            

            if((chainTrigger != null) && (multipleTriggers.Count != 0))
            {
                string errorMsg = String.Format("Triggers inside {0} are not set.", this.name);
                throw new NotImplementedException(errorMsg);
            }

        }

    }
}