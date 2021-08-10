using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    [CreateAssetMenu(fileName = "new GalleryReward", menuName = "LewdOwl/QuestSystem/Reward/GalleryReward")]
    public class GalleryReward : AbstractReward
    {
        public GalleryItem galleryItem;

        public override void RunReward()
        {
            galleryItem.UnlockGalleryItem();
            given = true;
        }
    }
}