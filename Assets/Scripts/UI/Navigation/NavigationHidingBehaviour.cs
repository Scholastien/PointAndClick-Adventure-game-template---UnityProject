using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ask to close with EventManager.instance.Interract();

public class NavigationHidingBehaviour : MonoBehaviour
{
    public NavBarAnimBehaviour navBarAnim;
    public RectTransform topPanel;

    public bool hide = false;

    public float initialHeight = 0f;

    public float speed = 15f;




    private void OnEnable()
    {
        EventManager.onInteraction += HideInterface;
        //EventManager.onEndOfDialogue += ShowInterface;

        ShowInterface();
    }

    private void OnDisable()
    {
        EventManager.onInteraction -= HideInterface;
        //EventManager.onEndOfDialogue += ShowInterface;
    }

    public void LateUpdate()
    {
        if (NavigationManager.instance.needToOpen)
        {
            if (navBarAnim != null)
            {
                ShowInterface();
                NavigationManager.instance.needToOpen = false;
            }
        }
    }


    #region Close UI
    void HideInterface()
    {
        //Debug.LogError("Hide UI");


        hide = true;
        navBarAnim.CloseNavBarIfNeeded();
        StartCoroutine(CloseTopPanel());
    }
    
    public IEnumerator CloseTopPanel()
    {
        //Debug.LogError("Closing");



        // loop to animate navbar
        for (float i = 1; i >= 0.0f; i -= Time.deltaTime *speed)
        {
            yield return null;
            topPanel.localScale = new Vector3(1, i, 1);
            topPanel.sizeDelta = new Vector2(topPanel.anchoredPosition.x, i * initialHeight);
        }

        topPanel.localScale = new Vector3(0, 0, 1);
        topPanel.sizeDelta = new Vector2(topPanel.anchoredPosition.x, 0);

        // end of the loop
        //Debug.LogError("Closing done");
        GameManager.instance.managers.interractableManager.interacted = false;
    }

    #endregion


    #region OpenUI

    public void ShowInterface()
    {
        //Debug.LogError("Show UI");


        hide = false;
        navBarAnim.OpenNavBarIfNeeded();
        StartCoroutine(OpenTopPanel());
    }

    public IEnumerator OpenTopPanel()
    {
        Debug.Log("Close");

        yield return new WaitForEndOfFrame();


        // loop to animate navbar
        for (float i = 0; i >= 1; i += Time.deltaTime * speed)
        {
            yield return null;
            topPanel.localScale = new Vector3(1, i, 1);
            topPanel.sizeDelta = new Vector2(topPanel.anchoredPosition.x, i * initialHeight);
        }

        topPanel.localScale = new Vector3(1, 1, 1);
        topPanel.sizeDelta = new Vector2(topPanel.anchoredPosition.x, initialHeight);

        // end of the loop

    }

    #endregion
}
