using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using QuestSystem;
using UnityEngine.Events;

public class QuestManager : MonoBehaviour
{



    public bool debug_QuestLog;

    private VariablesManager vm;

    [HideInInspector]
    public QuestCollection questCollection;

    public List<Quest> currentQuests;

    public List<Quest> displayedQuest;

    private bool init = true;

    public bool needSpawnTask = false;


    public List<TriggerCheckObjective> objectivesBuffer;






    #region Listeners

    private UnityAction NeedSpawnTaskListener;
    private UnityAction AnimateObjectiveListener;


    #endregion

    #region MonoBehaviour


    public void OnEnable()
    {
        EventManager.StartListening("SpawnTask", NeedSpawnTaskListener);
        EventManager.StartListening("AnimateObjective", AnimateObjectiveListener);
    }

    public void OnDisable()
    {
        EventManager.StopListening("SpawnTask", NeedSpawnTaskListener);
        EventManager.StopListening("AnimateObjective", AnimateObjectiveListener);
    }

    private void Awake()
    {
        NeedSpawnTaskListener = new UnityAction(NeedSpawnTask);
        AnimateObjectiveListener = new UnityAction(TagObjectiveForAnimation);









        objectivesBuffer = new List<TriggerCheckObjective>();
    }

    // Use this for initialization
    void Start()
    {

        vm = GameManager.instance.managers.variablesManager;
        questCollection = vm.questCollection;
        foreach (Quest quest in questCollection.allQuest)
            foreach (AbstractObjective objective in quest.objectives)
            {
                Debug.Log(quest.name);
                objective.isComplete = false;
            }
        //IQuestObjective qo = new CollectionObjective("Gather", 10, item, "Gather 10 meat!", false);
        //Debug.Log(qo.ToString());

        ResetReward();
        ResetObjectives();
    }


    // Update is called once per frame
    void LateUpdate()
    {

        if (init)
        {

            foreach (Quest quest in questCollection.allQuest)
                foreach (AbstractObjective objective in quest.objectives)
                    objective.isComplete = false;
            init = false;
        }

        if (GameManager.instance.gameState == GameState.MainMenu)
        {
            currentQuests = new List<Quest>();
            displayedQuest = new List<Quest>();
        }
        else
        {
            Init(false);
            CheckObjectives();


            GameManager.instance.LinkData(false);
        }



        CheckObjectives();

        // Delete duplicate, Sort quest: awating first, completed last.

        displayedQuest = DeleteDuplicate(displayedQuest);
        displayedQuest = SortListByState(displayedQuest);



    }


    #endregion

    #region Callback

    public void NeedSpawnTask()
    {
        GameManager.instance.managers.questManager.needSpawnTask = true;
    }


    public void TagObjectiveForAnimation()
    {

    }

    #endregion

    #region QuestManipulation

    public void Init(bool loaded)
    {

        displayedQuest = new List<Quest>();

        currentQuests = questCollection.InitAllQuest(GameManager.instance.managers.variablesManager.gameSavedData.activeQuest, loaded);

        foreach (Quest quest in currentQuests)
        {
            if (!quest.hiddenInTaskTab)
                displayedQuest.Add(quest);
        }

        foreach (Quest quest in questCollection.allQuest)
        {
            if (quest.state == QuestState.Completed && !quest.hiddenInTaskTab)
            {
                displayedQuest.Add(quest);
            }
        }

    }

    public void CheckObjectives()
    {

        EventManager.Instance.QuestUpdated(isQuestCompleted());



        QuestManager qm = GameManager.instance.managers.questManager;
        foreach (Quest quest in currentQuests)
        {
            if (quest.IsComplete())
            {
                StartCoroutine(WaitOneFrame());
                StartCoroutine(WaitOneFrame());
                quest.OnCompletion();
                foreach (int chainedQuestID in quest.identifier.chainQuestID)
                {
                    if (qm.debug_QuestLog)
                        Debug.Log("<color=#cc66ff>Completion :</color> " + this.name + " : \n" + "<color=red>is awaiting </color>" + questCollection.FindWithID(chainedQuestID).textDescription.title);
                    questCollection.FindWithID(chainedQuestID).state = QuestState.Awaiting;
                }
                if (qm.debug_QuestLog)
                    Debug.Log("<color=green> Current state for " + quest.name + " : </color>" + quest.state);
                SendCurrentQuestToGM();

            }
        }

        // Quest Check Completed, give the hand to the scene manager to load the navigation scene

    }

    public bool isQuestCompleted()
    {
        foreach (Quest q in currentQuests)
            if (q.IsComplete())
                return true;
        return false;
    }


