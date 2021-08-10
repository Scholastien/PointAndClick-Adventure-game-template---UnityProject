using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TitleScreen
{
    [System.Serializable]
    public class LoadGameButtonBehaviour
    {
        public List<GameData> gameDatas;
        public GameObject SavePrefabPreview;
        public Transform parent;
        public Vector3 StartPosition;
        public float Z_padding;
        public List<GameObject> savePreviews;


        public void GetSaves()
        {
            gameDatas = new List<GameData>();

            if (GameManager.instance != null)
                gameDatas = GameManager.instance.managers.saveManager.saves;
            else
            {
                Debug.LogWarning("Can't pull saves from GameManager");
                gameDatas = new List<GameData> { new GameData(), new GameData(), new GameData() };
            }
        }

        public void SpawnSaves()
        {
            float padding = Z_padding;

            DestroySaves();

                GetSaves();

            foreach (GameData save in gameDatas)
            {
                GameObject go = GameObject.Instantiate(SavePrefabPreview);
                go.transform.parent = parent;
                savePreviews.Add(go);

                go.transform.localPosition = StartPosition + new Vector3(0, 0, padding);

                padding += Z_padding;
            }
        }

        public void DestroySaves()
        {
            foreach (GameObject go in savePreviews)
            {
                GameObject.Destroy(go);
            }
            savePreviews = new List<GameObject>();
        }
    }



    public class MainMenuGroup : MonoBehaviour
    {
        public LoadGameButtonBehaviour loadButtonBehaviour;
        public List<MainMenuButton> menuButtons;


        public Sprite tabIdle;
        public Sprite tabHover;
        public Sprite tabActive;

        public Transform holder;


        public SaveLoadWindowController windowController;


        public MainMenuButton selectedMenuButton;

        public AnimationController animCtrl;

        public void Start()
        {
            windowController.SpawnAllSavesPreview();
        }

        public void Subscribe(MainMenuButton menuButton)
        {
            if (menuButtons == null)
            {
                menuButtons = new List<MainMenuButton>();
            }
            menuButtons.Add(menuButton);
        }

        public void OnButtonEnter(MainMenuButton menuButton)
        {
            ResetTabs();
            if (menuButton != selectedMenuButton)
                menuButton.background.sprite = tabHover;
        }

        public void OnButtonExit(MainMenuButton menuButton)
        {
            ResetTabs();


            //menuButton.background.sprite = tabIdle;
        }

        public void OnButtonSelected(MainMenuButton menuButton)
        {


            if (selectedMenuButton != null)
            {
                menuButton.Deselect();
            }

            menuButton.background.sprite = tabActive;
            selectedMenuButton = menuButton;

            StartCoroutine(CloseMainMenu(menuButton));

            //menuButton.Select();

            ResetTabs();

            int index = menuButton.transform.GetSiblingIndex(); // return the index of the button selected in the main menu

            animCtrl.SetOpen(false);
        }

        public void ResetTabs()
        {
            foreach (MainMenuButton button in menuButtons)
            {
                if (selectedMenuButton != null && button == selectedMenuButton) { continue; }
                button.background.sprite = tabIdle;
            }
        }





        public void InstanceSavesPreview()
        {
            loadButtonBehaviour.SpawnSaves();
        }


        #region SpecialUIBehaviours

        public void NewGame()
        {
            StartCoroutine(NewGameAfterAnim());
        }

        public IEnumerator NewGameAfterAnim()
        {
            //yield return new WaitWhile(() => animCtrl.IsCurrentCameraTagAnimated("NewGame"));

            yield return new WaitForSeconds(1f);

            GameManager.instance.managers.saveManager.NewGame();
        }

        #endregion


        public IEnumerator CloseMainMenu(MainMenuButton menuButton)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitUntil(() => !animCtrl.IsCurrentUITagAnimated("SlideOut"));
            menuButton.Select();

        }
    }
}