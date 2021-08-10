using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationBehaviour : MonoBehaviour {

    public Text Day;
    public Text Time;
    public Text playerName;
    public Text currentMoney;

    public void UpdateInformations()
    {
        Day.text = GameManager.instance.managers.timeManager.dayString;
        Time.text = GameManager.instance.managers.timeManager.timeString;
        playerName.text = GameManager.instance.managers.variablesManager.gameSavedData.GameInfo.mainCharacter.name;
        currentMoney.text = "Money : " + GameManager.instance.managers.variablesManager.gameSavedData.GameInfo.mainCharacter.money.ToString() + "$";
    }
}
