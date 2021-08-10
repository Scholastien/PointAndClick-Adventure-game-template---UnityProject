using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ScheduleSystem
{
    [ExecuteInEditMode]
    public class ScheduleFormatter : MonoBehaviour
    {


    //    public NpcSchedule baseSchedule;

    //    // Use this for initialization
    //    void Start()
    //    {
    //    }

    //    // Update is called once per frame
    //    void Update()
    //    {
    //        Formatter();
    //    }

    //    private void Formatter()
    //    {
    //        if (baseSchedule.DaysOfWeek.Count != 7)
    //        {
    //            baseSchedule.DaysOfWeek = new List<DaySchedule>();
    //            for (int i = 0; i < 7; i++)
    //            {
    //                baseSchedule.DaysOfWeek.Add(new DaySchedule());
    //            }
    //        }
    //        else
    //        {
    //            NameFields(baseSchedule.DaysOfWeek);
    //        }
    //    }

    //    private void NameFields(List<DaySchedule> DaysOfWeek)
    //    {
    //        for (int i = 0; i < 7; i++)
    //        {
    //            DaysOfWeek[i].name = ((day)i).ToString();

    //            for (int j = 0; j < DaysOfWeek[i].schedule.Count; j++)
    //            {
    //                DaysOfWeek[i].schedule[j].name = TransformTimeToString(((time)j));
    //                DaysOfWeek[i].schedule[j].dayTime = (time)j;

    //                if (DaysOfWeek[i].schedule[j].Activity.Count > 0)
    //                {
    //                    for (int k = 0; k < DaysOfWeek[i].schedule[j].Activity.Count; k++)
    //                    {
    //                        if (DaysOfWeek[i].schedule[j].Activity[k].activity != null)
    //                        {
    //                            DaysOfWeek[i].schedule[j].Activity[k].name = DaysOfWeek[i].schedule[j].Activity[k].activity.label;
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }

    //    public string TransformTimeToString(time timeToTransform)
    //    {
    //        string result = "";
    //        switch (timeToTransform)
    //        {
    //            case (time)0:
    //                result = "7:00AM";
    //                break;
    //            case (time)1:
    //                result = "8:00AM";
    //                break;
    //            case (time)2:
    //                result = "9:00AM";
    //                break;
    //            case (time)3:
    //                result = "10:00AM";
    //                break;
    //            case (time)4:
    //                result = "11:00AM";
    //                break;
    //            case (time)5:
    //                result = "12:00PM";
    //                break;
    //            case (time)6:
    //                result = "1:00PM";
    //                break;
    //            case (time)7:
    //                result = "2:00PM";
    //                break;
    //            case (time)8:
    //                result = "3:00PM";
    //                break;
    //            case (time)9:
    //                result = "4:00PM";
    //                break;
    //            case (time)10:
    //                result = "5:00PM";
    //                break;
    //            case (time)11:
    //                result = "6:00PM";
    //                break;
    //            case (time)12:
    //                result = "7:00PM";
    //                break;
    //            case (time)13:
    //                result = "8:00PM";
    //                break;
    //            case (time)14:
    //                result = "9:00PM";
    //                break;
    //            case (time)15:
    //                result = "10:00PM";
    //                break;
    //            case (time)16:
    //                result = "11:00PM";
    //                break;
    //            case (time)17:
    //                result = "12:00AM";
    //                break;
    //            case (time)18:
    //                result = "1:00AM";
    //                break;
    //            case (time)19:
    //                result = "2:00AM";
    //                break;
    //        }
    //        return result;
    //    }
    }
}