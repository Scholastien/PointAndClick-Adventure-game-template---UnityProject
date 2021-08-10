using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InventorySystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemUIBehaviour : MouseoverBehaviour
{

    public Item item;

    public Image imagePreview;

    public Text item_occ;

    public GameObject DetailTab;

    public InventoryDisplayer inventoryDisplayer;

    // Use this for initialization
    protected override void Start()
    {

        base.Start();

        if (item != null)
        {

            displayName = item.label;


            imagePreview.sprite = item.itemIcon;

            InventoryManager im = GameManager.instance.managers.inventoryManager;

            item_occ.text =
                im.displayedInventory.ItemOccurence(item, im.inventory).ToString();

            if (im.displayedInventory.ItemOccurence(item, im.inventory) > 1 && !item.unique)
            {
                item_occ.transform.parent.gameObject.SetActive(true);
            }
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
        {
            EnterInteractible();
            showInfoBox = true;
        }


        // update the offset value

    }


    public void ClickDetails()
    {
        if (item == null)
        {
            CloseDetail();
        }
        else
        {
            OpenDetail();
        }
    }

    public void OpenDetail()
    {
        DetailTab.SetActive(true);
        StartCoroutine(OpenDetailTab());
        DetailTab.GetComponent<DetailItemUIBehaviour>().item = item;
        DetailTab.GetComponent<DetailItemUIBehaviour>().UpdateDetails(inventoryDisplayer);

    }
    public void CloseDetail()
    {
        StartCoroutine(CloseDetailTab());
    }

    public IEnumerator OpenDetailTab()
    {
        DetailTab.GetComponent<Transform>().localScale = new Vector3(1, 0.1f, 1);
        Debug.Log("IncreaseScaleSubmenu " + DetailTab.name);
        // go from 0.1f to 1f (Y pos)

        for (float i = 0.1f; i <= 1.1f; i += 0.2f)
        {
            yield return null;
            DetailTab.GetComponent<Transform>().localScale = new Vector3(i, i, 1);
        }
        DetailTab.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
    }

    public IEnumerator CloseDetailTab()
    {
        Debug.Log("ReduceScaleSubmenu " + DetailTab.name);
        // go from 1f to 0.1f (Y pos)
        for (float i = 1; i >= 0.0f; i -= 0.2f)
        {
            yield return null;
            DetailTab.GetComponent<Transform>().localScale = new Vector3(i, i, 1);
        }

        DetailTab.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);


        DetailTab.SetActive(false);
    }
}
