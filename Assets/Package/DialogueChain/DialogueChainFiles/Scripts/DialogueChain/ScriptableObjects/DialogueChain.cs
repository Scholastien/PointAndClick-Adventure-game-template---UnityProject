using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using InventorySystem;
using System;
using ScheduleSystem;
using SceneReferenceInEditor;
using System.Collections;

[CreateAssetMenu(fileName = "NewDialogueChain", menuName = "Dialogue Chains/Chain")]
public class DialogueChain : ScriptableObject
{
    #region Declarations
    [HideInInspector] public int nodeIDCount = 0;
    [HideInInspector] public bool paused = false;
    [HideInInspector] public bool hasEnded = false;

    private bool originalHaltMovement;

    public bool haltMovement = true;
    public bool defaultShowImages = DialogueChainPreferences.defaultShowSpeakerImage;
    public bool defaultShowNames = DialogueChainPreferences.defaultShowSpeakerNameBox;
    public Sprite defaultSprite;
    public string defaultSpeaker;
    public ContainerType defaultContainerType = DialogueChainPreferences.defaultContainerType;
    public float defaultTextDelay = 0.02f;

    private bool waitForConfirm = false;

    [HideInInspector] public DialogueChain beforeSubDialogue;
    /*[HideInInspector]*/
    public bool isSubDialogueChain;

    [HideInInspector] public List<string> speakers = new List<string>();

    [HideInInspector] public ChainEvent startEvent = null;
    [HideInInspector] public ChainEvent currentEvent;
    [HideInInspector] private ChainEvent nextEvent;

    [HideInInspector] public List<ChainEvent> chainEvents = new List<ChainEvent>();
    [HideInInspector] public ChainAdditions additions;
    #endregion

    public void Awake()
    {
        Array values = Enum.GetValues(typeof(CharacterFunction));
        string[] EnumToStringArray = Enum.GetNames(typeof(CharacterFunction));
        speakers = new List<string>();
        speakers.AddRange(EnumToStringArray);
        waitForConfirm = false;
    }

    public void SecureOutOfRangeCharacterFunction()
    {
        // List<ChainEvent> chainEvents

        foreach (ChainEvent cEvent in chainEvents)
        {
            Debug.Log("Current Speaker" + cEvent.speaker);
            bool exist = false;
            foreach (string speakerName in speakers)
            {
                if (cEvent.speaker == speakerName)
                    exist = true;
            }
            if (!exist)
            {
                cEvent.speaker = "MainCharacter";
            }
        }
    }

    private void OnEnable()
    {
        paused = false;
    }

    #region StartChain
    public void StartChain()
    {



        DialogueController.instance.isRunning = true;
        if (!isSubDialogueChain)
        {
            originalHaltMovement = DialogueChainPreferences.GetHaltMovement();
            DialogueChainPreferences.SetHaltMovement(haltMovement);
        }

        hasEnded = false;

        DialogueController.instance.currentDialogueChain = this;

        if (!paused)
        {
            RunEvent(startEvent);
        }
        else
        {
            paused = false;
            GetNextEvent();
        }
    }
    #endregion

    #region ProcessChainFunction

    // Return current id
    public int GetCurrentEventID()
    {
        for (int i = 0; i < chainEvents.Count; i++)
        {
            if (currentEvent == chainEvents[i])
                return i;
        }
        return 0;
    }

    #endregion

