using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;
using System.Linq;
using UnityEditor;

[CreateAssetMenu(fileName = "new QuestCollection", menuName = "LewdOwl/QuestSystem/QuestCollection")]
public class QuestCollection : ScriptableObject
{

    [Tooltip("Quest Identifier.Id")]
    public int starterQuest;

    public List<Quest> allQuest;
    public List<QuestIdentifier> test;

    // Search and copy every object here
    public void Init()
    {
        // Quests are stored in my Resource folder (Resources/ScriptableObjects/QuestSystem)
        Quest[] quest_gameFolder = Resources.LoadAll<Quest>("ScriptableObjects/QuestSystem");

        // This message should display 4 if all my items are in the folder
        //Debug.Log("Number of item in the game folder : " + quest_gameFolder.Length);

        List<Quest> newQuests = new List<Quest>();

        // Store inexisting items
        for (int i = 0; i < allQuest.Count; i++)
        {
            if (!quest_gameFolder.Contains(allQuest[i]))
            {

                newQuests.Add(allQuest[i]);
            }
        }

        // remove inexisting items
        for (int i = 0; i < newQuests.Count; i++)
        {
            allQuest.Remove(newQuests[i]);
        }
        // Foreach item found, check if the item exist in the items collection
        // If it doesnt exist, add that item to the collection
        for (int i = 0; i < quest_gameFolder.Length; i++)
        {
            if (!allQuest.Contains(quest_gameFolder[i]))
            {
                quest_gameFolder[i].identifier.id = VerifyQuestID(quest_gameFolder[i].identifier.id);
                NameQuestProperly(quest_gameFolder[i]);
                allQuest.Add(quest_gameFolder[i]);
            }
        }

    }




    // Should check if an id is unique
    public int VerifyQuestID(int id)
    {
        int current_id = id;
        List<int> id_taken = new List<int>();
        bool id_isValid = false;

        for (int i = 0; i < allQuest.Count; i++)
        {
            id_taken.Add(allQuest[i].identifier.id);
        }
        while (!id_isValid)
        {
            if (id_taken.Contains(current_id))
            {
                current_id++;
            }
            else
            {
                id_isValid = true;
            }
        }
        return current_id;
    }

    public void NameQuestProperly(Quest myAsset)
    {
#if UNITY_EDITOR
        string assetPath = AssetDatabase.GetAssetPath(myAsset.GetInstanceID());

        string newName = assetPath;
        string[] separatingStrings = { "/", ".asset" };
        string[] words = newName.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);
        newName = words[words.Length - 1];

        char[] delimiterChars = { '{', '}' };
        words = newName.Split(delimiterChars);
        newName = words[0] + "{" + myAsset.identifier.id + "}" + words[words.Length - 1];


        Debug.LogWarning(newName);
        AssetDatabase.RenameAsset(assetPath, newName);
        AssetDatabase.SaveAssets();
#endif
    }



    public List<Quest> InitCurrentQuest(List<Quest> currentQuest, bool loaded)
    {
        foreach (Quest quest in allQuest)
        {
            if (loaded)
                quest.state = QuestState.Unassigned;
        }
        if (FindStart())
        {
            currentQuest = new List<Quest>();
            foreach (Quest quest in allQuest)
            {
                foreach (int id in GameManager.instance.managers.variablesManager.gameSavedData.activeQuest)
                {
                    if (quest.identifier.id == id)
                    {
                        quest.state = QuestState.Awaiting;
                        currentQuest.Add(quest);
                        DisablePrevious(quest);
                    }
                }
            }
        }
        else
        {
            Debug.LogError("Please set the SourceID of the starter quest to its ID");
        }
        return currentQuest;
    }

    public List<Quest> InitAllQuest(List<int> activeQuest, bool loaded)
    {
        foreach (Quest quest in allQuest)
        {
            if (loaded)
                quest.state = QuestState.Unassigned;
        }
        List<Quest> currentQuest = new List<Quest>();
        if (FindStart())
        {

            foreach (int index in activeQuest)
            {
                bool found = false;

                foreach (Quest quest in allQuest)
                {
                    if (quest.identifier.id == index)
                    {
                        found = true;
                        quest.state = QuestState.Awaiting;
                        currentQuest.Add(quest);
                        DisablePrevious(quest);

                    }

                }

                if (!found)
                {

                }

            }
        }



        return currentQuest;
    }

    public void LoadToCurrent(List<int> indexAllQuest)
    {
        foreach (int index in indexAllQuest)
        {
            DisablePrevious(allQuest[index]);
        }
    }

    //recursive to make all previous quest completed
    public void DisablePrevious(Quest quest)
    {
        if (quest.identifier.id != quest.identifier.sourceID)
        {
            FindWithID(quest.identifier.sourceID).state = QuestState.Completed;
            DisablePrevious(FindWithID(quest.identifier.sourceID));
        }
    }

    public void DisableAll()
    {
        foreach (Quest q in allQuest)
        {
            q.state = QuestState.Unassigned;
        }
    }



    // find quest with an id
    public Quest FindWithID(int id)
    {
        foreach (Quest quest in allQuest)
        {
            if (quest.identifier.id == id)
            {
                return quest;
            }
        }
        return allQuest[starterQuest];
    }

    public bool FindStart()
    {
        return FindWithID(starterQuest).identifier.id == FindWithID(starterQuest).identifier.sourceID;
    }
}
