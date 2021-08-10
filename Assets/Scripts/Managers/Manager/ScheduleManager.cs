using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScheduleSystem;
using System.Linq;
using System;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class ScheduleManager : MonoBehaviour
{

    public static ScheduleManager instance;

    [HideInInspector]
    public List<NpcScheduleBehaviour> npcsSchedulebehaviour;

    [HideInInspector]
    public ScheduleChangesCollection scheduleChangesCollection;

    public List<NpcSchedule> npcSchedules;

    public List<NpcScheduleChanges> npcScheduleChanges;

    [HideInInspector]
    public List<NpcSerializedSchedule> npcSerializedInfos;

    public TimeManager tm;

    public Sprite transparentInterractible;

    private VariablesManager vm;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }

    // Use this for initialization
    void Start()
    {
        tm = gameObject.GetComponent<TimeManager>();
        vm = GameManager.instance.managers.variablesManager;
        scheduleChangesCollection = vm.scheduleChangesCollection;
        npcsSchedulebehaviour = new List<NpcScheduleBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        Init();

    }

    public void Init()
    {
        //remove duplicate
        //list.Distinct().ToList();


        npcSerializedInfos = vm.gameSavedData.npcSerializedInfos;
        npcScheduleChanges = TransformIntToNpcSchedule();

        foreach (NpcSchedule npcSchedule in npcSchedules)
        {
            npcSchedule.name = npcSchedule.character.ToString();
        }

    }

    public void UpdateScheduleChange()
    {
        vm.gameSavedData.npcSerializedInfos = TransformNpcScheduleToInt();
        NpcScheduleBehaviour[] scheduleBehaviours = FindObjectsOfType<NpcScheduleBehaviour>();
        foreach (NpcScheduleBehaviour scheduleBehaviour in scheduleBehaviours)
        {
            scheduleBehaviour.needRefresh = true;
        }
    }

    // Retrieve a list of int in serialized infos and convert them with the schedulechangecollection into a list of scheduledchange
    public List<NpcScheduleChanges> TransformIntToNpcSchedule()
    {
        List<NpcScheduleChanges> result = new List<NpcScheduleChanges>();

        foreach (NpcSerializedSchedule npcSerialized in npcSerializedInfos)
        {
            if (npcSerialized.scheduleChangesSerialized != null)
            {
                result.Add(new NpcScheduleChanges(npcSerialized.name, CreateScheduleChangeListFromIntList(npcSerialized.scheduleChangesSerialized)));
            }
        }


        return result;
    }





    public List<NpcSerializedSchedule> TransformNpcScheduleToInt()
    {
        List<NpcSerializedSchedule> result = new List<NpcSerializedSchedule>();

        foreach (CharacterFunction type in Enum.GetValues(typeof(CharacterFunction)))
        {
            //Debug.Log(type.ToString());
            result.Add(new NpcSerializedSchedule(type.ToString()));
            result[(int)type].scheduleChangesSerialized = CreateIntListFromScheduleChangeList(npcScheduleChanges[(int)type]);
        }

        foreach (NpcScheduleChanges NpcSchedule in npcScheduleChanges)
        {


        }

        return result;
    }

    public ScheduleChanges CreateScheduleChangeListFromIntList(List<int> serializedList)
    {
        List<ScheduledActivity> deserializeList = scheduleChangesCollection.DeserializeList(serializedList);
        ScheduleChanges result = new ScheduleChanges();
        foreach (ScheduledActivity s_Activity in deserializeList)
        {
            result.serializableScheduledActivities.Add(new SerializableScheduledActivity(s_Activity));
        }

        return result;
    }

    public List<int> CreateIntListFromScheduleChangeList(NpcScheduleChanges serializedList)
    {
        return scheduleChangesCollection.SerializableList(serializedList.scheduleChanges.serializableScheduledActivities);
    }


    public ScheduleChanges GetCharacterScheduleChanges(CharacterFunction character)
    {
        foreach (NpcScheduleChanges scheduleChanges in npcScheduleChanges)
        {
            if (scheduleChanges.name == character.ToString())
            {
                return scheduleChanges.scheduleChanges;
            }
        }
        return null;
    }


    public Schedule GetCharacterSchedule(CharacterFunction character)
    {
        foreach (NpcSchedule npcSchedule in npcSchedules)
        {
            if (npcSchedule.character == character)
            {
                return npcSchedule.schedule;
            }
        }
        return null;
    }


    public void SetNextActivity(Activity currentActivity)
    {
        ScheduleChanges scheduleChanges = GetCharacterScheduleChanges(currentActivity.characterName);


        if (currentActivity.nextActivity != null)
        {
            Debug.Log("<color=purple>" + currentActivity.nextActivity.name + " </color>");
            if (currentActivity.overwriteActivityWithNewOne)
            {
                scheduleChanges.RemplaceScheduledActivityWithNext(currentActivity);
            }
            else
            {
                scheduleChanges.AddNewScheduledActivityOrRemoveIt(currentActivity.nextActivity);

            }
        }
        else
        {
            if (currentActivity.playOnlyOnce)
            {
                scheduleChanges.RemoveScheduledActivity(currentActivity);
            }
        }

        UpdateScheduleChange();
    }






    public void SendCurrentScheduleStateToGM()
    {
        vm.gameSavedData.npcSerializedInfos = new List<NpcSerializedSchedule>();

    }





    #region UI

    public NpcScheduleBehaviour GetCurrentScheduleBehaviour(CharacterFunction function)
    {
        NpcScheduleBehaviour scheduleBehaviour = new NpcScheduleBehaviour();
        foreach(NpcScheduleBehaviour npcSchedule in npcsSchedulebehaviour)
        {
            if(npcSchedule.character == function)
            {
                scheduleBehaviour = npcSchedule;
            }
        }
        return scheduleBehaviour;
    }

    #endregion
}
