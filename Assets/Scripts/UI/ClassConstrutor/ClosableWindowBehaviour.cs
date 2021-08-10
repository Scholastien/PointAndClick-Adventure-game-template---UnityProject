using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ClosableWindowBehaviour : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    public bool inside = false;


    public virtual void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!inside)
            {
                EventManager.instance.ClosingAllWindow();
            }
        }
    }


    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        inside = true;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        inside = false;
    }
}