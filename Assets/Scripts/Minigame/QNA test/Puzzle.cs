using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Minigame_QnA_Test
{
    [System.Serializable]
    public class Materials
    {
        public List<Material> material;
    }


    public class Puzzle : MinigameBehaviour
    {
        public static Puzzle instance = null;

        // -8 8 x
        // 4 -1 y
        public Sprite imageBorder;
        public Transform SelectionUI_holder;

        // original image 7*8
        public BoardSelection boardSelection;

        public Camera sceneCamera;

        public Texture2D filler;
        public Texture2D outline;

        public Shader shader;
        public Material outlineMat;
        [SerializeField]
        public List<Materials> materials;

        public bool populate = true;
        public bool destroy = true;

        public Block[] emptyBlocks; // must be replace by the right tile
        public Block[,] blocks;     // actual board
        public List<GameObject> board;

        [Range(4, 8)]
        public int columnCount = 7;
        [Range(4, 8)]
        public int lineCount = 8;
        public float margin = 2;

        public Transform CollidersHolder;
        public List<GameObject> answers;

        public Transform Questions;

        public int numberOfQuestions = 3;
        public List<Question> questionsList;

        // public Question question1;
        // public Question question2;
        // public Question question3;

        [Header("Materials")]
        public Material unselectedMat;
        public Material mouseOverMat;
        public Material ClickedMaterial;
        public Material LockedMaterial;
        public Material CorrectMaterial;
        public Material WrongMaterial;
        public Material transparentMat;

        public Sprite RedCross;
        public Sprite CheckMark;

        [Header("Progression")]
        public int maxIteration = 3;
        public int achieved = 1;
        public int goodAnswers = 0;
        public bool win = false;
        public bool quit = false;

        [Header("ui")]
        public QNA_UI_manager uiManager;

        [Header("Animations")]
        public BoardAnimator boardAnimator;

        public Progression progression;
        float cameraSize = 0;

        public void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }


        void Start()
        {
            progression = new Progression();
            columnCount = progression.ModifyDifficulty();
            lineCount = progression.ModifyDifficulty();

            boardSelection = new BoardSelection(maxIteration);
            board = new List<GameObject>();

            if (GameManager.instance != null)
                sceneCamera = GameManager.instance.GetComponent<Camera>();
            cameraSize = sceneCamera.orthographicSize;
            sceneCamera.orthographicSize = (lineCount + margin);

            // Generate Board with starting conditions from the main game;
            Populate();


            uiManager.SetMinigameManager(this);
        }


        public void LateUpdate()
        {
            if (!boardAnimator.animated)
            {
                uiManager.SetMinigameManager(this);
                uiManager.SetGridCollider(uiManager.isDisclaimerOpen);
                uiManager.SetButtonInterratable(uiManager.isDisclaimerOpen && !boardAnimator.animated);
            }
        }

        #region BoardGeneration
        public void Populate()
        {

            SetBoardHolderSize();

            blocks = new Block[columnCount, lineCount];
            Tile[,] tiles = TileCreator.GetTiles(columnCount, lineCount);
            for (int y = 0; y < lineCount; y++)
            {
                for (int x = 0; x < columnCount; x++)
                {
                    GameObject blockObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    board.Add(blockObject);
                    //blockObject.transform.position = -Vector2.one * new Vector2(lineCount - 1, columnCount - margin - 6) + new Vector2(x * 2, y);
                    blockObject.transform.localPosition = (-new Vector2(1, 0) * (columnCount - 1) * 0.5f + -new Vector2(0, 1) * (lineCount - 1) * 0.5f + new Vector2(x, y)) * new Vector2(2.2f, 1.2f);
                    blockObject.transform.localPosition = blockObject.transform.localPosition + new Vector3(0, transform.position.y, -3);
                    //blockObject.transform.position = -new Vector2(lineCount - 1, columnCount) * 0.5f + new Vector2(x * 2, y);
                    blockObject.transform.localScale = new Vector3(2, 1, 1);

                    //Debug.LogWarning("BlockObject Y : " + blockObject.transform.position.y);

                    blockObject.transform.parent = transform;
                    blockObject.name = "Block [" + x + "," + y + "]";

                    Block block = blockObject.AddComponent<Block>();


                    //int indexMat = (int)Random.Range(0, materials.Count - 1);
                    block.Init(new Vector2Int(x, y), tiles[x, y], filler, outline, shader, materials);

                    blocks[x, y] = block;
                }
            }

            transform.position = new Vector3(0, (lineCount - 1) * 0.5f, 1);


            CreateButtons();
            CreateQuestions();


            boardAnimator.SpawnAnimation(blocks, columnCount, lineCount);
        }


        private void CreateButtons()
        {

            answers = new List<GameObject>();

            int i = 0;
            //float coordX = -(columnCount-2);
            // columnCount = 4
            //  y     =    -2.2f +4x
            // -8x + y     =    -6.6f
            float coordX = -(columnCount * 1.1f) + 2.2f;
            //line count = 4
            // 4x + y = 0.3
            // y = 0.3 -4x
            // 8x + y = -0.1
            // 8x -4x = -0.4
            // x = -0.1
            // y = 0.3 + 0.4 = 0.7
            float coordY = -(lineCount * 0.1f) + 0.7f;


            for (int y = 0; y < lineCount; y++)
            {
                for (int x = 0; x < columnCount; x++)
                {
                    if (x + 1 < columnCount && y + 1 < lineCount)
                    {

                        GameObject colliderObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
                        board.Add(colliderObject);
                        DestroyImmediate(colliderObject.GetComponent<MeshCollider>());
                        colliderObject.transform.parent = CollidersHolder;
                        colliderObject.name = "Button[" + i + "]";

                        colliderObject.GetComponent<MeshRenderer>().material = ClickedMaterial;
                        colliderObject.GetComponent<MeshRenderer>().enabled = false;

                        colliderObject.transform.localPosition = new Vector3(coordX, coordY, 0);
                        colliderObject.transform.localScale = new Vector3(4.5f, 2.5f, 1);

                        //EdgeCollider2D collider = colliderObject.AddComponent<EdgeCollider2D>();
                        BoxCollider2D collider = colliderObject.AddComponent<BoxCollider2D>();
                        collider.size = new Vector2(0.5f, 0.5f);
                        Vector2[] tempArray = new Vector2[5];

                        // blocks[x, y];
                        //CreateButtonCollider(tempArray, x, y);
                        CreateAnswer(colliderObject.AddComponent<Answer>(), x, y);
                        colliderObject.GetComponent<Answer>().manager = this;
                        colliderObject.GetComponent<Answer>().boardSelection = boardSelection;
                        colliderObject.GetComponent<Answer>().defaultMat = transparentMat;

                        i++;
                        coordX += 2.2f;

                        answers.Add(colliderObject);
                    }
                }
                coordX = -(columnCount * 1.1f) + 2.2f;
                coordY += 1.2f;


            }
        }

        private void CreateAnswer(Answer answer, int x, int y)
        {
            answer.selection = new Selection(blocks, x, y);
        }

        private void CreateButtonCollider(Vector2[] points, int x, int y)
        {
            points[0] = new Vector2(blocks[x, y].transform.position.x / 2, blocks[x, y].transform.position.y);
            points[1] = new Vector2(blocks[x + 1, y].transform.position.x / 2, blocks[x + 1, y].transform.position.y);
            points[2] = new Vector2(blocks[x + 1, y + 1].transform.position.x / 2, blocks[x + 1, y + 1].transform.position.y);
            points[3] = new Vector2(blocks[x, y + 1].transform.position.x / 2, blocks[x, y + 1].transform.position.y);
            points[4] = new Vector2(blocks[x, y].transform.position.x / 2, blocks[x, y].transform.position.y);
        }

        private void CreateQuestions()
        {

            Questions.position = new Vector3(0, -(lineCount + 2) / 2f, 0);

            List<int> exclude = new List<int>();

            if (progression.currentSkillLevel == 0 && numberOfQuestions == 3)
            {
                numberOfQuestions = 1;
            }
            else if (progression.currentSkillLevel == 0)
            {
                numberOfQuestions++;
            }
            questionsList = new List<Question>();
            for (int i = 0; i < numberOfQuestions; i++)
            {
                questionsList.Add(CreateQuestion(exclude, ClickedMaterial));
                questionsList[i].name = "Question " + (i + 1);
                if (numberOfQuestions == 1)
                    questionsList[i].transform.localPosition = new Vector3(0, 0, 0);
                else if (numberOfQuestions == 2)
                    questionsList[i].transform.localPosition = new Vector3(-3 + 6 * i, 0, 0);
                else if (numberOfQuestions == 3)
                {
                    questionsList[i].transform.localPosition = new Vector3(-6 + 6 * i, 0, 0);
                }
            }
            //maxIteration = numberOfQuestions;
            boardSelection = new BoardSelection(maxIteration);

            // question1 = CreateQuestion(exclude, ClickedMaterial);
            // question2 = CreateQuestion(exclude, ClickedMaterial);
            // question3 = CreateQuestion(exclude, ClickedMaterial);


            // question1.transform.localPosition = new Vector3(-6, 0, 0);
            // question2.transform.localPosition = new Vector3(0, 0, 0);
            // question3.transform.localPosition = new Vector3(6, 0, 0);

            // question1.name = "Question1";
            // question2.name = "Question2";
            // question3.name = "Question3";

            boardAnimator.SpawnQuestions(questionsList);
        }

        private Question CreateQuestion(List<int> exclude, Material mat)
        {
            GameObject question = GameObject.CreatePrimitive(PrimitiveType.Quad);
            board.Add(question);
            question.GetComponent<MeshRenderer>().material = outlineMat;

            DestroyImmediate(question.GetComponent<MeshCollider>());
            question.transform.parent = Questions;
            // X = nbre de colonne x ratio de largeur + ecart
            // Y = nbre de ligne x ratio de hauteur + ecart
            float x = 2 * 2.2f + 0.5f;
            float y = 2 * 1.2f + 0.5f;
            question.transform.localScale = new Vector3(x, y, 1);
            Question q = question.AddComponent<Question>();
            q.defaultMat = unselectedMat;
            q.mouseOverMat = mouseOverMat;
            q.selectedMat = mat;
            q.manager = this;
            q.boardSelection = boardSelection;

            int index = Random.Range(0, answers.Count - 1);
            while (exclude.Contains(index))
            {
                index = Random.Range(0, answers.Count - 1);
            }
            exclude.Add(index);
            q.selection = answers[index].GetComponent<Answer>().selection;
            q.selection.InstantiateSelection(question.transform);

            question.AddComponent<BoxCollider2D>();

            return q;
        }

        public void SetBoardHolderSize()
        {
            //(columnCount + margin) * 0.85f
            //margin * 2


            //transform.position = new Vector3(0, (columnCount) * 0.5f, 1);
            transform.localScale = new Vector3(columnCount * 2.2f + 0.5f, lineCount * 1.2f + 0.5f, 1);
        }

        #endregion

        #region Replay

        public void DestroyChildren()
        {
            //foreach (Transform child in transform)
            //{
            //    DestroyImmediate(child.gameObject);
            //}

            foreach (GameObject go in board)
            {
                Destroy(go);
            }
        }


        #endregion

        #region WinCondition

        public bool CompareGivenAnswers()
        {
            bool result = true;
            foreach (Question q in questionsList)
            {
                if (!q.IsCorrect())
                    result = false;
            }
            return result;

            //return question1.IsCorrect() && question2.IsCorrect() && question3.IsCorrect();
        }

        public void NextStage()
        {
            if (CompareGivenAnswers())
            {
                goodAnswers++;
            }
            achieved++;
            boardAnimator.StopAllCoroutines();
            boardAnimator.StartCoroutine(boardAnimator.MoveBoardOnLeft(transform, Questions, this));

            DestroyChildren();
            //Populate();


        }

        public void Setup()
        {
            DestroyChildren();

            Populate();

            uiManager.SetMinigameManager(this);

            boardSelection = new BoardSelection(maxIteration);
        }

        #endregion

        #region MinigameBehaviour

        public override void CheckProgress()
        {
            if ((win || quit) && done)
            {

                // reset camera settings
                sceneCamera.orthographicSize = cameraSize;

                if (GameManager.instance != null)
                {

                    if (win)
                    {
                        GameManager.instance.managers.variablesManager.gameSavedData.GameInfo.mainCharacter.IncreaseSkillValue(SkillType.Study);
                    }


                    GameManager.instance.managers.minigameManager.EndMinigame(win);
                    GameManager.instance.gameState = GameState.Navigation;
                }
            }
        }

        public override void UpdateProgress()
        {
            win = CompareGivenAnswers();

        }

        #endregion
    }
}








