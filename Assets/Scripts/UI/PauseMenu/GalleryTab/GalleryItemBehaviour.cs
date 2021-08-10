using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalleryItemBehaviour : MonoBehaviour {

    [Header("Properties")]
    public GalleryItem galleryItem;
    public GalleryItemType galleryItemType;
    public GalleryContent galleryContent;

    [Header("Object references")]
    public Button galleryButton;
    public Image previewImage;
    public Image watermark;


    private GalleryTabBehaviour galleryTabBehaviour;


    public void SetGalleryTabScript(GalleryTabBehaviour _galleryTabBehaviour)
    {
        galleryTabBehaviour = _galleryTabBehaviour;
    }


    public void SetGalleryItemType(GalleryItemType _galleryItemType, Sprite _previewPic, Sprite _watermark)
    {
        previewImage.enabled = true;
        galleryItemType = _galleryItemType;
        previewImage.sprite = _previewPic;
        watermark.sprite = _watermark;
    }

    public void IsGalleryItemUnlocked(Sprite lockedWatermark)
    {
        if (galleryItem.unlocked)
        {
            // Add a behaviour based on the button type
            AddingButtonBehaviour();
        }
        else
        {
            // Use the lock watermark and don't add any behaviour to the button
            previewImage.enabled = false;
            watermark.sprite = lockedWatermark;
        }
    }


    public void AddingButtonBehaviour()
    {
        switch (galleryItemType)
        {
            case GalleryItemType.Picture:
                galleryButton.onClick.AddListener(() => SetDisplayer());
                break;
            case GalleryItemType.Video:
                galleryButton.onClick.AddListener(() => SetDisplayer());
                break;
            case GalleryItemType.Dialogue:
                galleryButton.onClick.AddListener(() => SetDisplayer()); 
                break;
        }
    }

    public void SetDisplayer()
    {
        galleryTabBehaviour.SetDisplayer(gameObject.GetComponent<GalleryItemBehaviour>());

    }
}

[Serializable]
public class GalleryContent
{
    public Sprite picture;
    public Video video;
    public DialogueChain dialogue;

    public object GetContent(GalleryItemType galleryItemType)
    {
        switch (galleryItemType)
        {
            case GalleryItemType.Picture:
                return picture;
            case GalleryItemType.Video:
                return video;
            case GalleryItemType.Dialogue:
                return dialogue;
            default:
                return picture;
        }
    }
}

public enum GalleryItemType
{
    Picture,
    Video,
    Dialogue
}
