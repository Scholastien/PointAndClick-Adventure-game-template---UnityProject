using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Sub_MenuBehaviour : MonoBehaviour
{
    [Header("Sub-Menu Buttons")]
    public TaskTabBehaviour taskTabBehaviour;
    public Button BagButton;
    public Button LogButton;
    public Button SaveButton;
    public Button CogButton;

    [Header("Sub-Menus")]
    public SubMenuState subMenuState = SubMenuState.Closed;
    public GameObject BagPanel;
    public GameObject LogPanel;
    public GameObject SavePanel;
    public GameObject CogPanel;
    public List<Image> pointerArray;

    [Header("Detailed window")]
    public Image contextualWindowHandlerWithBlurryBG;

    [Header("Sub-menus controllers")]
    public SaveLoadWindowController saveLoadWindowCtrl; 

    #region Rawdata

    public enum SubMenuState
    {
        Closed = 4,
        Bag = 0,
        Log = 1,
        Save = 2,
        Cog = 3
    }

    #endregion

    private void OnEnable()
    {
        EventManager.onNewValues += CloseAllWindow;
        EventManager.onClosingWindow += CloseAllWindow;
    }

    private void OnDisable()
    {
        EventManager.onNewValues -= CloseAllWindow;
        EventManager.onClosingWindow -= CloseAllWindow;
    }

    private void Start()
    {
        subMenuState = SubMenuState.Closed;
    }



    void CloseAllWindow()
    {
        CloseSubmenus();
        subMenuState = SubMenuState.Closed;
        contextualWindowHandlerWithBlurryBG.enabled = false;

        // Set all window to false
        foreach(Transform t in contextualWindowHandlerWithBlurryBG.transform)
        {
            t.gameObject.SetActive(false);
        }
    }

    

    #region ContentManagement


    #region Sub-Menu Buttons

    public GameObject GetCurrentSubmenu()
    {
        switch (subMenuState)
        {
            case SubMenuState.Closed:
                return null;
            case SubMenuState.Bag:
                return BagPanel;
            case SubMenuState.Log:
                return LogPanel;
            case SubMenuState.Save:
                return SavePanel;
            case SubMenuState.Cog:
                return CogPanel;
            default:
                return null;
        }
    }

    public void SubMenuButton(Button button)
    {
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
        switch (subMenuState)
        {
            case SubMenuState.Closed:
                if (button == BagButton)
                {
                    OpenSubmenu(BagPanel);
                    subMenuState = SubMenuState.Bag;
                }
                else if (button == LogButton)
                {
                    OpenSubmenu(LogPanel);
                    taskTabBehaviour.SpawnTask();
                    subMenuState = SubMenuState.Log;
                }
                else if (button == SaveButton)
                {
                    OpenSubmenu(SavePanel);
                    subMenuState = SubMenuState.Save;
                }
                else if (button == CogButton)
                {
                    OpenSubmenu(CogPanel);
                    subMenuState = SubMenuState.Cog;
                }
                break;
            default:
                DisplaySubmenu(button);
                break;
        }
    }

    public void DisplaySubmenu(Button button)
    {
        if (button == BagButton && subMenuState != SubMenuState.Bag)
        {
            EnableOnlyOneSubmenu(BagPanel);
            subMenuState = SubMenuState.Bag;
        }
        else if (button == LogButton && subMenuState != SubMenuState.Log)
        {
            EnableOnlyOneSubmenu(LogPanel);
            taskTabBehaviour.SpawnTask();
            subMenuState = SubMenuState.Log;
        }
        else if (button == SaveButton && subMenuState != SubMenuState.Save)
        {
            EnableOnlyOneSubmenu(SavePanel);
            saveLoadWindowCtrl.SpawnAllSavesPreview();
            subMenuState = SubMenuState.Save;
        }
        else if (button == CogButton && subMenuState != SubMenuState.Cog)
        {
            EnableOnlyOneSubmenu(CogPanel);
            subMenuState = SubMenuState.Cog;
        }
        else
        {
            CloseSubmenus();
        }
        HandlePointerArray();
    }

    public void EnableOnlyOneSubmenu(GameObject submenu)
    {
        BagPanel.SetActive(false);
        LogPanel.SetActive(false);
        SavePanel.SetActive(false);
        CogPanel.SetActive(false);

        submenu.SetActive(true);
    }

    public void OpenSubmenu(GameObject submenu)
    {
        Debug.Log("OpenSubmenu");

        BagPanel.SetActive(false);
        LogPanel.SetActive(false);
        SavePanel.SetActive(false);
        CogPanel.SetActive(false);

        // Animation open menu
        submenu.SetActive(true);
        StartCoroutine(IncreaseScaleSubmenu(submenu));
    }

    public void CloseSubmenus()
    {
        StopAllCoroutines();
        // get active submenu

        // Animation close submenu
        if (GetCurrentSubmenu() != null)
            StartCoroutine(ReduceScaleSubmenu(GetCurrentSubmenu()));
    }

    public IEnumerator IncreaseScaleSubmenu(GameObject submenuPanel)
    {

        submenuPanel.GetComponent<Transform>().localScale = new Vector3(1, 0.1f, 1);
        Debug.Log("IncreaseScaleSubmenu " + submenuPanel.name);
        // go from 0.1f to 1f (Y pos)

        for (float i = 0.1f; i <= 1.1f; i += 0.2f)
        {
            yield return null;
            submenuPanel.GetComponent<Transform>().localScale = new Vector3(1, i, 1);
        }
        submenuPanel.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
        HandlePointerArray();
    }

    public IEnumerator ReduceScaleSubmenu(GameObject submenuPanel)
    {
        Debug.Log("ReduceScaleSubmenu " + submenuPanel.name);
        // go from 1f to 0.1f (Y pos)
        for (float i = 1; i >= 0.0f; i -= 0.2f)
        {
            yield return null;
            submenuPanel.GetComponent<Transform>().localScale = new Vector3(1, i, 1);
        }



        BagPanel.SetActive(false);
        LogPanel.SetActive(false);
        SavePanel.SetActive(false);
        CogPanel.SetActive(false);

        submenuPanel.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);

        subMenuState = SubMenuState.Closed;

        HandlePointerArray();
    }

    public void HandlePointerArray()
    {
        foreach (Image image in pointerArray)
        {
            image.enabled = false;
        }
        if (subMenuState != SubMenuState.Closed)
            pointerArray[(int)subMenuState].enabled = true;
    }


    #endregion

    #endregion
}
