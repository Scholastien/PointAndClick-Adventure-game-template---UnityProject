using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TitleScreen
{
    /// <summary>
    /// Queue coroutines to handle camera anim on title screen
    /// Contains buttons methods for camera movement 
    /// </summary>
    public class TS_CameraPositionHandler : MonoBehaviour
    {
        public SmoothFollow camera_sf;
        public bool skip = false;
        public float smoothTime = 0.3f;

        [Header("Camera Position")]
        public Transform firstPosition;
        public Transform titlePosition;
        public Transform downPosition;
        public Transform defaultPosition;

        [Header("Props")]
        public SpriteRenderer logo;
        public SpriteRenderer title;
        public TextMeshPro disclaimer;

        [Header("References")]
        public AnimationController animationController;


        private CoroutineQueue queue;

        #region Monobehaviour

        private void Start()
        {
            Color color = new Color(1, 1, 1, 0);
            logo.color = color;
            title.color = color;
            disclaimer.color = color;

            camera_sf.smoothTime = smoothTime;


            queue = new CoroutineQueue(1, StartCoroutine);

            if(GameManager.instance != null)
            {
                if (GameManager.instance.returnMainMenu)
                {
                    StartCoroutine(SpawnTitle());

                    StartCoroutine(ShiftRight());
                }
                else
                {
                    queue.Run(SpawnLogo());
                    queue.Run(SpawnDisclaimer());
                    queue.Run(SpawnTitle());
                    queue.Run(TranslateDown());
                    queue.Run(ShiftRight());
                }
            }
            else
            {
                queue.Run(SpawnLogo());
                queue.Run(SpawnDisclaimer());
                queue.Run(SpawnTitle());
                queue.Run(TranslateDown());
                queue.Run(ShiftRight());
            }
        }

        private void Update()
        {
            if (Input.GetButton("Submit"))
            {
                skip = true;
            }
        }

        #endregion

        #region FirstAnimations

        public IEnumerator ZoomOut()
        {
            camera_sf.transform.position = firstPosition.position;
            camera_sf.smoothTime = 2f;
            camera_sf.target = titlePosition;
            yield return new WaitForSecondsRealtime(3f);
            camera_sf.enabled = true;
            // callback Animation : Spawn Main menu

        }

        public IEnumerator SpawnLogo()
        {
            float wait = 0.05f;
            // Spawn game studio logo
            Color color = new Color();
            color = new Color(1, 1, 1, 0);
            while (color.a < 1)
            {
                color.a += Time.deltaTime;
                logo.color = color;
                yield return new WaitForSeconds(wait);


            }
            yield return new WaitForSecondsRealtime(3f);


            color = new Color(1, 1, 1, 1);
            while (color.a > 0)
            {
                color.a -= Time.deltaTime;
                logo.color = color;
                yield return new WaitForSeconds(wait);

            }
        }

        public IEnumerator SpawnDisclaimer()
        {
            float wait = 0.05f;

            // Spawn disclaimer
            Color color = new Color(1, 1, 1, 0);
            while (color.a < 1)
            {
                color.a += Time.deltaTime;
                disclaimer.color = color;
                yield return new WaitForSeconds(wait);

            }
            yield return new WaitForSecondsRealtime(3f);
            wait = 0f;


            color = new Color(1, 1, 1, 1);
            while (color.a > 0f)
            {
                color.a -= Time.deltaTime;
                disclaimer.color = color;
                yield return new WaitForSeconds(wait);

            }
        }


        public IEnumerator SpawnTitle()
        {
            StartCoroutine(ZoomOut());
            float wait = 0.05f;
            // Spawn game studio logo
            Color color = new Color();
            // Spawn tile
            color = new Color(1, 1, 1, 0);
            while (color.a < 1)
            {
                color.a += Time.deltaTime;
                title.color = color;
                yield return new WaitForSeconds(wait);
            }
            yield return new WaitForSecondsRealtime(3f);
            Debug.Log("Spawn Title");
        }

        public IEnumerator TranslateDown()
        {
            camera_sf.smoothTime = 0.5f;
            camera_sf.target = downPosition;
            yield return new WaitForSeconds(0.75f);
            camera_sf.smoothTime = 0.3f;
            // callback Animation : Spawn Main menu
        }

        public IEnumerator ShiftRight()
        {

            yield return new WaitForSeconds(0.25f);
            camera_sf.smoothTime = 0.5f;
            camera_sf.target = defaultPosition;
            yield return new WaitForSeconds(0.75f);
            camera_sf.smoothTime = 0.3f;

            animationController.SetOpen(true);
        }


        #endregion



        #region ButtonMethods

        public void NewCameraPosition(Transform newPos)
        {
            camera_sf.target = newPos;
        }

        #endregion
    }
}
