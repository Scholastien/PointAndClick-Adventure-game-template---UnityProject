using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueAudio
{

    public List<DialogueAudioSound> currentMusicDialogue;

    // Switcher
    public Tuple<GameObject, GameObject, int> dialogueMusic;

    public Sound previousLocationMusic;

    public void AddNewDialogueSound(DialogueAudioSound dialogueAudioSound)
    {
        //currentMusicDialogue.Add(new DialogueAudioSound(_sound, _wait, _loopDelay));

        AudioManager am = AudioManager.instance;

        switch (dialogueAudioSound.sound.audioType)
        {
            case AudioType.Music:
                // Use SwitcherAudio to switch between tracks
                // If the sound is null, stop the current track and play the previous
                break;
            case AudioType.Sfx:
                // Play the sound , then destroy the gameObject when the currentEvent (ChainEvent) is done
                am.CreateDialogueSFX(dialogueAudioSound);
                break;
            case AudioType.UiSFX:
                // Do nothing, ui sound is useless in dialogues
                break;
            case AudioType.Voice:
                // Do nothing for now, no voice in the game
                break;
        }
    }
}

[Serializable]
public class DialogueAudioSound
{
    public Sound sound;
    public float wait;
    public bool loopSfx;
    public float loopDelay;

    public DialogueAudioSound(Sound _sound, bool _loopSfx, float _wait, float _loopDelay)
    {
        this.sound = _sound;
        this.wait = _wait;
        this.loopSfx = _loopSfx;
        this.loopDelay = _loopDelay;
    }
}
