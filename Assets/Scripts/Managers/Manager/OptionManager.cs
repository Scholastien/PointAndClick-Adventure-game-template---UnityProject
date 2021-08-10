using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NsfwFilter
{
    public ChainTrigger Foot;
    public ChainTrigger Pregnancy;

    public void ChangeFootValue()
    {
        if (Foot.triggered)
            Foot.triggered = false;
        else
            Foot.triggered = true;
    }

    public void ChangePregoValue()
    {
        if (Pregnancy.triggered)
            Pregnancy.triggered = false;
        else
            Pregnancy.triggered = true;
    }
}

public class OptionManager : MonoBehaviour {

    public static OptionManager instance = null;

    private AudioManager am;

    [Header("Graphic")]
    public bool fullScreen;
    [Header("Sound")]
    public MixerVolumes mixerVolumes;
    [Header("Text")]
    public Language language;
    [Range(0.1f, 0.5f)]
    public float textSpeed = 0.1f;

    [Header("Filters")]
    public NsfwFilter nsfwFilter;


    private PersistentData persistentData;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    // Use this for initialization
    void Start () {
        am = AudioManager.instance;
        persistentData = FindObjectOfType<PersistentData>();

        //fullScreen = persistentData.persistantSaves.fullScreen;
        fullScreen = Screen.fullScreen;


    }
	
	// Update is called once per frame
	void Update () {
        GetAllOptions();
    }

    public void GetAllOptions()
    {
        GetCurrentVolume();
        fullScreen = Screen.fullScreen;
        language = persistentData.persistantSaves.language;
        textSpeed = persistentData.persistantSaves.textSpeed;
    }

    public void GetCurrentVolume()
    {
        mixerVolumes = am.mixerVolumes;
    }

    public void SaveAllOptionsToPersistent()
    {
        persistentData.persistantSaves.textSpeed = textSpeed;
        persistentData.persistantSaves.language = language;
        persistentData.persistantSaves.fullScreen = fullScreen;
        am.mixerVolumes = mixerVolumes;
        am.SaveSoundToPersistant();
    }

}
