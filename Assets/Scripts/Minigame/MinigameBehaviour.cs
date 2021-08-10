using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class MinigameBehaviour : MonoBehaviour, IMinigame
{
    


    [Header("Minigame values")]
    public bool done;

    public bool IsDone
    {
        get
        {
            return done;
        }
    }


    


    public abstract void CheckProgress();

    public abstract void UpdateProgress();
}
