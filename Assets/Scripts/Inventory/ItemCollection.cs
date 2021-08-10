using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InventorySystem;
using System.Linq;

[CreateAssetMenu(fileName = "new ItemCollection", menuName = "LewdOwl/Inventory/ItemCollection")]
public class ItemCollection : ScriptableObject {

    [Tooltip("All items in the game")]
    public List<Item> items;

    // Search and copy every object here
    public void Init()
    {
        // Items are stored in my Resource folder (Resources/ScriptableObjects/Inventory)
        Item[] items_gameFolder = Resources.LoadAll<Item>("ScriptableObjects/Inventory");

        // This message should display 4 if all my items are in the folder
        //Debug.Log("Number of item in the game folder : " + items_gameFolder.Length);

        List<Item> ItemToRemove = new List<Item>();

        // Store inexisting items
        for (int i = 0; i < items.Count; i++)
        {
            if (!items_gameFolder.Contains(items[i]))
            {
                ItemToRemove.Add(items[i]);
            }
        }

        // remove inexisting items
        for (int i = 0; i < ItemToRemove.Count; i++)
        {
            items.Remove(ItemToRemove[i]);
        }
        // Foreach item found, check if the item exist in the items collection
        // If it doesnt exist, add that item to the collection
        for (int i = 0; i < items_gameFolder.Length; i++)
        {
            if (!items.Contains(items_gameFolder[i]))
            {
                items.Add(items_gameFolder[i]);
            }
        }

    }

    public List<string> SerializeInventory (List<Item> inventory) {
        List<string> inventorySerialized = new List<string>();

        foreach(Item item in inventory) {
            inventorySerialized.Add(item.label);
        }

        return inventorySerialized;
    }
    
    public List<Item> DeserializeInventory(List<string> inventorySerialized) {
        List<Item> inventory = new List<Item>();
        if (inventorySerialized != null) {
            foreach (string itemName in inventorySerialized) {
                inventory.Add(GetItemWithName(itemName));
            }
        }
        return inventory;
    }

    public Item GetItemWithName(string name) {
        foreach(Item item in items) {
            if(item.label == name) {
                return item;
            }
        }
        return items[0];
    }
    
}
