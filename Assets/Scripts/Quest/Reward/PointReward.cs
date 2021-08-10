using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem {
    [CreateAssetMenu(fileName = "new PointReward", menuName = "LewdOwl/QuestSystem/Reward/PointReward")]
    public class PointReward : AbstractReward {

        public ChainIntType variable;

        public int value;

        public override void RunReward() {

            DialogueChainPreferences.AddToChainInt(variable, value);
            given = true;
        }
    }
    
}