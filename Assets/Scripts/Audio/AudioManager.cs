using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum AudioType
{
    Music,
    Sfx,
    UiSFX,
    Voice
}

// Adding a layer on top of the audioSource component.
// That way, we can have full control over w/e source.
[System.Serializable]
public class Sound
{
    private AudioSource source;

    public AudioType audioType;

    public string clipName;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1f;
    [Range(0f, 1f)]
    public float pitch = 1f;

    public bool loop = false;
    public bool playOnAwake = false;

    public Sound(Sound sound)
    {
        this.audioType = sound.audioType;
        this.clipName = sound.clipName;
        this.clip = sound.clip;
        this.volume = sound.volume;
        this.pitch = sound.pitch;
        this.loop = sound.loop;
        this.playOnAwake = sound.playOnAwake;
    }
    public Sound()
    {
        this.audioType = AudioType.Music;
        this.clipName = "";
        this.clip = null;
        this.volume = 1f;
        this.pitch = 1f;
        this.loop = false;
        this.playOnAwake = false;
    }

    public void SetSourceAndOutput(AudioSource _source, AudioMixerGroup outputAudio)
    {
        source = _source;
        source.clip = clip;
        source.outputAudioMixerGroup = outputAudio;
        source.pitch = pitch;
        source.volume = volume;
        source.playOnAwake = playOnAwake;
        source.loop = loop;
    }

    public void SetSource(AudioSource _source)
    {
        source = _source;
    }

    public void Play()
    {
        if (AudioManager.instance.Debug_AudioLog)
            Debug.Log("current track (<color=#cc66ff>" + audioType + "</color>) : " + clip.name + "\n" + "Volume : " + volume);
        source.Play();
    }

    public void RandomPitch(float min, float max)
    {
        //Debug.LogError("Player random pitch");
        source.pitch = UnityEngine.Random.Range(min, max);
    }
}

[System.Serializable]
public class AudioOutputMixer
{
    public AudioMixerGroup masterOutput;
    public AudioMixerGroup musicOutput;
    public AudioMixerGroup SfxOutput;
    public AudioMixerGroup UiSfxOutput;
    public AudioMixerGroup voiceOutput;

    public AudioMixerGroup OutputSelector(AudioType audioType)
    {
        switch (audioType)
        {
            case AudioType.Music:
                return musicOutput;
            case AudioType.Sfx:
                return SfxOutput;
            case AudioType.UiSFX:
                return UiSfxOutput;
            case AudioType.Voice:
                return voiceOutput;
            default:
                return musicOutput;
        }
    }


    public void UpdateVolumes(List<float> volumes)
    {
        if (AudioManager.instance.Debug_AudioLog)
        {
            Debug.Log("_____________UDPATE_____________");
            Debug.Log("MusicVolume : " + volumes[1]);
        }

        masterOutput.audioMixer.SetFloat("MasterVolume", volumes[0]);
        musicOutput.audioMixer.SetFloat("MusicVolume", volumes[1]);
        SfxOutput.audioMixer.SetFloat("SfxVolume", volumes[2]);
        UiSfxOutput.audioMixer.SetFloat("UiSfxVolume", volumes[3]);
        voiceOutput.audioMixer.SetFloat("VoiceVolume", volumes[4]);

        float i = 0f;
        musicOutput.audioMixer.GetFloat("MusicVolume", out i);
        if (AudioManager.instance.Debug_AudioLog)
            Debug.Log("MusicVolume : " + i);

    }
}

[System.Serializable]
public class MixerVolumes
{
    [Range(-80f, 0f)]
    public float masterVolume;
    [Range(-80f, 0f)]
    public float musicVolume;
    [Range(-80f, 0f)]
    public float fxVolume;
    [Range(-80f, 0f)]
    public float uxVolume;
    [Range(-80f, 0f)]
    public float voiceVolume;

    public MixerVolumes()
    {
        masterVolume = 0f;
        musicVolume = -5f;
        fxVolume = -5f;
        uxVolume = -5f;
        voiceVolume = -5f;
    }

}


