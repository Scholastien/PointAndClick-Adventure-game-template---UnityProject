using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



/// <summary>
/// Base of every interractible (character, minigame, item, furniture, dialoguetrigger....)
/// This class contain the base logic of interractible
/// </summary>

[RequireComponent(typeof(Button), typeof(Image))]
public class Interractible : MouseoverBehaviour
{
    public InterractableManager im;


    [Header("Identifier")]
    public string nameIdentifier;
    public Room room;
    [HideInInspector]
    public Image image;

    [Header("Display Settings")]
    public TriggerDependency triggerDependency;
    public bool hide;

    [Header("DaytimeSpriteVariation")]
    public bool useDaytimeVariance = false;
    public List<DaytimeSpriteVariation> daytimeSpriteVariation;

    private GameObject instantiatedPopUp = null;

    [Header("UI animation")]
    public bool animCompleted = false;
    public bool isAnimDone = false;


    #region EventManager subscription



    #endregion


    // Use this for initialization
    protected override void Start()
    {
        im = GameManager.instance.managers.interractableManager;
        image = gameObject.GetComponent<Image>();
        if (gameObject.transform.parent.GetComponent<RoomBehaviour>() != null)
            room = gameObject.transform.parent.GetComponent<RoomBehaviour>().room != null ? gameObject.transform.parent.GetComponent<RoomBehaviour>().room : room;
        gameObject.GetComponent<Button>().onClick.AddListener(UnselectButton);
        nameIdentifier = nameIdentifier + "_" + room.name;

        base.Start();
    }


    public void FixedUpdate()
    {
        if (GameManager.instance.managers.interractableManager.showPopup)
        {
            GameManager.instance.managers.interractableManager.UpdateInfoBoxPosition();
        }
    }


    // Update is called once per frame
    protected virtual void LateUpdate()
    {
        if (useDaytimeVariance)
        {
            ReplaceWithDaytimeVariation();
        }

        if (triggerDependency.trigger != null)
        {
            if (triggerDependency.invertDependency)
            {
                disable = !triggerDependency.trigger.triggered;
            }
            else
            {
                disable = triggerDependency.trigger.triggered;
            }
        }

        image.enabled = !hide || (triggerDependency.hideWithTrigger && disable);
        //image.gameObject.SetActive(!hide || (triggerDependency.hideWithTrigger && disable));
        //gameObject.GetComponent<Button>().enabled = !disable;

        if (GameManager.instance.managers.interractableManager.showPopup)
        {
            GameManager.instance.managers.interractableManager.UpdateInfoBoxPosition();
        }

    }

    public bool isPopUp()
    {
        return (instantiatedPopUp != null);
    }

    public bool CheckIfPopUpExistElsewhere()
    {
        Interractible[] currentExistingInterractibles = gameObject.GetComponents<Interractible>();

        foreach (Interractible interractible in currentExistingInterractibles)
        {
            if (interractible.isPopUp())
            {
                return true;
            }
        }
        return false;
    }




    protected void UnselectButton()
    {
        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
    }

    public virtual void Interract()
    {
        if (!disable)
        {
            GameManager.instance.managers.interractableManager.interractibles.Add(this);
        }
    }

    #region DaytimeVariation

    public Sprite give_DaytimeSprite(TimeOfTheDay currentTime)
    {
        foreach (DaytimeSpriteVariation daytimeSprite in daytimeSpriteVariation)
        {
            if (daytimeSprite.IsActive(currentTime))
            {
                //if(GameManager.instance.managers.navigationManager.Debug_NavigationLog)
                //    Debug.Log("Picking a BG for this room from : \n" +"<color=green>" + daytimeSprite.begin + "</color> \t to   <color=green>" + daytimeSprite.end + "</color> ");
                return daytimeSprite.sprite;
            }
        }
        return null;
    }

    public void ReplaceWithDaytimeVariation()
    {
        image.sprite = give_DaytimeSprite(GameManager.instance.managers.timeManager.currentTime);
    }

    #endregion
}

[System.Serializable]
public class TriggerDependency
{
    // Interractible.Disable == trigger
    [Tooltip("Interactible will be active only when this Trigger is checked")]
    public ChainTrigger trigger;
    public bool invertDependency;
    public bool hideWithTrigger;
}
