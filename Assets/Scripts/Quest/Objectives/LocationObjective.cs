using UnityEngine;
using System;

namespace QuestSystem
{
    [CreateAssetMenu(fileName = "new LocationObjective", menuName = "LewdOwl/QuestSystem/Objective/LocationObjective")]
    [System.Serializable]
    public class LocationObjective : AbstractObjective
    {

        [Header("Properties")]
        public Room targetRoom;    //zone, 2d cord, 3d cord


        private Room currentRoom;

        public override void CheckProgress()
        {
            //if players location is equal to our target location then we are complete and we have finished objective


            if (currentRoom == targetRoom)
                isComplete = true;
            else
                isComplete = false;

            //Debug.Log(isComplete);
        }

        public override void UpdateProgress()
        {
            currentRoom = GameManager.instance.managers.navigationManager.currentRoom;
        }
    }
}
