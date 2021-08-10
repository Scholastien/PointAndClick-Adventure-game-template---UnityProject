using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//using a namespace to ogranize all of our quest information
namespace QuestSystem
{

    public enum QuestState
    {
        Unassigned,
        Awaiting,
        Completed
    }

    [CreateAssetMenu(fileName = "_order - {id} new Quest", menuName = "LewdOwl/QuestSystem/Quest")]
    [System.Serializable]
    //basic quest class object
    public class Quest : ScriptableObject
    {
        public int test;


        [Header("State")]
        public QuestState state = QuestState.Unassigned;
        //Name
        //DescriptionSummary
        //Quest Hint
        //Quest Dialog
        //sourceID
        //questID
        [Header("Identifier")]
        public QuestIdentifier identifier;
        //chain quest and the next quest is blank
        //chainquestiD

        [Header("Display Settings")]
        public bool hiddenInTaskTab;
        public QuestText textDescription;

        [Header("Properties")]
        //objectives
        public List<AbstractObjective> objectives;
        //Collection Objective
        //10 feathers
        //killing 4 enemies
        //Location Objective
        //go from point A to B
        //Timed you have 10 mins to get to point B from A

        //bonus objectives
        //rewards

        public List<AbstractReward> rewards;

        public Quest()
        {
            //QuestManager qm = GameManager.instance.managers.questManager;
        }


        //events
        //on completetion
        //on failed
        //on update
        /// <summary>
        /// Run rewards on completion giving prority to Dialogue rewards
        /// </summary>
        public void OnCompletion()
        {
            QuestManager qm = GameManager.instance.managers.questManager;
            state = QuestState.Completed;

            //Ordering Rewards
            rewards = OrderingRewards();

            qm.CheckRewardState(rewards);


            // Make the reward run
            if (rewards.Count > 0)
            {
                if (rewards.Count == 1)
                {
                    rewards[0].RunReward();
                }
                else
                {
                    bool containDialog = GameManager.instance.managers.dialogueManager.isRunning;
                    List<DialogueReward> dialogueRewards = new List<DialogueReward>(); // priority to dialogue
                    List<AbstractReward> rewardsToRun = new List<AbstractReward>();
                    foreach (AbstractReward reward in rewards)
                    {
                        if (reward != null)
                        {
                            if (reward.GetType() == typeof(DialogueReward))
                            {
                                containDialog = true;
                                dialogueRewards.Add((DialogueReward)reward);
                            }
                            else
                            {
                                rewardsToRun.Add(reward);
                            }
                        }
                    }


                    

                    if (containDialog)
                    {
                        //qm.StartCoroutine(WaitEndOfDialogue(dialogueRewards));
                        foreach (DialogueReward dialogueReward in dialogueRewards)
                        {
                            qm.StartCoroutine(WaitEndOfDialogue(dialogueReward));
                        }
                        foreach (AbstractReward rewardToRun in rewardsToRun)
                        {
                            if (rewardToRun.GetType() != typeof(DialogueReward))
                                qm.StartCoroutine(WaitEndOfDialogue(rewardToRun));
                        }
                    }
                    else
                    {

                        foreach (AbstractReward reward in rewards)
                        {
                            if (reward != null)
                            {
                                reward.RunReward();
                            }
                            else
                            {
                                if (qm.debug_QuestLog)
                                    Debug.Log("Quest " + textDescription.title + "Have no reward");
                            }
                        }
                    }
                }
            }
            if (qm.debug_QuestLog)
                Debug.Log("<color=#cc66ff>Completion :</color> " + this.name + "\n" + "<color=yellow>completed </color>" + this.textDescription.title);

            if(identifier.chainQuestID.Count != 0)
                if(identifier.chainQuestID[0] != identifier.id)
                    EventManager.TriggerEvent("SpawnTask");
        }

        public bool IsComplete()
        {
            QuestManager qm = GameManager.instance.managers.questManager;
            List<bool> resultList = new List<bool>();
            foreach (AbstractObjective objective in objectives)
            {
                objective.UpdateProgress();
                objective.CheckProgress();
                if (!objective.IsComplete && objective.IsBonus == false)
                {
                    if (qm.debug_QuestLog)
                        Debug.Log("<color=#66ccff>Objective :</color> " + objective.title + "\n" + "<color=red>Uncompleted</color>");
                    resultList.Add(false);
                }
                else
                {
                    if (qm.debug_QuestLog)
                        Debug.Log("<color=#66ccff>Objective :</color> " + objective.title + "\n" + "<color=yellow>Passed</color>");
                    resultList.Add(true);
                }    //get reward!! fire on complete event!
            }
            foreach (bool result in resultList)
            {
                if (result == false)
                {
                    foreach (AbstractObjective objective in objectives)
                    {
                        objective.isComplete = false;
                    }
                    return false;
                }
            }
            return true;
        }

        public List<AbstractReward> OrderingRewards()
        {
            List<AbstractReward> orderedRewards = new List<AbstractReward>();

            foreach (AbstractReward dialogueReward in rewards)
            {
                if (dialogueReward.GetType() == typeof(DialogueReward))
                {
                    orderedRewards.Add(dialogueReward);
                }
            }

            foreach (AbstractReward reward in rewards)
            {
                if (reward.GetType() != typeof(DialogueReward))
                {
                    orderedRewards.Add(reward);
                }
            }
            return orderedRewards;
        }


        public IEnumerator WaitEndOfDialogue(AbstractReward reward)
        {
            EventManager.Instance.QuestUpdated(true, true);

            if (!GameManager.instance.managers.dialogueManager.isRunning && reward.GetType() == typeof(DialogueReward))
                reward.RunReward();
            else
            {

                yield return new WaitForSeconds(0.3f);
                yield return new WaitForEndOfFrame();

                //if (reward.GetType() != typeof(DialogueReward))
                //    yield return new WaitForSeconds(0.1f);

                yield return new WaitUntil(() => !GameManager.instance.managers.dialogueManager.isRunning);



                EventManager.Instance.NeedFadeIn();

                reward.RunReward();
            }



            EventManager.TriggerEvent("SpawnTask");
        }



        #region Getter

        public bool isObjectivesContainingLocation()
        {
            foreach(AbstractObjective objective in objectives)
            {
                if (objective.GetType() == typeof(LocationObjective))
                    return true;
            }
            return false;
        }

        public LocationObjective getLocationObjective()
        {
            foreach (AbstractObjective objective in objectives)
            {
                if (objective.GetType() == typeof(LocationObjective))
                    return objective as LocationObjective;
            }
            return null;
        }

        public DialogueReward getDialogueReward()
        {
            foreach (AbstractReward reward in rewards)
            {
                if (reward.GetType() == typeof(DialogueReward))
                    return reward as DialogueReward;
            }
            return null;
        }


        public bool isRewardContainingDialogue()
        {
            foreach (AbstractReward reward in rewards)
            {
                if (reward.GetType() == typeof(DialogueReward))
                    return true;
            }
            return false;
        }

        #endregion

    }
}

