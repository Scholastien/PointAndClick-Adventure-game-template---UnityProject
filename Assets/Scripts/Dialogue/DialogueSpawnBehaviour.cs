using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DialogueSpawnBehaviour : MonoBehaviour {

    public int skipSpeed;

    public bool showLog;
    public bool skip;
    public bool hide;

    public GameObject CanvasSceneBG;

    public GameObject logScrollList;

    public Image background;

    [HideInInspector]
    public GameObject dialogueUi;

    private int textSpeed;

    public GameObject BlackScreen;

    public void Awake()
    {
        ActivateBlackScreen();
    }


    // Use this for initialization
    void Start () {
        GameManager.instance.managers.dialogueManager.dialogueSpawn = gameObject.GetComponent<DialogueSpawnBehaviour>();
    }

    // Update is called once per frame
    void Update() {
        if (CheckDialogueRun()) {
            CanvasSceneBG.SetActive(true);
            background.sprite = DialogueController.instance.transparentBg; 
            ContainerUI containerUI = (ContainerUI)FindObjectOfType(typeof(ContainerUI));
            if(containerUI != null)
                dialogueUi = containerUI.gameObject;
        }
        int n = gameObject.transform.childCount;

        ContainerUI[] containerUIs = FindObjectsOfType<ContainerUI>();

        //foreach(ContainerUI containerUI in containerUIs)
        //{
        //    if(containerUI.gameObject != dialogueUi)
        //    {
        //        Destroy(containerUI.gameObject.transform.parent.gameObject);
        //    }
        //}

        //if (n > 2) {
        //    for (int i = 0; i < n - 1; i++) {
        //        Destroy(gameObject.transform.GetChild(gameObject.transform.childCount - 1).gameObject);
        //    }
        //}
        logScrollList.SetActive(showLog);

	}

    public bool CheckDialogueRun() {
        return GameManager.instance.managers.dialogueManager.isRunning;
    }

    public void ActivateBlackScreen()
    {
        StartCoroutine(LockInputDuringBlackScreen());
    }

    public IEnumerator LockInputDuringBlackScreen()
    {
        BlackScreen.SetActive(true);
        DialogueManager.instance.blockNext = true;
        float alpha = 1f;
        while (alpha > 0)
        {
            Color color = new Color(0, 0, 0, alpha);
            BlackScreen.GetComponent<Image>().color = color;
            alpha -= Time.deltaTime * 2;
            yield return null;
        }

        yield return new WaitUntil(() => TransitionManager.instance.IsAnimCompleted());

        DialogueManager.instance.blockNext = false;

        BlackScreen.SetActive(false);

    }

}
