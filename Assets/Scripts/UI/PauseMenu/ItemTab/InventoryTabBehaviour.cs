using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InventorySystem;

public class InventoryTabBehaviour : MonoBehaviour
{

    [Header("Script Reference")]
    public Sub_MenuBehaviour menuBehaviour;

    [Header("Data Manipulation")]
    public DisplayedInventory Manager_displayedInventory;


    public List<Item> inventory;

    public DisplayedInventory displayedData;

    public Transform scrollableList;

    public GameObject ItemSlotPrebab;

    public GameObject ItemDetailsPanel;

    public int currentSize;

    public List<GameObject> inventorySlots;

    public List<GameObject> displayedInventory;

    // Use this for initialization
    void Start()
    {
        inventory = GameManager.instance.managers.inventoryManager.DisplayedItem();
        displayedData = GameManager.instance.managers.inventoryManager.displayedInventory;
        Manager_displayedInventory = GameManager.instance.managers.inventoryManager.displayedInventory;
        //CreateDisplayedInventory();
    }




    public void UpdateInventory(InventoryDisplayer inventoryDisplayer)
    {
        Manager_displayedInventory = GameManager.instance.managers.inventoryManager.displayedInventory;
        if (displayedData.display != Manager_displayedInventory.display)
        {
            Debug.LogWarning("DISPLAY NEW ELEMENT");


            DestroyDisplayedInventory();

            inventory = GameManager.instance.managers.inventoryManager.DisplayedItem();
            inventorySlots = new List<GameObject>();

            displayedData = Manager_displayedInventory;
            CreateDisplayedInventory(inventoryDisplayer);
            //inventory = GameManager.instance.managers.inventoryManager.DisplayedItem();
        }

    }

    public void CreateDisplayedInventory(InventoryDisplayer inventoryDisplayer)
    {
        ManageCurrentSize();
        for (int i = 0; i < currentSize; i++)
        {
            GameObject itemSlot = Instantiate(ItemSlotPrebab, scrollableList);
            inventorySlots.Add(itemSlot);
            itemSlot.GetComponent<ItemUIBehaviour>().DetailTab = ItemDetailsPanel;
        }

        for (int i = 0; i < displayedData.display.Count; i++)
        {
            inventorySlots[i].GetComponent<ItemUIBehaviour>().item = displayedData.display[i].item;
            inventorySlots[i].GetComponent<ItemUIBehaviour>().inventoryDisplayer = inventoryDisplayer;

        }
    }

    public void ManageCurrentSize()
    {
        if (inventory.Count < 15)
        {
            currentSize = 15;
        }
        else
        {
            currentSize = inventory.Count;
            MakeSomeEmptyInventorySlot();
        }
    }

    public void MakeSomeEmptyInventorySlot()
    {
        if (currentSize % 5 != 0)
        {
            currentSize++;
            MakeSomeEmptyInventorySlot();
        }
    }

    public void DestroyDisplayedInventory()
    {
        foreach (GameObject go in inventorySlots)
        {
            Destroy(go);
        }
    }
}
