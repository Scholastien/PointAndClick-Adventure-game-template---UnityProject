using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace QuestSystem
{
    [CreateAssetMenu(fileName = "new TalkToObjective", menuName = "LewdOwl/QuestSystem/Objective/TalkToObjective")]
    [System.Serializable]
    public class TalkToObjective : AbstractObjective
    {

        [Header("Properties")]
        public CharacterFunction character;    //zone, 2d cord, 3d cord

        private DialogueManager dm;


        public override void CheckProgress()
        {
            //if players location is equal to our target location then we are complete and we have finished objective

            if (dm.talkTo == character && dm.triggeredByInteractable)
            {
                dm.StartCoroutine(WaitEndOfDialogue());
            }
            else 
            {
                isComplete = false;
            }



            //Debug.Log(isComplete);
        }

        public override void UpdateProgress()
        {
            dm = GameManager.instance.managers.dialogueManager;
        }

        public IEnumerator WaitEndOfDialogue()
        {
            yield return new WaitUntil(() => !DialogueController.instance.isRunning);
            isComplete = true;

        }
    }
}
