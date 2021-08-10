using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.
using WindowsInput;
using WindowsInput.Native;
using QuestSystem;

[System.Serializable]
public class ConversationLog
{
    public string speaker;
    [TextArea(3, 10)]
    public List<string> dialogueLines;

    public string characterFunction_string;

    public ConversationLog(ChainEvent chainEvent)
    {
        characterFunction_string = chainEvent.speaker;
        speaker = DialogueChainPreferences.GetCharacterNameWithString(chainEvent.speaker)/*chainEvent.speaker*/;
        //dialogueLines = new List<string>();
        AddDialogueLines(chainEvent);
    }

    public void AddDialogueLines(ChainEvent chainEvent)
    {
        if (dialogueLines == null)
            dialogueLines = new List<string>();
        if (dialogueLines != null && chainEvent.dialogue != "")
            dialogueLines.Add(chainEvent.dialogue);
    }
}

public class DialogueManager : MonoBehaviour
{

    public static DialogueManager instance = null;

    public bool debug_ConversationLog = false;


    public bool runningDialogue = false;
    public DialogueChain ChainToRun;

    public DialogueCtrl dialCtrl;

    public int chainId;

    public List<ConversationLog> chainHistory;

    public bool btnPressed;

    public bool hiddenUi = false;

    public CharacterFunction talkTo;
    public bool triggeredByInteractable;

    public bool isRunning;
    public bool isWriting;
    public bool isFading;
    public bool isWaiting;
    public bool finishWriting;
    public float tempTextDelay;

    public bool skip;

    public bool alreadySelecting;

    public bool blockNext = false;


    public float TextSpeed = 0.1f;

    public DialogueSpawnBehaviour dialogueSpawn;

    public GameState previousGamestate;

    [HideInInspector]
    public GameObject myEventSystem;

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

