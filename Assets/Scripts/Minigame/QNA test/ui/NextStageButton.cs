using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace Minigame_QnA_Test
{
    public class NextStageButton : MonoBehaviour
    {
        public Puzzle minigameManager;
        public TextMeshProUGUI progression;
        public TextMeshProUGUI icon;





        public void InvokeNewBoad()
        {
            if (minigameManager.achieved < minigameManager.maxIteration)
            {
                GameObject myEventSystem = GameObject.Find("EventSystem");
                myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);

                minigameManager.NextStage();

                progression.text = "Next\n" + minigameManager.achieved + "/" + minigameManager.maxIteration;
                if (minigameManager.achieved == minigameManager.maxIteration)
                {
                    FinishButton();
                }
            }
            else
            {
                FinishButton();
            }
        }

        public void FinishButton()
        {
            progression.text = "Submit\n" + minigameManager.achieved + "/" + minigameManager.maxIteration;

            minigameManager.UpdateProgress();

            gameObject.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (minigameManager.CompareGivenAnswers())
                {
                    minigameManager.uiManager.VictoryDisclaimer();
                }
                else if(minigameManager.achieved == minigameManager.maxIteration)
                {
                    minigameManager.uiManager.GameOverDisclaimer();
                }
            });
        }


        public void MinigameDone(bool done)
        {
            minigameManager.done = done;
            minigameManager.UpdateProgress();
            minigameManager.quit = true;
            minigameManager.CheckProgress();
        }
    }
}