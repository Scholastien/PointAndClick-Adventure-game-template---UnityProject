using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

namespace QuestSystem
{
    public interface IQuestIdentifier
    {
        int ID { get; set ; }
        List<int> ChainQuestID { get; set; }
        int SourceID { get; set; }
    }
}
