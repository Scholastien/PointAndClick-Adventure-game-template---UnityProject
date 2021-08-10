using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{

    [System.Serializable]
    public class UI_QuestDisplay
    {
        public Quest quest;

        public string label;
        public string statusText;

        public List<UI_NestedObjectiveDisplay> nestedObjectives;

        public UI_QuestDisplay(Quest _quest)
        {
            quest = _quest;

            label = LanguageManager.instance.GetTranslation( GetQuestLabel());

            statusText = LanguageManager.instance.GetTranslation(GetStatusText());

            nestedObjectives = GetNestedObjectives();
        }

        #region FieldWorker

        public string GetQuestLabel()
        {
            return quest.textDescription.title;
        }

        public string GetStatusText()
        {
            if (quest.state == QuestState.Completed)
            {
                return "COMPLETED";
            }
            //if((CheckForHiddenObjectives() && quest.state != QuestState.Completed))
            //{
            //    Text_StatusValue.text = "AWAITING";
            //}
            else
            {
                return "AWAITING";
            }
        }

        public List<UI_NestedObjectiveDisplay> GetNestedObjectives()
        {

            List<UI_NestedObjectiveDisplay> result = new List<UI_NestedObjectiveDisplay>();

            List<TriggerCheckObjective> nestedObjectives = new List<TriggerCheckObjective>();

            // Add trigger objectives to the nestedobjective list in order to display them
            foreach (AbstractObjective objective in quest.objectives)
            {
                if (objective.GetType() == typeof(TriggerCheckObjective))
                {
                    nestedObjectives.Add((TriggerCheckObjective)objective);
                }
            }
            //nestedObjectives.Reverse();

            foreach (TriggerCheckObjective objective in nestedObjectives)
            {
                if (objective.questAssociated != null && !objective.hideInQuestMenu)
                {
                    if (!objective.hideInQuestMenu)
                    {
                        if (objective.questAssociated.state != QuestState.Unassigned)
                        {
                            if (!GameManager.instance.managers.questManager.objectivesBuffer.Exists(element => element == objective))
                            {
                                GameManager.instance.managers.questManager.objectivesBuffer.Add(objective);
                            }
                            result.Add(new UI_NestedObjectiveDisplay(objective));
                        }
                    }
                }

            }

            return result;
        }

        public bool CheckForHiddenObjectives()
        {

            List<TriggerCheckObjective> objectives = new List<TriggerCheckObjective>();
            foreach (AbstractObjective objective in quest.objectives)
            {
                if (objective.GetType() == typeof(TriggerCheckObjective))
                    objectives.Add((TriggerCheckObjective)objective);
            }

            foreach (TriggerCheckObjective objective in objectives)
            {
                if (!objective.hideInQuestMenu)
                {
                    return false;
                }
            }
            return true;
        }

        #endregion



    }

    [System.Serializable]
    public class UI_NestedObjectiveDisplay
    {
        public string label;
        public bool status;

        public UI_NestedObjectiveDisplay (TriggerCheckObjective objective)
        {
            label = LanguageManager.instance.GetTranslation(objective.description);
            status = objective.trigger.triggered;
        }
    }
}