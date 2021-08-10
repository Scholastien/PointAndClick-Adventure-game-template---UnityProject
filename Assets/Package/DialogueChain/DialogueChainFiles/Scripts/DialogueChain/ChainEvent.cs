using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InventorySystem;
using UnityEngine.Video;
using ScheduleSystem;
using QuestSystem;
using SceneReferenceInEditor;
using UnityEditor;

[System.Serializable]
public class ChainEvent
{
    public ChainEventType cEventType;
    public int eventID;

    public ChainEvent previousChainEvent;

    public List<int> nextEventIDs = new List<int>();
    public List<int> previousEventIDs = new List<int>();
    public List<int> lateralConnections = new List<int>();

    [HideInInspector]
    public Rect windowRect;

    public int rank;

    #region Dialogue
    [TextArea(3, 3)]
    public string dialogue;
    public ContainerType dialogueContainer;

    public Sprite speakerImage;
    public int playerImageIndex;
    public bool useCustomPlayerImage;
    public bool flipImage;
    public bool leftSide = true;
    public bool showImage;
    public string speaker;
    public bool noSpeaker;
    public float textDelay = 0.02f;
    public float dialogueWaitTime = 0;
    public float dialoguefadeTime = 0;

    // Automatic fade in on start.
    public bool autoFadeAtStart = false;
    public float autoFadeDuration = 0.5f;
    public bool SkipWhenFadeDone = false;

    // End / start Tag
    public bool isEnd = false;

    //Call the AudioManager
    public bool useSound = false;
    public bool useTransparentBG = true;
    public bool useBlurryBG = true;
    public bool waitUntilNextorEnd = false;
    public DialogueAudioSound dialogueAudioSound;

    //Overwrite the default image with the random selected one
    public bool useRandomSprite = false;
    public List<Sprite> randomSprites = new List<Sprite>();

    // Stacked picture allow a VN dialogue look alike
    public bool useStackedPicture = true;
    public List<StackedSprite> stackedSprites = new List<StackedSprite>();

    // Video as BG
    public bool inVideoChain = false;
    public bool overwritePreviousVideo = false;
    public bool useVideo = false;
    public Video video;

    // Transition Start or End
    public TransitionType transitionType;


    #endregion

    #region UserInput
    public List<DialogueEventInputButton> inputButtons = new List<DialogueEventInputButton>();
    #endregion

    #region TriggerSet
    public List<ChainTrigger> triggers = new List<ChainTrigger>();
    public List<bool> triggerBools = new List<bool>();
    #endregion

    #region Item/Experience
    public List<Item> itemsGiven = new List<Item>();
    public List<string> itemsGivenString = new List<string>();
    public List<Item> itemsTaken = new List<Item>();
    public List<string> itemsTakenString = new List<string>();
    public int experienceGiven;
    #endregion

    #region Sub Dialogue
    public DialogueChain subDialogue;
    #endregion

    #region Audio
    public AudioClip audio;
    public bool loop = false;
    public bool overlay = true;
    public float fadeTime;
    public bool playOriginalAfter = true;
    public float originalFadeTime = 1.25f;
    public float audioVolume = 1;
    #endregion

    #region IntegerAdjustment
    public List<IntAdjustment> chainIntAdjustments = new List<IntAdjustment>();
    #endregion

    #region TriggerCheck
    public List<ChainTrigger> triggerChecks = new List<ChainTrigger>();
    public List<bool> triggerCheckBools = new List<bool>();
    #endregion

    #region ItemCheck
    public List<Item> itemChecks = new List<Item>();
    public List<string> itemChecksString = new List<string>();
    #endregion

    #region IntegerCheck
    public List<IntCheck> chainIntChecks = new List<IntCheck>();
    #endregion

    #region QuestCheck

    public List<QuestCheck> chainQuestChecks = new List<QuestCheck>();

    #endregion

    #region SecondaryInput
    public List<DialogueEventInputButton> secondaryInputButtons = new List<DialogueEventInputButton>();
    #endregion

    #region Message
    public bool[] sendMessage = new bool[3];
    public float messageFloat;
    public string messageString;
    public bool messageBool;
    #endregion

    #region ScheduledActivity
    public List<ScheduledActivity> scheduledActivities = new List<ScheduledActivity>();
    #endregion

    #region VariableModifier
    public List<VariableModifier> variableModifiers = new List<VariableModifier>();
    #endregion

    #region MinigameLauncher
    public string scenePath;
    public SceneReference minigameSceneRef;

    #endregion

    #region SetNewLocation

    public Location location;
    public bool useCustomRoom = false;
    public Room room;

    #endregion



}

#region Integers for adjustment or check nodes
[System.Serializable]
public class IntCheck
{
    public ChainIntType intNeeded;
    [Range(0,2)]
    public int equator;
    public int value = 1;
}
[System.Serializable]
public class IntAdjustment
{
    public ChainIntType intAdjusted;
    public int value;
}
#endregion


#region Quest check

[System.Serializable]
public class QuestCheck
{
    public Quest quest; // the quest we want to check
    public QuestState questState; // the value we vant to compare with quest.questState
}

#endregion

[System.Serializable]
public class DialogueEventInputButton
{
    public string buttonText;
    public List<int> nextEventIDsForInputs = new List<int>();
}

[System.Serializable]
public class VariableModifier
{
    public ChainIntType intNeeded;
    [Range(0, 2)]
    public int equator;
    [Range(0, 1)]
    public int valueType;
    public int value = 1;
    public ChainIntType valueToOperate;
}




public enum ChainEventType
{
    Start,
    Dialogue,
    SetTrigger,
    UserInput,
    ItemManagement,
    Pause,
    Audio,
    IntAdjustment,
    SubDialogue,
    Check,
    VariableModifier,
    Message,
    SecondaryInput,
    ScheduledActivity,
    MinigameLauncher,
    SetLocation
}