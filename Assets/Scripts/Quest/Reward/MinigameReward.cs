using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneReferenceInEditor;


namespace QuestSystem
{
    [CreateAssetMenu(fileName = "new MinigameReward", menuName = "LewdOwl/QuestSystem/Reward/MinigameReward")]
    public class MinigameReward : AbstractReward
    {
        public int index;
        public SceneReference miniGameScene;

        public override void RunReward()
        {
            Debug.Log(miniGameScene.ScenePath);
            GameManager.instance.managers.minigameManager.UpdateCurrentMinigame(miniGameScene);
            given = true;
        }
    }
}
