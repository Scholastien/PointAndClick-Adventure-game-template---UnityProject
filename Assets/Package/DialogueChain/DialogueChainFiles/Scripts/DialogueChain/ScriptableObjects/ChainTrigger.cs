using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[CreateAssetMenu(fileName = "NewChainTrigger", menuName = "Dialogue Chains/Trigger")]
[System.Serializable]
public class ChainTrigger : ScriptableObject
{
    [HideInInspector]
    public int id;

    public bool triggered = false;
    
}
