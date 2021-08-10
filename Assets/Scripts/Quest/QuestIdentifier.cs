using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace QuestSystem
{
    [System.Serializable]
    public class QuestIdentifier : IQuestIdentifier
    {

        [Tooltip("Unique ID")]
        public int id;
        [Tooltip("Size: How many quest(s) this quest will activate when completed \nElements: The identifier.id of those quest(s)")]
        public List<int> chainQuestID;
        [Tooltip("The quest identifier.id that activated this quest")]
        public int sourceID;


        // linking quest directly
        private List<Quest> chainQuest;
        private Quest source;

        #region Getter/Setter
        public List<int> ChainQuestID {
            get
            {
                return chainQuestID;
            }
            set
            {
                chainQuestID = value;
            }
        }

        public int ID {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        public int SourceID {
            get
            {
                return sourceID;
            }
            set
            {
                sourceID = value;
            }
        }

        #endregion

        #region QuestManipulation

        // Link the list to a real scriptable object
        public void LinkChainQuestID()
        {
            chainQuestID = new List<int>();
            foreach (Quest quest in chainQuest)
            {
                chainQuestID.Add(quest.identifier.id);
            }
        }

        public void LinkSourceID()
        {
            sourceID = source.identifier.id;
        }

        #endregion
    }
}
