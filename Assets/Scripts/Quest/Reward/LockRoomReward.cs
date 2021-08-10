using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace QuestSystem
{
    [CreateAssetMenu(fileName = "new LockRoomReward", menuName = "LewdOwl/QuestSystem/Reward/LockRoomReward")]
    public class LockRoomReward : AbstractReward
    {
        public List<Room> roomsToLock;

        public override void RunReward()
        {
            if(GameManager.instance.managers.questManager.debug_QuestLog)
                Debug.Log("<color=red> Lock Room </color>");
            foreach (Room room in roomsToLock)
            {
                room.locked = true;

                
            }

            GameManager.instance.StartCoroutine(WaitUntilReady());
        }

        public IEnumerator WaitUntilReady()
        {
            yield return new WaitUntil(() => NavigationController.instance != null);
            NavigationController.instance.UpdateLocation();
            given = true;
        }
    }
}