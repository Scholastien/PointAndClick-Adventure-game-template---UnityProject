using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TitleScreen
{
    public class AnimationController : MonoBehaviour
    {
        public Animator UIAnimator;


        public bool isAnimated;
        public bool skipToNextAnim;
        public bool firstAnimPlayed;

        public Transform Camera;
        public Transform Rail;
        public Transform Background;
        public Transform Title;
        public Transform Ground;

        public AnimationCurve ac;

        public MainMenuGroup menuGroup;

        private CoroutineQueue queue;

        public int SkipIndex = 0;

        private void Start()
        {
            menuGroup.animCtrl = this;
            queue = new CoroutineQueue(1, StartCoroutine);
            SetFirstSpawn(true);
            SetOpen(false);
        }

        public void Update()
        {

            

            bool open = UIAnimator.GetBool("Open");
            bool firstSpawn = UIAnimator.GetBool("FirstSpawn");
        }

        public void LateUpdate()
        {
            isSkipped();
        }

        public void isSkipped()
        {
            if (skipToNextAnim)
            {
                skipToNextAnim = false;
            }
        }

        #region AnimationCoroutine

        public IEnumerator Wait(float duration)
        {

            Debug.Log("start waiting");
            isAnimated = true;
            bool done = false;
            int i = 1;
            while (!done && !skipToNextAnim)
            {
                //isSkipped();
                if (i >= 5)
                    done = true;
                yield return new WaitForSeconds(duration / 5);
                Debug.Log("waiting");
                i++;
            }


            isAnimated = false;
        }


        public IEnumerator FadeIn(SpriteRenderer sr, float fadeDuration)
        {

            float alpha = 0f;
            Color color = sr.color;


            for (float i = 0; i <= fadeDuration; i += Time.deltaTime)
            {
                if (skipToNextAnim)
                {
                    //isSkipped();
                    sr.color = new Color(color.r, color.g, color.b, 1);
                    break;
                }


                // increase alpha
                alpha = i / fadeDuration;


                if (sr != null)
                    sr.color = new Color(color.r, color.g, color.b, alpha);
                else
                    break;
                yield return null;
            }

        }
        public IEnumerator GoToward(Transform t, Vector3 start, Vector3 end, float duration, bool allowCamMovement = false)
        {
            Debug.Log("Start gotoward");
            isAnimated = true;
            // start 0,17, 0
            // end   0, 0, 0

            t.position = start;



            bool done = false;
            Vector3 originalStart = start;


            while (!done)
            {
                //isSkipped();
                if (start.x == end.x && start.y == end.y && start.z == end.z)
                {
                    done = true;
                }

                if (start.x != end.x)
                    start.x = DeterminePosition(start.x, end.x, duration, originalStart.x);
                if (start.y != end.y)
                    start.y = DeterminePosition(start.y, end.y, duration, originalStart.y);
                if (start.z != end.z)
                    start.z = DeterminePosition(start.z, end.z, duration, originalStart.z);

                t.position = start;
                yield return null;
            }
            t.position = end;

            isAnimated = false;
        }

        IEnumerator CircleMove(Vector3 pos1, Vector3 pos2, AnimationCurve ac, float time)
        {
            float timer = 0.0f;
            while (timer <= time)
            {
                transform.position = Vector3.Lerp(pos1, pos2, ac.Evaluate(timer / time));
                timer += Time.deltaTime;
                yield return null;
            }
        }

        #endregion

        #region CoordinateCalculation

        public float DeterminePosition(float a, float b, float duration, float original)
        {
            float result = 0;

            // vector P = ( a, 0 ) 
            // vector Q = ( b, 0 )
            // d(P,Q) = √ (b - a)² + 0²

            float dist = Mathf.Sqrt((b - original) * (b - original));
            float speed = dist / duration;

            if (a > b)
            {
                result = a - Time.deltaTime * speed;
                if (result < b)
                    result = b;
            }
            else
            {
                result = a + Time.deltaTime * speed;
                if (result > b)
                    result = b;
            }
            return result;
        }
        #endregion


        #region AnimatorRequest

        public void SetFirstSpawn(bool firstSpawn)
        {
            UIAnimator.SetBool("FirstSpawn", firstSpawn);
        }

        public void SetOpen(bool open)
        {
            UIAnimator.SetBool("Open", open);
        }

        public string GetCurrentAnimationName()
        {
            var currentClip = UIAnimator.GetCurrentAnimatorClipInfo(0);
            return currentClip[0].clip.name;
        }


        public bool IsCurrentUITagAnimated(string tag)
        {
            return UIAnimator.GetCurrentAnimatorStateInfo(0).IsTag(tag);
        }



        #endregion




        



    }
}