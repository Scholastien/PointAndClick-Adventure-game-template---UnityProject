using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Manage the interraction Queue
/// Every interractible would send themself to this manager
/// The role of this manager is to make a Queue that will prioritize the order of execution
/// Currently i have an object with a dialogue interractible and a minigame interractible
/// dialogue should be executed first, then minigame
/// 
/// 
/// </summary>
public class InterractableManager : MonoBehaviour
{

    public Camera uiCamera;

    // Make a static instance, allow me to do what is called "Singleton pattern"
    //public static InterractableManager instance = null;

    public GameObject PopUpMouseOverPrefab;
    public string interractableDisplayName;
    public bool showPopup = false;

    // List of interractibles, ready to be processed
    public List<Interractible> interractibles;

    public Vector3 infoBoxOffset;

    [Header("Input Locks")]
    public bool interacted = false;



    [Header("Dialogue related")]
    public bool CR_running = false;

    #region MonoBehaviour

    private void Awake()
    {
        // Singleton Pattern
        // if instance is null => instance will be equal that instance of class
        //if (instance = null)
        //{
        //    instance = this;
        //}
        //// if instance is different from the instance => Destroy that instance of class
        //else if (instance != this)
        //{
        //    Destroy(this);
        //}
        // Singleton pattern allow me to have one and only one instance of class
        // Moreover since that class is alone, i can call it from everywhere
    }

    // Use this for initialization
    void Start()
    {
        CR_running = false;
        infoBoxOffset = new Vector3(0, -50, 0);
    }

