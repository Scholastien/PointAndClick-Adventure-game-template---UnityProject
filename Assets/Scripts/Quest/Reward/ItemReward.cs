
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InventorySystem;

namespace QuestSystem
{
    [CreateAssetMenu(fileName = "new ItemReward", menuName = "LewdOwl/QuestSystem/Reward/ItemReward")]
    public class ItemReward : AbstractReward
    {
        public Item item;

        public override void RunReward()
        {
            GameManager.instance.managers.inventoryManager.PickItem(item);
            given = true;
        }
    }
}