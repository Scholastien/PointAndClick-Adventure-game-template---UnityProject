using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace QuestSystem
{
    [CreateAssetMenu(fileName = "new TeleportationReward", menuName = "LewdOwl/QuestSystem/Reward/TeleportationReward")]
    public class TeleportationReward : AbstractReward
    {
        public Room room;

        public override void RunReward()
        {
            NavigationManager.instance.SetRoom(room.name);
            //EventManager.Instance.NeedFadeIn();
            given = true;
        }
    }
}
