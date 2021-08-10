using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NavigationController : MonoBehaviour
{

    public static NavigationController instance = null;

    public bool readyToShare;

    public Room currentRoom;
    public Location currentLocation;

    public TextMeshProUGUI tooltip;


    private Room previousRoom;
    private Location previousLocation;

    private PlayerController playerCtrl;
    private NavigationUIBehaviour navUI;
    private LocationCanvasBehaviour locationCanvasBehaviour;
    private NavigationManager nm;

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

        nm = GameManager.instance.managers.navigationManager;

        currentRoom = GameManager.instance.managers.navigationManager.currentRoom;
        currentLocation = GameManager.instance.managers.navigationManager.currentLocation;

        playerCtrl = FindObjectOfType<PlayerController>();
        navUI = FindObjectOfType<NavigationUIBehaviour>();




    }

    // Update is called once per frame
    void Update()
    {

        if (GameManager.instance.gameState == GameState.Navigation)
        {
            navUI.gameObject.SetActive(true);
            UpdatePosition();
        }
        else
        {
            navUI.gameObject.SetActive(false);
        }

        InterractableManager im = GameManager.instance.managers.interractableManager;
        tooltip.text = im.interractableDisplayName;
        tooltip.transform.parent.gameObject.SetActive(im.showPopup);
    }

    private void OnDestroy()
    {
        // Unload locationscene
        GameManager.instance.managers.sceneManager.UnloadSceneWithName(currentLocation.sceneName);
    }

    public void UpdatePosition()
    {
        currentRoom = GameManager.instance.managers.navigationManager.currentRoom;
        currentLocation = GameManager.instance.managers.navigationManager.currentLocation;

        //if (nm.Debug_NavigationLog)
        //    Debug.Log("<color=#9999ff>" + "UpdatePosition" + "</color>:   <color=yellow> \n"
        //        + currentLocation.name + "</color> -> " + "<color=yellow> " + currentRoom.name + " </color>");


        locationCanvasBehaviour = FindObjectOfType<LocationCanvasBehaviour>();

        if (currentRoom != previousRoom && locationCanvasBehaviour != null)
        {
            // if the room change

            // Ui change
        }
        else
        {

        }
        if (currentLocation != previousLocation)
        {

            // if the location change

            // Scene change
            UpdateLocation();
            //GameManager.instance.managers.sceneManager.LoadNavigationScene(currentLocation.sceneName, currentLocation.scenePath);
        }
        else
        {
        }

        previousLocation = currentLocation;
        previousRoom = currentRoom;
    }

    public void UpdateLocation()
    {
        // Call Transition anim

        // wait until the first anim is done

        // Trigger the end and load at the same time



        // Sync with GameManager
        GameManager.instance.managers.sceneManager.LoadNavigationScene(currentLocation.sceneName, currentLocation.scenePath);
        // Get all info

        // update ui
        navUI.SpawnNavButtons();

    }
}
