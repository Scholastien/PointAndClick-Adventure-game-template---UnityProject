using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ScheduleSystem
{
    [CreateAssetMenu(fileName = "new ScheduleChangesCollection", menuName = "LewdOwl/ScheduleSystem/ScheduleChangesCollection")]
    public class ScheduleChangesCollection : ScriptableObject
    {
        public List<ScheduledActivity> allScheduleChanges;


        // Search and copy every object here
        public void Init()
        {
            // ScheduledActivitys are stored in my Resource folder (Resources/ScriptableObjects/Inventory)
            ScheduledActivity[] ScheduledActivitys_gameFolder = Resources.LoadAll<ScheduledActivity>("ScriptableObjects/ScheduleSystem");

            // This message should display 4 if all my ScheduledActivitys are in the folder
            //Debug.Log("Number of ScheduledActivity in the game folder : " + ScheduledActivitys_gameFolder.Length);

            List<ScheduledActivity> ScheduledActivityToRemove = new List<ScheduledActivity>();

            // Store inexisting ScheduledActivitys
            for (int i = 0; i < allScheduleChanges.Count; i++)
            {
                if (!ScheduledActivitys_gameFolder.Contains(allScheduleChanges[i]))
                {
                    ScheduledActivityToRemove.Add(allScheduleChanges[i]);
                }
            }

            // remove inexisting ScheduledActivitys
            for (int i = 0; i < ScheduledActivityToRemove.Count; i++)
            {
                allScheduleChanges.Remove(ScheduledActivityToRemove[i]);
            }
            // Foreach ScheduledActivity found, check if the ScheduledActivity exist in the ScheduledActivitys collection
            // If it doesnt exist, add that ScheduledActivity to the collection
            for (int i = 0; i < ScheduledActivitys_gameFolder.Length; i++)
            {
                if (!allScheduleChanges.Contains(ScheduledActivitys_gameFolder[i]))
                {
                    allScheduleChanges.Add(ScheduledActivitys_gameFolder[i]);
                }
            }

        }


        public int SearchID(ScheduledActivity scheduledActivity)
        {
            for (int i = 0; i < allScheduleChanges.Count; i++)
            {
                if(allScheduleChanges[i] == scheduledActivity)
                {
                    return i;
                }
            }
            return 0;
        }

        public List<int> SerializableList(List<SerializableScheduledActivity> listToSer)
        {
            List<int> sl = new List<int>();

            foreach(SerializableScheduledActivity serializedScheduledActivity in listToSer)
            {
                sl.Add(SearchID(serializedScheduledActivity.scheduledActivity));
            }

            return sl;
        } 

        //Search scheduled activity with index
        public ScheduledActivity SearchScheduledActivity(int index)
        {
            //Debug.Log("SearchScheduledActivity at " + index);
            return allScheduleChanges[index];
        }

        public List<ScheduledActivity> DeserializeList(List<int> indexList)
        {
            List<ScheduledActivity> saL = new List<ScheduledActivity>();

            foreach(int index in indexList)
            {
                saL.Add(SearchScheduledActivity(index));
            }

            return saL;
        }


    }
}