using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    [CreateAssetMenu(fileName = "new CharacterMetReward", menuName = "LewdOwl/QuestSystem/Reward/CharacterMet")]
    public class CharacterMetReward : AbstractReward
    {
        public CharacterFunction character;

        public override void RunReward()
        {
            GameManager.instance.managers.variablesManager.gameSavedData.GameInfo.MetCharacter(character);
            given = true;
        }
    }
}