using UnityEngine;
using System.Collections;
using System;
using InventorySystem;

namespace QuestSystem
{
    [CreateAssetMenu(fileName = "new collectionobjective", menuName = "LewdOwl/QuestSystem/Objective/CollectionObjective")]
    [System.Serializable]
    public class CollectionObjective : AbstractObjective
    {
        [Header("Properties")]
        public string verb;
        public int collectionAmount;   //total amount of whatever we need
        public int currentAmount; //starts at 0
        public Item itemToCollect;

        //collect 10 meat
        /// <summary>
        /// This constructor builds a collection objective.
        /// </summary>
        /// <param name="titleVerb">Describes the type of collection.</param>
        /// <param name="totalAmount">Amount required to complete objective.</param>
        /// <param name="item">Item to be collected.</param>
        /// <param name="descrip">Describe what will be collected.</param>
        /// <param name="bonus">Is this a bonus objective?</param>
        public CollectionObjective(string titleVerb, int totalAmount, Item item, string descrip, bool bonus)
        {
            title = titleVerb + " " + totalAmount + " " + item.name;
            verb = titleVerb;
            description = descrip;
            itemToCollect = item;
            collectionAmount = totalAmount;
            currentAmount = 0;
            isBonus = bonus;
            CheckProgress();
        }

        public int CollectionAmount
        {
            get
            {
                return collectionAmount;
            }
        }

        public int CurrentAmount
        {
            get
            {
                return currentAmount;
            }
        }

        public Item ItemToCollect
        {
            get
            {
                return itemToCollect;
            }
        }

        public override void UpdateProgress()
        {
            currentAmount = GameManager.instance.managers.inventoryManager.ItemOccurence(itemToCollect);
        }

        public override void CheckProgress()
        {
            if (currentAmount >= collectionAmount)
            {
                isComplete = true;
            }
            else
            {
                isComplete = false;
            }
            
        }

        // 0/10 meat gathered
        public override string ToString()
        {
            return currentAmount + "/" + collectionAmount + " " + itemToCollect.name + " " + verb + "ed!";
        }

    }
}


//location is a specific point in the world
