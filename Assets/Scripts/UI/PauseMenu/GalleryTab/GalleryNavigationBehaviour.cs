using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalleryNavigationBehaviour : MonoBehaviour
{

    public List<GalleryItemBehaviour> galleryItemBehaviours;
    public GalleryItemBehaviour currentGalleryItemBehaviour;

    [Header("Displayer References")]
    // Displayer References
    public Image pictureDiaporama;
    public RawImage videoWhiteScreen;
    public Button next;
    public Button previous;
    public Button back;

    public int currentID;
    public int previousID;
    public int nextID;

    public void Display()
    {
        System.Object content = currentGalleryItemBehaviour.galleryContent.GetContent(currentGalleryItemBehaviour.galleryItemType);

        if (content.GetType() == typeof(Sprite))
        {
            if (GalleryManager.instance.debug_GalleryLog)
                Debug.Log("<color=purple>PictureBehaviour()</color>");
            videoWhiteScreen.enabled = false;
            pictureDiaporama.enabled = true;
            pictureDiaporama.sprite = (Sprite)content;
            SetButtons();
        }
        if (content.GetType() == typeof(Video))
        {
            videoWhiteScreen.enabled = true;
            pictureDiaporama.enabled = false;

            Debug.Log("StartVideo");
            //Start Vid
            VideoPlayerManager.instance.videoToPlay = currentGalleryItemBehaviour.galleryContent.video;
            VideoPlayerManager.instance.videoToPlay.InitVideoPlayer(VideoPlayerManager.instance.videoPlayer);
            VideoPlayerManager.instance.SetRawImage(videoWhiteScreen);
            currentGalleryItemBehaviour.galleryContent.video.Play(false);

            SetButtons();

        }
        if (content.GetType() == typeof(DialogueChain))
        {
            videoWhiteScreen.enabled = false;
            pictureDiaporama.enabled = false;

            // Start Dial
        }
    }

    private void SetButtons()
    {
        if (GalleryManager.instance.debug_GalleryLog)
            Debug.Log("Setting Gallery display buttons");

        next.onClick.RemoveAllListeners();
        previous.onClick.RemoveAllListeners();
        back.onClick.RemoveAllListeners();

        SetPreviousAndNextID();

        switch (currentGalleryItemBehaviour.galleryItemType)
        {
            case GalleryItemType.Picture:
                SetButtonsForNextGalleryItem();
                break;
            case GalleryItemType.Video:
                SetButtonsForNextGalleryItem();
                break;
            case GalleryItemType.Dialogue:

                break;
        }

    }

    #region DiaporamaSetup
    private void SetButtonsForNextGalleryItem()
    {
        GameManager.instance.galleryOpenned = true;
        next.onClick.AddListener(() => GoToNextGalleryItem());
        previous.onClick.AddListener(() => GoToPreviousGalleryItem());
        back.onClick.AddListener(() => CloseGalleryItem());
    }

    public void GoToNextGalleryItem()
    {
        currentGalleryItemBehaviour = galleryItemBehaviours[nextID];
        Display();
    }
    public void GoToPreviousGalleryItem()
    {
        currentGalleryItemBehaviour = galleryItemBehaviours[previousID];
        Display();
    }
    public void CloseGalleryItem()
    {
        if (currentGalleryItemBehaviour.galleryItemType == GalleryItemType.Picture)
        {
            pictureDiaporama.enabled = false;
            FindObjectOfType<GalleryTabBehaviour>().HideDisplay();
        }
        else if (currentGalleryItemBehaviour.galleryItemType == GalleryItemType.Video)
        {
            currentGalleryItemBehaviour.galleryContent.video.Stop(false, videoWhiteScreen, false);

            videoWhiteScreen.enabled = false;

            FindObjectOfType<GalleryTabBehaviour>().HideDisplay();
        }
        GameManager.instance.galleryOpenned = false;
    }
    #endregion

    #region VideoSetup
    private void SetButtonForVideo()
    {
        next.onClick.AddListener(() => GoToNextGalleryItem());
        previous.onClick.AddListener(() => GoToPreviousGalleryItem());
        back.onClick.AddListener(() => CloseGalleryItem());
    }

    public void GoToNextVideo()
    {
        currentGalleryItemBehaviour = galleryItemBehaviours[nextID];
        Display();
    }
    public void GoToPreviousVideo()
    {
        currentGalleryItemBehaviour = galleryItemBehaviours[previousID];
        Display();
    }
    public void BackFromVideo()
    {
        pictureDiaporama.enabled = false;
        FindObjectOfType<GalleryTabBehaviour>().HideDisplay();
    }
    #endregion

    #region DialogueSetup
    // Hide those buttons during dialogue
    #endregion

    private void SetPreviousAndNextID()
    {
        int id = 0;
        for (int i = 0; i < galleryItemBehaviours.Count; i++)
        {
            if (galleryItemBehaviours[i] == currentGalleryItemBehaviour)
                id = i;
        }
        currentID = id;
        Debug.Log("Current ID : " + id);
        previousID = (id == 0) ? galleryItemBehaviours.Count - 1 : id-1;
        Debug.Log("previous ID : " + previousID);
        nextID = (id == galleryItemBehaviours.Count - 1) ? 0 : id+1;
        Debug.Log("next ID : " + nextID);
    }
}
