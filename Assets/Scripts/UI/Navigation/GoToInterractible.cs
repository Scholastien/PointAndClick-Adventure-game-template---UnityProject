using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GoToInterractible : Interractible /*, IPointerEnterHandler, IPointerExitHandler*/
{

    public int locationId;
    public int roomId;
    //public GameObject popUpMouseOver;
    public Text destinationName;

    // Use this for initialization
    protected override void Start()
    {
        displayName = GameManager.instance.managers.navigationManager.GetLocationDisplayName(locationId);
        base.Start();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }


    public void GoToRoom()
    {
        if (!disable)
        {
            GameManager.instance.managers.navigationManager.SetRoom(locationId, roomId);
            //popUpMouseOver.SetActive(false);
        }
    }

    public void GoToPreviousRoom()
    {
        if (!disable)
        {
            GameManager.instance.managers.navigationManager.GoToPreviousRoom();
        }
    }

    //public override void OnPointerEnter(PointerEventData eventData)
    //{
    //    if (popUpMouseOver != null)
    //    {
    //        popUpMouseOver.SetActive(true);
    //        destinationName.text = GameManager.instance.managers.navigationManager.GetLocationDisplayName(locationId);
    //    }
    //}

    //public override void OnPointerExit(PointerEventData eventData)
    //{
    //    if (popUpMouseOver != null)
    //    {
    //        popUpMouseOver.SetActive(false);
    //    }
    //}
}