public class AudioManager : MonoBehaviour
{

    public static AudioManager instance = null;

    [Header("Debugger")]
    public bool Debug_AudioLog;

    [Header("Settings")]
    public MixerVolumes mixerVolumes;


    [Header("References")]
    public AudioOutputMixer audioOutputMixer;

    public Transform locationMusicContainer;
    public Transform DialogueMusicContainer;
    public Transform SfxContainer;
    public Transform UiSoundsContainer;
    public Transform VideoAudioContainer;

    public AudioMixer mixerGroup;

    private PersistentData persistentData;


    [Header("Properties")]
    [Range(0f, 1f)]
    public float crossFadeDuration;

    [SerializeField]
    Sound[] sounds;

    Sound locationMusic;


    public Sound currentLocationSound;


    public DialogueAudio dialogueAudio;

    // Private fields
    private GameState gameState;
    private GameManager gameManager;
    private Location currentLocation;
    private Location previousLocation;


    private Tuple<GameObject, GameObject, int> musicSystem;

    private GameObject dialogueSFX;
    public bool deletableDialogSfx;

    public List<GameObject> videoAudioTracks;

    #region MonoBehaviour

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        musicSystem = new Tuple<GameObject, GameObject, int>(new GameObject(), new GameObject(), 0);
        musicSystem.Item1.transform.SetParent(locationMusicContainer.transform);
        musicSystem.Item1.AddComponent<AudioSource>();
        musicSystem.Item2.transform.SetParent(locationMusicContainer.transform);
        musicSystem.Item2.AddComponent<AudioSource>();

        //Debug.Log("PlaySound(test);");
        //PlaySound("test");
        persistentData = FindObjectOfType<PersistentData>();
        mixerVolumes = persistentData.persistantSaves.savedMixerVolumes;

