using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TitleScreen
{

    public class BackgroundBehaviour : MonoBehaviour
    {
        public Transform animatedCamera;

        public Vector3 offset;
        public Vector3 defaultPosition;

        // Start is called before the first frame update
        void Start()
        {
            offset = CalcuateDistanceOffset(transform.position, animatedCamera.position);
            defaultPosition = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            // if i should anim on the floor
            if (animatedCamera.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsTag("ClampBG"))
            {
                transform.parent = animatedCamera;
                transform.position = animatedCamera.transform.position + offset;
            }
            else
            {
                transform.parent = animatedCamera.parent;
                transform.position = defaultPosition;
            }
        }

        public Vector3 CalcuateDistanceOffset(Vector3 u, Vector3 v)
        {
            // dx = squareroot ( a.x - b.x)²
            // dy = squareroot ( a.y - b.y)²
            // dz = squareroot ( a.z - b.z)²
            return new Vector3(CalculateDistance(u.x, v.x), CalculateDistance(u.y, v.y), CalculateDistance(u.z, v.z));
        }
        //                                   0        3
        public float CalculateDistance(float a, float b)
        {
            // dx = squareroot ( a.x - b.x)²
            if (a < b)
                return Mathf.Sqrt((a - b) * (a - b));
            else
                return -Mathf.Sqrt((a - b) * (a - b));
        }
    }
}