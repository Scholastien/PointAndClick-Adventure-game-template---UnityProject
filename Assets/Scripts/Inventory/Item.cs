using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem {

    [CreateAssetMenu(fileName = "new Item", menuName = "LewdOwl/Inventory/Item")]
    [System.Serializable]
    public class Item : ScriptableObject {

        [Header("Display Settings")]
        public bool hiddenInInventory;
        public string label;
        public Sprite itemIcon;
        [TextArea]
        public string description;

        [Header("Properties")]
        public bool unique = true;
        public ItemCategory itemCategory;
        public bool unlockedWithTrigger = false;
        public ChainTrigger triggerToUnlock;

        [Header("Shop Parameter")]
        public int price;
        public int sellPrice;

        public Item() {
            label = "new Item";
            price = 5;
            sellPrice = price / 2;
        }

    }

    public enum ItemCategory
    {
        books,
        hardware,
        fashion,
        cosmetic,
        upgrades,
        sexshop 
    }
}