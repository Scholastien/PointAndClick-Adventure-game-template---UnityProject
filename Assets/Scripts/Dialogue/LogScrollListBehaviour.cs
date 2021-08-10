using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogScrollListBehaviour : MonoBehaviour {

    public Transform logListContent;

    public GameObject logHistoryPrefab;
    public GameObject LogDialogueLinePrefab;

    private List<ConversationLog> logHistory;

    // Use this for initialization
    void Start () {
        ClearContentList();
        SpawnLogHistory();
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void ClearContentList() {
        foreach(Transform child in logListContent) {
            Destroy(child.gameObject);
        }
    }

    public void SpawnLogHistory() {
        ClearContentList();

        logHistory = GameManager.instance.managers.dialogueManager.chainHistory;

        foreach(ConversationLog log in logHistory) {
            Debug.Log("log" + log.speaker);
            GameObject logHistory = Instantiate(logHistoryPrefab, logListContent);
            Text LogName = logHistory.transform.Find("LogName").GetComponent<Text>();
            LogName.text = log.speaker;
            foreach (string line in log.dialogueLines) {
                Debug.Log("line" + line);
                GameObject dialogueLine = Instantiate(LogDialogueLinePrefab, logHistory.transform.Find("LogDialogueList"));
                dialogueLine.GetComponent<Text>().text = line;
            }
        }
    }
}
