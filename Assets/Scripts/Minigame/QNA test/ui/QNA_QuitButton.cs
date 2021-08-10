using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame_QnA_Test
{
    public class QNA_QuitButton : MonoBehaviour
    {
        public Puzzle minigameManager;

        public void QuitMinigame()
        {
            minigameManager.quit = true;
            minigameManager.done = true;
            minigameManager.CheckProgress();
        }

        public void QuitDisclaimer()
        {
            minigameManager.uiManager.QuitDisclaimer();
        }

        public void ResumeGame()
        {
            minigameManager.uiManager.VanishDisclaimer();
        }
    }
}