        SetPrivateFields();
    }

    void Update()
    {
        List<float> volumes = new List<float>();
        volumes.Add(mixerVolumes.masterVolume);
        volumes.Add(mixerVolumes.musicVolume);
        volumes.Add(mixerVolumes.fxVolume);
        volumes.Add(mixerVolumes.uxVolume);
        volumes.Add(mixerVolumes.voiceVolume);

        audioOutputMixer.UpdateVolumes(volumes);

        mixerGroup.SetFloat("MasterVolume", mixerVolumes.masterVolume);


        musicSystem.Item1.transform.SetParent(locationMusicContainer.transform);
        musicSystem.Item2.transform.SetParent(locationMusicContainer.transform);
        //persistentData.persistantSaves.savedMixerVolumes = AudioManager.instance.mixerVolumes;
    }

    #endregion

    public int SwitcherIndex()
    {
        switch (musicSystem.Item3)
        {
            case 0:
                return 1;
            case 1:
                return 0;
            default:
                return 0;
        }
    }

    public GameObject GetPreviousRoom()
    {
        switch (musicSystem.Item3)
        {
            case 0:
                return musicSystem.Item2;
            case 1:
                return musicSystem.Item1;
            default:
                return musicSystem.Item2;
        }
    }

    public GameObject SwitcherSoundSource()
    {
        switch (musicSystem.Item3)
        {
            case 0:
                return musicSystem.Item1;
            case 1:
                return musicSystem.Item2;
            default:
                return musicSystem.Item1;
        }
    }

    public GameObject MusicTupleRemaker(Sound sound)
    {

        musicSystem = new Tuple<GameObject, GameObject, int>(musicSystem.Item1, musicSystem.Item2, SwitcherIndex());
        GameObject go = SwitcherSoundSource();
        go.name = sound.audioType + "_" + sound.clipName;
        sound.SetSourceAndOutput(go.GetComponent<AudioSource>(), audioOutputMixer.OutputSelector(sound.audioType));

        return go;


    }

    public void GetLocationMusic(Sound sound)
    {
        if (locationMusic != sound)
        {

            locationMusic = sound;

            //GameObject go = new GameObject(locationMusic.audioType + "_" + locationMusic.clipName);
            //go.transform.SetParent(this.transform);
            //locationMusic.SetSourceAndOutput(go.AddComponent<AudioSource>(), audioOutputMixer.OutputSelector(locationMusic.audioType));
            GameObject go = MusicTupleRemaker(sound);
            locationMusic.SetSource(go.GetComponent<AudioSource>());
            if(Debug_AudioLog)
                Debug.Log("Play : " + sound.clipName);
            PlayLocationMusic();

            AudioSource previousAudioSource = GetPreviousRoom().GetComponent<AudioSource>();
            previousAudioSource.volume = 0; //Mathf.Lerp(previousAudioSource.volume, 0, crossFadeDuration);

            StartCoroutine(FadeOut(previousAudioSource));
        }
    }

    private void SetPrivateFields()
    {
        gameManager = GameManager.instance;
        gameState = gameManager.gameState;
        currentLocation = gameManager.managers.navigationManager.currentLocation;
    }

    private bool IsLocationChange()
    {
        return (currentLocation != previousLocation);
    }

    public void PlayLocationMusic()
    {
        locationMusic.Play();
    }

    public void PlaySound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].clipName == _name)
            {
                sounds[i].Play();
                return;
            }
        }
    }

    public void MuteLocationMusic()
    {
        SwitcherSoundSource().GetComponent<AudioSource>().volume = 0f;
    }

    public void UnmuteLocationMusic()
    {
        SwitcherSoundSource().GetComponent<AudioSource>().volume = 1f;
    }

    public void SaveSoundToPersistant()
    {
        persistentData.persistantSaves.savedMixerVolumes = mixerVolumes;
        persistentData.needToSave = true;
    }

    public IEnumerator FadeOut(AudioSource audioSource)
    {
        //crossFadeDuration
        //audioSource.volume
        yield return 0;
    }

    public IEnumerator SfxStartAfterDelay(DialogueAudioSound _dialogueAudioSound)
    {
        yield return new WaitForSeconds(_dialogueAudioSound.wait);
        deletableDialogSfx = true;
        _dialogueAudioSound.sound.Play();
        if (_dialogueAudioSound.loopSfx)
        {
            yield return new WaitForSeconds(_dialogueAudioSound.loopDelay);

            while (deletableDialogSfx || dialogueSFX != null)
            {
                _dialogueAudioSound.sound.Play();
                yield return new WaitForSeconds(_dialogueAudioSound.loopDelay);
            }

        }


    }

    public IEnumerator DestroyDialogueSFX()
    {

        yield return new WaitUntil(() => deletableDialogSfx);
        if (dialogueSFX != null)
        {
            Destroy(dialogueSFX);
            deletableDialogSfx = false;
        }
    }
    public void CreateDialogueSFX(DialogueAudioSound _dialogueAudioSound)
    {

        dialogueSFX = new GameObject(_dialogueAudioSound.sound.audioType + "_dialog_" + _dialogueAudioSound.sound.clipName);
        dialogueSFX.transform.SetParent(SfxContainer.transform);
        _dialogueAudioSound.sound.SetSourceAndOutput(dialogueSFX.AddComponent<AudioSource>(), audioOutputMixer.OutputSelector(_dialogueAudioSound.sound.audioType));
        StartCoroutine(SfxStartAfterDelay(_dialogueAudioSound));
    }

    public AudioSource CreateVideoAudio(Video video, int index, int count)
    {
        if (videoAudioTracks != null)
        {
            foreach (GameObject go in videoAudioTracks)
                Destroy(go);
        }
        videoAudioTracks = new List<GameObject>();

        for (int i = 0; i < count; i++)
        {
            GameObject audioGo = new GameObject(video.audioType + "_video_" + video.videoClip.name);
            videoAudioTracks.Add(audioGo);
            audioGo.transform.SetParent(VideoAudioContainer.transform);
            audioGo.AddComponent<AudioSource>().outputAudioMixerGroup = audioOutputMixer.OutputSelector(video.audioType);
        }

        return videoAudioTracks[index].GetComponent<AudioSource>();
    }
    
}
