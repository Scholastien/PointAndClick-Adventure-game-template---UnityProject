using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem {
    public abstract class AbstractReward : ScriptableObject, IQuestReward {

        public bool given = false;

        public abstract void RunReward();
    }
}