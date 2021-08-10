using InventorySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button)), RequireComponent(typeof(MinigameInterractible))]
public class Shop : MonoBehaviour {


    public List<Item> itemsToSell;


    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(delegate {
            SendItemsListToMinigameManager();
        });
    }


    public void SendItemsListToMinigameManager()
    {
        ShopParameter shopParameter = MinigameManager.instance.shopParameter;

        shopParameter.ItemToDisplay = itemsToSell;

        NavigationManager nm = GameManager.instance.managers.navigationManager;
        TimeManager tm = GameManager.instance.managers.timeManager;

        shopParameter.Background = nm.currentRoom.give_DaytimeSprite(tm.currentTime);
    }
}
