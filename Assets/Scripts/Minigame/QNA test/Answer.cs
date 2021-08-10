using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Minigame_QnA_Test
{
    [System.Serializable]
    public class Selection
    {
        public Block block1;
        public Block block2;
        public Block block3;
        public Block block4;

        public Selection(Block[,] blocks, int x, int y)
        {
            block1 = blocks[x, y]; // -1,-5,0  1,-5,0
            block2 = blocks[x + 1, y];
            block3 = blocks[x + 1, y + 1];
            block4 = blocks[x, y + 1];
        }

        public void InstantiateSelection(Transform parent)
        {
            GameObject select1 = InstantiateBlock(parent, block1);
            GameObject select2 = InstantiateBlock(parent, block2);
            GameObject select3 = InstantiateBlock(parent, block3);
            GameObject select4 = InstantiateBlock(parent, block4);

            select1.transform.localPosition = new Vector3(-1.1f, -0.6f, 0);
            select2.transform.localPosition = new Vector3(1.1f, -0.6f, 0);
            select3.transform.localPosition = new Vector3(1.1f, 0.6f, 0);
            select4.transform.localPosition = new Vector3(-1.1f, 0.6f, 0);

            select1.transform.localScale = new Vector3(2, 1, 1);
            select2.transform.localScale = new Vector3(2, 1, 1);
            select3.transform.localScale = new Vector3(2, 1, 1);
            select4.transform.localScale = new Vector3(2, 1, 1);

            select1.transform.parent = parent;
            select2.transform.parent = parent;
            select3.transform.parent = parent;
            select4.transform.parent = parent;
        }

        private GameObject InstantiateBlock(Transform parent, Block block)
        {
            GameObject go = GameObject.Instantiate(block.gameObject);
            go.name = "Tile[" + block.coord.x + "," + block.coord.y + "]";
            GameObject.DestroyImmediate(go.GetComponent<MeshCollider>());

            return go;
        }

        public bool CompareSelection(Selection selection)
        {
            bool a = block1.CompareBlock(selection.block1);
            bool b = block2.CompareBlock(selection.block2);
            bool c = block3.CompareBlock(selection.block3);
            bool d = block4.CompareBlock(selection.block4);

            return a && b && c && d;
        }
    }


    public class Answer : BlocksSelection
    {
        public Selection selection;

        public bool selected;

        public Question question;

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (!selected && boardSelection.selectedQuestion != null)
            {
                selected = true;
                boardSelection.selectedQuestion.answer = this;
                question = boardSelection.selectedQuestion;
                selectedMat = Puzzle.instance.LockedMaterial;
                gameObject.GetComponent<MeshRenderer>().material = selectedMat;
                boardSelection.RegisterAnswer(this);


                manager.boardSelection.ApplyLayeredDisplay(this);

                boardSelection.selectedQuestion = null;

            }
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (boardSelection.selectedQuestion != null && boardSelection.isCurrentQuestionWithoutAnswer())
            {
                gameObject.GetComponent<MeshRenderer>().material = boardSelection.selectedQuestion.selectedMat;
                gameObject.GetComponent<MeshRenderer>().enabled = true;
            }
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            if (boardSelection.isCurrentQuestionWithoutAnswer())
            {
                if (!selected)
                    gameObject.GetComponent<MeshRenderer>().enabled = false;
                else
                    gameObject.GetComponent<MeshRenderer>().material = selectedMat;
            }
        }

        /// <summary>
        /// Update the material to the selected material
        /// put the asnwer in the selected question
        /// </summary>
        public void RegisterAnswer()
        {

            boardSelection.selectedQuestion.answer = this;
            boardSelection.RegisterAnswer(this);
            //ApplySelectedMaterial();
        }

        public void UnregisterAnswer()
        {
            selected = false;
            question.answer = null;
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
            ApplyDefaultMaterial();
            DisableMeshRenderer();
        }

        public void UpdateMaterial(bool show)
        {
            if (show)
            {
                gameObject.GetComponent<MeshRenderer>().material = boardSelection.selectedQuestion.selectedMat;
                gameObject.GetComponent<MeshRenderer>().enabled = true;

            }
            else
            {
                gameObject.GetComponent<MeshRenderer>().enabled = false;

            }
        }
    }
}