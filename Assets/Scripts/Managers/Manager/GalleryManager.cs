using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalleryManager : MonoBehaviour {

    public static GalleryManager instance = null;

    public bool debug_GalleryLog;

    #region MonoBehaviour

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(this);
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    #endregion


    #region ExternalFunctions

    public void StartDiaporama()
    {

    }

    public void StartDialogue()
    {

    }

    public void StartVideo()
    {

    }
    #endregion
}