    #region RunEvent
    public void RunEvent(ChainEvent cEvent)
    {
        DialogueController.instance.isRunning = true;
        DialogueController.instance.CloseDialogue();
        if (hasEnded)
        {
            ChainEnded();
            return;
        }

        currentEvent = cEvent;
        DialogueController.instance.TransitionHandler(currentEvent);

        GameManager.instance.managers.dialogueManager.UpdateValues();

        if (cEvent.cEventType == ChainEventType.Dialogue)
        {
            DialogueEvent();
        }
        else if (cEvent.cEventType == ChainEventType.ItemManagement)
        {
            ItemManagement();
        }
        else if (cEvent.cEventType == ChainEventType.Pause)
        {
            Pause();
        }
        else if (cEvent.cEventType == ChainEventType.SetTrigger)
        {
            SetTrigger();
        }
        else if (cEvent.cEventType == ChainEventType.ScheduledActivity)
        {
            SetScheduledActivities();
        }
        else if (cEvent.cEventType == ChainEventType.UserInput)
        {
            currentEvent.nextEventIDs.Clear();
            UserInput();
        }
        else if (cEvent.cEventType == ChainEventType.SubDialogue)
        {
            SubDialogue();
        }
        else if (cEvent.cEventType == ChainEventType.Audio)
        {
            Audio();
        }
        else if (cEvent.cEventType == ChainEventType.IntAdjustment)
        {
            ModifyVariable();
        }
        else if (cEvent.cEventType == ChainEventType.VariableModifier)
        {
            ModifyVariable();
        }
        else if (cEvent.cEventType == ChainEventType.Message)
        {
            Message();
        }
        else if (cEvent.cEventType == ChainEventType.Start)
        {
            StartNode();
        }
        else if (cEvent.cEventType == ChainEventType.Check)
        {
            Check();
        }
        else if (cEvent.cEventType == ChainEventType.MinigameLauncher)
        {
            StartMinigame();
        }
        else if (cEvent.cEventType == ChainEventType.SetLocation)
        {
            SetLocation();
        }

    }
    #endregion

