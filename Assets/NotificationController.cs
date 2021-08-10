using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotificationSystem;

public class NotificationController : MonoBehaviour
{
    // Singleton
    public static NotificationController instance = null;

    public List<GameObject> notifications;



    #region MonoBehaviour
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            NotificationManager.instance.notificationController = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        notifications = new List<GameObject>();
    }



    #endregion

    #region PublicFunction

    public GameObject SpawnNotification(AbstractNotification notification)
    {
        // Reset lifetime and clicked value
        //NotificationManager.instance.notificationLiving = true;
        //NotificationManager.instance.notificationSkipped = false;

        GameObject go = Instantiate(NotificationManager.instance.notificationPrefab, transform);
        notifications.Add(go);
        go.transform.SetAsFirstSibling();
        NotificationBehaviour notificationBehaviour = go.GetComponent<NotificationBehaviour>();

        //Debug.LogWarning("<color=yellow>" + notification.stringValue + " " + go.name + " " + go.transform.parent + "</color>");

        ChangeNotificationText(notification, notificationBehaviour);

        // Switch for the notification type
        switch (notification.GetNotificationType())
        {
            case NotificationType.Blanck:
                // Do Nothing
                break;
            default:
                StartCoroutine(StartNotificationLifeCycle(notificationBehaviour));
                break;
        }
        return go;
    }

    #endregion

    #region Coroutine

    public IEnumerator StartNotificationLifeCycle(NotificationBehaviour notificationBehaviour)
    {

        StartCoroutine(PlaySlideInAnim(notificationBehaviour));

        // wait until the spawn anim is done
        yield return new WaitUntil(() => notificationBehaviour.spawnAnim == false);

        // start the alive timer
        for (float i = 0; i < NotificationManager.instance.lifeTime; i += Time.deltaTime)
        {

            yield return null;

            // if notif is clicked => break the alive timer
            if (notificationBehaviour.clicked)
            {
                break;
            }
        }

        // If notif clicked => click behaviour
        if (notificationBehaviour.clicked)
        {
            // TODO : click behaviour
            StartCoroutine(DepopOnClick(notificationBehaviour));
        }
        // Else (When alive timer reach its end) => depop notif from the screen
        else
        {
            // when it's dead => despawn anim
            StartCoroutine(PlaySlideOutAnim(notificationBehaviour));
        }


    }

    // Animate the Notification

    public IEnumerator PlaySlideInAnim(NotificationBehaviour notificationBehaviour)
    {
        float height = 0f;
        float scaleY = 0f;
        float spawnAnimSpeed = NotificationManager.instance.spawnAnimSpeed;
        RectTransform rt = notificationBehaviour.GetComponent<RectTransform>();

        //set height and Y scale to 0
        rt.sizeDelta = new Vector2(notificationBehaviour.width, 0);
        rt.localScale = new Vector3(1, 0, 1);


        for (float i = 0; i < spawnAnimSpeed; i += Time.deltaTime)
        {
            // increase gradually height and Y scale to the max value

            // height = max height x i / spawnAnimSpeed
            height = notificationBehaviour.height * i / spawnAnimSpeed;
            rt.sizeDelta = new Vector2(notificationBehaviour.width, height);

            // scale Y =  i / spawnAnimSpeed
            scaleY = i / spawnAnimSpeed;
            rt.localScale = new Vector3(1, scaleY, 1);

            notificationBehaviour.childPanel.localScale = new Vector3(scaleY, scaleY, 1);


            yield return null;
        }
        rt.localScale = new Vector3(1, 1, 1);
        notificationBehaviour.childPanel.localScale = new Vector3(1, 1, 1);

        notificationBehaviour.spawnAnim = false;
    }

    public IEnumerator PlaySlideOutAnim(NotificationBehaviour notificationBehaviour)
    {
        float height = 0f;
        float scaleY = 0f;
        float despawnAnimSpeed = NotificationManager.instance.despawnAnimSpeed;
        RectTransform rt = notificationBehaviour.GetComponent<RectTransform>();

        //set height to notificationBehaviour.height and Y scale to 1
        rt.sizeDelta = new Vector2(notificationBehaviour.width, notificationBehaviour.height);
        rt.localScale = new Vector3(1, 1, 1);

        for (float i = 0; i < despawnAnimSpeed; i += Time.deltaTime)
        {
            // decrease gradually height and Y scale

            // height = max height - ( max height x i / spawnAnimSpeed)
            height = notificationBehaviour.height - (notificationBehaviour.height * i / despawnAnimSpeed);
            rt.sizeDelta = new Vector2(notificationBehaviour.width, height);
            // scale Y = max height - ( i / spawnAnimSpeed)
            scaleY = 1 - (i / despawnAnimSpeed);
            rt.localScale = new Vector3(1, scaleY, 1);

            notificationBehaviour.childPanel.localScale = new Vector3(scaleY, scaleY, 1);

            yield return null;
        }

        rt.localScale = new Vector3(1, 0, 1);
        notificationBehaviour.childPanel.localScale = new Vector3(0, 0, 1);

        notificationBehaviour.despawnAnim = true;
    }

    public IEnumerator DepopOnClick(NotificationBehaviour notificationBehaviour)
    {
        float height = 0f;
        float scaleY = 0f;
        float despawnAnimSpeed = NotificationManager.instance.despawnAnimSpeed;
        RectTransform rt = notificationBehaviour.GetComponent<RectTransform>();

        for (float i = 0; i < despawnAnimSpeed; i += Time.deltaTime)
        {
            // decrease gradually width and Y,X scale

            // height = max height - ( max height x i / spawnAnimSpeed)
            height = notificationBehaviour.height - (notificationBehaviour.height * i / despawnAnimSpeed);
            rt.sizeDelta = new Vector2(notificationBehaviour.width, height);
            // scale Y = max height - ( i / spawnAnimSpeed)
            scaleY = 1 - (i / despawnAnimSpeed);
            rt.localScale = new Vector3(scaleY, scaleY, 1);

            notificationBehaviour.childPanel.localScale = new Vector3(scaleY, scaleY, 1);

            yield return null;
        }

        rt.localScale = new Vector3(0, 0, 1);
        notificationBehaviour.childPanel.localScale = new Vector3(0, 0, 1);

        notificationBehaviour.despawnAnim = true;
    }

    #endregion

    #region Internal Process

    // Replace the Text values in the current notification

    private void ChangeNotificationText(AbstractNotification notification, NotificationBehaviour notificationBehaviour)
    {
        string title = LanguageManager.instance.GetTranslation(notification.title);
        string stringValue = LanguageManager.instance.GetTranslation(notification.stringValue);
        string verb = LanguageManager.instance.GetTranslation(notification.verb);


        notificationBehaviour.title.text = title;
        switch (notification.GetNotificationType())
        {
            case NotificationType.Blanck:
                break;
            case NotificationType.StatChanged:
                notificationBehaviour.title.text = "Stat";
                notificationBehaviour.value.text = stringValue + notification.intValue + " ";
                notificationBehaviour.verb.text = verb;
                break;
            case NotificationType.LocationUnlocked:
                notificationBehaviour.title.text = "Location";
                notificationBehaviour.value.text = stringValue + " ";
                notificationBehaviour.verb.text = verb;
                break;
            case NotificationType.CharacterMet:
                notificationBehaviour.title.text = "Character met";
                notificationBehaviour.value.text = stringValue;
                notificationBehaviour.verb.text = "";
                break;
            case NotificationType.NewQuest:
                notificationBehaviour.title.text = "New quest";
                notificationBehaviour.value.text = stringValue;
                notificationBehaviour.verb.text = "";
                break;
            case NotificationType.NewObjective:
                notificationBehaviour.title.text = "New objective";
                notificationBehaviour.value.text = stringValue;
                notificationBehaviour.verb.text = "";
                break;
            case NotificationType.Item:
                notificationBehaviour.title.text = "Item";
                notificationBehaviour.value.text = stringValue + " ";
                notificationBehaviour.verb.text = verb;
                break;
            default:
                break;
        }
    }
    

    

    #endregion
}
