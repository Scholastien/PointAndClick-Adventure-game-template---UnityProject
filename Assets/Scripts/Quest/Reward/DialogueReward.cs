using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    [CreateAssetMenu(fileName = "new DialogueReward", menuName = "LewdOwl/QuestSystem/Reward/DialogueReward")]
    public class DialogueReward : AbstractReward
    {
        public DialogueChain dialogueToRun;

        public override void RunReward()
        {
            QuestManager qm = GameManager.instance.managers.questManager;
            GameObject myEventSystem = GameObject.Find("EventSystem");
            myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
            GameManager.instance.managers.dialogueManager.StartCoroutine(WaitEndOfDialogue());
            if (qm.debug_QuestLog)
                Debug.Log("<color=#66ffcc>Reward :</color> " + this.name + "\n" + "<color=red> running</color>");
        }

        public IEnumerator WaitEndOfDialogue()
        {
            yield return new WaitForFixedUpdate();
            yield return new WaitUntil(() => !DialogueController.instance.isRunning);
            yield return new WaitForFixedUpdate();
            QuestManager qm = GameManager.instance.managers.questManager;
            yield return new WaitForSeconds(0.2f);
            GameManager.instance.managers.dialogueManager.RunDialogue(dialogueToRun, this);
            if (qm.debug_QuestLog)
                Debug.Log("<color=#66ffcc>Reward :</color> " + this.name + "\n" + "<color=yellow> given</color>");
            
        }
    }
}