using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneReferenceInEditor;

[System.Serializable]
public class MinigameLock
{
    public ChainTrigger lockDependency;
}

/// <summary>
/// Child of Interractible, inherite from every properties in the base class
/// </summary>
/// 
public class MinigameInterractible : Interractible
{
    [Header("Trigger Lock")]
    public MinigameLock locked;

    [Header("Minigame Info")]
    public SceneReference sceneReference;


    // Called by the Interractable manager
    public void StartMinigame()
    {
        if (locked.lockDependency != null)
        {


            if (!disable && !locked.lockDependency.triggered)
            {

                Debug.Log("Starting Minigame");
                GameManager.instance.managers.minigameManager.UpdateCurrentMinigame(sceneReference);

            }
        }
    }

    /// <summary>
    /// Function that should be linked to the button in the scene
    /// </summary>
    public override void Interract()
    {
        GameManager.instance.managers.minigameManager.currentMinigame = new Minigame(sceneReference);

        GameManager.instance.managers.minigameManager.currentMinigame.SetNameAndPath(GameManager.instance.managers.minigameManager.MinigamesAssetPath);

        base.Interract();
    } 
    

}
