using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NavBarAnimBehaviour : MonoBehaviour
{

    public NavBarState navBarState;
    public NavButtonExtender navButtonExtender;
    public NavBar navBar;


    #region monobehaviour




    public void Start()
    {
        HideNavBar();
    }

    public void Update()
    {
        if (!NavigationManager.instance.IsNavigationBarRequiered())
        {
            HideNavBar();
        }

        if (NavigationManager.instance.needToOpenTheNavBar)
        {
            

            // Check if the navbar can be open
            // it can open only when more than one room is available
            if (NavigationManager.instance.IsNavigationBarRequiered())
            {
                //if(navBarState == NavBarState.closed)
                StopAllCoroutines();
                navBar.rectTransform.localScale = new Vector3(0, 0, 1);
                StartCoroutine(OpenNavBar());
            }

            
            NavigationManager.instance.needToOpenTheNavBar = false;

        }

        DisplayNavButtonExtender();
    }




    #endregion

    public void HideNavBar()
    {
        navBarState = NavBarState.closed;
        navButtonExtender.UpdateButtonSprite(this, navBarState);
        navBar.rectTransform.localScale = new Vector3(0, 0, 1);
        navBar.rectTransform.sizeDelta = new Vector2(navBar.rectTransform.anchoredPosition.x, 0);

        DisplayNavButtonExtender();
    }

    public void ShowNavBar()
    {

        navBarState = NavBarState.openned;
        navButtonExtender.UpdateButtonSprite(this, navBarState);
        navBar.rectTransform.localScale = new Vector3(1, 1, 1);
        navBar.rectTransform.sizeDelta = new Vector2(navBar.rectTransform.anchoredPosition.x, navBar.initialHeight);
    }

    public void DisplayNavButtonExtender()
    {
        navButtonExtender.ShowButton(NavigationManager.instance.IsNavigationBarRequiered());
    }


    public void ChooseBetweenOpenOrClose()
    {
        switch (navBarState)
        {
            case NavBarState.openned:
                StartCoroutine(CloseNavBar());
                break;
            case NavBarState.closed:
                StartCoroutine(OpenNavBar());
                break;
            default:
                break;
        }
    }

    public void CloseNavBarIfNeeded()
    {
        if (navBarState == NavBarState.openned)
            StartCoroutine(CloseNavBar());
    }

    public void OpenNavBarIfNeeded()
    {
        if (navBarState == NavBarState.openned)
            StartCoroutine(OpenNavBar());
    }

    #region Ienumerator

    public IEnumerator OpenNavBar()
    {
        navButtonExtender.navBarMouseOver.isOpenning = true;

        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
        //Debug.Log("Open");
        yield return new WaitForEndOfFrame();

        // loop to animate navbar
        navBar.rectTransform.localScale = new Vector3(0, 0, 1);
        // go from 0.1f to 1f (Y pos)

        for (float i = 0.1f; i <= 1.1f; i += 0.2f)
        {
            yield return null;
            navBar.rectTransform.localScale = new Vector3(i, i, 1);
            navBar.rectTransform.sizeDelta = new Vector2(navBar.rectTransform.anchoredPosition.x, i * navBar.initialHeight);
        }
        navBar.rectTransform.localScale = new Vector3(1, 1, 1);
        navBar.rectTransform.sizeDelta = new Vector2(navBar.rectTransform.anchoredPosition.x, navBar.initialHeight);

        // end of the loop
        navBarState = NavBarState.openned;
        navButtonExtender.UpdateButtonSprite(this, navBarState);
    }

    public IEnumerator CloseNavBar()
    {
        Debug.Log("Close");
        yield return new WaitForEndOfFrame();



        // loop to animate navbar
        for (float i = 1; i >= 0.0f; i -= 0.2f)
        {
            yield return null;
            navBar.rectTransform.localScale = new Vector3(i, i, 1);
            navBar.rectTransform.sizeDelta = new Vector2(navBar.rectTransform.anchoredPosition.x, i * navBar.initialHeight);
        }

        navBar.rectTransform.localScale = new Vector3(0, 0, 1);
        navBar.rectTransform.sizeDelta = new Vector2(navBar.rectTransform.anchoredPosition.x, 0);

        // end of the loop
        navBarState = NavBarState.closed;
        navButtonExtender.UpdateButtonSprite(this, navBarState);

        navButtonExtender.navBarMouseOver.isOpenning = false;
    }

    #endregion


    public enum NavBarState
    {
        openned,
        closed
    }

    [System.Serializable]
    public struct NavButtonExtender
    {
        [Header("ExtenderButtonClick")]
        public Button button;
        public Sprite openIdle;
        public Sprite openClick;
        public Sprite closeIdle;
        public Sprite closeClick;

        [Header("MouseOverArea")]
        public NavBarMouseOver navBarMouseOver;

        public void UpdateButtonSprite(NavBarAnimBehaviour navBarAnimBehaviour, NavBarState navBarState)
        {
            Image image = button.GetComponent<Image>();
            SpriteState spriteState = new SpriteState();
            spriteState = button.spriteState;



            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => navBarAnimBehaviour.ChooseBetweenOpenOrClose());

            switch (navBarState)
            {
                case NavBarState.openned:
                    image.sprite = closeIdle;
                    spriteState.pressedSprite = closeClick;
                    break;
                case NavBarState.closed:
                    image.sprite = openIdle;
                    spriteState.pressedSprite = openClick;
                    break;
                default:
                    break;
            }

            button.spriteState = spriteState;
        }

        public void ShowButton(bool show)
        {
            button.GetComponent<Image>().enabled = show;
            button.interactable = show;
        }
    }

    [System.Serializable]
    public struct NavBar
    {
        public float initialHeight;
        public RectTransform rectTransform;
    }
}




