using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TimeSkipBehaviour : MonoBehaviour
{
    public ChainTrigger LockTimeSkip;

    public void SkipOneHour()
    {
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);

        if (!LockTimeSkip.triggered)
        {
            if (GameManager.instance.managers.variablesManager.gameSavedData.GameInfo.intTime < 7)
            {
                GameManager.instance.managers.timeManager.GoToNextHour();
            }
        }
    }

}
