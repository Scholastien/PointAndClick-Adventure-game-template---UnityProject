using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalleryTabBehaviour : MonoBehaviour
{
    [Header("Datas")]
    public List<GalleryItem> galleryItems;
    public List<GameObject> GalleryTextless; // pictures and videos
    public List<GameObject> GalleryDialogue; // only dialogue chains

    [Header("UI Elements")]
    public Toggle textless;
    public Scrollbar galleryScrollBar;
    public ScrollRect scrollRect;
    public Transform ScrollableList;


    [Header("Asset References")]
    public GameObject galleryItemPrefab;
    // WaterMark sprites
    public Sprite lockedImg;
    public Sprite pictureWaterMark;
    public Sprite videoWaterMark;

    [Header("Displayer References")]
    // Displayer References
    public GameObject displayerPanel;



    // Use this for initialization
    void Start()
    {
        galleryItems = VariablesManager.instance.galleryCollection.OrderingGalleryItems();
    }

    private void OnEnable()
    {
        galleryScrollBar.value = 0f;
        scrollRect.horizontalScrollbar.value = 0f;
        scrollRect.horizontalNormalizedPosition = 0f;
        scrollRect.horizontalScrollbar.value = 0f;
        scrollRect.horizontalNormalizedPosition = 0f;
    }

    public void SpawnRevelentItems()
    {
        foreach (GalleryItem galleryItem in galleryItems)
        {
            List<GameObject> galleryPictures = PrepareGalleryPictures(galleryItem);

            foreach (GameObject go in galleryPictures)
            {
                go.GetComponent<GalleryItemBehaviour>().SetGalleryTabScript(this);
            }

        }



        
    }

    public List<GameObject> PrepareGalleryPictures(GalleryItem _galleryItem)
    {
        List<GameObject> galleryPictures = new List<GameObject>();

        foreach (Sprite sprite in _galleryItem.sprites)
        {
            // creating the Go
            GameObject go = CreateGalleryGameObject(_galleryItem, GalleryItemType.Picture, sprite, pictureWaterMark);
            GalleryItemBehaviour galleryItemBehaviour = go.GetComponent<GalleryItemBehaviour>();
            // Set the content (will be displayed full screen)
            galleryItemBehaviour.galleryContent.picture = sprite;
            // Decide if it should be locked or not and method on button if necessary
            galleryItemBehaviour.IsGalleryItemUnlocked(lockedImg);

            galleryPictures.Add(go);
        }
        foreach(Video video in _galleryItem.videos)
        {
            // creating the Go
            GameObject go = CreateGalleryGameObject(_galleryItem, GalleryItemType.Video, _galleryItem.previewPicture, videoWaterMark);
            GalleryItemBehaviour galleryItemBehaviour = go.GetComponent<GalleryItemBehaviour>();
            // Set the content (will be displayed full screen)
            galleryItemBehaviour.galleryContent.video = video;
            // Decide if it should be locked or not and method on button if necessary
            galleryItemBehaviour.IsGalleryItemUnlocked(lockedImg);

            galleryPictures.Add(go);
        }

        return galleryPictures;
    }

    public GameObject CreateGalleryGameObject(GalleryItem _galleryItem, GalleryItemType _galleryItemType, Sprite _previewPic, Sprite _watermark)
    {
        // creating the Go
        GameObject go = Instantiate(galleryItemPrefab, ScrollableList);
        go.name = _galleryItem.name;
        // Init the GalleryItemBehaviour attached to it
        GalleryItemBehaviour galleryItemBehaviour = go.GetComponent<GalleryItemBehaviour>();
        galleryItemBehaviour.galleryItem = _galleryItem;
        galleryItemBehaviour.SetGalleryItemType(_galleryItemType, _previewPic, _watermark);

        return go;
    }

    public void SetDisplayer(GalleryItemBehaviour galleryItemBehaviour)
    {
        displayerPanel.SetActive(true);

        // Init gallery list
        displayerPanel.GetComponent<GalleryNavigationBehaviour>().galleryItemBehaviours = RetrieveUnlockedList(galleryItemBehaviour);

        displayerPanel.GetComponent<GalleryNavigationBehaviour>().currentGalleryItemBehaviour = galleryItemBehaviour;

        displayerPanel.GetComponent<GalleryNavigationBehaviour>().Display();
    }

    public void HideDisplay()
    {
        displayerPanel.SetActive(false);
    }



    private List<GalleryItemBehaviour> RetrieveUnlockedList(GalleryItemBehaviour galleryItemBehaviour)
    {



        List<GalleryItemBehaviour> galleryItemBehaviours = new List<GalleryItemBehaviour>();

        foreach (Transform child in ScrollableList)
        {
            galleryItemBehaviours.Add(child.gameObject.GetComponent<GalleryItemBehaviour>());
        }

        if (GalleryManager.instance.debug_GalleryLog)
            Debug.Log("galleryItemBehaviours.count : " + galleryItemBehaviours.Count);



        List<GalleryItemBehaviour> DialogueList = new List<GalleryItemBehaviour>();
        List<GalleryItemBehaviour> Textless = new List<GalleryItemBehaviour>();

        foreach (GalleryItemBehaviour _galleryItemBehaviour in galleryItemBehaviours)
        {
            if (_galleryItemBehaviour.galleryItem.unlocked)
            {
                switch (_galleryItemBehaviour.galleryItemType)
                {
                    case GalleryItemType.Dialogue:
                        DialogueList.Add(_galleryItemBehaviour);
                        break;
                    case GalleryItemType.Picture:
                        Textless.Add(_galleryItemBehaviour);
                        break;
                    case GalleryItemType.Video:
                        Textless.Add(_galleryItemBehaviour);
                        break;
                }
            }
        }
        switch (galleryItemBehaviour.galleryItemType)
        {
            case GalleryItemType.Dialogue:
                return DialogueList;
            case GalleryItemType.Picture:
                return Textless;
            case GalleryItemType.Video:
                return Textless;
            default:
                return Textless;
        }

    }

    private List<GalleryItemBehaviour> SortingGalleryList(List<GalleryItemBehaviour> galleryList)
    {
        List<GalleryItemBehaviour> result = new List<GalleryItemBehaviour>();
        List<int> indexList = new List<int>();
        foreach (GalleryItemBehaviour galleryItemBehaviour in galleryList)
        {
            if(!indexList.Contains(galleryItemBehaviour.galleryItem.index))
                indexList.Add(galleryItemBehaviour.galleryItem.index);
        }
        indexList.Sort();
        foreach (int index in indexList)
        {
            foreach(GalleryItemBehaviour galleryItemBehaviour in galleryList)
            {
                if (galleryItemBehaviour.galleryItem.index == index)
                {
                    result.Add(galleryItemBehaviour);
                }
            }
        }


        return result;
    }
}
