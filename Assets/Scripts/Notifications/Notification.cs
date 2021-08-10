using QuestSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotificationSystem
{
    [System.Serializable]
    public abstract class AbstractNotification
    {
        public NotificationType notificationType = NotificationType.Blanck;
        public string title = "";
        public string stringValue, verb = "";
        public int intValue;

        public NotificationType GetNotificationType()
        {
            return notificationType;
        }
    }
    [System.Serializable]
    public class BlanckNotification : AbstractNotification
    {

        public BlanckNotification()
        {
            notificationType = NotificationType.Blanck;
        }
        public BlanckNotification(NotificationReward notificationReward)
        {
            title = notificationReward.title;
            notificationType = notificationReward.notificationType;
            stringValue = notificationReward.StringValue;
            verb = notificationReward.Verb;
            intValue = notificationReward.IntValue;
        }
    }
    [System.Serializable]
    public class StatChangedNotification : AbstractNotification
    {

        public StatChangedNotification()
        {
            notificationType = NotificationType.StatChanged;
        }
        public StatChangedNotification(NotificationReward notificationReward)
        {
            notificationType = notificationReward.notificationType;
            stringValue = notificationReward.StringValue;
            verb = notificationReward.Verb;
            intValue = notificationReward.IntValue;
        }
    }
    [System.Serializable]
    public class LocationUnlockedNotification : AbstractNotification
    {

        public LocationUnlockedNotification()
        {
            notificationType = NotificationType.LocationUnlocked;
        }
        public LocationUnlockedNotification(NotificationReward notificationReward)
        {
            notificationType = notificationReward.notificationType;
            stringValue = notificationReward.StringValue;
            verb = notificationReward.Verb;
            intValue = notificationReward.IntValue;
        }
    }
    [System.Serializable]
    public class CharacterMetNotification : AbstractNotification
    {

        public CharacterMetNotification()
        {
            notificationType = NotificationType.CharacterMet;
        }
        public CharacterMetNotification(NotificationReward notificationReward)
        {
            notificationType = notificationReward.notificationType;
            stringValue = notificationReward.StringValue;
            verb = notificationReward.Verb;
            intValue = notificationReward.IntValue;
        }
    }
    [System.Serializable]
    public class NewQuestNotification : AbstractNotification
    {

        public NewQuestNotification()
        {
            notificationType = NotificationType.CharacterMet;
        }
        public NewQuestNotification(NotificationReward notificationReward)
        {
            notificationType = notificationReward.notificationType;
            stringValue = notificationReward.StringValue;
            verb = notificationReward.Verb;
            intValue = notificationReward.IntValue;
        }
    }

    [System.Serializable]
    public class NewObjectiveNotification : AbstractNotification
    {

        public NewObjectiveNotification()
        {
            notificationType = NotificationType.CharacterMet;
        }
        public NewObjectiveNotification(NotificationReward notificationReward)
        {
            notificationType = notificationReward.notificationType;
            stringValue = notificationReward.StringValue;
            verb = notificationReward.Verb;
            intValue = notificationReward.IntValue;
        }
    }


}