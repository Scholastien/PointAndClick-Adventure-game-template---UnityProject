using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame_QnA_Test
{

    [System.Serializable]
    public class Progression
    {

        public int currentSkillLevel = 0;

        public Progression()
        {
            if (GameManager.instance != null)
            {
                currentSkillLevel = GameManager.instance.managers.variablesManager.gameSavedData.GameInfo.mainCharacter.GetSkillValue(SkillType.Study);
            }
            else
            {
                currentSkillLevel = 0;
            }
        }

        public int ModifyDifficulty()
        {

            if (currentSkillLevel > 7)
            {
                return 8;
            }
            else if (currentSkillLevel > 5)
            {
                return 7;
            }
            else if (currentSkillLevel > 3)
            {
                return 6;
            }
            else if (currentSkillLevel > 1)
            {
                return 5;
            }
            else
            {
                return 4;  // Increment number of answers possible for tutorial
            }
        }
    }

}