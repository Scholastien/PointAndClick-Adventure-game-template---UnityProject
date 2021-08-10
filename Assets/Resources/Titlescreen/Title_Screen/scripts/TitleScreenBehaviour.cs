using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TitleScreen
{

    [RequireComponent(typeof(CameraController), typeof(MenuDragger), typeof(AnimationController))]
    public class TitleScreenBehaviour : MonoBehaviour
    {
        private CameraController cameraController;
        private MenuDragger menuDragger;

        public Animator titleAnimator;
        public Animator cameraAnimator;

        // Start is called before the first frame update
        void Start()
        {
            cameraController = gameObject.GetComponent<CameraController>();
            menuDragger = gameObject.GetComponent<MenuDragger>();

            //titleAnimator.Play("TitleScreen_titleFadeIn");
            //cameraAnimator.Play("TitleScreen_CameraIdleTop");
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}