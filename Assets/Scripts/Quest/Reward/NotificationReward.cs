using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotificationSystem;

namespace QuestSystem
{
    // See if it's better to use it as a reward or including notifications on existing reward (might get too big)
    [CreateAssetMenu(fileName = "new NotificationReward", menuName = "LewdOwl/QuestSystem/Reward/Notification")]
    public class NotificationReward : AbstractReward
    {
        [Header("Title")]
        public NotificationType notificationType;

        public ItemState itemState;
        public StatSigned statSigned;

        [HideInInspector]
        public string title;

        [Header("Values")]
        public string StringValue;
        public int IntValue;

        [Header("Verb")]
        public string Verb;

        public LocationState locationState;


        public override void RunReward()
        {
            NotificationManager.instance.InvokeNotification(CreateEmbedNotification());
            given = true;
        }
        
        public AbstractNotification CreateEmbedNotification()
        {
            switch (notificationType)
            {
                case NotificationType.StatChanged:
                    title = "Stat";
                    return new StatChangedNotification(this);
                case NotificationType.LocationUnlocked:
                    title = "Location";
                    return new LocationUnlockedNotification(this);
                case NotificationType.CharacterMet:
                    title = "Character met";
                    return new CharacterMetNotification(this);
                default:
                    return new BlanckNotification(this);
            }
        }

    }
}

public enum NotificationType
{
    Blanck,
    StatChanged,
    LocationUnlocked,
    CharacterMet,
    NewQuest,
    NewObjective,
    Item
}

public enum LocationState
{
    locked, unlocked
}
public enum ItemState
{
    obtained, removed
}

public enum StatSigned
{
    none, plus, minus
}