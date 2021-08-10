using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class UI_NavigationBehaviour
{
    public GameObject mapButton;
    public GameObject navButton_prefab;
    public Transform navButton_spawner;
}

[System.Serializable]
public class UI_EnergyBehaviour
{
    [Header("Display")]
    public Sprite EnergyBarFilled;
    public Sprite EnergyBarEmpty;
    public TextMeshProUGUI Energy_Text;

    [Header("Spawner")]
    public Transform EnergySpawner;
    public GameObject EnergyPrefab;
    public List<GameObject> EnergyHolderList;

    public void Update(int current, int max)
    {
        string energy = LanguageManager.instance.GetTranslation("Energy");

        Energy_Text.text = energy + " " + current + "/" + max;

        foreach (GameObject go in EnergyHolderList)
        {
            GameObject.Destroy(go);
        }

        EnergyHolderList = new List<GameObject>();
        for (int i = 0; i < max; i++)
        {
            GameObject gameObject = GameObject.Instantiate(EnergyPrefab, EnergySpawner);
            EnergyHolderList.Add(gameObject);
            if (i < current)
            {
                gameObject.GetComponent<Image>().sprite = EnergyBarFilled;
            }
            else
            {
                gameObject.GetComponent<Image>().sprite = EnergyBarEmpty;

            }
            gameObject.GetComponent<Image>().raycastTarget = false;
        }
    }
}


public class NavigationUIBehaviour : MonoBehaviour
{

    public NavigationController navigationCtrl;

    public Image FadeTransitionImage;
    public Image BlurryMaskImage;

    [Header("Navigation")]
    public GameObject mapButton;
    public GameObject navButton_prefab;
    public Transform navButton_spawner;
    public TextMeshProUGUI interractableDisplayName;

    [Header("Location")]
    public TextMeshProUGUI LocationDisplay;

    public TextMeshProUGUI RoomDisplay;

    [Header("Time")]
    public TextMeshProUGUI DayTimeDisplay;
    public Slider TimeSlider;


    [Header("Energy")]
    public UI_EnergyBehaviour UI_energyBehaviour;

    [Header("Money")]
    public TextMeshProUGUI MoneyDisplay;

    [Header("Canvas")]
    public Canvas NavigationUI;
    public Canvas ContextualUI;


    private bool show;
    private bool coroutineDone = true;


    private void OnEnable()
    {
        EventManager.onNewValues += SpawnNavButtons;
    }

    private void OnDisable()
    {
        EventManager.onNewValues -= SpawnNavButtons;
    }


    // Use this for initialization
    void Start()
    {
        GameManager.instance.managers.interractableManager.PopUpMouseOverPrefab = interractableDisplayName.transform.parent.gameObject;
        SpawnNavButtons();
        show = false;
    }

    // Update is called once per frame
    void Update()
    {
        UiDisplayerController();
        UpdateDisplayedInfo();
        MaskTransparency();
        DisplayEnergy();


        if (coroutineDone)
        {
            StartCoroutine(InterractablePopup());
        }
    }

    private void LateUpdate()
    {
        if (coroutineDone)
        {
            StartCoroutine(InterractablePopup());
        }
    }

    void MaskTransparency()
    {

        float fade = GameManager.instance.managers.navigationManager.fadeValue;

        // black fade
        //FadeTransitionImage.color = new Color(0, 0, 0, fade);
        FadeTransitionImage.raycastTarget = false;

        // blur fade
        BlurryMaskImage.material.SetFloat("Blur", fade * 5f);
        BlurryMaskImage.enabled = fade == 0;

    }

    public void SpawnNavButtons()
    {
        ClearSpawn();

        foreach (Room room in GameManager.instance.managers.navigationManager.currentLocation.rooms)
        {

            if (!room.locked)
            {
                GameObject newButton = Instantiate(navButton_prefab, navButton_spawner);
                NavigationButtonBehaviour btn_bh = newButton.GetComponentInChildren<NavigationButtonBehaviour>();
                btn_bh.room = room;
                btn_bh.uiBehaviour = this;
                newButton.name = "Btn_" + room.name;


            }
        }
    }

    public void ClearSpawn()
    {
        foreach (Transform child in navButton_spawner)
        {
            Destroy(child.gameObject);
        }
    }

    public void UpdateDisplayedInfo()
    {
        string translated = LanguageManager.instance.GetTranslation(navigationCtrl.currentLocation.displayName);
        LocationDisplay.text = translated;

        translated = LanguageManager.instance.GetTranslation(navigationCtrl.currentRoom.displayName);
        RoomDisplay.text = translated;

        string dayStringTranslated = LanguageManager.instance.GetTranslation(GameManager.instance.managers.timeManager.dayString);
        string timeStringTranslated = LanguageManager.instance.GetTranslation(GameManager.instance.managers.timeManager.timeString);
        translated = dayStringTranslated + ", " + timeStringTranslated;
        DayTimeDisplay.text = translated;



        MoneyDisplay.text = GameManager.instance.managers.variablesManager.GetGameData().GameInfo.mainCharacter.money.ToString();

        TimeSlider.value = (int)GameManager.instance.managers.timeManager.currentTime;

        mapButton.GetComponentInChildren<MapButtonBehaviour>().DetermineButtonBehaviour();
        mapButton.GetComponentInChildren<MapButtonBehaviour>().DisableMap();
    }

    private void UiDisplayerController()
    {
        MapButtonBehaviour mapButtonBehaviour = mapButton.GetComponent<MapButtonBehaviour>();

        if (navigationCtrl.currentLocation == mapButtonBehaviour.townRoom)
        {
            //mapButton.SetActive(false);
            mapButtonBehaviour.icon.sprite = mapButtonBehaviour.townIcon;
            mapButtonBehaviour.RoomToGo = mapButtonBehaviour.townRoom;


            navButton_spawner.gameObject.SetActive(false);
        }
        else
        {
            //mapButton.SetActive(true);
            mapButtonBehaviour.icon.sprite = mapButtonBehaviour.bedIcon;
            mapButtonBehaviour.RoomToGo = mapButtonBehaviour.bedRoom;

            navButton_spawner.gameObject.SetActive(navigationCtrl.currentLocation.rooms.Count != 1);
        }
    }

    private void DisplayEnergy()
    {
        //public Image Energy_amount;
        //public Text Energy_Text;

        int current = VariablesManager.instance.gameSavedData.GameInfo.mainCharacter.currentEnergy;
        int max = VariablesManager.instance.gameSavedData.GameInfo.mainCharacter.GetSkillValue(SkillType.Energy);

        //Debug.Log("Current Energy : " + current);

        UI_energyBehaviour.Update(current, max);

    }

    private IEnumerator InterractablePopup()
    {
        coroutineDone = false;

        show = GameManager.instance.managers.interractableManager.showPopup;
        interractableDisplayName.text = GameManager.instance.managers.interractableManager.interractableDisplayName;
        interractableDisplayName.transform.parent.gameObject.SetActive(show);




        yield return new WaitForSeconds(0.1f);




        coroutineDone = true;



    }


    #region ScreenShoot

    public void ChangeTargetDisplay(int id) // Either 1 or 2 depending of what need to be done
    {
        NavigationUI.targetDisplay = id;
        ContextualUI.targetDisplay = id;
    }

    #endregion
}
