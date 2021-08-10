using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Minigame_QnA_Test
{
    public class BlocksSelection : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        public Puzzle manager;
        public BoardSelection boardSelection;

        public Material defaultMat;
        public Material selectedMat;

        public Material mouseOverMat;

        protected MeshRenderer meshRenderer;


        #region MonoBehaviour

        public void Start()
        {
            boardSelection = Puzzle.instance.boardSelection;
            meshRenderer = gameObject.GetComponent<MeshRenderer>();
        }

        #endregion



        public void ApplySelectedMaterial()
        {
            boardSelection = Puzzle.instance.boardSelection;
            meshRenderer.enabled = true;
            meshRenderer.material = defaultMat;



        }

        public void ApplyDefaultMaterial()
        {
            boardSelection = Puzzle.instance.boardSelection;
            meshRenderer.enabled = true;
            meshRenderer.material = selectedMat;
        }

        public void DisableMeshRenderer()
        {
            boardSelection = Puzzle.instance.boardSelection;
            meshRenderer.enabled = false;
        }

        public virtual void OnPointerEnter(PointerEventData eventData) { }
        public virtual void OnPointerExit(PointerEventData eventData) { }
        public virtual void OnPointerDown(PointerEventData eventData) { }
    }

}