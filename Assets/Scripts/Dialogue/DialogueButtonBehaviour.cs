using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WindowsInput;
using WindowsInput.Native;

public class DialogueButtonBehaviour : MonoBehaviour, IPointerEnterHandler
{

    public bool pressed = false;

    public Text text;
    public Button button;

    [Range(0f, 1f)]
    public float pitchGap;
    public Sound sound;


    public bool isRunning;
    public bool isWriting;
    public bool isFading;
    public bool isWaiting;
    public bool finishWriting;
    public float tempTextDelay;


    // Use this for initialization
    public void Start () {
        sound.audioType = AudioType.UiSFX;
        sound.loop = false;
        GameObject go = new GameObject(sound.audioType + "_" + sound.clipName);
        go.transform.SetParent(this.transform);
        sound.SetSourceAndOutput(go.AddComponent<AudioSource>(), AudioManager.instance.audioOutputMixer.OutputSelector(sound.audioType));
    }
	
	// Update is called once per frame
	protected void Update () {
        UpdateState();

    }

    private void UpdateState() {
        isRunning = DialogueController.instance.isRunning;
        isWriting = DialogueController.instance.isWriting;
        isFading = DialogueController.instance.isFading;
        isWaiting = DialogueController.instance.isWaiting;
        finishWriting = DialogueController.instance.finishWriting;
        tempTextDelay = DialogueController.instance.tempTextDelay;

        pressed = false;
    }

    protected void UnselectCurrent()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.LogError("Pointer enter dialogue btn");


        if (sound.pitch - pitchGap > 0)
            sound.RandomPitch(sound.pitch - pitchGap, sound.pitch);
        else
            sound.RandomPitch(0, sound.pitch);
        if(sound.clip != null)
            sound.Play();
    }
}