    #region EventCode
    private void StartNode()
    {
        GetNextEvent();
    }
    private void DialogueEvent()
    {
        waitForConfirm = true;
        DialogueController.instance.ShowDialogue(currentEvent);
        GetNextEvent();
    }
    private void ItemManagement()
    {
        if (DialogueChainPreferences.itemsAreScriptableObjects)
        {
            foreach (Item item in currentEvent.itemsGiven)
            {
                DialogueChainPreferences.AddToInventory(item);
            }
            foreach (Item item in currentEvent.itemsTaken)
            {
                DialogueChainPreferences.RemoveFromInventory(item);
            }
        }
        else
        {
            foreach (string itemString in currentEvent.itemsGivenString)
            {
                DialogueChainPreferences.AddToInventory(itemString);
            }
            foreach (string itemString in currentEvent.itemsTakenString)
            {
                DialogueChainPreferences.RemoveFromInventory(itemString);
            }
        }


        DialogueChainPreferences.AddToPlayerExperience(currentEvent.experienceGiven);

        GetNextEvent();
    }
    public void Pause()
    {
        paused = true;
        DialogueChainPreferences.SetHaltMovement(originalHaltMovement);
        DialogueController.instance.isRunning = false;

    }
    private void SetTrigger()
    {
        for (int i = 0; i < currentEvent.triggers.Count; i++)
        {
            currentEvent.triggers[i].triggered = currentEvent.triggerBools[i];
        }

        GetNextEvent();
    }
    private void SetScheduledActivities()
    {
        for (int i = 0; i < currentEvent.scheduledActivities.Count; i++)
        {
            //currentEvent.triggers[i].triggered = currentEvent.triggerBools[i];

            ScheduleChanges scheduleChanges = GameManager.instance.managers.scheduleManager.GetCharacterScheduleChanges(currentEvent.scheduledActivities[i].characterName);
            scheduleChanges.AddNewScheduledActivityOrRemoveIt(currentEvent.scheduledActivities[i]);
            GameManager.instance.managers.scheduleManager.UpdateScheduleChange();

        }

        GetNextEvent();
    }
    private void UserInput()
    {
        currentEvent.secondaryInputButtons.Clear();

        for (int i = 0; i < currentEvent.lateralConnections.Count; i++)
        {
            foreach (ChainEvent dEvent2 in chainEvents)
            {
                if (dEvent2.eventID == currentEvent.lateralConnections[i])
                {
                    if (EventPassesCheck(dEvent2))
                    {
                        foreach (DialogueEventInputButton inputButton in dEvent2.inputButtons)
                        {
                            currentEvent.secondaryInputButtons.Add(inputButton);
                        }
                    }
                    break;
                }
            }
        }

        DialogueController.instance.ShowDialogue(currentEvent);
    }
    private void SubDialogue()
    {
        paused = true;

        currentEvent.subDialogue.isSubDialogueChain = true;
        currentEvent.subDialogue.beforeSubDialogue = this;
        currentEvent.subDialogue.StartChain();
    }
    private void Audio()
    {
        if (currentEvent.overlay)
        {
            ChainAudioController.instance.AddTempSource(currentEvent.audio, currentEvent.fadeTime, currentEvent.audioVolume);
        }
        else
        {
            ChainAudioController.instance.CrossFade(currentEvent.audio, currentEvent.fadeTime, currentEvent.loop, currentEvent.playOriginalAfter, currentEvent.originalFadeTime, currentEvent.audioVolume);
        }
        GetNextEvent();
    }
    private void StatAdjust()
    {
        for (int i = 0; i < currentEvent.chainIntAdjustments.Count; i++)
        {
            DialogueChainPreferences.AddToChainInt(currentEvent.chainIntAdjustments[i].intAdjusted, currentEvent.chainIntAdjustments[i].value);
        }

        GetNextEvent();
    }
    private void ModifyVariable()
    {
        Debug.Log(currentEvent.cEventType);

        for (int i = 0; i < currentEvent.variableModifiers.Count; i++)
        {
            int valueToWorkWith = 0;
            VariableModifier curr = currentEvent.variableModifiers[i];
            valueToWorkWith = curr.valueType == 0 ? curr.value : DialogueChainPreferences.GetChainInt(curr.valueToOperate);

            if (valueToWorkWith == 0)
                Debug.Log("<color=blue> ValueToWorkWith is equal 0.");

            switch (curr.equator)
            {
                // +
                case 0:
                    //int currentValue = DialogueChainPreferences.GetChainInt(curr.intNeeded);
                    //currentValue += valueToWorkWith;
                    //DialogueChainPreferences.SetChainInt(curr.intNeeded, currentValue);


                    int valueToAdd = DialogueChainPreferences.GetChainInt(curr.intNeeded);
                    valueToAdd += valueToWorkWith;
                    DialogueChainPreferences.SetChainInt(curr.intNeeded, valueToAdd);


                    break;
                // -
                case 1:


                    int valueToSub = DialogueChainPreferences.GetChainInt(curr.intNeeded);
                    valueToSub -= valueToWorkWith;
                    DialogueChainPreferences.SetChainInt(curr.intNeeded, valueToSub);


                    break;
                // =   (set value)
                case 2:

                    DialogueChainPreferences.SetChainInt(curr.intNeeded, valueToWorkWith);
                    break;
                default:
                    Debug.Log("Interger for operator out of range");
                    break;
            }
        }


        GetNextEvent();
    }
    private void Message()
    {
        if (currentEvent.sendMessage[0])
        {
            additions.ChainMessage(currentEvent.messageFloat);
        }
        if (currentEvent.sendMessage[1])
        {
            additions.ChainMessage(currentEvent.messageString);
        }
        if (currentEvent.sendMessage[2])
        {
            additions.ChainMessage(currentEvent.messageBool);
        }

        GetNextEvent();
    }
    private void Check()
    {
        GetNextEvent();
    }
    private void StartMinigame()
    {
        //currentEvent.minigameSceneRef = new SceneReference(currentEvent.sceneAsset);
        GameManager.instance.managers.minigameManager.currentMinigame = new Minigame(currentEvent.minigameSceneRef);
        GameManager.instance.managers.minigameManager.currentMinigame.SetNameAndPath(GameManager.instance.managers.minigameManager.MinigamesAssetPath);

        GetNextEvent();

        GameManager.instance.managers.minigameManager.UpdateCurrentMinigame(currentEvent.minigameSceneRef);
    }
    private void SetLocation()
    {
        if (currentEvent.useCustomRoom && currentEvent.room != null)
        {
            NavigationManager.instance.SetRoom(currentEvent.room.name);
        }
        else
        {
            Location location = currentEvent.location;
            string firstRoomName = location.rooms[0].name;
            int locationID = GameManager.instance.managers.variablesManager.locationCollection.GetLocationID(firstRoomName);
            NavigationManager.instance.SetRoom(locationID, currentEvent.location.defaultRoomID);


            DialogueManager.instance.StartCoroutine(WaitTransitionToChangeGameState(location.sceneName, location.scenePath));

        }
        GetNextEvent();
    }

    private IEnumerator WaitTransitionToChangeGameState(string sceneName, string scenePath)
    {

        //Debug.LogErrorFormat("Trying to load : {0}", sceneName);
        GameManager.instance.managers.sceneManager.LoadNavigationScene(sceneName, scenePath);

        NavigationController.instance.UpdatePosition();
        yield return new WaitUntil(() => TransitionManager.instance.maskAnimationState == AnimationState.Waiting);


        if (currentEvent.isEnd)
        {
            GameManager.instance.gameState = GameState.Navigation;
        }

        NavigationManager.instance.needToOpenTheNavBar = true;

        yield return null;
    }
    #endregion

