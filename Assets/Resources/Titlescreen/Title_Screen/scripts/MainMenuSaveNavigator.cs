using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TitleScreen
{
    [RequireComponent(typeof(SavePreviewBehaviour))]
    public class MainMenuSaveNavigator : MonoBehaviour, IPointerEnterHandler
    {
        private SavePreviewBehaviour previewBehaviour;

        public AnimationController animationController;
        public Transform BoardToMove;

        public Vector3 original_position;
        public float Z_step;

        // Start is called before the first frame update
        void Start()
        {
            previewBehaviour = gameObject.GetComponent<SavePreviewBehaviour>();
            animationController = FindObjectOfType<AnimationController>();
            BoardToMove = animationController.Ground;
            original_position = BoardToMove.position;
        }
        

        public void OnPointerEnter(PointerEventData eventData)
        {
            animationController = FindObjectOfType<AnimationController>();
            BoardToMove = animationController.Ground;
            Vector3 destination = original_position - new Vector3(0, 0, (previewBehaviour.ID+1) * Z_step);
            //BoardToMove.position = original_position - new Vector3(0, 0, (previewBehaviour.ID+1) * Z_step);
            Debug.LogWarning("enter");
            animationController.StartCoroutine(animationController.GoToward(BoardToMove, BoardToMove.position, destination, 0.5f));
        }
    }
}