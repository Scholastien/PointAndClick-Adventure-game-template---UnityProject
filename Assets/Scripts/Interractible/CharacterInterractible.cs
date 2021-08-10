using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ScheduleSystem;

public class CharacterInterractible : Interractible {

    [HideInInspector]
    public NpcScheduleBehaviour npcScheduleBehaviour;

    [Header("Properties")]
    public Character character;
    //public CharacterFunction character;
    public Activity activity;

    public DialogueChain ChainToRun;

    public bool refresh;

    public bool interracted = false;


    private void OnEnable()
    {
        EventManager.onEndOfDialogue += ResetInterrated;
    }

    private void OnDisable()
    {
        EventManager.onEndOfDialogue -= ResetInterrated;
        interracted = false;
    }


    // Use this for initialization
    new void Start () {
        base.Start();

        NpcScheduleBehaviour[] npcScheduleBehaviours = GameObject.FindObjectsOfType<NpcScheduleBehaviour>();
        foreach(NpcScheduleBehaviour scheduleBehaviour in npcScheduleBehaviours)
        {
            if (scheduleBehaviour.character == character.function)
                npcScheduleBehaviour = scheduleBehaviour;
        }
        refresh = false;
        displayInfoBox = true;
        character.name = FindObjectOfType<PlayerController>().GetNpcName(character.function);
        displayName = "Talk to " + char.ToUpper(character.name.ToString()[0]) + character.name.ToString().Substring(1);

    }
    private void Update()
    {
        
    }

    private new void LateUpdate()
    {
        if (interracted)
        {
            image.enabled = !interracted;
            Debug.LogError("disapear = " + !interracted);
        }
        else
        {
            if (room == GameManager.instance.managers.navigationManager.currentRoom)
            {

                if (npcScheduleBehaviour.IsScheduled(this))
                {
                    hide = false;
                    npcScheduleBehaviour.MakeInterractible(this);
                    npcScheduleBehaviour.currentCharacterInterractible = this;
                    npcScheduleBehaviour.previousCharacterInterractible = this;
                }
                else if (refresh == true)
                {
                    npcScheduleBehaviour.MakeInterractible(this);
                    refresh = false;
                }
                else
                {
                    hide = true;
                }


                //
                //{
                //    if (npcScheduleBehaviour.currentCharacterInterractible != null)
                //        npcScheduleBehaviour.currentCharacterInterractible.image.sprite = npcScheduleBehaviour.sm.transparentInterractible;

                //}
                //
            }
            else
            {
                hide = true;
                refresh = true;
                //npcScheduleBehaviour.currentCharacterInterractible.image.sprite = npcScheduleBehaviour.sm.transparentInterractible;
            }
            base.LateUpdate();
        }
    }

    public void StartDialoque()
    {
        EventManager.instance.Interract();
        if (!disable)
        {
            if(DialogueManager.instance.debug_ConversationLog)
                Debug.Log("Start Dialogue with interractible");

            GameManager.instance.managers.interractableManager.interacted = true;

            interracted = true;
            //Debug.LogError("Interracted");
            GameManager.instance.managers.dialogueManager.talkTo = character.function;
            GameManager.instance.managers.dialogueManager.triggeredByInteractable = true;
            GameManager.instance.managers.dialogueManager.RunDialogue(ChainToRun);
            npcScheduleBehaviour.NextActivity(activity);
        }

    }


    void ResetInterrated()
    {
        image.enabled = !interracted;
        interracted = false;
        LateUpdate();
    }


}
