using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "new MailCollection", menuName = "LewdOwl/Mail/Collection")]
public class MailCollection : ScriptableObject {
    public List<Mail> mails;

    // Search and copy every object here
    public void Init()
    {
        // Items are stored in my Resource folder (Resources/ScriptableObjects/Mail)
        Mail[] mails_gameFolder = Resources.LoadAll<Mail>("ScriptableObjects/Mail");

        // This message should display 4 if all my items are in the folder
        //Debug.Log("Number of item in the game folder : " + mails_gameFolder.Length);

        List<Mail> ItemToRemove = new List<Mail>();

        // Store inexisting items
        for (int i = 0; i < mails.Count; i++)
        {
            if (!mails_gameFolder.Contains(mails[i]))
            {
                ItemToRemove.Add(mails[i]);
            }
        }

        // remove inexisting items
        for (int i = 0; i < ItemToRemove.Count; i++)
        {
            mails.Remove(ItemToRemove[i]);
        }
        // Foreach item found, check if the item exist in the items collection
        // If it doesnt exist, add that item to the collection
        for (int i = 0; i < mails_gameFolder.Length; i++)
        {
            if (!mails.Contains(mails_gameFolder[i]))
            {
                mails.Add(mails_gameFolder[i]);
            }
        }

    }

    // return the unlocked list of mails
    public List<Mail> GetUnlockedMails()
    {
        List<Mail> _mails = new List<Mail>();

        foreach (Mail mail in mails)
            if (mail.unlocked)
                _mails.Add(mail);

        return _mails;
    }

    // Transfor the List of mail to something that can be saved by the Save System
    public List<SerializedMail> SerializeMails(List<Mail> _mails)
    {
        List<SerializedMail> serializedMails = new List<SerializedMail>();

        foreach (Mail mail in _mails)
        {
            serializedMails.Add(new SerializedMail(mail.unlocked, mail.alreadyRead));
        }

        //Debug.Log("<color = purple> Current Serialized List Lenght : " +  serializedMails.Count);

        return serializedMails;
    }

    // Take a saved list from the Save system and transform it to a new list of mails
    public List<Mail> DeserializeMails(List<SerializedMail> serializedMails)
    {
        List<Mail> _mails = mails;

        if(serializedMails != null)
            for (int i = 0; i < serializedMails.Count; i++)
            {
                _mails[i].unlocked = serializedMails[i].unlocked;
                _mails[i].alreadyRead = serializedMails[i].alreadyRead;
            }

        return _mails;
    }

    // Send Serialized list to Gamemanager
    public void UpdateBoolValues()
    {
        GameManager.instance.managers.variablesManager.gameSavedData.mailsState = SerializeMails(mails);


    }

    // Initialize all recieved/read bool in mails to false (New game behaviour)
    public List<SerializedMail> InitializeMailListForNewGame()
    {
        List<SerializedMail> result = new List<SerializedMail>();
        foreach(Mail mail in mails)
        {
            result.Add(new SerializedMail());
        }
        DeserializeMails(result);
        return result;
    }
}

[System.Serializable]
public class SerializedMail
{
    public bool unlocked;
    public bool alreadyRead;

    public SerializedMail(bool _unlocked, bool _alreadyRead)
    {
        unlocked = _unlocked;
        alreadyRead = _alreadyRead;
    }

    // Called at the Init of the GameData in the SaveLoad script
    public SerializedMail()
    {
        unlocked = false;
        alreadyRead = false;
    }
}
