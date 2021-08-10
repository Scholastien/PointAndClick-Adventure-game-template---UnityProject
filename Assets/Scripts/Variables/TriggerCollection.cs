using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "new TriggerCollection", menuName = "LewdOwl/Variable/TriggerCollection")]
public class TriggerCollection : ScriptableObject
{

    public List<ChainTrigger> chainTriggers;

	public void Init()
    {
        ChainTrigger[] Ct_list = Resources.LoadAll<ChainTrigger>(DialogueChainPreferences.triggerAssetPathway);

        List<ChainTrigger> chainTriggersToRemove = new List<ChainTrigger>();

        for (int i = 0; i < chainTriggers.Count; i++)
        {
            if (!Ct_list.Contains(chainTriggers[i]))
            {
                chainTriggersToRemove.Add(chainTriggers[i]);
            }
        }
        for (int i = 0; i < chainTriggersToRemove.Count; i++)
        {
            chainTriggers.Remove(chainTriggersToRemove[i]);
        }

        foreach (ChainTrigger ct in chainTriggers)
        {
            if (!Ct_list.Contains(ct))
            {
                chainTriggersToRemove.Add(ct);
            }
        }

        for (int i = 0; i < Ct_list.Length; i++)
        {
            if (!chainTriggers.Contains(Ct_list[i]))
            {
                chainTriggers.Add(Ct_list[i]);
                chainTriggers[i].id = i;
            }
        }



    }


    public void LoadDataFromSaveSystem(List<bool> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            chainTriggers[i].triggered = list[i];
        }
    }

    public void UpdateBoolValues()
    {
        for (int i = 0; i < GameManager.instance.managers.variablesManager.gameSavedData.progressionTab.Count; i++)
        {
            GameManager.instance.managers.variablesManager.gameSavedData.progressionTab[i] = chainTriggers[i].triggered;
        }
    }
}
