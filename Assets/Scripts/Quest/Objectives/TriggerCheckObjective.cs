using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace QuestSystem
{
    [CreateAssetMenu(fileName = "new TriggerCheckObjective", menuName = "LewdOwl/QuestSystem/Objective/TriggerCheckObjective")]
    [System.Serializable]
    public class TriggerCheckObjective : AbstractObjective
    {
        [Header("Trigger Properties")]
        public ChainTrigger trigger;

        public bool animate = false;

        [Header("Nested Quest Properties (Keep null if not used)")]
        public bool hideInQuestMenu;
        public Quest questAssociated;

        public override void CheckProgress()
        {
            if (trigger.triggered)
                isComplete = true;
            else
            {
                isComplete = false;
            }
        }


        public override void UpdateProgress()
        {
        }
    }
}
