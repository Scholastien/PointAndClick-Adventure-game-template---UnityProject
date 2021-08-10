using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownmapBehaviour : MonoBehaviour {

    public GameObject BackButton;
    public ChainTrigger showBackButton;



    //private void Start()
    //{
    //    BackButton.SetActive(showBackButton.triggered);

    //    BackButton.GetComponent<Image>().enabled = showBackButton;
    //}


    //private void Update()
    //{
    //    BackButton.SetActive(showBackButton.triggered);


    //    BackButton.GetComponent<Image>().enabled = showBackButton;
    //}

    //private void LateUpdate()
    //{
    //    BackButton.SetActive(showBackButton.triggered);


    //    BackButton.GetComponent<Image>().enabled = showBackButton;
    //}

    private void FixedUpdate()
    {
        BackButton.SetActive(!showBackButton.triggered);


        //BackButton.GetComponent<Image>().enabled = showBackButton;
    }

}
