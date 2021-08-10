using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace QuestSystem
{
    [CreateAssetMenu(fileName = "new UnlockRoomReward", menuName = "LewdOwl/QuestSystem/Reward/UnlockRoomReward")]
    public class UnlockRoomReward : AbstractReward
    {
        public List<Room> roomsToUnlock;

        public override void RunReward()
        {
            Debug.Log("<color=red> Unlock Room </color>");
            foreach (Room room in roomsToUnlock)
            {
                room.locked = false;
            }
            NavigationManager.instance.needToOpenTheNavBar = true;
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