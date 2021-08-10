using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem {
    public abstract class AbstractObjective : ScriptableObject, IQuestObjective {

        [Header("Display Settings")]
        public string title;
        public string description;

        [Header("State")]
        public bool isComplete;
        public bool isBonus;
        public virtual string Description {
            get
            {
                return description;
            }
        }

        public virtual bool IsBonus {
            get
            {
                return isBonus;
            }
        }

        public virtual bool IsComplete {
            get
            {
                return isComplete;
            }
        }

        public virtual string Title {
            get
            {
                return title;
            }
        }

        public virtual void CheckProgress() {
        }

        public virtual void UpdateProgress() {
        }
        
    }
}
