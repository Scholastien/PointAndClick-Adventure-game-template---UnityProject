using InventorySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BikeRidingMinigame;

public class ShopManager : MinigameBehaviour {

    public static ShopManager instance = null;

    public Button close_Btn;

    [Header("Shop Behaviour")]
    public PopulateContentList contentList;

    public PreviewWindow previewWindow;

    #region MinigameBehaviour
    public override void CheckProgress()
    {
        if (done)
        {
            if (GameManager.instance != null)
            {
                GameManager.instance.managers.minigameManager.EndMinigame(true);
                GameManager.instance.gameState = GameState.Navigation;
            }
            else
            {
                Debug.Log("Closing Shop");
            }
        }
    }

    public override void UpdateProgress()
    {
        
    }
    #endregion

    #region MonoBehaviour
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }else if(instance != this)
        {
            Destroy(instance.gameObject);
        }
    }

    // Use this for initialization
    void Start () {

        if (MinigameManager.instance != null)
        {
            contentList.temp_ListItem = MinigameManager.instance.shopParameter.ItemToDisplay;
            //contentList.RemoveAllNonTriggeredItem();
            contentList.Background.sprite = MinigameManager.instance.shopParameter.Background;
            previewWindow.SetMoneyDisplay();

        }


        contentList.Init();
        close_Btn.onClick.AddListener(() => done = true);
    }
	
	// Update is called once per frame
	void Update () {

        UpdateProgress();

        CheckProgress();

    }
    #endregion
}


[System.Serializable]
public class ShopParameter
{
    public List<Item> ItemToDisplay;
    public Sprite Background;
}