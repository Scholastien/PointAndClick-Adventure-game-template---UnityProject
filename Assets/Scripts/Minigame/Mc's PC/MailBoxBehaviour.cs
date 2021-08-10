using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailBoxBehaviour : MonoBehaviour
{

    public MailReaderBehaviour MailReader;

    public GameObject MailPreviewPrefab;

    public Transform contentScrollList;

    public List<Mail> displayedMails;

    // Use this for initialization
    void Start()
    {

        if (GameManager.instance != null)
        {

            displayedMails = GameManager.instance.managers.variablesManager.mailCollection.GetUnlockedMails();
        }
        foreach (Mail mail in displayedMails)
        {
            //mail.CreateContentList();
            GameObject go = Instantiate(MailPreviewPrefab, contentScrollList);
            MailBehaviour mb = go.GetComponent<MailBehaviour>();
            mb.associatedMail = mail;
            mb.MailReader = MailReader;
        }

    }
}