    #region GettingNextEvent
    public void GetNextEvent()
    {

        if (currentEvent.nextEventIDs.Count == 1)
        {
            nextEvent = NextEventOneOption(currentEvent);
        }
        else if (currentEvent.nextEventIDs.Count == 0)
        {
            hasEnded = true;
        }
        else
        {
            nextEvent = NextEventMoreOptions(currentEvent);
        }
        if (nextEvent == null)
        {
            hasEnded = true;
        }

        if (!paused)
        {
            if (waitForConfirm)
            {
                waitForConfirm = false;

                if (DialogueManager.instance.debug_ConversationLog)
                    Debug.Log("Start video here?");

                DialogueController.instance.StartCoroutine(DialogueController.instance.RunNextEventAfterUserConfirms(this, nextEvent));
            }
            else
            {
                DialogueController.instance.CloseDialogue();
                RunEvent(nextEvent);
            }
        }
    }

    ChainEvent NextEventOneOption(ChainEvent dEvent)
    {
        foreach (ChainEvent dEvent2 in chainEvents)
        {
            if (dEvent2.eventID == dEvent.nextEventIDs[0])
            {
                if (EventPassesCheck(dEvent2))
                {
                    return dEvent2;
                }
            }
        }

        Debug.Log("Couldn't Get next Quest event " + dEvent.eventID);
        return null;
    }

    ChainEvent NextEventMoreOptions(ChainEvent dEvent)
    {
        //Gets all next events
        List<ChainEvent> possibleEvents = new List<ChainEvent>();
        for (int i = 0; i < dEvent.nextEventIDs.Count; i++)
        {
            foreach (ChainEvent dEvent2 in chainEvents)
            {
                if (dEvent2.eventID == dEvent.nextEventIDs[i])
                {
                    possibleEvents.Add(dEvent2);
                    break;
                }
            }
        }
        //Orders next events by rank
        List<ChainEvent> eventsOrdered = new List<ChainEvent>();
        int checkRank = possibleEvents.Count - 1;
        int whileCheck = 0;
        while (eventsOrdered.Count < possibleEvents.Count || whileCheck < 400)
        {
            int sameRankCount = 0;
            for (int i = 0; i < possibleEvents.Count; i++)
            {
                if (possibleEvents[i].rank >= checkRank && !eventsOrdered.Contains(possibleEvents[i]))
                {
                    eventsOrdered.Insert(sameRankCount, possibleEvents[i]);
                    sameRankCount++;
                }
            }
            checkRank--;
            whileCheck++;
        }
        //Finds first event on list that passes requirements
        for (int i = 0; i < eventsOrdered.Count; i++)
        {
            if (EventPassesCheck(eventsOrdered[i]))
            {
                return eventsOrdered[i];
            }
        }

        Debug.Log("Couldn't Get next chain event " + dEvent.eventID);
        return null;
    }

