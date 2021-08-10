using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ConversationLog : MonoBehaviour
{
    public ScrollRect scrollRect;
    public Scrollbar scrollbar;
    public GameObject ConversationSamplePrefab;
    public List<ConversationLog> chainHistory;

    private bool enable = false;

    // Start is called before the first frame update
    void OnEnable()
    {
        enable = true;
    }

    private void OnGUI()
    {
        if (enable)
        {

            //scrollbar.value = 0f;


            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            chainHistory = new List<ConversationLog>();
            chainHistory = GameManager.instance.managers.dialogueManager.chainHistory;

            foreach (ConversationLog chain in chainHistory)
            {
                GameObject go = Instantiate(ConversationSamplePrefab);
                go.transform.SetParent(transform);
                go.transform.localScale = new Vector3(1, 1, 1);
                go.GetComponent<UI_ConversationSampleBehaviour>().CreateDialogueSample(chain);
            }



            //scrollbar.value = 0f;

            scrollRect.verticalScrollbar.value = 0f;
            //scrollRect.horizontalNormalizedPosition = 0f;
            scrollRect.verticalNormalizedPosition = 0f;

            enable = false;
        }
    }


}
