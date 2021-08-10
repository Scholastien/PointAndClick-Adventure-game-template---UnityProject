using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneReferenceInEditor;

[RequireComponent(typeof(MinigameManager)), ExecuteInEditMode]
public class MinigameEditorUpdater : MonoBehaviour {

    private MinigameManager minigameManager;

	// Use this for initialization
	void Start () {
        minigameManager = gameObject.GetComponent<MinigameManager>();
    }
	
	// Update is called once per frame
	void Update () {

#if UNITY_EDITOR

        foreach (Minigame minigame in minigameManager.minigames)
        {
            minigame.SetNameAndPath(minigameManager.MinigamesAssetPath);
        }


        if (minigameManager.currentMinigame.scene.IsValidSceneAsset)
            minigameManager.currentMinigame.SetNameAndPath(minigameManager.MinigamesAssetPath);
        else
            minigameManager.currentMinigame.path = string.Empty;
#endif

    }
}
