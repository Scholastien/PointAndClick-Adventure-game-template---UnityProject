using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Minigame_QnA_Test
{

    public class Question : BlocksSelection
    {

        public Selection selection;

        public Answer answer;

        public bool selected = false;

        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (!Puzzle.instance.boardSelection.QuestionAlreadySelected(this) && !Puzzle.instance.boardAnimator.showNext)
            {
                meshRenderer.enabled = true;
                meshRenderer.material = mouseOverMat;
            }
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            if (!Puzzle.instance.boardSelection.QuestionAlreadySelected(this) && !Puzzle.instance.boardAnimator.showNext)
            {
                meshRenderer.enabled = true;
                meshRenderer.material = defaultMat;
            }
        }


        public override void OnPointerDown(PointerEventData eventData)
        {
            if (!Puzzle.instance.boardAnimator.showNext)
            {
                manager.boardSelection.AddQuestionToQueue(this);
                //manager.boardSelection.SelectQuestion(this);
                boardSelection.SelectQuestion(this);
            }
        }

        public void UnselectQuestion()
        {
            Debug.LogWarning("Unselect question");
            ApplyDefaultMaterial();
        }

        public bool IsCorrect()
        {
            if (answer != null)
                return selection.CompareSelection(answer.selection);
            else
                return false;
        }


    }
}