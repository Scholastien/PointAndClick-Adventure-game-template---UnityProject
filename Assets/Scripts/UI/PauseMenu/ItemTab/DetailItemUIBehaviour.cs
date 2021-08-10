using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InventorySystem;
using TMPro;
using UnityEngine.EventSystems;

public class DetailItemUIBehaviour : ClosableWindowBehaviour
{

    public Item item;

    public Image imagePreview;

    public Text itemName;

    public Text itemDesc;

    public TextMeshProUGUI itemNameTMP;

    public TextMeshProUGUI itemDescTMP;


    public InventoryDisplayer inventoryDisplayer;


    public override void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!inside && !inventoryDisplayer.inside)
            {
                EventManager.instance.ClosingAllWindow();
            }
        }
    }

    public void UpdateDetails(InventoryDisplayer _inventoryDisplayer)
    {
        imagePreview.sprite = item.itemIcon;
        inventoryDisplayer = _inventoryDisplayer;
        if (itemName != null && itemDesc != null)
        {
            itemName.text = LanguageManager.instance.GetTranslation(item.label);
            itemDesc.text = LanguageManager.instance.GetTranslation(item.description);
        }

        if (itemNameTMP != null && itemDescTMP != null)
        {
            itemNameTMP.text = LanguageManager.instance.GetTranslation(item.label);
            itemDescTMP.text = LanguageManager.instance.GetTranslation(item.description);
        }
    }
}
