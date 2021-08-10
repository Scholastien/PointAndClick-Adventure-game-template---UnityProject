using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

	private float x, y, z;
	private int width, height;

	public Transform target;
	private float defaultXPos;


	// Use this for initialization
	void Start()
	{
		width = Screen.width;
		height = Screen.height;
		defaultXPos = target.localPosition.x;
	}

	// Update is called once per frame
	void Update()
	{

		Vector3 test = Input.mousePosition;

		x = defaultXPos;
		y = Mathf.Sin((test.y - (height / 2))/height);
		z = Mathf.Sin((test.x - (width / 2)) / width);


		target.localPosition = new Vector3(x, y, z);
	}
}