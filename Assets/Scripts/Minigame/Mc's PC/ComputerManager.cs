using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace BikeRidingMinigame
{
    public class ComputerManager : MinigameBehaviour
    {
        public static ComputerManager instance = null;

        [Header("Values")]
        public bool powerOff = false;

        [Header("Buttons")]
        [Range(0,100)]
        public int defaultApp;
        public List<PcApplication> pcApplications;

        [Space(20)]

        public Button PowerOff_btn;
        public Button Folder_btn;
        public Button Internet_btn;
        public Button Webcam_btn;
        public Button Mail_btn;
        public Button Cmd_btn;

        #region MonoBehaviour

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(this);
            }


            SetupButtons();
        }


        // Use this for initialization
        void Start()
        {
            LoadApp(defaultApp);
        }

        

        // Update is called once per frame
        void Update()
        {
            UpdateProgress();
            CheckProgress();
        }
        #endregion



        #region MinigameBehaviour
        public override void CheckProgress()
        {
            if (done)
            {
                GameManager.instance.managers.minigameManager.EndMinigame(true);
                GameManager.instance.gameState = GameState.Navigation;
            }
        }

        public override void UpdateProgress()
        {
            // Update done boolean
            done = powerOff;

            // Update values linked to the computer UI


            // Load selected UI
        }
        #endregion

        #region ManagerBehaviour

        public void LoadApp(int index)
        {
            for (int i = 0; i < pcApplications.Count; i++)
            {
                if(i != index)
                {
                    // unload
                    pcApplications[i].appGameObject.SetActive(false);
                }
                else
                {
                    //load
                    pcApplications[i].appGameObject.SetActive(true);
                    pcApplications[i].standardButtons.SetupStandardButtons();
                }
            }
        }

        public void SetupButtons()
        {
            Debug.Log("Setting up Buttons");
            PowerOff_btn.onClick.AddListener(() => PowerOff());
            Mail_btn.onClick.AddListener(() => LoadApp(1));
            Internet_btn.onClick.AddListener(() => LoadApp(2));
            Folder_btn.onClick.AddListener(() => Debug.Log("Folder_btn"));
            Webcam_btn.onClick.AddListener(() => Debug.Log("Webcam_btn"));
            Cmd_btn.onClick.AddListener(() => Debug.Log("Cmd_btn"));

        }

        public void PowerOff()
        {
            Debug.Log("leaving...");

            powerOff = true;
        }

        #endregion
        
    }


    [System.Serializable]
    public class PcApplication
    {
        public string name;
        public GameObject appGameObject;
        public StandardButtons standardButtons;
        public List<Button> otherButtons;
    }

    [System.Serializable]
    public class StandardButtons
    {
        public Button PowerOff;
        public Button Close;

        public void SetupStandardButtons()
        {
            PowerOff.onClick.AddListener(
                delegate
                {
                    ComputerManager.instance.powerOff = true;
                }
            );
            Close.onClick.AddListener(
                delegate
                {
                    ComputerManager.instance.LoadApp(ComputerManager.instance.defaultApp);
                }
            );
        }
    }
}
