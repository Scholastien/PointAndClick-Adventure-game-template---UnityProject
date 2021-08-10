using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DayOfTheWeek
{
    Monday = 0,
    Tuesday = 1,
    Wednesday = 2,
    Thursday = 3,
    Friday = 4,
    Saturday = 5,
    Sunday = 6
}

//public enum TimeOfTheDay {
//    SevenAM = 0,
//    EightAM = 1,
//    NineAM = 2,
//    TenAM = 3,
//    ElevenAM = 4,
//    TwelvePM = 5,
//    OnePM = 6,
//    TwoPM = 7,
//    ThreePM = 8,
//    FourPM = 9,
//    FivePM = 10,
//    SixPM = 11,
//    SevenPM = 12,
//    EightPM = 13,
//    NinePM = 14,
//    TenPM = 15,
//    ElevenPM = 16,
//    TwelveAM = 17,
//    OneAM = 18,
//    TwoAM = 19
//}

public enum TimeOfTheDay
{
    Dawn = 0,
    Morning = 1,
    Noon = 2,
    Afternoon = 3,
    Dusk = 4,
    Evening = 5,
    Night = 6,
    LateNight = 7,
}

public class TimeManager : MonoBehaviour
{

    public VariablesManager variableManager;

    public bool Debug_TimeLog = false;


    //[HideInInspector]
    public TimeOfTheDay currentTime;
    [HideInInspector]
    public DayOfTheWeek currentDay;

    public string timeString;
    public string dayString;


    private void OnEnable()
    {
        EventManager.onNewValues += GetTime;
    }

    private void OnDisable()
    {
        EventManager.onNewValues -= GetTime;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // CurrentTime is modified here 

        if (GameManager.instance.managers.saveManager.justLoaded)
        {
            Debug.LogWarning("VM\t: " + (DayOfTheWeek)variableManager.gameSavedData.GameInfo.intDay + " (" + variableManager.gameSavedData.GameInfo.intDay + ") "
                 + (TimeOfTheDay)variableManager.gameSavedData.GameInfo.intTime + "(" + variableManager.gameSavedData.GameInfo.intTime + ")");
        }


        currentTime = (TimeOfTheDay)(variableManager.gameSavedData.GameInfo.intTime % 8);

        if (currentDay != (DayOfTheWeek)(variableManager.gameSavedData.GameInfo.intDay % 7))
        {

            currentDay = (DayOfTheWeek)(variableManager.gameSavedData.GameInfo.intDay % 7);
            variableManager.gameSavedData.GameInfo.intTime = 0;
        }



        GetTimeAndDayStrings();
    }
    
    public void GetTime()
    {
        currentTime = (TimeOfTheDay)(variableManager.gameSavedData.GameInfo.intTime);
        currentDay = (DayOfTheWeek)(variableManager.gameSavedData.GameInfo.intDay);
    }

    public void GetTimeAndDayStrings()
    {
        timeString = TransformTimeToString(currentTime);
        dayString = currentDay.ToString();
    }

    public void GoToNextHour()
    {
        variableManager.gameSavedData.GameInfo.intTime = (variableManager.gameSavedData.GameInfo.intTime + 1) % 8;
        currentTime = (TimeOfTheDay)(variableManager.gameSavedData.GameInfo.intTime % 8);
        //if (variableManager.gameSavedData.GameInfo.intTime == 0)
        //    GoToNextDay();
    }
    public void GoToNextDay()
    {
        variableManager.gameSavedData.GameInfo.intDay = (variableManager.gameSavedData.GameInfo.intDay + 1) % 7;
    }

    public void GoToTime(int time)
    {
        variableManager.gameSavedData.GameInfo.intTime = time;
    }
    public void GoToDay(int day)
    {
        variableManager.gameSavedData.GameInfo.intDay = day;
    }
    #region AbstractHour
    public string TransformTimeToString(TimeOfTheDay timeToTransform)
    {
        string result = "";
        switch (timeToTransform)
        {
            case (TimeOfTheDay)0:
                result = "Dawn";
                break;
            case (TimeOfTheDay)1:
                result = "Morning";
                break;
            case (TimeOfTheDay)2:
                result = "Noon";
                break;
            case (TimeOfTheDay)3:
                result = "Afternoon";
                break;
            case (TimeOfTheDay)4:
                result = "Dusk";
                break;
            case (TimeOfTheDay)5:
                result = "Evening";
                break;
            case (TimeOfTheDay)6:
                result = "Night";
                break;
            case (TimeOfTheDay)7:
                result = "Late night";
                break;
        }
        return result;
    }
    #endregion

    #region FullHour
    //public string TransformTimeToString(TimeOfTheDay timeToTransform) {
    //    string result = "";
    //    switch (timeToTransform) {
    //        case (TimeOfTheDay)0:
    //            result = "7:00AM";
    //            break;
    //        case (TimeOfTheDay)1:
    //            result = "8:00AM";
    //            break;
    //        case (TimeOfTheDay)2:
    //            result = "9:00AM";
    //            break;
    //        case (TimeOfTheDay)3:
    //            result = "10:00AM";
    //            break;
    //        case (TimeOfTheDay)4:
    //            result = "11:00AM";
    //            break;
    //        case (TimeOfTheDay)5:
    //            result = "12:00PM";
    //            break;
    //        case (TimeOfTheDay)6:
    //            result = "1:00PM";
    //            break;
    //        case (TimeOfTheDay)7:
    //            result = "2:00PM";
    //            break;
    //        case (TimeOfTheDay)8:
    //            result = "3:00PM";
    //            break;
    //        case (TimeOfTheDay)9:
    //            result = "4:00PM";
    //            break;
    //        case (TimeOfTheDay)10:
    //            result = "5:00PM";
    //            break;
    //        case (TimeOfTheDay)11:
    //            result = "6:00PM";
    //            break;
    //        case (TimeOfTheDay)12:
    //            result = "7:00PM";
    //            break;
    //        case (TimeOfTheDay)13:
    //            result = "8:00PM";
    //            break;
    //        case (TimeOfTheDay)14:
    //            result = "9:00PM";
    //            break;
    //        case (TimeOfTheDay)15:
    //            result = "10:00PM";
    //            break;
    //        case (TimeOfTheDay)16:
    //            result = "11:00PM";
    //            break;
    //        case (TimeOfTheDay)17:
    //            result = "12:00AM";
    //            break;
    //        case (TimeOfTheDay)18:
    //            result = "1:00AM";
    //            break;
    //        case (TimeOfTheDay)19:
    //            result = "2:00AM";
    //            break;
    //    }
    //    return result;
    //}

    #endregion
}


/*
 
     
     using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunChain : MonoBehaviour {
	public DialogueChain chainToRun;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	public void chainRun () {
		chainToRun.StartChain ();
	}
}

     
     
     
     
     
     */
