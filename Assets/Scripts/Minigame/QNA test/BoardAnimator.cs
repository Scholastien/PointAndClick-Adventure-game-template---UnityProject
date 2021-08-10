using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Minigame_QnA_Test
{
    public class BoardAnimator : MonoBehaviour
    {
        public bool showNext = false;
        public float speed = 5;
        public bool animated = false;

        private Vector2 originaNextPos;
        private Vector2 originaQuitPos;


        private void Start()
        {
            //boardAnimator.StartCoroutine(boardAnimator.MoveBoardOnLeft(transform, Questions, this));
            Puzzle p = Puzzle.instance;
            //StartCoroutine(MoveBoardOnLeft(p.transform, p.Questions, p));

            StartCoroutine(SetupUI(Puzzle.instance.uiManager.Quit.GetComponent<RectTransform>()));
        }


        #region boardAnimation


        public void SpawnAnimation(Block[,] blocks, int columnCount, int lineCount)
        {
            GameObject[,] gameObjects = new GameObject[columnCount, lineCount];
            for (int y = 0; y < lineCount; y++)
            {
                for (int x = 0; x < columnCount; x++)
                {
                    blocks[x, y].gameObject.SetActive(false);
                    gameObjects[x, y] = blocks[x, y].gameObject;
                }
            }

            StartCoroutine(SpawnBoard(gameObjects, columnCount, lineCount));

        }

        public IEnumerator SpawnBoard(GameObject[,] gameObjects, int columnCount, int lineCount)
        {
            animated = true;
            bool done = false;

            //// Gradually spawn each pieces
            //for (int y = 0; y < lineCount; y++)
            //{
            //    for (int x = 0; x < columnCount; x++)
            //    {

            //        gameObjects[x, y].SetActive(true);
            //        yield return null;
            //    }
            //}

            List<GameObject> list = new List<GameObject>();
            for (int y = 0; y < lineCount; y++)
            {
                for (int x = 0; x < columnCount; x++)
                {
                    list.Add(gameObjects[x, y]);
                }
            }
            // random spawn
            while (!done)
            {
                if (list.Count == 0)
                {
                    done = true;
                }
                else
                {
                    int index = Random.Range(0, list.Count);

                    if (list[index] != null)
                    {
                        list[index].SetActive(true);
                        list.Remove(list[index]);
                    }
                }

                yield return null;
            }
            animated = false;
        }

        #endregion

        #region questionAnimation

        public void SpawnQuestions(List<Question> questions)
        {
            foreach (Question q in questions)
            {
                q.gameObject.SetActive(false);
            }

            StartCoroutine(QuestionAnimation(questions));
        }

        public IEnumerator QuestionAnimation(List<Question> questions)
        {


            animated = true;
            foreach (Question q in questions)
            {
                if (q != null)
                {
                    q.gameObject.SetActive(true);
                    Vector3 originalPos = q.transform.position;
                    q.transform.position = new Vector3(originalPos.x, originalPos.y - 5, originalPos.z);

                    bool done = false;

                    while (!done)
                    {
                        if (q != null)
                        {
                            if (q.transform.position == originalPos)
                                done = true;
                            q.transform.position = new Vector3(0, 1, 0) * speed * Time.deltaTime + q.transform.position;

                            if (q.transform.position.y > originalPos.y)
                            {
                                q.transform.position = originalPos;
                            }
                        }
                        else
                            done = true;



                        yield return null;
                    }
                }
            }
            animated = false;
        }

        #endregion

        #region UiAnimation

        public void HideUI(QNA_UI_manager UI_Manager, bool hide)
        {
            if (hide)
                StartCoroutine(
                    HideButtons(UI_Manager.Next.GetComponent<RectTransform>(),
                                UI_Manager.Quit.GetComponent<RectTransform>()));
            else
                StartCoroutine(ShowButtons(UI_Manager.Next.GetComponent<RectTransform>(),
                                UI_Manager.Quit.GetComponent<RectTransform>()));
        }

        public IEnumerator HideButtons(RectTransform next, RectTransform quit)
        {

            float nextMax_X = 1050f;
            float quitMax_Y = 500f;


            bool done = false;

            while (!done)
            {
                if (next.anchoredPosition.x > nextMax_X)
                {
                    done = true;
                }
                yield return null;
                next.anchoredPosition = new Vector2(1, 0) * speed * 60 * Time.deltaTime + next.anchoredPosition;
                quit.anchoredPosition = new Vector2(0, 1) * speed * 60 * Time.deltaTime + quit.anchoredPosition;
            }
        }

        public IEnumerator ShowButtons(RectTransform next, RectTransform quit)
        {

            bool done = false;
            StartCoroutine(ShowNext(next));
            while (!done)
            {
                if (quit.anchoredPosition.y < originaQuitPos.y)
                {
                    done = true;
                }

                yield return null;
                quit.anchoredPosition = new Vector2(0, -1) * speed * 60 * Time.deltaTime + quit.anchoredPosition;
            }
            quit.anchoredPosition = originaQuitPos;
        }

        public IEnumerator ShowNext(RectTransform next)
        {
            BoardSelection bs = Puzzle.instance.boardSelection;
            //Debug.LogErrorFormat("!bs.isCurrentQuestionWithoutAnswer() " +  !bs.isCurrentQuestionWithoutAnswer());
            yield return new WaitUntil(() => showNext);
            showNext = false;
            bool done = false;
            while (!done)
            {
                if (next.anchoredPosition.x < originaNextPos.x)
                {
                    done = true;
                }

                yield return null;
                next.anchoredPosition = new Vector2(-1, 0) * speed * 60 * Time.deltaTime + next.anchoredPosition;
            }
            next.anchoredPosition = originaNextPos;
        }


        public IEnumerator SetupUI(RectTransform quit)
        {
            originaNextPos = Puzzle.instance.uiManager.Next.GetComponent<RectTransform>().anchoredPosition;
            originaQuitPos = Puzzle.instance.uiManager.Quit.GetComponent<RectTransform>().anchoredPosition;

            HideUI(Puzzle.instance.uiManager, true);

            yield return new WaitWhile(() => quit.anchoredPosition.y < originaQuitPos.y);


            HideUI(Puzzle.instance.uiManager, false);
        }

        #endregion

        #region NextAnimation

        public IEnumerator MoveBoardOnLeft(Transform board, Transform questions, Puzzle puzzle)
        {

            originaNextPos = puzzle.uiManager.Next.GetComponent<RectTransform>().anchoredPosition;
            originaQuitPos = puzzle.uiManager.Quit.GetComponent<RectTransform>().anchoredPosition;

            while (animated)
            {
                yield return null;
            }

            HideUI(puzzle.uiManager, true);

            animated = true;
            float max_XPos = -30f;
            Vector3 originalBoardPos = board.position;
            Vector3 originalQuestionPos = questions.position;

            bool done = false;

            while (!done)
            {
                board.transform.position = new Vector3(-1, 0, 0) * speed * 5 * Time.deltaTime + board.transform.position;
                questions.transform.position = new Vector3(-1, 0, 0) * speed * 5 * Time.deltaTime + questions.transform.position;

                if (board.position.x < max_XPos || questions.position.x < max_XPos)
                {
                    board.position = originalBoardPos;
                    questions.position = originalQuestionPos;
                    done = true;
                }

                yield return null;
            }
            HideUI(puzzle.uiManager, false);




            animated = false;


            puzzle.Setup();
        }

        #endregion
    }
}
