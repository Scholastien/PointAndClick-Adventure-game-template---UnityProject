using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    #region Save/Load
    /// <summary>
    /// Should be called everytime a value is changed and that we need a global update
    /// </summary>


    // Whenever we need to update the variable manager with the loaded values from Save/load
    public delegate void LoadedFromSave();
    public static event LoadedFromSave isLoadedFromSave;

    // Whenever the values are updated in the variable manager
    public delegate void NewValues();
    public static event NewValues onNewValues;

    // whenever we need a blink to transition from a room to another
    public delegate void FadeIn();
    public static event FadeIn onFadeIn;

    // whenever we need the quest system is ready to give other script the hand
    public delegate void QuestUpdate();
    public static event QuestUpdate onQuestUpdate;
    
    // whenever we need the quest system is ready to give other script the hand
    public delegate void WaitQuestCompletion();
    public static event WaitQuestCompletion onQuestCompletion;


    // whenever a dialogue ends
    public delegate void WaitEndOfDialogue();
    public static event WaitEndOfDialogue onEndOfDialogue;

    // whenever a dialogue start
    public delegate void Interacted();
    public static event Interacted onInteraction;




    // Whenever we need to close any window subscribed to this event
    public delegate void CloseAllWindow();
    public static event CloseAllWindow onClosingWindow;


    // Whenever we need a black screen for the dialogue
    public delegate void BlackScreen();
    public static event BlackScreen onBlackScreen;


    #endregion


    private Dictionary<string, UnityEvent> eventDictionary;

    public static EventManager Instance {
        get
        {
            instance = FindObjectOfType(typeof(EventManager)) as EventManager;

            if (!instance)
            {
                Debug.LogError("There needs to be one active EventManager script on a GameObject in your scene.");
            }
            else
            {
                instance.Init();
            }
            return instance;
        }
    }

    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, UnityEvent>();
        }
    }

    #region actionBasedOnListeners
    public static void StartListening(string eventName, UnityAction listener)
    {
        //Debug.Log("Start Listening <color=red>" + eventName + "</color>");
        UnityEvent thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            Instance.eventDictionary.Add(eventName, thisEvent);
        }

    }

    public static void StopListening(string eventName, UnityAction listener)
    {
        //Debug.Log("Stop Listening <color=red>" + eventName + "</color>");
        if (instance == null)
            return;
        UnityEvent thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName)
    {
        UnityEvent thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            Debug.Log("Trigger <color=red>" + eventName + "</color>");
            thisEvent.Invoke();
        }
    }
    #endregion

    #region MonoBehaviour
    public void LateUpdate()
    {
        if (!GameManager.instance.naming)
            EventSystem.current.SetSelectedGameObject(null);
    }
    #endregion

    #region DelegateEvent
    public void IsLoaded()
    {
        if(isLoadedFromSave != null)
        {
            isLoadedFromSave();
        }
    }

    public void UpdatedValues()
    {
        if(onNewValues != null)
        {
            onNewValues();
        }
    }

    public void NeedFadeIn()
    {
        if(onFadeIn != null)
        {
            onFadeIn();
        }
    }

    public void EndOfDialogue()
    {
        if (onEndOfDialogue != null)
        {
            onEndOfDialogue();
        }
    }

    public void ClosingAllWindow()
    {
        if(onClosingWindow != null)
        {
            onClosingWindow();
        }
    }

    public void NeedBlackScreen()
    {
        if(onBlackScreen != null)
        {
            onBlackScreen();
        }
    }

    public void Interract()
    {
        if(onInteraction != null)
        {
            //Debug.LogError("Sharing interract msg");
            onInteraction();
        }
    }




    public void QuestUpdated(bool waitAfterCompletion, bool dialogue_given = false)
    {
        if(onQuestUpdate != null && onQuestCompletion != null && dialogue_given == false)
        {
            if (!waitAfterCompletion)
                onQuestUpdate(); // you can update your state whenever you want
        }
        else if (onQuestUpdate != null && onQuestCompletion != null && dialogue_given == true) // you can update when the dialogue is given
        {
            onQuestUpdate();
        }
    }
    #endregion

}
