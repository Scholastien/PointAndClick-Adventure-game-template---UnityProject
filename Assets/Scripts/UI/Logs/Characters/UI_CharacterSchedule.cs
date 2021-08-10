using ScheduleSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_CharacterSchedule : MonoBehaviour
{
    [Header("setter")]
    public CharacterFunction character;
    public bool characterMet;

    [Header("manager ref")]
    public Schedule schedule;
    public NpcScheduleBehaviour scheduleBehaviour;

    public Activity currentActivity;

    private ScheduleManager manager;


    [Header("ui elements")]
    public TextMeshProUGUI activity;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI room;
    public TextMeshProUGUI progression;
    public Image profilePic;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameManager.instance.managers.scheduleManager;
        schedule = manager.GetCharacterSchedule(character);
        scheduleBehaviour = manager.GetCurrentScheduleBehaviour(character);
        currentActivity = scheduleBehaviour.currentActivity;
    }

    // Update is called once per frame
    void Update()
    {
        characterName.text = GameManager.instance.managers.variablesManager.gameSavedData.GameInfo.GetCharacterNameWithFunction(character);
        currentActivity = scheduleBehaviour.currentActivity;
        activity.text = currentActivity.display_name;
        room.text = currentActivity.room.displayName;
        progression.text = "0%"; // TODO: Progression bar
        profilePic.sprite = Resources.Load<Sprite>(GetPP_path(character));
    }


    public string GetPP_path(CharacterFunction function)
    {
        switch (function)
        {
            case CharacterFunction.test:
                return "Graphic/UI/NPCs Profile pic/test_avatar";
            default:
                return "Graphic/UI/NPCs Profile pic/test_avatar";
        }
    }
}
