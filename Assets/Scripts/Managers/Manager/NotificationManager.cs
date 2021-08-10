using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NotificationSystem;

public class NotificationManager : MonoBehaviour
{
    //singleton
    public static NotificationManager instance = null;


    // gameobject reference
    public NotificationController notificationController;

    // raw data
    public float lifeTime = 3f;
    public float spawnAnimSpeed = .5f;
    public float despawnAnimSpeed = .5f;
    public float timeBetweenNotifSpawn = 0f;


    // prefab reference
    public GameObject notificationPrefab;

    // processed variables
    public AbstractNotification currentNotification;
    public List<AbstractNotification> notifications;
    public int currentID = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }


    #region External Function

    // Callback
    public void InvokeNotification(AbstractNotification notification)
    {
        currentNotification = notification;

        if (notifications == null)
            notifications = new List<AbstractNotification>();

        notifications.Add(notification);

        //NotificationController.instance.AddNewnotification(notification);

        StartCoroutine(PlayNotification(notifications.IndexOf(notification)));

    }


    #endregion


    #region Coroutine

    public IEnumerator PlayNotification(int index)
    {
        // Wait a few frames to let dialogue launch
        yield return new WaitForSeconds(0.5f);

        // Wait the end of dialogue
        yield return new WaitUntil(() => !GameManager.instance.managers.dialogueManager.isRunning);




        // Wait until notification index is the current ID in the playlist
        yield return new WaitUntil(() => index == currentID);


        yield return new WaitUntil(() => NotificationController.instance != null);
        // Spawn notification
        GameObject go = NotificationController.instance.SpawnNotification(notifications[index]);
        NotificationBehaviour notificationBehaviour = go.GetComponent<NotificationBehaviour>();

        // increase ID to make the next notif spawn
        yield return new WaitUntil(() => notificationBehaviour.spawnAnim == false);
        // increase currentID
        //Debug.LogWarning("Done");
        yield return new WaitForSeconds(timeBetweenNotifSpawn);
        currentID++;

        // Wait until end of lifetime OR clicked
        yield return new WaitUntil(() => notificationBehaviour.despawnAnim);

        // Destroy it
        if (go != null)     // <--- Enable when done with notif anim
            Destroy(go);

        
    }

    #endregion



    #region Click Behaviours

    // TODO

    // Here we'll describe each behaviour that can be put on the button.onClick
    // Depending on the type of notification, the behaviour will change
    // example: Quest notif opens quest log
    //          Room unlocked, transport you there if you're in the right location
    //          Character met opens the character log
    //          StatChanged opens the stat menu
    //          Item opens the inventory and the detail tab of that item

    #endregion
}


