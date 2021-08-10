using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using QuestSystem;

public class TaskTabBehaviour : MonoBehaviour
{

    public bool displayOnlyAwaiting = false;

    public List<Quest> questToDisplay;

    [HideInInspector]
    public List<GameObject> TaskBuffer;

    [Tooltip("Parent")]
    public GameObject TaskScrollableList;


    public GameObject TaskPrefab;




    #region MonoBehaviour

    public void OnEnable()
    {
        //Debug.Log("Task enable");
        GameManager.instance.managers.questManager.needSpawnTask = true;
    }


    private void Update()
    {
        if (GameManager.instance.managers.questManager.needSpawnTask)
            SpawnTask();
    }



    #endregion



    public void SpawnTask()
    {

        //Debug.Log("Spawntask");

        questToDisplay = GameManager.instance.managers.questManager.displayedQuest;
        CleanTaskList();
        for (int i = 0; i < questToDisplay.Count; i++)
        {
            if (displayOnlyAwaiting)
            {
                if (questToDisplay[i].state == QuestState.Awaiting)
                {
                    GameObject Task = Instantiate(TaskPrefab, TaskScrollableList.transform);
                    TaskBuffer.Add(Task);
                    Task.GetComponent<TaskBehabiour>().quest = questToDisplay[i];
                }
            }
            else
            {
                if (questToDisplay[i].state != QuestState.Unassigned)
                {
                    GameObject Task = Instantiate(TaskPrefab, TaskScrollableList.transform);
                    TaskBuffer.Add(Task);
                    Task.GetComponent<TaskBehabiour>().quest = questToDisplay[i];
                }
            }

        }
        GameManager.instance.managers.questManager.needSpawnTask = false;
    }

    public void CleanTaskList()
    {
        TaskBuffer = new List<GameObject>();
        foreach (Transform child in TaskScrollableList.transform)
            Destroy(child.gameObject);
    }

}
