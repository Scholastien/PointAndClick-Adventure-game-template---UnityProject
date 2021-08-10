using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

public class TabButtonBehaviour : MonoBehaviour, ISelectHandler, IPointerEnterHandler// required interface when using the OnSelect method. 
    {

    public GameObject selectedPanel;
    public GameObject contentContainer;

    //Do this when the selectable UI object is selected.
    public void OnSelect(BaseEventData eventData) {
        //Debug.Log(this.gameObject.name + " was selected");



        if (!selectedPanel.activeInHierarchy) {
            foreach (Transform child in contentContainer.transform) {
                child.gameObject.SetActive(false);
            }
            selectedPanel.SetActive(true);
        }
        
    }

    // When highlighted with mouse.
    public void OnPointerEnter(PointerEventData eventData) {
        // Do something.
        //Debug.Log("<color=red>Event:</color> Completed mouse highlight.");
        gameObject.GetComponent<Button>().Select();
    }
    
}
