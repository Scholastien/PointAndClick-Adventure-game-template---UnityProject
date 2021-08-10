using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class NavBarMouseOver : MonoBehaviour, IPointerEnterHandler
{
    public bool activated = true;


    public bool isOpenning = false;

    public NavBarAnimBehaviour navBarAnim;


    void Update()
    {
            RectTransform rt = gameObject.GetComponent<RectTransform>();

            Vector2 v2 = navBarAnim.navBar.rectTransform.sizeDelta;

            rt.sizeDelta = new Vector2(v2.x, 20);
        
    }



    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isOpenning && activated)
        {
            // open the navbar
            navBarAnim.ChooseBetweenOpenOrClose();
        }
    }
}
