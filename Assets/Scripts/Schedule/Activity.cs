using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace ScheduleSystem
{
    [System.Serializable]
    public class SerializableActivity
    {
        [HideInInspector]
        public string name;
        [Tooltip("If more than one, pick one random")]
        public Activity activity;
    }

    [System.Serializable, CreateAssetMenu(fileName = "new Activity", menuName = "LewdOwl/ScheduleSystem/Activity")]
    public class Activity : ScriptableObject
    {
        [Header("Display Settings")]
        public string display_name;

        public Room room;

        [Tooltip("If more than one, pick one random")]
        public List<Sprite> interractibleImg;

        [Header("Identifier")]
        public CharacterFunction characterName;

        [Tooltip("Keep it to none if there's no")]
        public ScheduledActivity nextActivity;
        public bool overwriteActivityWithNewOne;

        [Header("Properties")]
        [Tooltip("Keep to none if no dialogue is needed")]
        public DialogueChain dialogueToStart;
        [Tooltip("Should this activity be played only once")]
        public bool playOnlyOnce;

        public Sprite GetSprite (){
            if (interractibleImg.Count == 1)
            {
                return interractibleImg[0];
            }
            else if(interractibleImg.Count > 1)
            {
                return interractibleImg[(int)Random.Range(0, interractibleImg.Count)];
            }
            else
            {
                Debug.Log("Sprite missing in " + this.display_name);
                return null;
            }
        }

    }


    
}