using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    /// <summary>
    /// Set the unlocked property of a mail to a given value

        /// value by default is true (recieve mail)
        /// You can imagine at a moment of the story, a mail could be deleted too (Hacked or MC delete it during a cutscene ...)
    /// </summary>
    [CreateAssetMenu(fileName = "new MailReward", menuName = "LewdOwl/QuestSystem/Reward/MailReward")]
    public class MailReward : AbstractReward
    {
        public Mail mailToRecieve;
        public bool Recieve = true;

        public override void RunReward()
        {
            mailToRecieve.unlocked = Recieve;
            given = true;
        }
    }
}