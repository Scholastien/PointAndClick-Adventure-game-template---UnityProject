using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum LogTab
{
    Task = 0,
    Character = 1,
    Conversation = 2
}

public class UI_LogWindowBehaviour : ClosableWindowBehaviour
{
    [Header("Tab Management")]
    public LogTab logTab = LogTab.Task;

    public Sprite selected;
    public Sprite unselected;

    public List<Button> tabsButton;
    public List<GameObject> tabContent;

    public Image current_img;

    public TaskTabBehaviour taskTabBehaviour;

    [Header("Character Met")]
    public Transform characterList;



    public void OnEnable()
    {
        logTab = LogTab.Task;
        SetEnableOnCharacterMet();
        ChangeState(0);
        taskTabBehaviour.SpawnTask();
    }


    public void SetEnableOnCharacterMet()
    {
        foreach (Transform child in characterList)
        {
            CharacterFunction character = child.gameObject.GetComponent<UI_CharacterSchedule>().character;
            Npc Npc = GameManager.instance.managers.variablesManager.gameSavedData.GameInfo.GetNpcWithFunction(character);

            child.gameObject.SetActive(Npc.alreadyMet);
        }
    }

    #region ClickBehaviour

    public void ChangeState(int value)
    {
        Debug.Log(this.gameObject.name);
        logTab = (LogTab)value;
        foreach (GameObject go in tabContent)
        {
            go.SetActive(false);
        }
        tabContent[(int)logTab].SetActive(true);
        if (current_img != null)
            current_img.sprite = unselected;
        current_img = tabsButton[(int)logTab].GetComponent<Image>();
        current_img.sprite = selected;


    }


    #endregion
}
