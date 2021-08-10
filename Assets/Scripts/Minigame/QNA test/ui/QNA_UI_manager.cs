using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Minigame_QnA_Test
{
    [System.Serializable]
    public class QNA_UI_manager
    {
        public Button Next;
        public Button Quit;

        private Transform buttonHolder;

        private List<Question> Questions;

        public GameObject Disclaimer;
        public GameObject victory;
        public GameObject quit;
        public GameObject gameOver;
        public bool isDisclaimerOpen = false;

        internal void SetMinigameManager(Puzzle puzzle)
        {
            Next.GetComponent<NextStageButton>().minigameManager = puzzle;
            Quit.GetComponent<QNA_QuitButton>().minigameManager = puzzle;
            buttonHolder = puzzle.CollidersHolder;
            Questions = puzzle.questionsList;
        }

        #region DisclaimersSpawn

        public void QuitDisclaimer()
        {
            isDisclaimerOpen = true;
            Disclaimer.SetActive(true);
            victory.SetActive(false);
            gameOver.SetActive(false);
            quit.SetActive(true);
        }

        public void VictoryDisclaimer()
        {
            isDisclaimerOpen = true;
            Disclaimer.SetActive(true);
            victory.SetActive(true);
            quit.SetActive(false);
            gameOver.SetActive(false);
        }

        public void GameOverDisclaimer()
        {
            isDisclaimerOpen = true;
            Disclaimer.SetActive(true);
            victory.SetActive(false);
            quit.SetActive(false);
            gameOver.SetActive(true);
        }

        public void VanishDisclaimer()
        {
            isDisclaimerOpen = false;
            Disclaimer.SetActive(false);
            victory.SetActive(false);
            quit.SetActive(false);
            gameOver.SetActive(false);
        }

        #endregion

        #region Interractivity

        // Disable/enable grid collider depending of the state of the ui ( isDisclaimerOpen )
        public void SetGridCollider(bool state)
        {
            if (!Questions.Contains(null))
            {
                foreach (Transform child in buttonHolder)
                {
                    child.GetComponent<BoxCollider2D>().enabled = !state;
                }
                foreach (Question q in Questions)
                    q.GetComponent<BoxCollider2D>().enabled = !state;
            }
        }

        public void SetButtonInterratable(bool state)
        {
            Next.interactable = !state;
            Quit.interactable = !state;
        }

        #endregion

    }
}