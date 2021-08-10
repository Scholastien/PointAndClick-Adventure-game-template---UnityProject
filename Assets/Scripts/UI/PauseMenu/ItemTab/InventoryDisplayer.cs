using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDisplayer : ClosableWindowBehaviour
{
    public InventoryTabBehaviour tabBehaviour;

    public void OnEnable()
    {
        tabBehaviour.UpdateInventory(this);
    }



    public override void Update()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        tabBehaviour.UpdateInventory(this);
    }

}
