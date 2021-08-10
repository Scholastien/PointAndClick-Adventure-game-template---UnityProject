using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QuestSystem;
using System.Linq;
using TMPro;

public class TaskBehabiour : MonoBehaviour
{
    [Header("Given Data")]
    public Quest quest;

    [Header("Processed Data")]
    public UI_QuestDisplay questDisplay;

    [Header("GameObjectReference")]
    //public Text label;
    public TextMeshProUGUI labelTMP;
    public TextMeshProUGUI Text_StatusValue;

    [Header("Asset Reference")]
    public GameObject objectivePrefab;

    

    // Use this for initialization
    void Start()
    {
        StartCoroutine(CreateNewQuestDiplay());

        //label.text = quest.textDescription.descriptionSummary;

        //if (quest.state == QuestState.Awaiting)
        //    CheckForNestedObjectives();



        ////awaitingValue.SetActive(CheckForHiddenObjectives() && quest.state != QuestState.Completed);
        ////completedValue.SetActive(quest.state == QuestState.Completed);

        //if((CheckForHiddenObjectives() && quest.state != QuestState.Completed))
        //{
        //    Text_StatusValue.text = "AWAITING";
        //}
        //else if(quest.state == QuestState.Completed)
        //{
        //    Text_StatusValue.text = "COMPLETED";
        //}

    }

    IEnumerator CreateNewQuestDiplay()
    {
        yield return new WaitForEndOfFrame();
        questDisplay = new UI_QuestDisplay(quest);

        DisplayQuest();
        
    }

    private void DisplayQuest()
    {
        labelTMP.text = questDisplay.label;
        Text_StatusValue.text = questDisplay.statusText;

        foreach(UI_NestedObjectiveDisplay objective in questDisplay.nestedObjectives)
        {
            // Instantiate a new line
            GameObject go = Instantiate(objectivePrefab, transform);

            //Get objective value in GO
            ObjectiveBehaviour questLineInfo = go.GetComponent<ObjectiveBehaviour>();
            questLineInfo.objective = objective;
        }
    }
    

    /// <summary>
    /// 
    /// </summary>
    private void CheckForNestedObjectives()
    {

        List<TriggerCheckObjective> nestedObjectives = new List<TriggerCheckObjective>();

        // Add trigger objectives to the nestedobjective list in order to display them
        foreach (AbstractObjective objective in quest.objectives)
        {
            if (objective.GetType() == typeof(TriggerCheckObjective))
            {
                nestedObjectives.Add((TriggerCheckObjective)objective);
            }
        }


        nestedObjectives.Reverse();

        foreach (TriggerCheckObjective objective in nestedObjectives)
        {
            Debug.Log("Objective desc : " + objective.description);

            if (objective.questAssociated != null && !objective.hideInQuestMenu)
            {
                Debug.Log("Nested Quest : " + objective.description);
                if (!objective.hideInQuestMenu)
                {
                    if (objective.questAssociated.state != QuestState.Unassigned)
                    {
                        if (!GameManager.instance.managers.questManager.objectivesBuffer.Exists(element => element == objective))
                        {
                            GameManager.instance.managers.questManager.objectivesBuffer.Add(objective);
                            objective.animate = true;
                        }

                        SpawnNestedObjective(objective, objective.animate);
                    }
                }
            }

        }

    }

    private void SpawnNestedObjective(TriggerCheckObjective objective, bool animate)
    {
        // Instantiate a new line
        GameObject go = Instantiate(objectivePrefab, transform);

        //Get objective value in GO
        QuestLineInfoBehaviour questLineInfo = go.GetComponent<QuestLineInfoBehaviour>();
        

        questLineInfo.obj_label.text = "\t- " + objective.title;
        questLineInfo.obj_awaitingValue.SetActive(!objective.trigger.triggered);
        questLineInfo.obj_completedValue.SetActive(objective.trigger.triggered);

        go.SetActive(false);

        if (animate)
        {
            Debug.Log(objective.description + " \t <color=red>need animation</color>");


            objective.animate = false;


            StartCoroutine(AnimateObjectiveLine(questLineInfo.GetComponent<RectTransform>()));

        }

        go.SetActive(true);
    }

    private IEnumerator AnimateObjectiveLine(RectTransform rt)
    {
        // init 
        Vector2 sizeBuffer = new Vector2(rt.sizeDelta.x, rt.sizeDelta.y);
        rt.sizeDelta = new Vector2(sizeBuffer.x, 0);
        rt.gameObject.SetActive(false);

        //Get objective value in GO
        QuestLineInfoBehaviour questLineInfo = rt.GetComponent<QuestLineInfoBehaviour>();

        // wait for end of dialogue
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil((() => !GameManager.instance.managers.dialogueManager.isRunning));

        // display the game object
        rt.gameObject.SetActive(true);

        // Animate its height
        for (int i = 0; i < sizeBuffer.y; i += 2)
        {
            yield return null;
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, i);

            //  i  0 => 45
            //  x  0 => 1

            questLineInfo.obj_label.transform.localScale = new Vector3(1, i / sizeBuffer.y, 1);
            questLineInfo.obj_awaitingValue.transform.localScale = new Vector3(1, i / sizeBuffer.y, 1);
            questLineInfo.obj_completedValue.transform.localScale = new Vector3(1, i / sizeBuffer.y, 1);
        }

        // polish height value (get rid off float numbers)
        rt.sizeDelta = new Vector2(sizeBuffer.x, sizeBuffer.y);
        questLineInfo.obj_label.transform.localScale = new Vector3(1, 1, 1);
        questLineInfo.obj_awaitingValue.transform.localScale = new Vector3(1, 1, 1);
        questLineInfo.obj_completedValue.transform.localScale = new Vector3(1, 1, 1);

    }
}