    bool EventPassesCheck(ChainEvent dEvent)
    {
        if (dEvent.cEventType == ChainEventType.Check || dEvent.cEventType == ChainEventType.SecondaryInput)
        {
            for (int i = 0; i < dEvent.triggerChecks.Count; i++)
            {
                if (dEvent.triggerChecks[i].triggered != dEvent.triggerCheckBools[i])
                {
                    return false;
                }
            }
            for (int i = 0; i < dEvent.itemChecks.Count; i++)
            {
                if (DialogueChainPreferences.itemsAreScriptableObjects)
                {
                    if (!DialogueChainPreferences.InventoryContains(dEvent.itemChecks[i]))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!DialogueChainPreferences.InventoryContainsString(dEvent.itemChecksString[i]))
                    {
                        return false;
                    }
                }
            }
            for (int i = 0; i < dEvent.chainIntChecks.Count; i++)
            {
                if (dEvent.chainIntChecks[i].equator == 0)
                {
                    if (DialogueChainPreferences.GetChainInt(dEvent.chainIntChecks[i].intNeeded) >= dEvent.chainIntChecks[i].value)
                    {
                        return false;
                    }
                }
                else if (dEvent.chainIntChecks[i].equator == 1)
                {
                    if (DialogueChainPreferences.GetChainInt(dEvent.chainIntChecks[i].intNeeded) <= dEvent.chainIntChecks[i].value)
                    {
                        return false;
                    }
                }
                else
                {
                    if (DialogueChainPreferences.GetChainInt(dEvent.chainIntChecks[i].intNeeded) != dEvent.chainIntChecks[i].value)
                    {
                        return false;
                    }
                }
            }
            for (int i = 0; i < dEvent.chainQuestChecks.Count; i++)
            {
                if (dEvent.chainQuestChecks[i].quest != null)
                {
                    if (dEvent.chainQuestChecks[i].questState != dEvent.chainQuestChecks[i].quest.state)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    public void GetNextEventFromInput(int inputIndex)
    {
        List<int> nextEventIDs;


        if (inputIndex <= currentEvent.inputButtons.Count - 1)
        {
            nextEventIDs = currentEvent.inputButtons[inputIndex].nextEventIDsForInputs;
        }
        else
        {
            nextEventIDs = currentEvent.secondaryInputButtons[inputIndex - (currentEvent.inputButtons.Count)].nextEventIDsForInputs;
        }

        if (nextEventIDs.Count == 1)
        {
            nextEvent = NextEventOneOptionUserInput(nextEventIDs[0]);
        }
        else if (nextEventIDs.Count == 0)
        {
            hasEnded = true;
        }
        else
        {
            nextEvent = NextEventMoreOptionsUserInput(nextEventIDs);
        }
        if (nextEvent == null)
        {
            hasEnded = true;
        }

        if (!paused)
        {
            if (waitForConfirm)
            {
                waitForConfirm = false;
                DialogueController.instance.StartCoroutine(DialogueController.instance.RunNextEventAfterUserConfirms(this, nextEvent));
            }
            else
            {
                DialogueController.instance.CloseDialogue();
                RunEvent(nextEvent);
            }
        }
    }

    ChainEvent NextEventOneOptionUserInput(int eventID)
    {
        foreach (ChainEvent dEvent2 in chainEvents)
        {
            if (dEvent2.eventID == eventID)
            {
                if (EventPassesCheck(dEvent2))
                {
                    return dEvent2;
                }
            }
        }

        Debug.Log("Couldn't Get next Quest event from user input");
        return null;
    }

    ChainEvent NextEventMoreOptionsUserInput(List<int> nextEventIDs)
    {
        //Gets all next events
        List<ChainEvent> possibleEvents = new List<ChainEvent>();
        for (int i = 0; i < nextEventIDs.Count; i++)
        {
            foreach (ChainEvent dEvent2 in chainEvents)
            {
                if (dEvent2.eventID == nextEventIDs[i])
                {
                    possibleEvents.Add(dEvent2);
                    break;
                }
            }
        }
        //Orders next events by rank
        List<ChainEvent> eventsOrdered = new List<ChainEvent>();
        int checkRank = possibleEvents.Count - 1;
        int whileCheck = 0;
        while (eventsOrdered.Count < possibleEvents.Count || whileCheck < 400)
        {
            int sameRankCount = 0;
            for (int i = 0; i < possibleEvents.Count; i++)
            {
                if (possibleEvents[i].rank >= checkRank && !eventsOrdered.Contains(possibleEvents[i]))
                {
                    eventsOrdered.Insert(sameRankCount, possibleEvents[i]);
                    sameRankCount++;
                }
            }
            checkRank--;
            whileCheck++;
        }
        //Finds first event on list that passes requirements
        for (int i = 0; i < eventsOrdered.Count; i++)
        {
            if (EventPassesCheck(eventsOrdered[i]))
            {
                return eventsOrdered[i];
            }
        }

        Debug.Log("Couldn't Get next Quest event from user input");
        return null;
    }
    #endregion


    void ChainEnded()
    {
        if (!isSubDialogueChain)
        {
            if (additions != null)
            {
                additions.OnChainEnd();
            }
            DialogueChainPreferences.SetHaltMovement(originalHaltMovement);
            DialogueController.instance.isRunning = false;
            GameManager.instance.managers.dialogueManager.triggeredByInteractable = false;
            GameManager.instance.managers.dialogueManager.GoToPreviousState();

        }
        else
        {
            beforeSubDialogue.paused = false;

            beforeSubDialogue.GetNextEvent();
        }

    }
}
