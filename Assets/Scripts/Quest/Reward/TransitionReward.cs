using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    [CreateAssetMenu(fileName = "new TransitionReward", menuName = "LewdOwl/QuestSystem/Reward/TransitionReward")]
    public class TransitionReward : AbstractReward
    {
        public TransitionType transitionType;

        public override void RunReward()
        {
            TransitionManager.instance.SetNextTransition(transitionType);
            given = true;

        }
    }
}