using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public enum AnimationState
{
    Done,
    Starting,
    Waiting,
    Ending,
}




/// <summary>
/// V Automation on scene transition, select a default transition between scene change ( sceneLoader.LoadSceneWithGameState)
/// X This default transition could be overwritten by an external call (Quest reward, dialogue node)
/// V based on the selected transition, adapt the transitionTime to wait during a proper timer
/// V While transition is running, Disable all interraction (unsing the Raycast block) and all inputs (prevent skipping through dialogue while transitionning)
/// X Relocate the navUI animation and its management here
/// </summary>
public class TransitionManager : MonoBehaviour
{
    [Header("Manager Reference")]
    public static TransitionManager instance = null;
    private SceneLoaderManager sceneLoader;   // Depending on the transition effect, load a scene after the transition is completed

    [Header("Animator Reference")]
    public Animator transition;           // manage the masked transitions
    public Animator navCtrlAnimator;      // manade the UI slides transitions
    public TransitionAnimationControllers transitionControllers;

    [Header("Processed Data")]
    public AnimationState maskAnimationState = AnimationState.Done;

    [Header("Tweekable values")]
    public float transitionTimeStart = 1f;
    public float transitionTimeEnd = 1f;
    public TransitionType askedTransition = TransitionType.None; // <= Quest reward and dialogue nodes can affect that variable


    #region Unity Events

    private UnityAction StartEndAnimation;


    private void OnEnable()
    {
        EventManager.StartListening("StartEndTransitionAnimation", StartEndAnimation);
    }
    private void OnDisable()
    {
        EventManager.StopListening("StartEndTransitionAnimation", StartEndAnimation);
    }


    public void StartEndingAnim()
    {
        maskAnimationState = AnimationState.Ending;
    }



    #endregion


    #region MonoBehaviour

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
        StartEndAnimation = new UnityAction(StartEndingAnim);
    }

    private void Start()
    {
        sceneLoader = GameManager.instance.managers.sceneManager;
        maskAnimationState = AnimationState.Done;
    }

    #endregion


    #region Coroutine

    public IEnumerator StartTransition(GameState currGameState)
    {

        maskAnimationState = AnimationState.Starting;
        // If no external source asked for a perticular transition
        if (askedTransition == TransitionType.None)
        {
            transition.runtimeAnimatorController = transitionControllers.GetAnimator(currGameState, sceneLoader.GetPreviousGameState());
        }
        else
        {
            transition.runtimeAnimatorController = transitionControllers.GetAnimator(askedTransition);
            askedTransition = TransitionType.None;
        }

        GetCurrentClipLenght();


        //Debug.LogError("Start : " + transitionTimeStart + "\nEnd : " + transitionTimeEnd);

        // Lock player input during dialogue
        DialogueManager.instance.blockNext = true;


        transition.ResetTrigger("End");
        transition.ResetTrigger("Start");
        transition.Play("Start");


        transition.SetTrigger("Start");

        //Debug.LogError("transitionTimeStart: " + transitionTimeStart);
        yield return new WaitForSeconds(transitionTimeStart);

        maskAnimationState = AnimationState.Waiting;

        //if (currGameState == GameState.Navigation)
        //{
        //    yield return new WaitUntil(() => sceneLoader.navigationSceneLoaded);
        //    sceneLoader.navigationSceneLoaded = false;
        //}

        // Extend the the transition for the quest manager to finish his job
        // Set a value to say to the scenemanager "hey you can load now"

        yield return new WaitUntil(() => maskAnimationState == AnimationState.Ending);


        transition.SetTrigger("End");

        yield return new WaitForSeconds(transitionTimeEnd);

        maskAnimationState = AnimationState.Done;

        // Unlock player input during dialogue
        DialogueManager.instance.blockNext = false;
        transition.runtimeAnimatorController = transitionControllers.GetAnimator(askedTransition);
        transition.SetTrigger("End");
    }




    #endregion


    #region LocalGetter

    private void GetCurrentClipLenght()
    {
        AnimationClip[] clips = transition.runtimeAnimatorController.animationClips;
        transitionTimeStart = clips[0].length;
        transitionTimeEnd = clips[1].length;
    }

    #endregion

    #region ExternalGetter

    public bool IsAnimCompleted()
    {
        return maskAnimationState == AnimationState.Done;
    }


    #endregion

    #region ExternalSetter

    public void SetNextTransition(TransitionType transitionType)
    {
        if (transitionType != TransitionType.None)
            askedTransition = transitionType;
    }


    #endregion

    #region ExternalMethod

    public void SetAndStartTransition(GameState currGameState, TransitionType transitionType)
    {
        askedTransition = transitionType;
        StartCoroutine(StartTransition(currGameState));
    }

    #endregion
}
