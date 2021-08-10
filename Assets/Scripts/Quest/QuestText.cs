using UnityEngine;

namespace QuestSystem
{
    [System.Serializable]
    public class QuestText : IQuestText
    {
        public string title;
        [TextArea(5, 15)]
        public string descriptionSummary;

        public string DescriptionSummary
        {
            get
            {
                return descriptionSummary;
            }
        }

        public string Title
        {
            get
            {
                return title;
            }
        }

   



    }



}
