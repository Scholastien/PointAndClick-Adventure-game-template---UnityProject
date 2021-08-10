using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InventorySystem;

public class InventoryManager : MonoBehaviour
{
    public bool Debug_InventoryLog;


    private ItemCollection itemCollection;

    public bool usePickUpSound; 

    public Sound pickUpSound;
    public DisplayedInventory displayedInventory;

    public List<Item> inventory;



    #region MonoBehaviour
    // Use this for initialization
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.gameState == GameState.MainMenu)
        {
            inventory = new List<Item>();
        }
    }
    #endregion

    #region Loader
    public void Init()
    {
        itemCollection = GameManager.instance.managers.variablesManager.itemCollection;
        ReloadInventory();
    }

    public void ReloadInventory()
    {
        inventory = itemCollection.DeserializeInventory(GameManager.instance.managers.variablesManager.gameSavedData.inventory);

        displayedInventory = new DisplayedInventory(inventory);
    }
    #endregion

    #region Shopping

    public void SellItem(Item itemToSell, int quantity)
    {
        int i = 0;
        foreach (Item item in inventory)
        {
            if (item == itemToSell && i < quantity)
            {
                inventory.Remove(item);
                i++;
            }
        }
        GameManager.instance.managers.variablesManager.gameSavedData.GameInfo.mainCharacter.money += itemToSell.sellPrice;
        GameManager.instance.managers.variablesManager.gameSavedData.inventory = itemCollection.SerializeInventory(inventory);
    }


    public bool BuyItem(Item itemToBuy)
    {
        int current_money = GameManager.instance.managers.variablesManager.gameSavedData.GameInfo.mainCharacter.money;

        if (current_money - itemToBuy.price >= 0)
        {
            GameManager.instance.managers.variablesManager.gameSavedData.GameInfo.mainCharacter.money = current_money - itemToBuy.price;
            PickItem(itemToBuy);
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion

    #region ItemManipulation


    public void PickItem(Item item)
    {
        inventory.Add(item);
        CreateSoundFeedback();
        GameManager.instance.managers.variablesManager.gameSavedData.inventory = itemCollection.SerializeInventory(inventory);
    }
    public void RemoveItem(Item item)
    {
        inventory.Remove(item);

        GameManager.instance.managers.variablesManager.gameSavedData.inventory = itemCollection.SerializeInventory(inventory);
    }

    public int ItemOccurence(Item item)
    {
        int result = 0;
        foreach (Item i in inventory)
        {
            if (i == item)
                result++;
        }
        return result;
    }


    public bool isObtained(Item item)
    {
        foreach(Item _item in inventory)
        {
            if(item == _item)
            {
                return true;
            }
        }

        return false;
    }

    #endregion

    public List<Item> DisplayedItem()
    {
        List<Item> displayedItems = new List<Item>();

        displayedInventory = new DisplayedInventory(inventory);

        foreach (Item item in inventory)
        {
            if (!item.hiddenInInventory)
            {
                displayedItems.Add(item);
            }
        }

        return displayedItems;
    }

    public void CreateSoundFeedback()
    {
        StartCoroutine(WaitEndSoundSourcePlay());

    }

    IEnumerator WaitEndSoundSourcePlay()
    {
        if (Debug_InventoryLog)
            Debug.Log("Play item pickup sound");

        if (usePickUpSound)
        {
            GameObject go = new GameObject(pickUpSound.audioType + "_" + pickUpSound.clipName);
            go.transform.SetParent(this.transform);
            pickUpSound.SetSourceAndOutput(go.AddComponent<AudioSource>(), AudioManager.instance.audioOutputMixer.OutputSelector(pickUpSound.audioType));
            pickUpSound.Play();

            yield return new WaitForSeconds(pickUpSound.clip.length);

            //Go to next scene
            Destroy(go);
        }
    }
}

[System.Serializable]
public class DisplayedInventory
{
    public List<DisplayedItem> display;

    public DisplayedInventory(List<Item> inventory)
    {
        display = new List<DisplayedItem>();
        foreach (Item item in inventory)
        {
            // if the item doesn't exist in the current display, add it to the list
            if (!SearchIfItemExist(item) && display != null)
            {
                display.Add(new DisplayedItem(item, ItemOccurence(item, inventory)));
            }
            // else do nothing;
        }
    }


    public bool SearchIfItemExist(Item item)
    {
        foreach (DisplayedItem displayedItem in display)
        {
            if (item == displayedItem.item)
            {
                return true;
            }
        }
        return false;
    }

    public int ItemOccurence(Item item, List<Item> inventory)
    {
        int result = 0;
        foreach (Item i in inventory)
        {
            if (i == item)
                result++;
        }
        return result;
    }
}

[System.Serializable]
public class DisplayedItem
{
    public Item item;
    public int count;

    public DisplayedItem(Item _item, int _count)
    {
        item = _item;
        count = _count;
    }
}