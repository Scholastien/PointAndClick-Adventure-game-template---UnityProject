using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class GalleryUiBehaviour : MonoBehaviour {

    ScrollRect scrollRect;

    // Use this for initialization
    void Start () {
        scrollRect = gameObject.GetComponent<ScrollRect>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        scrollRect = gameObject.GetComponent<ScrollRect>();
        scrollRect.horizontalScrollbar.value = 0f;
        scrollRect.horizontalNormalizedPosition = 0f;
        scrollRect.horizontalScrollbar.value = 0f;
        scrollRect.horizontalNormalizedPosition = 0f;
    }
}
