using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Minigame_QnA_Test
{

    [System.Serializable]
    public class QNA_Selection
    {
        public Question question;
        public Answer answer;

        public QNA_Selection(Question question)
        {
            this.question = question;
        }

        public bool isCorrect()
        {
            return answer.selection.CompareSelection(question.selection);
        }

        public void ApplyCheckMark()
        {
            Puzzle.instance.boardAnimator.showNext = true;

            GameObject go = new GameObject("Mark", typeof(SpriteRenderer));
            go.transform.parent = question.transform;
            go.transform.localScale = new Vector3(1, 1, 1);
            go.transform.localPosition = new Vector3(0, -0.75f, 0);
            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();

            if (isCorrect())
            {
                sr.sprite = Puzzle.instance.CheckMark;
                sr.color = Color.green;
                question.GetComponent<MeshRenderer>().material = Puzzle.instance.CorrectMaterial;
                answer.GetComponent<MeshRenderer>().material = Puzzle.instance.CorrectMaterial;
            }
            else
            {
                sr.sprite = Puzzle.instance.RedCross;
                sr.color = Color.red;
                question.GetComponent<MeshRenderer>().material = Puzzle.instance.WrongMaterial;
                answer.GetComponent<MeshRenderer>().material = Puzzle.instance.WrongMaterial;
            }
            question.transform.localScale = new Vector3(4.4f, 2.4f, 0f);
            answer.transform.localScale = new Vector3(4.4f, 2.4f, 0f);
        }
    }



    [System.Serializable]
    public class BoardSelection
    {
        public int maxIteration;
        public int QuestionsCount;

        public Question selectedQuestion = null;

        public List<Question> questionsQ;

        public List<QNA_Selection> selections;


        public BoardSelection(int iteration)
        {
            QuestionsCount = Puzzle.instance.questionsList.Count;
            maxIteration = iteration;
            selectedQuestion = null;
            questionsQ = new List<Question>();
            selections = new List<QNA_Selection>();
        }

        #region Select
        public void SelectQuestion(Question question)
        {

            if (selections == null || selections.Count == QuestionsCount)
            {
                selections = new List<QNA_Selection>();
            }
            if (selections.Count != QuestionsCount)
            {
                // If the question hasn't been selected yet
                if (!QuestionAlreadySelected(question))
                {
                    if (question == selectedQuestion)
                    {
                        UnselectQuestion(question);
                    }
                    else
                    {
                        if (isCurrentQuestionWithoutAnswer())
                        {
                            UnselectQuestion(selectedQuestion);
                        }
                    }

                    if (selections.Count == 0)
                    {
                        selections = new List<QNA_Selection>();
                    }
                    // Addin it to the selection list
                    //Debug.LogErrorFormat("Empty: Selecting {0}, \n seletion count {1} ", question.name, selections.Count);
                    selections.Add(new QNA_Selection(question));
                    selectedQuestion = question;
                    //Debug.LogErrorFormat("Added: Selecting {0}, \n seletion count {1} ", question.name, selections.Count);
                    // apply material Select
                    ApplySelectedMaterial(true);

                }
                else
                {
                    // If answer is not registered yet
                    if (GetSelection(question).answer == null)
                    {
                        // Unselect question
                        UnselectQuestion(question);
                    }
                }
                AddQuestionToQueue(question);
            }

        }
        public void AddQuestionToQueue(Question q)
        {
            if (questionsQ == null)
            {
                questionsQ = new List<Question>();
            }
            if (questionsQ.Contains(q))
            {
                questionsQ.Remove(q);
            }
            questionsQ.Insert(0, q);
        }

        public bool QuestionAlreadySelected(Question q)
        {
            return GetSelection(q) != null;
        }

        public QNA_Selection GetSelection(Question q)
        {
            if (selections.Count > 0)
            {
                IEnumerable<QNA_Selection> selectedEnum = selections.Where(c => c.question == q);
                if (selectedEnum.Count() > 0)
                {
                    return selectedEnum.First();
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public void UnselectQuestion(Question q)
        {
            questionsQ.Remove(q);
            ApplySelectedMaterial(false);
            selectedQuestion = null;
            selections.Remove(GetSelection(q));
        }

        public bool isCurrentQuestionWithoutAnswer()
        {
            if (selectedQuestion != null)
            {
                return GetSelection(selectedQuestion).answer == null;
            }
            else
            {
                if (selections.Count == QuestionsCount)
                    if (selections[QuestionsCount - 1].answer == null)
                        return true;

                return false;
            }

        }

        public bool showNextButton()
        {
            if (selections.Count == QuestionsCount)
            {
                bool max_Iteration = (selections.Count == QuestionsCount);
                bool lastRegistered = selections[QuestionsCount - 1].answer != null;
                bool result = (max_Iteration && lastRegistered);
                Debug.LogError(selections[0].question.name);
                return result;
            }
            else
                return false;
        }


        public void RegisterAnswer(Answer a)
        {
            GetSelection(selectedQuestion).answer = a;
            selectedQuestion.GetComponent<MeshRenderer>().material = Puzzle.instance.LockedMaterial;
            //a.GetComponent<MeshRenderer>().material = Puzzle.instance.LockedMaterial;
            VerifyAnswers();
        }



        #endregion


        #region Feedback

        public void ApplyLockedMaterial()
        {

        }
        public void ApplySelectedMaterial(bool selected)
        {
            if (selected)
            {
                selectedQuestion.GetComponent<MeshRenderer>().material = selectedQuestion.selectedMat;

            }
            else
            {
                selectedQuestion.GetComponent<MeshRenderer>().material = selectedQuestion.defaultMat;
            }
        }

        public void ApplyLayeredDisplay(Answer a)
        {

            for (int i = 0; i < questionsQ.Count; i++)
            {
                Vector3 v3 = questionsQ[i].answer.transform.localPosition;
                questionsQ[i].answer.transform.localPosition = new Vector3(v3.x, v3.y, 0) + new Vector3(0, 0, 0.1f) * (i + 1);

            }

        }

        public void VerifyAnswers()
        {
            // Verify once all answers are given
            if (selections.Count == QuestionsCount)
            {
                foreach (QNA_Selection select in selections)
                {
                    select.ApplyCheckMark();
                }
            }
        }

        #endregion







    }
}