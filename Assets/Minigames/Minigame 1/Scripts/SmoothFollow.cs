// Smooth towards the target

using UnityEngine;
using System.Collections;

public class SmoothFollow : MonoBehaviour
{
    public float distance = 5f;
    public Transform target;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    private float update;
    public int frame = 0;

    private void Start()
    {
        Application.targetFrameRate = 60;
    }

    // 60fps
    // 0.3 x 36 = 10.8 sec for a complete transition
    // 5 x 36 = 180.0 sec for a complete transition
    // * x 36 = 5
    void Update()
    {
        // Define a target position above and behind the target transform
        Vector3 targetPosition = target.TransformPoint(new Vector3(-distance * 2, 0, 0));

        // Smoothly move the camera towards that target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);


        //update += Time.deltaTime;
        //frame++;
        //Debug.Log(frame + " : " + update);
        //if (update > 1.0f)
        //{
        //    update = 0.0f;
        //    Debug.Log("Update");
        //    frame = 0;
        //}
    }
}