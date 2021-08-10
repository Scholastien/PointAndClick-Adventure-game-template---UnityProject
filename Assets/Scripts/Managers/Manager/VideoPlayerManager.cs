using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Video;


public class VideoPlayerManager : MonoBehaviour
{

    // Singleton pattern on VideoPlayerManager class
    public static VideoPlayerManager instance = null;

    public Video videoToPlay;

    public VideoPlayer videoPlayer;

    
    public RawImage rawImage;

    public Canvas videoCanvas;

    public AudioSource audioTrack;

    public AudioMixerGroup audioMixerGroup;


    public int index = 0;
    public bool keepPlaying = false;

    #region MonoBehaviour

    void Awake()
    {
        // If instance is not created yet, instantiate it
        if (instance == null)
        {
            instance = this;
        }
        // else if the current running instance is not instantiate with this class, destroy this class
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    // Use this for initialization
    void Start()
    {
        //audioTrack = AudioManager.instance.CreateVideoAudio(videoToPlay);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion

    #region DialogueVideo
    public RawImage CreateRawImage()
    {
        if (!keepPlaying)
        {
            GameObject go = new GameObject("White Screen_" + index);
            index++;
            go.transform.parent = videoCanvas.transform;
            //go.transform.position = new Vector3(0f, 0f, 0f);
            RectTransform panelRectTransform = go.AddComponent<RectTransform>();
            //panelRectTransform.position = new Vector3(0f, 0f, 0f);
            panelRectTransform.offsetMin = new Vector2(0, 0); // new Vector2(left, bottom); 
            panelRectTransform.offsetMax = new Vector2(0, 0); // new Vector2(-right, -top);

            panelRectTransform.anchorMin = new Vector2(0, 0);
            panelRectTransform.anchorMax = new Vector2(1, 1);
            panelRectTransform.pivot = new Vector2(0.5f, 0.5f);
            rawImage = go.AddComponent<RawImage>();
        }
        return rawImage;
    }

    public void DeleteRawImage()
    {
        if (rawImage != null)
            Destroy(rawImage.gameObject);
    }

    #endregion

    #region GalleryVideo

    public void SetRawImage(RawImage _rawImage)
    {
        rawImage = _rawImage;
    }

    #endregion

    public void destroyGo(GameObject go)
    {
        Destroy(go);
    }
    
}

// This class will wrap all infos on a video clip (loop, speed, ...)
[Serializable]
public class Video
{
    private VideoPlayer videoPlayer;
    public VideoClip videoClip;
    private AudioSource audioSource;
    public bool loop;
    [Range(0f, 10f)]
    public float playbackSpeed = 1f;
    public AudioType audioType;

    // class constructor, called in the editor with a Video object to instanciate a new one
    public Video(Video video)
    {
        this.videoClip = video.videoClip;
        this.loop = video.loop;
        this.playbackSpeed = video.playbackSpeed;
    }

    // constructor, in case we need to create a neutral video class
    public Video()
    {
        this.videoClip = null;
        this.loop = true;
        this.playbackSpeed = 1f;
    }

    // Set VideoPlayer
    public void SetVideoPlayer(VideoPlayer _videoPlayer)
    {
        this.videoPlayer = _videoPlayer;
    }

    // Initialized VideoPlayer
    public void InitVideoPlayer(VideoPlayer _videoPlayer)
    {
        SetVideoPlayer(_videoPlayer);
        videoPlayer.clip = videoClip;
        videoPlayer.isLooping = loop;
        videoPlayer.playbackSpeed = playbackSpeed;

        ConfigureVideoPlayer();
    }

    // Some values need to be adjusted by script (camera, render mode, ...)
    public void ConfigureVideoPlayer()
    {
        videoPlayer.playOnAwake = false;
        videoPlayer.waitForFirstFrame = false;

        // Test Rendermode and choose the one that plays on top of the current ui
        //videoPlayer.renderMode = VideoRenderMode.CameraFarPlane;
        videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;

        videoPlayer.targetCamera = GameManager.instance.GetComponent<Camera>();

        videoPlayer.targetCameraAlpha = 1f;

        #region AudioSetup

        // Setting an audiosource
        audioSource = AudioManager.instance.CreateVideoAudio(this, 0, 1);
        audioSource.loop = videoPlayer.isLooping;
        //audioSource.outputAudioMixerGroup = VideoPlayerManager.instance.audioMixerGroup;


        // Setting output to Audio Source
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;



        #endregion

        //videoPlayer.SetTargetAudioSource(0, AudioManager.instance.CreateVideoAudio(this));

    }

    public void Play(bool createWhiteScreen)
    {

        VideoPlayerManager.instance.StartCoroutine(PlayVideo(createWhiteScreen));
        
    }


    public IEnumerator PlayVideo(bool createWhiteScreen)
    {

        audioSource.playOnAwake = false;

        ////videoPlayer.controlledAudioTrackCount = 1;
        //Assign the Audio from video to AudioSource to be played
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSource);
        //videoPlayer.EnableAudioTrack(0, true);


        videoPlayer.Prepare();
        //Wait until video is prepared
        WaitForSeconds waitTime = new WaitForSeconds(0.5f);
        while (!videoPlayer.isPrepared)
        {
            Debug.Log("Preparing Video");
            //Prepare/Wait for 5 sceonds only
            yield return null;
            //Break out of the while loop after 5 seconds wait
            //break;
        }

        AudioManager.instance.MuteLocationMusic();

        // set Texture on the flat raw image
        if (createWhiteScreen)
            VideoPlayerManager.instance.CreateRawImage().texture = videoPlayer.texture;
        else
            VideoPlayerManager.instance.rawImage.texture = videoPlayer.texture;

        audioSource.Play();

        videoPlayer.Play();
    }

    public void Stop(bool keepPlaying, RawImage rawImage, bool destroy)
    {
        if (videoPlayer != null)
        {
            if (videoPlayer.isPlaying)
                videoPlayer.Stop();
            if (audioSource != null)
                if (audioSource.isPlaying)
                    audioSource.Stop();

            VideoPlayerManager.instance.keepPlaying = keepPlaying;

            if (!keepPlaying)
            {
                if (audioSource != null && destroy)
                    VideoPlayerManager.instance.destroyGo(audioSource.gameObject);
                if (rawImage != null && destroy)
                    VideoPlayerManager.instance.StartCoroutine(DeleteRawImageAfter(rawImage.gameObject));
                AudioManager.instance.UnmuteLocationMusic();
            }
        }
    }


    public IEnumerator DeleteRawImageAfter(GameObject go)
    {
        GameObject temp = go;
        yield return new WaitForSeconds(0.1f);

        Debug.Log("<color=red> Destroying "+ go.name  +" </color>");
        VideoPlayerManager.instance.destroyGo(temp);
    }

    public bool IsPlaying()
    {
        if (videoPlayer != null)
            return videoPlayer.isPlaying;
        else
            return false;
    }
}