        myEventSystem = GameObject.Find("EventSystem");
        triggeredByInteractable = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
        GameState gs = GameManager.instance.gameState;
        if (gs == GameState.Dialogue && previousGamestate == GameState.Dialogue)
        {
            if (!isRunning)
            {
                GameManager.instance.gameState = GameState.Navigation;
                previousGamestate = GameState.Navigation;
                GameManager.instance.managers.sceneManager.LoadSceneWithGameState(GameState.Navigation);
            }
        }
    }

    private void LateUpdate()
    {
        GameState gs = GameManager.instance.gameState;
        if (gs == GameState.Dialogue || gs == GameState.Navigation)
        {
            if (Input.GetButtonDown("Skip"))
            {
                if (skip)
                    skip = false;
                else
                    skip = true;
            }
        }
    }

    private void UpdateState()
    {
        isRunning = DialogueController.instance.isRunning;
        isWriting = DialogueController.instance.isWriting;
        isFading = DialogueController.instance.isFading;
        isWaiting = DialogueController.instance.isWaiting;
        finishWriting = DialogueController.instance.finishWriting;
        tempTextDelay = DialogueController.instance.tempTextDelay;

        alreadySelecting = myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().alreadySelecting;
    }

    public void UpdateValues()
    {
        chainId = DialogueController.instance.currentDialogueChain.GetCurrentEventID();

        ChainEvent cEvent = DialogueController.instance.currentDialogueChain.currentEvent;


        switch (cEvent.cEventType)
        {
            case ChainEventType.Start:
                break;
            case ChainEventType.Dialogue:
                CheckSpeaker(/*DialogueController.instance.currentDialogueChain.currentEvent*/cEvent);
                Debug.Log("current Event: \t speaker = " + cEvent.speaker + "\t id = " + cEvent.eventID + "\n" +
                "\t \t dialogueLines = " + cEvent.dialogue);
                break;
            case ChainEventType.SetTrigger:
                break;
            case ChainEventType.UserInput:
                CheckSpeaker(/*DialogueController.instance.currentDialogueChain.currentEvent*/cEvent);
                Debug.Log("current Event: \t speaker = " + cEvent.speaker + "\t id = " + cEvent.eventID + "\n" +
                "\t \t dialogueLines = " + cEvent.dialogue);
                break;
            case ChainEventType.ItemManagement:
                break;
            case ChainEventType.Pause:
                break;
            case ChainEventType.Audio:
                break;
            case ChainEventType.IntAdjustment:
                break;
            case ChainEventType.SubDialogue:
                break;
            case ChainEventType.Check:
                break;
            case ChainEventType.VariableModifier:
                break;
            case ChainEventType.Message:
                break;
            case ChainEventType.SecondaryInput:
                break;
            case ChainEventType.ScheduledActivity:
                break;
            default:
                break;
        }
    }

    public void CheckSpeaker(ChainEvent currentEvent)
    {

        ConversationLog tempLog = new ConversationLog(currentEvent);

        if (chainHistory.Count == 0)
        {
            chainHistory.Add(tempLog);
        }
        else
        {
            if (tempLog.speaker == chainHistory[chainHistory.Count - 1].speaker)
            {
                chainHistory[chainHistory.Count - 1].AddDialogueLines(currentEvent);
            }
            else
            {
                chainHistory.Add(tempLog);
            }
        }

    }

    public bool GetBtnPressed()
    {
        return btnPressed;
    }

    public void SetBtnPressed(bool pressed)
    {
        btnPressed = pressed;
    }

    // Start Dialogue
    public void RunDialogue(DialogueChain dialogueChain, DialogueReward dialogueReward = null)
    {
        if (dialogueReward == null)
        {
            StartCoroutine(WaitUntilUIAnimIsComplete(dialogueChain, dialogueReward));
        }
        else
        {
            //Debug.LogError("TRying to run dialogue");
            previousGamestate = GameManager.instance.gameState;
            GameManager.instance.gameState = GameState.Dialogue;
            ChainToRun = dialogueChain;
            dialogueSpawn = (DialogueSpawnBehaviour)FindObjectOfType(typeof(DialogueSpawnBehaviour));

            StopAllCoroutines();
            StartCoroutine(WaitForDialogueSpawnToBeReady(dialogueReward));
        }
    }


    public IEnumerator WaitUntilUIAnimIsComplete(DialogueChain dialogueChain, DialogueReward dialogueReward = null)
    {
        // Retrieve the start node from dialogue chain and send the transition type to the transition manager
        TransitionType askedTransition = dialogueChain.startEvent.transitionType;

        TransitionManager.instance.SetNextTransition(askedTransition);

        while (GameManager.instance.managers.interractableManager.interacted)
        {
            yield return null;
        }

        previousGamestate = GameManager.instance.gameState;
        GameManager.instance.gameState = GameState.Dialogue;

        //yield return new WaitUntil(() => TransitionManager.instance.maskAnimationState == AnimationState.Waiting);

        ChainToRun = dialogueChain;
        dialogueSpawn = (DialogueSpawnBehaviour)FindObjectOfType(typeof(DialogueSpawnBehaviour));

        StopAllCoroutines();
        StartCoroutine(WaitForDialogueSpawnToBeReady(dialogueReward));
    }


    public void NewGame_RunDialogue(DialogueChain dialogueChain)
    {
        previousGamestate = GameState.Navigation;
        ChainToRun = dialogueChain;
        dialogueSpawn = (DialogueSpawnBehaviour)FindObjectOfType(typeof(DialogueSpawnBehaviour));

        StopAllCoroutines();
        StartCoroutine(WaitForDialogueSpawnToBeReady());
    }

    IEnumerator WaitForDialogueSpawnToBeReady(DialogueReward dialogueReward = null)
    {
        if(dialogueReward != null)
        {
            GameManager.instance.managers.sceneManager.LoadSceneWithGameState(GameState.Dialogue, true);
        }


        //while(dialogueSpawn == null)
        //{
        //    yield return new WaitForEndOfFrame();
        //}
        yield return new WaitUntil(() => !(dialogueSpawn == null));
        runningDialogue = true;
        dialCtrl = new DialogueCtrl(ChainToRun);

        if (dialogueReward != null)
            dialogueReward.given = true;
    }

    // Next
    public void NextEvent()
    {

        StopCoroutine("Fade");
        StopCoroutine("Shake");
        StopCoroutine("Translate");

        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
        if (GetCurrentEventType() == ChainEventType.Dialogue || GetCurrentEventType() == ChainEventType.UserInput || GetCurrentEventType() == ChainEventType.SubDialogue)
        {
            InputSimulator i = new InputSimulator();
            i.Keyboard.KeyPress(VirtualKeyCode.SPACE);
        }

    }

    public void Skip()
    {

        //Debug.Log("skip");

        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);

        StartCoroutine(SpamMaxWritingSpeed());

    }

    public ChainEventType GetCurrentEventType()
    {
        return ChainToRun.currentEvent.cEventType;
    }

    public IEnumerator SpamMaxWritingSpeed()
    {
        // while current event is not a choice and button or input pressed



        yield return new WaitForSeconds(0.25f);

        if (ChainToRun.currentEvent.cEventType != ChainEventType.UserInput)
        {
            InputSimulator i = new InputSimulator();
            i.Keyboard.KeyPress(VirtualKeyCode.SPACE);
        }

        //InputSimulator i = new InputSimulator();
        //i.Keyboard.KeyPress(VirtualKeyCode.SPACE);

    }

    public void GoToPreviousState()
    {
        GameManager.instance.gameState = previousGamestate;
    }

    #region External Coroutine

    // Auto fade on background
    public void CallForAutoFade(GameObject go, float fadeDuration, bool skip)
    {
        Image image = go.GetComponent<Image>();
        DialogueManager.instance.StartCoroutine(AutoFade(image, fadeDuration, skip));
    }

    IEnumerator AutoFade(Image image, float fadeDuration, bool skip)
    {
        Color start = image.color;
        for (float i = fadeDuration; i >= 0; i -= Time.deltaTime)
        {
            // decrease alpha
            float alpha = i / fadeDuration;
            if (image != null)
                image.color = new Color(start.r, start.g, start.b, alpha);
            else
                break;
            yield return null;
        }
        if (image != null)
            image.color = new Color(start.r, start.g, start.b, 0);

        if (skip)
        {
            GameManager.instance.managers.dialogueManager.NextEvent();
        }
    }

    #endregion

}