    // Physic cycle
    private void FixedUpdate()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Start coroutine to Manage the Queue ordering
        // Coroutine musn't be running and the list shouldn't be empty
        if (!CR_running && interractibles.Count > 0)
        {
            StartCoroutine(OrderTheListAndExecute());
        }
    }

    // After Animation routine
    private void LateUpdate()
    {

    }

    // Yield wait end of frame is the last part before redoing the update cycle

    #endregion

    #region Manager

    // Coroutine that waith the end of the frame before ordering the list
    public IEnumerator OrderTheListAndExecute()
    {
        CR_running = true;
        // wait the end of the frame 
        // When every interractible on the gameobject have been sent to this manager
        yield return new WaitForEndOfFrame();

        // Ordering list
        interractibles = OrderInterractibleList();



        // wait while dialogue is running
        // => Just test if dialogue is running
        // if not running : execute the first interractible in the list


        if (!GameManager.instance.managers.dialogueManager.isRunning)
        {
            // Executing list starting by first element
            // interractibles[0] should be a dialogue interractible if the game object contained a dialogue interractible (dialogue or character)
            if (interractibles[0].GetType() == typeof(CharacterInterractible) ||
                interractibles[0].GetType() == typeof(DialogueTriggerInterractible))
            {
                // Start dialogue
                if (interractibles[0].GetType() == typeof(CharacterInterractible))
                {
                    ClickCharacter((CharacterInterractible)interractibles[0]);
                }
                else if (interractibles[0].GetType() == typeof(DialogueTriggerInterractible))
                {
                    ClickDialogueTrigger((DialogueTriggerInterractible)interractibles[0]);
                }

                // Wait for 0.01s (next frame)
                yield return new WaitForSeconds(0.001f);

                // remove this element
                interractibles.RemoveAt(0);
            }
            else if (GameManager.instance.gameState != GameState.Dialogue) // Execute all other interractions
            {
                //Syield return new WaitForSeconds(0.001f);
                yield return new WaitForEndOfFrame();

                if (interractibles[0].GetType() == typeof(MinigameInterractible))
                {
                    while (GameManager.instance.gameState == GameState.Dialogue)
                    {
                        yield return null;
                    }
                    yield return new WaitUntil(() => !GameManager.instance.managers.dialogueManager.isRunning);
                    ClickMinigame((MinigameInterractible)interractibles[0]);

                    //yield return new WaitForSeconds(0.001f);
                    //GameManager.instance.gameState = GameState.Navigation;

                    //yield return new WaitForSeconds(0.001f);

                }
                else if (interractibles[0].GetType() == typeof(ItemInterractible))
                {
                    ClickItem((ItemInterractible)interractibles[0]);
                }
                else if (interractibles[0].GetType() == typeof(FurnitureInterractible))
                {
                    ClickFurniture((FurnitureInterractible)interractibles[0]);
                }
                // remove this element
                interractibles.RemoveAt(0);
            }
        }

        // This Couroutine will be launched again and will alway try to see if the dialogue is over to give other interractible behaviour
        CR_running = false;
    }



    // order the list (dialogue first, minigame after)
    public List<Interractible> OrderInterractibleList()
    {
        List<Interractible> orderedList = new List<Interractible>();

        // Order:
        // - CharacterInterractible
        // - DialogueTriggerInterractible
        // - MinigameInterractible
        // - ItemInterractible
        // - FurnitureInterractible

        foreach (Interractible interractible in interractibles)
        {
            // - CharacterInterractible
            if (interractible.GetType() == typeof(CharacterInterractible))
            {
                orderedList.Add(interractible);
            }
        }
        foreach (Interractible interractible in interractibles)
        {
            // - DialogueTriggerInterractible
            if (interractible.GetType() == typeof(DialogueTriggerInterractible))
            {
                orderedList.Add(interractible);
            }
        }
        foreach (Interractible interractible in interractibles)
        {
            // Be sure there's not more that 1 interractible that can start a Dialogue
            int dialogueCount = 0;
            if (interractible.GetType() == typeof(CharacterInterractible))
            {
                dialogueCount++;
            }
            else if (interractible.GetType() == typeof(DialogueTriggerInterractible))
            {
                dialogueCount++;
            }
            // - MinigameInterractible
            else if (interractible.GetType() == typeof(MinigameInterractible))
            {
                orderedList.Add(interractible);
            }

            if (dialogueCount > 1)
            {
                Debug.Log("<color=red> More than one interractible script on this object (" + interractible.gameObject.name + ") </color>");
            }
        }
        foreach (Interractible interractible in interractibles)
        {
            // - ItemInterractible
            if (interractible.GetType() == typeof(ItemInterractible))
            {
                orderedList.Add(interractible);
            }
        }
        foreach (Interractible interractible in interractibles)
        {
            // - FurnitureInterractible
            if (interractible.GetType() == typeof(FurnitureInterractible))
            {
                orderedList.Add(interractible);
            }
        }

        return orderedList;

    }




    #endregion

    #region ClickBehaviour


    public void ClickCharacter(CharacterInterractible current)
    {
        Debug.Log("Start Dialogue with interractible");
        GameManager.instance.managers.dialogueManager.talkTo = current.character.function;
        GameManager.instance.managers.dialogueManager.triggeredByInteractable = true;
        GameManager.instance.managers.dialogueManager.RunDialogue(current.ChainToRun);

        current.npcScheduleBehaviour.NextActivity(current.activity);
    }

    public void ClickDialogueTrigger(DialogueTriggerInterractible current)
    {
        if (current.ChainToRun != null)
            GameManager.instance.managers.dialogueManager.RunDialogue(current.ChainToRun);
        else
            Debug.Log("ChainToRun in this DialogueTrigger <color=red>undefined</color> at \n" + current.displayName + "(<color=blue>" + current.nameIdentifier + "</color>)");
    }

    public void ClickMinigame(MinigameInterractible current)
    {
        current.StartMinigame();
        //Debug.Log("Starting Minigame");
        //GameManager.instance.managers.minigameManager.UpdateCurrentMinigame(current.sceneReference);

    }

    public void ClickItem(ItemInterractible current)
    {
        Debug.Log("Picking item: " + current.item.name);
        GameManager.instance.managers.inventoryManager.PickItem(current.item);
        current.trigger.triggered = true;

    }

    public void ClickFurniture(FurnitureInterractible current)
    {
        Debug.Log("Furniture");

        ///TODO

    }





    #endregion

    #region MouseoverBehaviour

    public void DisplayPopup(string displayName)
    {
        //Debug.Log(displayName);
        showPopup = true;
        interractableDisplayName = displayName;
        PopUpMouseOverPrefab.transform.position = Input.mousePosition + new Vector3(0, -50, 0);
    }


    public void UpdateInfoBoxPosition()
    {
        //Vector2 localPoint = new Vector2();
        // PopUpMouseOverPrefab
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(PopUpMouseOverPrefab.transform.parent.GetComponent<RectTransform>(), Input.mousePosition, uiCamera, out localPoint);


        PopUpMouseOverPrefab.transform.position = Vector3.Lerp(PopUpMouseOverPrefab.transform.position, Input.mousePosition + infoBoxOffset, 0.1f);
    }

    public void HidePopup()
    {
        //Debug.Log("Hide the popup");
        showPopup = false;
        //PopUpMouseOverPrefab.gameObject.SetActive(false);
    }

    #endregion

    public IEnumerator WaitBeforeNextInteraction()
    {
        yield return new WaitForSeconds(0.1f);
    }

}
