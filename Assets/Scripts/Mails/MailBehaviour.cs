using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BikeRidingMinigame;

[RequireComponent(typeof(Button))]
public class MailBehaviour : MonoBehaviour {

    public MailReaderBehaviour MailReader;

    private Button buttonClickToRead;


    [Header("Referenced properties")]
    public Toggle toggleRead;
    public Image ToggleImg;

    public Image profilePic;

    public Text from;

    public Text Preview;

    [Header("Processed properties"), Space(20)]

    public Mail associatedMail;

	// Use this for initialization
	void Start () {
        buttonClickToRead = GetComponent<Button>();
	}

     void Update()
    {
        toggleRead.isOn = !associatedMail.alreadyRead;
        ToggleImg.gameObject.SetActive(toggleRead.isOn);

        if (!associatedMail.alreadyRead)
        {
            // Apply bold font
            from.fontStyle = FontStyle.Bold;
            Preview.fontStyle = FontStyle.Bold;
        }
        else
        {
            // Apply normal font
            from.fontStyle = FontStyle.Normal;
            Preview.fontStyle = FontStyle.Normal;
        }

        from.text = associatedMail.author;
        Preview.text = associatedMail.subject + " - " + associatedMail.content;
    }


    public void OpenMailReader()
    {
        associatedMail.alreadyRead = true;
        MailReader.mailToRead = associatedMail;
        MailReader.CreateContent();
        ComputerManager.instance.LoadApp(3);
    }


}
