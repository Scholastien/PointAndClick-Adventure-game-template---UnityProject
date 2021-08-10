using InventorySystem;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class PopulateContentList
{


    [Header("Object from manager")]
    public List<Item> temp_ListItem; // Created after verifying objectList is only composed of items
    public List<GameObject> instantiatedSlotList;

    [Header("Parameters")]
    public int rows;
    public int minimumItemToDisplay;
    public Sprite emptySlot;


    [Header("Scene reference")]
    public Transform Content; // we will also get the grid layout component from that transform
    public Image Background; // update with shop parameter from minigame manager


    #region MonoBehaviour
    // Use this for initialization
    public void Init()
    {
        temp_ListItem = RemoveAllNonTriggeredItem();
        // populate
        instantiatedSlotList = ListToInstanciate();

    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion
    


    #region Processor

    public List<Item> RemoveAllNonTriggeredItem()
    {
        //temp_ListItem
        List<Item> result = new List<Item>();

        foreach (Item item in temp_ListItem)
        {
            if ((item.unlockedWithTrigger && item.triggerToUnlock.triggered) || !item.unlockedWithTrigger)
            {
                result.Add(item);
            }
        }



        return result;
    }

    #endregion

    #region Spawner

    public GameObject CreateEmptyGameObject()
    {
        // GameObject + Image
        GameObject go = new GameObject();

        go.AddComponent<Image>().sprite = emptySlot;

        go.name = "Empty Slot";

        return go;
    }

    public GameObject CreateItemButtonGameObject(GameObject emptyGameObject, Item item)
    {


        GameObject item_preview = new GameObject();

        emptyGameObject.name = "Slot_" + item.name;
        item_preview.name = "Preview_" + item.name + "_btn";

        item_preview.transform.parent = emptyGameObject.transform;

        // modify item thumbnail
        Image itemImage = item_preview.AddComponent<Image>();
        itemImage.sprite = item.itemIcon;

        Button item_btn = item_preview.AddComponent<Button>();

        item_btn.onClick.AddListener(delegate

        {
            ShopManager.instance.previewWindow.ItemInItemSlot = itemImage;
        });


        // Add button component to the empty gameobject
        // modify the button behaviour to fit the item specs
        // if item is a unique item and not exist in current inventory :
        InventoryManager im = GameManager.instance.managers.inventoryManager;
        if ((item.unique && !im.isObtained(item)) || !item.unique)
        {

            Color color = new Color(1, 1, 1, 1);

            itemImage.color = color;

            item_btn.onClick.AddListener(delegate
            {
                Debug.Log("Button clicked " + item.label);
                AddItemPreviewButtonBehaviour(item);
            });
        }
        else
        {

            Color color = new Color(1, 1, 1, 0.2f);

            itemImage.color = color;

            item_btn.onClick.AddListener(delegate
            {
                UniqueItemAlreadyObtained();
            });

        }

        return emptyGameObject;
    }

    public List<GameObject> ListToInstanciate()
    {
        List<GameObject> itemsSlots = new List<GameObject>();
        // temp_ListItem
        if (temp_ListItem.Count <= minimumItemToDisplay)
        {
            // Generate itemslots until 12
            for (int i = 0; i < minimumItemToDisplay; i++)
            {
                itemsSlots.Add(CreateEmptyGameObject());
            }
        }
        else
        {
            // Generate all slot %rows
            Debug.Log(temp_ListItem.Count + (rows - (temp_ListItem.Count % rows)));

            int maxToDisplay = (int)(Math.Ceiling((decimal)temp_ListItem.Count / (decimal)rows) * (decimal)rows);

            for (int i = 0; i < maxToDisplay; i++)
            {
                itemsSlots.Add(CreateEmptyGameObject());
            }
        }


        for (int i = 0; i < temp_ListItem.Count; i++)
        {
            CreateItemButtonGameObject(itemsSlots[i], temp_ListItem[i]);
        }



        foreach (GameObject go in itemsSlots)
        {
            //go.transform.parent = Content;
            go.transform.SetParent(Content);
        }


        return itemsSlots;
    }

    #endregion


    #region ButtonFonction

    //ShopManager.instance.previewWindow.OnSelectedItem(item);
    public void AddItemPreviewButtonBehaviour(Item item)
    {
        // Change the text button to "Buy"
        ShopManager.instance.previewWindow.ResetBuyButton();
        ShopManager.instance.previewWindow.OnSelectedItem(item);
    }


    public void UniqueItemAlreadyObtained()
    {
        // Change text button to "-"


    }

    #endregion


}

[System.Serializable]
public class PreviewWindow
{

    public Image ItemInItemSlot;

    public bool hadBought = false;

    public Image preview_img;
    public Text preview_txt;
    public Text currentCash_txt;
    public Button buy_btn;



    public void OnSelectedItem(Item item)
    {
        preview_img.gameObject.SetActive(true);
        preview_txt.gameObject.SetActive(true);
        buy_btn.gameObject.SetActive(true);

        hadBought = false;
        preview_img.sprite = item.itemIcon;
        preview_txt.text = item.description;

        SetMoneyDisplay();

        InventoryManager im = GameManager.instance.managers.inventoryManager;

        if ((item.unique && im.isObtained(item)))
        {
            ResetBuyButton();
            buy_btn.GetComponentInChildren<Text>().text = "-";
        }
        else
        {

            buy_btn.onClick.AddListener(delegate
            {

                ResetBuyButton();

                // Buy function

                if (GameManager.instance != null)
                {
                    Debug.Log("Buy " + item.name);
                    hadBought = GameManager.instance.managers.inventoryManager.BuyItem(item);

                    // Coroutine from Shop Manager to execute code, depending on has bought (shake window, ...)


                    // Change unique item transparency
                    if (item.unique)
                    {
                        Color color = new Color(1, 1, 1, 0.2f);
                        ItemInItemSlot.color = color;
                    }
                    // reset the item buy button
                    OnSelectedItem(item);
                    //ResetBuyButton();

                }

            });
        }
    }


    public void SetMoneyDisplay()
    {
        if (GameManager.instance != null)
        {
            Debug.Log(GameManager.instance.managers.variablesManager.gameSavedData.GameInfo.mainCharacter.money.ToString());
            currentCash_txt.text = GameManager.instance.managers.variablesManager.gameSavedData.GameInfo.mainCharacter.money.ToString();
        }
    }

    public void ResetBuyButton()
    {
        if (buy_btn != null)
            buy_btn.onClick.RemoveAllListeners();
    }
}

