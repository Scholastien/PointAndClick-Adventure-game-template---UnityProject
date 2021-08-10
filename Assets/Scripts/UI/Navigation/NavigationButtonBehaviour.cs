using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NavigationButtonBehaviour : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
{


    public Text btn_Txt;
    public Image btn_Img;

    public Room room;

    public Sound sound;

    private bool onlyOnce = true;

    [HideInInspector]
    public NavigationUIBehaviour uiBehaviour;

    private Animator animator;

    private bool alreadyInRoom = false;

    private void OnEnable()
    {
        EventManager.onNewValues += UpdateButtonInfo;
    }

    private void OnDisable()
    {
        EventManager.onNewValues -= UpdateButtonInfo;
    }


    // Use this for initialization
    void Start()
    {

        animator = transform.parent.GetComponent<Animator>();

        gameObject.GetComponent<Button>().onClick.AddListener(
            delegate
            {
                CallForChange();
            }
        );
        sound.audioType = AudioType.UiSFX;
        sound.loop = false;
        GameObject go = new GameObject(sound.audioType + "_" + sound.clipName);
        go.transform.SetParent(this.transform);
        sound.SetSourceAndOutput(go.AddComponent<AudioSource>(), AudioManager.instance.audioOutputMixer.OutputSelector(sound.audioType));
    }

    // Update is called once per frame
    void Update()
    {

        if (room != null)
        {
            UpdateButtonInfo();
        }

        if (room == GameManager.instance.managers.navigationManager.currentRoom)
        {
            // desactiver button
            gameObject.GetComponent<Button>().enabled = false;

            Image img = gameObject.GetComponent<Image>();
            Color color = new Color(1, 1, 1, 0.2f);
            img.color = color;

        }
        else
        {
            gameObject.GetComponent<Button>().enabled = true;
        }
    }

    private void UpdateButtonInfo()
    {
        string roomNameTranslated = LanguageManager.instance.GetTranslation(room.displayName);
        btn_Txt.text = roomNameTranslated;
        room.GetNavIcon((TimeOfTheDay)GameManager.instance.managers.variablesManager.gameSavedData.GameInfo.intTime);
        btn_Img.sprite = room.navIcon;

        onlyOnce = false;
    }

    private void FixedUpdate()
    {

        alreadyInRoom = (room == GameManager.instance.managers.navigationManager.currentRoom);
        if (Input.GetMouseButtonDown(0) && !alreadyInRoom)
        {
            // Check if the mouse was clicked over a UI element
            //if (EventSystem.current.IsPointerOverGameObject())
            if (EventSystem.current.gameObject == this.gameObject && EventSystem.current.IsPointerOverGameObject())
            {
                CallForChange();
            }
        }
    }

    void CallForChange()
    {
        StartCoroutine(WaitRoomLoad());
        sound.RandomPitch(0.99f, 1f);
        sound.Play();
    }


    IEnumerator WaitRoomLoad()
    {
        TransitionManager.instance.SetAndStartTransition(GameState.Navigation, TransitionType.CrossFadeShort);


        yield return new WaitUntil(() => TransitionManager.instance.maskAnimationState == AnimationState.Waiting);


        // check is we need to wait more for the quest manager to give rewards
        if (GameManager.instance.managers.questManager.needTowaitForDialogueReward(room))
        {
            yield return new WaitForEndOfFrame();
            GameManager.instance.managers.questManager.CheckObjectives();
            GameManager.instance.managers.questManager.RunDialogueRewardIfNeeded(room);

            //yield return new WaitUntil(() => DialogueManager.instance.runningDialogue);
            //yield return new WaitUntil(() => DialogueController.instance.isRunning);
            yield return new WaitForSeconds(0.2f);
        }


        EventManager.TriggerEvent("StartEndTransitionAnimation");


        GameManager.instance.managers.navigationManager.SetRoom(room.name);

        yield return new WaitUntil(() => room == NavigationManager.instance.currentRoom);

        uiBehaviour.SpawnNavButtons();
    }





    public void OnPointerDown(PointerEventData eventData)
    {
        if (!alreadyInRoom && NavigationManager.instance.fadeValue == 0f)
        {
            EventManager.Instance.NeedFadeIn();
            CallForChange();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (NavigationManager.instance.Debug_NavigationLog)
            Debug.Log("OnPointerEnter : " + this.gameObject.name);
    }
}


