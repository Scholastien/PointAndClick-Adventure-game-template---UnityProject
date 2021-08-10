using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TitleScreen
{
    public class CameraController : MonoBehaviour
    {

        private float x, y, z;
        private int width, height;
        public Transform target;
        private float defaultXPos;

        public float offsetX, offsetY;

        public AnimationController animationController;

        void Start()
        {
            width = Screen.width;
            height = Screen.height;
            defaultXPos = target.localPosition.x;
            y = target.localPosition.y;
            z = target.localPosition.z;
        }
        void Update()
        {
            if(!animationController.isAnimated)
                MakeCircle(Input.mousePosition);
            
        }

        private void MakeCircle(Vector3 mousePos)
        {
            x = defaultXPos;
            float vX = -((width / 2) - mousePos.x);
            float vY = -((height / 2) - mousePos.y);
            Vector2 v = new Vector2(vX, vY);
            //Vector2 scale = new Vector2(vX, vY) / new Vector2(-width / 2, -height / 2);
            Vector2 scale = new Vector2(vX / (-width / 2), vY / (-height / 2));
            float magnitudeScale = GetVectorMagnitude(scale);
            if ((mousePos.y > 0 && mousePos.y < height) && (mousePos.x > 0 && mousePos.x < width))
            {
                y = Mathf.Sin(calculateAngle(v)) * magnitudeScale;
                z = Mathf.Cos(calculateAngle(v)) * magnitudeScale;
            }
            if (vY > 0)
                target.localPosition = new Vector3(x, y, z);
            else
                target.localPosition = new Vector3(x, -y, z);
        }

        private float calculateAngle(Vector2 v)
        {
            // Θ = cos-1 ( ( u . v ) / ||u|| . ||v||)
            Vector2 u = new Vector2(1, 0);
            float uv = (u.x * v.x) + (u.y * v.y);
            float uMagnitude = GetVectorMagnitude(u);
            float vMagnitude = GetVectorMagnitude(v);
            float Θ = Mathf.Acos(uv / (uMagnitude * vMagnitude));
            return Θ;
        }
        private float GetVectorMagnitude(Vector2 a)
        {
            // ||a|| = √ a.x² + a.y²
            return Mathf.Sqrt((a.x * a.x) + (a.y * a.y));
        }
    }
}