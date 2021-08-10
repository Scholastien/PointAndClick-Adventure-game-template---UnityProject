using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class MouseoverBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Mouseover")]
    protected GameObject popUpMouseOver;
    public bool displayInfoBox = true;
    public string displayName;
    protected bool showInfoBox = false;
    public bool disable;
    protected Vector3 offset;
    protected float distance;


    private void OnDestroy()
    {
        HidePopUp();
    }

    public virtual void Awake()
    {
        distance = 50f;
        offset = new Vector3(0, -distance, 0);
    }


    protected virtual void Start()
    {
        popUpMouseOver = GameManager.instance.managers.interractableManager.PopUpMouseOverPrefab;
    }


    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        EnterInteractible();
        showInfoBox = true;


        // update the offset value

    }

    public void EnterInteractible()
    {


        if (displayInfoBox && !disable && !showInfoBox)
        {
            popUpMouseOver = GameManager.instance.managers.interractableManager.PopUpMouseOverPrefab;

            string translated_DisplayName = LanguageManager.instance.GetTranslation(displayName);

            GameManager.instance.managers.interractableManager.DisplayPopup(translated_DisplayName);
        }

        // update the offset value
        UpdateOffsetValue();
        GameManager.instance.managers.interractableManager.infoBoxOffset = offset;
    }


    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (displayInfoBox && !disable)
        {
            HidePopUp();
        }


    }

    void HidePopUp()
    {
        // Remote destroy with the ui manager
        GameManager.instance.managers.interractableManager.HidePopup();
        showInfoBox = false;

        
    }

    #region Offset
    public void UpdateOffsetValue()
    {
        // Input.mousePosition;

        Vector2 screenDim = GetScreenDimension();

        Vector2 newOrigin = new Vector2(0, 0);

        Vector2 newPos = GetNewMousePosition(Input.mousePosition, screenDim);

        if (newPos.x > 0) //right side
        {
            offset = new Vector3(-distance, -distance, 0);
        }
        else // left side
        {
            offset = new Vector3(distance, -distance, 0);
        }


    }

    public Vector2 GetScreenDimension()
    {
        Camera camera = GameManager.instance.gameObject.GetComponent<Camera>();
        // camera.pixelHeight = 1080
        // camera.pixelWidth = 1920
        return new Vector2Int(camera.pixelWidth, camera.pixelHeight);
    }

    public Vector2 GetNewMousePosition(Vector2 mousePos, Vector2 screenDim)
    {
        float x = GetNewCoordinate(mousePos.x, screenDim.x);
        float y = GetNewCoordinate(mousePos.y, screenDim.y);

        return new Vector2(x, y);
    }

    public float GetNewCoordinate(float pos, float dim)
    {
        float middle = dim / 2;

        return pos - middle;
    }





    #endregion
}