    #region RewardCheck

    public void CheckRewardState(List<AbstractReward> rewardsToCheck)
    {
        StartCoroutine(WaitUntilAllRewardGiven(rewardsToCheck));
    }

    public IEnumerator WaitUntilAllRewardGiven(List<AbstractReward> rewardsToCheck)
    {
        yield return new WaitUntil(() => isRewardGiven(rewardsToCheck));
        EventManager.TriggerEvent("RewardGiven");
    }

    public bool isRewardGiven(List<AbstractReward> rewardsToCheck)
    {
        foreach (AbstractReward r in rewardsToCheck)
            if (!r.given)
                return false;
        return true;
    }


    // function that take a destination as paramater and return bool sayin if we need to wait that the dialogue reward is given (if it exist)
    // check if a location objective is contained in the current quest
    // Take the destination room and see if that location objective is met
    // If it's met, check if the rewards contains a dialogueReward.
    // Return true in that case (contain reward, and objective location met)
    // else return false
    public bool needTowaitForDialogueReward(Room destination)
    {
        foreach (Quest quest in currentQuests)
        {
            if (quest.isObjectivesContainingLocation())
            {
                LocationObjective objective = quest.getLocationObjective();

                if((objective.targetRoom == destination) && quest.isRewardContainingDialogue())
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void RunDialogueRewardIfNeeded(Room destination)
    {
        if (needTowaitForDialogueReward(destination))
        {
            foreach (Quest quest in currentQuests)
            {
                if (quest.isObjectivesContainingLocation())
                {
                    LocationObjective objective = quest.getLocationObjective();

                    if ((objective.targetRoom == destination) && quest.isRewardContainingDialogue())
                    {
                        quest.getDialogueReward().RunReward();
                    }
                }
            }
        }
    }


    #endregion

    #endregion

    #region OrderingAndDisplay

    public List<Quest> DeleteDuplicate(List<Quest> quests)
    {
        return quests.Distinct<Quest>().ToList();
    }

    public List<Quest> SortListByState(List<Quest> quests)
    {
        List<Quest> orderedList = new List<Quest>();
        // First Awating
        foreach (Quest quest in quests)
        {
            if (quest.state == QuestState.Awaiting)
                orderedList.Add(quest);
        }

        int awaitingCount = orderedList.Count;

        // Last Completed
        foreach (Quest quest in quests)
        {
            if (quest.state == QuestState.Completed)
            {
                if (orderedList.Count == awaitingCount)
                    orderedList.Add(quest);
                else
                    orderedList.Insert(awaitingCount, quest);
            }
        }
        return orderedList;
    }

    #endregion

    #region GameManagerCommunication

    public void SendCurrentQuestToGM()
    {

        vm.gameSavedData.activeQuest = new List<int>();
        foreach (Quest quest in questCollection.allQuest)
        {
            if (quest.state == QuestState.Awaiting)
            {
                vm.gameSavedData.activeQuest.Add(quest.identifier.id);
            }
        }
    }

    #endregion

    #region Resetter

    // Load all reward and set the Given value to false
    public void ResetReward()
    {
        string path = "Assets/Resources/ScriptableObjects/QuestSystem/Reward";

        List<AbstractReward> rewards = new List<AbstractReward>();



#if UNITY_EDITOR
        Object[] objects = AssetDatabase.LoadAllAssetsAtPath(path);
#else
        Object[] objects = Resources.LoadAll<AbstractReward>(path);
#endif



        foreach (Object obj in objects)
        {
            if (obj.GetType() == typeof(AbstractReward))
            {
                rewards.Add((AbstractReward)obj);
            }
        }

        foreach (AbstractReward reward in rewards)
        {
            reward.given = false;
        }
    }

    public void ResetObjectives()
    {
        string path = "Assets/Resources/ScriptableObjects/QuestSystem/Objectives";

        List<AbstractObjective> objectives = new List<AbstractObjective>();

#if UNITY_EDITOR
        Object[] objects = AssetDatabase.LoadAllAssetsAtPath(path);

#else
        Object[] objects = Resources.LoadAll<AbstractObjective>(path);
#endif




        foreach (Object obj in objects)
        {
            if (obj.GetType() == typeof(AbstractObjective))
            {
                objectives.Add((AbstractObjective)obj);
            }
        }

        foreach (AbstractObjective objective in objectives)
        {
            objective.isComplete = false;
        }
    }

    #endregion

    #region Coroutine

    public IEnumerator WaitOneFrame()
    {
        yield return new WaitForEndOfFrame();
    }

    #endregion


}
