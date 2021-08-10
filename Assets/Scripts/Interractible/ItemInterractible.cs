using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InventorySystem;

public class ItemInterractible : Interractible {

    [Header("Properties")]
    public Item item;
    public ChainTrigger trigger;

	// Use this for initialization
	new void Start () {
        base.Start();
        displayName = "Pick up " + item.label.ToLower();
	}

    // Update is called once per frame
    

    new void LateUpdate()
    {
        base.LateUpdate();
        hide = trigger.triggered;
    }

    public void PickItem()
    {
        if (!disable)
        {
            GameManager.instance.managers.inventoryManager.PickItem(item);
            trigger.triggered = true;
        }
    }
}
