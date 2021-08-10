using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IMinigame
{
    bool IsDone { get; }

    void CheckProgress();
    void UpdateProgress();
}
