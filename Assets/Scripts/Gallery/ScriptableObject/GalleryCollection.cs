using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = "new GalleryCollection", menuName = "LewdOwl/Gallery/GalleryCollection")]
public class GalleryCollection : ScriptableObject {

    public List<GalleryItem> galleryItems;

    // Search and copy every object here
    public void Init()
    {
        // galleryItems are stored in my Resource folder (Resources/ScriptableObjects/Gallery)
        GalleryItem[] items_gameFolder = Resources.LoadAll<GalleryItem>("ScriptableObjects/Gallery");
        List<GalleryItem> ItemToRemove = new List<GalleryItem>();

        // Store inexisting items
        for (int i = 0; i < galleryItems.Count; i++)
        {
            if (!items_gameFolder.Contains(galleryItems[i]))
            {
                ItemToRemove.Add(galleryItems[i]);
            }
        }

        // remove inexisting items
        for (int i = 0; i < ItemToRemove.Count; i++)
        {
            galleryItems.Remove(ItemToRemove[i]);
        }
        // Foreach item found, check if the item exist in the items collection
        // If it doesnt exist, add that item to the collection
        for (int i = 0; i < items_gameFolder.Length; i++)
        {
            if (!galleryItems.Contains(items_gameFolder[i]))
            {
                galleryItems.Add(items_gameFolder[i]);
            }
        }

        

    }

    public List<bool> SerializeGallery()
    {
        List<bool> serializedGallery = new List<bool>();
        foreach(GalleryItem galleryItem in galleryItems)
        {
            serializedGallery.Add(galleryItem.unlocked);
        }
        return serializedGallery;
    }

    public void DeserializeGallery(List<bool> boolGallery)
    {
        if (boolGallery != null)
        {
            if (boolGallery.Count <= galleryItems.Count)
            {
                for (int i = 0; i < boolGallery.Count; i++)
                {
                    galleryItems[i].unlocked = boolGallery[i];
                }
            }
            else
            {
                for (int i = 0; i < galleryItems.Count; i++)
                {
                    galleryItems[i].unlocked = boolGallery[i];
                }
            }
        }
    }

    // Return a list of ordered Gallery Items 
    public List<GalleryItem> OrderingGalleryItems()
    {
        List<GalleryItem> _galleryItems = new List<GalleryItem>();



        for (int i = 1; i < galleryItems.Count + 1; i++)
        {
            foreach(GalleryItem galleryItem in galleryItems)
            {
                if (galleryItem.index == i)
                {
                    _galleryItems.Add(galleryItem);
                }
            }
        }
        foreach (GalleryItem galleryItem in galleryItems)
        {
            if (!_galleryItems.Contains(galleryItem))
            {
                _galleryItems.Add(galleryItem);
            }
        }

        return _galleryItems;
    } 

}
