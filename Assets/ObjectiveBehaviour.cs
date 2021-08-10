using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace QuestSystem
{
    public class ObjectiveBehaviour : MonoBehaviour
    {
        [Header("Processed Data")]
        public UI_NestedObjectiveDisplay objective;

        [Header("GameObject Reference")]
        public TextMeshProUGUI label;
        public Image checkmark;
        public Image panelBg;

        [Header("Assets")]
        public Sprite awaitingBg;
        public Sprite completedBg;


        // Start is called before the first frame update
        void Start()
        {
            label.text = objective.label;

            checkmark.enabled = objective.status;

            // true = completed
            if (objective.status)
            {
                panelBg.sprite = completedBg;
            }
            else
            {
                panelBg.sprite = awaitingBg;
            }
        }
    }
}