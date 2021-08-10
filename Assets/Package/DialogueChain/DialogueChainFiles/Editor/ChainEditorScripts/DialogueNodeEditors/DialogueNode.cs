using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Video;

public class DialogueNode : Node
{
    private GUIStyle dialogueStyle;
    bool newSpeaker = false;
    public bool initialFlag = false;
    int speakerIndex = 0;
    string newSpeakerName = "";
    float width;

    private void OnEnable()
    {
        baseHeight = 180;
        baseWidth = 377;
        originalWindowTitle = "Dialogue";
    }

    public override void DrawWindow(DialogueChain chain)
    {
        base.DrawWindow(chain);
        HandleTitle();

        width = baseWidth;
        cEvent.windowRect = windowRect;

        //If this is a new node
        #region StartingNewNode
        if (!initialFlag)
        {
            if (!chain.speakers.Contains(chain.defaultSpeaker))
            {
                chain.speakers.Add(chain.defaultSpeaker);
            }

            if (chain.defaultSpeaker != "")
            {
                cEvent.speaker = chain.defaultSpeaker;
            }

            cEvent.noSpeaker = !chain.defaultShowNames;
            cEvent.showImage = chain.defaultShowImages;

            if (cEvent.showImage)
            {
                cEvent.speakerImage = chain.defaultSprite;
            }

            cEvent.dialogueContainer = chain.defaultContainerType;
            cEvent.textDelay = chain.defaultTextDelay;

            initialFlag = true;
        }
        #endregion

        //If character count is over the limit
        #region CharacterCountOverLimit
        if (cEvent.dialogue != null && cEvent.dialogue.Length > DialogueChainPreferences.maxCharCount && DialogueChainPreferences.maxCharCount != 0)
        {
            GUI.color = Color.red;
        }
        #endregion

        //Overwrite the backgroud image for dialogue
        #region Background

        //If there is an image for the speaker
        #region FixedPicture
        EditorGUILayout.BeginHorizontal();
        if (cEvent.showImage)
        {
            height += 170;


            GUILayout.BeginVertical();
            GUILayout.Label("Background");
            //// Start BOX
            GUILayout.BeginVertical("GroupBox");
            GUILayout.BeginHorizontal();

            bool specialBG = cEvent.useRandomSprite || cEvent.useStackedPicture || cEvent.useVideo;

            //Place Image on left if leftSide is true
            if (cEvent.leftSide && !(specialBG) && !cEvent.useTransparentBG)
            {
                height += 105;
                cEvent.speakerImage = (Sprite)EditorGUILayout.ObjectField(cEvent.speakerImage, typeof(Sprite), true, GUILayout.Width(100), GUILayout.Height(100));
            }
            else
            {
                height += 112;
            }
            EditorGUILayout.BeginVertical();


            // TransparentBg
            if (!(specialBG))
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Transparent BG", GUILayout.Width(145));
                cEvent.useTransparentBG = EditorGUILayout.Toggle(cEvent.useTransparentBG, GUILayout.Width(20));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Blurry BG", GUILayout.Width(145));
                cEvent.useBlurryBG = EditorGUILayout.Toggle(cEvent.useBlurryBG, GUILayout.Width(20));
                EditorGUILayout.EndHorizontal();


                EditorGUILayout.BeginVertical();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.BeginVertical("groupbox");
                GUILayout.Label("Auto Fade");

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Automatic Fade at start", GUILayout.Width(145));
                cEvent.autoFadeAtStart = EditorGUILayout.Toggle(cEvent.autoFadeAtStart, GUILayout.Width(20));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Automatic Fade duration", GUILayout.Width(145));
                cEvent.autoFadeDuration = EditorGUILayout.FloatField(cEvent.autoFadeDuration, GUILayout.Width(40));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Skip when done", GUILayout.Width(145));
                cEvent.SkipWhenFadeDone = EditorGUILayout.Toggle(cEvent.SkipWhenFadeDone, GUILayout.Width(20));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();



                if (cEvent.useTransparentBG)
                {
                    cEvent.useRandomSprite = false;
                }

            }
            //Flip Image, choose image side, and choose if image from variable
            //cEvent.flipImage = EditorGUILayout.Toggle("Flip Image", cEvent.flipImage);
            //if (cEvent.leftSide)
            //{
            //    EditorGUILayout.BeginHorizontal();
            //    EditorGUILayout.LabelField("Left Screen", GUILayout.Width(130));
            //    if (GUILayout.Button("Switch", GUILayout.Width(50)))
            //    {
            //        cEvent.leftSide = !cEvent.leftSide;
            //    }
            //    EditorGUILayout.EndHorizontal();
            //}
            //else
            //{
            //    EditorGUILayout.BeginHorizontal();
            //    EditorGUILayout.LabelField("Right Screen", GUILayout.Width(130));
            //    if (GUILayout.Button("Switch", GUILayout.Width(50)))
            //    {
            //        cEvent.leftSide = !cEvent.leftSide;
            //    }
            //    EditorGUILayout.EndHorizontal();
            //}

            //cEvent.useCustomPlayerImage = EditorGUILayout.Toggle("Use variable for image?", cEvent.useCustomPlayerImage);

            if (cEvent.useCustomPlayerImage && !cEvent.useRandomSprite)
            {
                cEvent.speakerImage = null;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Index for image list", GUILayout.Width(145));
                cEvent.playerImageIndex = EditorGUILayout.IntField(cEvent.playerImageIndex, GUILayout.Width(20));
                EditorGUILayout.EndHorizontal();
            }



            EditorGUILayout.EndVertical();
            //Place Image on right if leftSide is not true
            if (!cEvent.leftSide && !cEvent.useRandomSprite)
            {
                cEvent.speakerImage = (Sprite)EditorGUILayout.ObjectField(cEvent.speakerImage, typeof(Sprite), false, GUILayout.Width(100), GUILayout.Height(100));
            }
        }
        EditorGUILayout.EndHorizontal();
        #endregion

        #region CustomBackground


        #region  RandomizedPicture
        cEvent.useRandomSprite = EditorGUILayout.Foldout(cEvent.useRandomSprite, "Randomized Pictures");
        HandleRandomSprite();
        #endregion

        #region StackedPictures
        cEvent.useStackedPicture = EditorGUILayout.Foldout(cEvent.useStackedPicture, "Stacked Pictures (.png with transparent bg)");
        HandleStackPicture();
        #endregion

        #region VideoSupport
        if (!cEvent.inVideoChain)
            cEvent.useVideo = EditorGUILayout.Foldout(cEvent.useVideo, "Play Video");
        else
            cEvent.useVideo = EditorGUILayout.Foldout(cEvent.useVideo, "Play Video (video chain)");
        HandleVideo();
        #endregion


        #endregion




        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.EndVertical();
        //// End BOX
        ///
        #endregion


        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Text Delay");
        cEvent.textDelay = EditorGUILayout.Slider(cEvent.textDelay, 0f, 1f);
        EditorGUILayout.EndHorizontal();



        //Dialogue Container
        HandleDialogueContainerChoice();

        //Handling the speaker
        // TODO: Correcting the bug where dialogue node disappear if user click + click the noSpeaker boolean
        // This part need to be cleared (many incoherent chunks of code)
        #region HandlingSpeaker
        if (!cEvent.noSpeaker)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Speaker:", GUILayout.Width(60));
            //Debug.Log("Adding Player in the speaker list");
            //if (!chain.speakers.Contains("Player"))
            //{
            //    chain.speakers.Insert(0, "Player");
            //}
            string[] speakersArray = chain.speakers.ToArray();
            GUI.SetNextControlName("Popup");
            if (cEvent.speaker != null)
            {
                speakerIndex = chain.speakers.IndexOf(cEvent.speaker);
            }
            speakerIndex = EditorGUILayout.Popup(speakerIndex, speakersArray);
            cEvent.speaker = chain.speakers[speakerIndex];

            //Adding a new name to the speaker list
            if (newSpeaker)
            {
                if (GUILayout.Button("Cancel", GUILayout.Width(55)))
                {
                    newSpeaker = false;
                    GUI.FocusControl("Popup");
                    newSpeakerName = "";
                }
            }
            else
            {
                if (GUILayout.Button("Add/Remove", GUILayout.Width(85)))
                {
                    newSpeaker = true;
                    GUI.FocusControl("Popup");
                }
            }

            if (newSpeaker)
            {
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                height += 20;

                newSpeakerName = EditorGUILayout.TextField(newSpeakerName, GUILayout.Width(290));
                if (GUILayout.Button("+", GUILayout.Width(20)))
                {
                    if (newSpeakerName == "")
                    {
                        Debug.Log("Must specify a name to add for the new speaker.");
                    }
                    else
                    {
                        chain.speakers.Insert(0, newSpeakerName);
                        cEvent.speaker = newSpeakerName;
                        newSpeaker = false;
                        speakerIndex = chain.speakers.Count - 1;
                        GUI.FocusControl("Popup");
                    }
                }

                //Removing a name from the list of speakers
                if (GUILayout.Button("-", GUILayout.Width(20)))
                {
                    if (newSpeakerName == "")
                    {
                        Debug.Log("Must specify a name to remove for the speaker.");
                    }
                    else
                    {
                        if (chain.speakers.Contains(newSpeakerName))
                        {
                            if (cEvent.speaker == newSpeakerName)
                            {
                                if (speakerIndex == 0)
                                {
                                    cEvent.speaker = chain.speakers[speakerIndex + 1];
                                }
                                else
                                {
                                    cEvent.speaker = chain.speakers[speakerIndex - 1];
                                }
                            }
                            chain.speakers.Remove(newSpeakerName);
                            newSpeaker = false;
                            height -= 20;
                            speakerIndex = chain.speakers.Count - 1;
                            GUI.FocusControl("Popup");
                        }
                        else
                        {
                            Debug.Log("Name does not appear in list of speakers.");
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                EditorGUILayout.EndHorizontal();
            }
        }
        else
        {
            height -= 20;
            cEvent.speaker = "Player";
        }
        #endregion

        dialogueStyle = GUI.skin.GetStyle("TextArea");
        dialogueStyle.wordWrap = true;
        cEvent.dialogue = EditorGUILayout.TextArea(cEvent.dialogue, dialogueStyle, GUILayout.Height(60));


        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(" ", GUILayout.Width(60 + 15 + 70 + 45));
        EditorGUILayout.LabelField("Don't use Speaker name", GUILayout.Width(150));
        cEvent.noSpeaker = EditorGUILayout.Toggle(cEvent.noSpeaker, GUILayout.Width(15));
        //EditorGUILayout.LabelField("No Image", GUILayout.Width(60));
        //cEvent.showImage = !EditorGUILayout.Toggle(!cEvent.showImage, GUILayout.Width(15));
        //EditorGUILayout.LabelField("     Wait", GUILayout.Width(70));
        //cEvent.dialogueWaitTime = EditorGUILayout.FloatField(cEvent.dialogueWaitTime, GUILayout.Width(45));
        //EditorGUILayout.LabelField(" Fade", GUILayout.Width(54));
        //cEvent.dialoguefadeTime = EditorGUILayout.FloatField(cEvent.dialoguefadeTime, GUILayout.Width(45));
        EditorGUILayout.EndHorizontal();

        HandleSoundChoice();

        if (!cEvent.showImage)
        {
            if (cEvent.leftSide)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Left Side", GUILayout.Width(80));
                if (GUILayout.Button("Switch", GUILayout.Width(50)))
                {
                    cEvent.leftSide = !cEvent.leftSide;
                }
                EditorGUILayout.LabelField("                    Text Delay", GUILayout.Width(145));
                cEvent.textDelay = EditorGUILayout.FloatField(cEvent.textDelay, GUILayout.Width(60));
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Right Side", GUILayout.Width(80));
                if (GUILayout.Button("Switch", GUILayout.Width(50)))
                {
                    cEvent.leftSide = !cEvent.leftSide;
                }
                EditorGUILayout.LabelField("                    Text Delay", GUILayout.Width(145));
                cEvent.textDelay = EditorGUILayout.FloatField(cEvent.textDelay, GUILayout.Width(60));
                EditorGUILayout.EndHorizontal();
            }
        }

        windowRect.height = height;
        windowRect.width = width;
    }

    public override void DrawCurves()
    {
        base.DrawCurves();
    }

    public override void DrawNodeCurve(Rect start, Rect end, float sTanMod, float eTanMod, Color color, bool rightLeftConnect)
    {
        base.DrawNodeCurve(start, end, sTanMod, eTanMod, color, rightLeftConnect);
    }

    public virtual void HandleDialogueContainerChoice()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Dialogue Box Container", GUILayout.Width(170));
        cEvent.dialogueContainer = (ContainerType)EditorGUILayout.EnumPopup(cEvent.dialogueContainer);
        EditorGUILayout.EndHorizontal();
    }


    public virtual void HandleVideo()
    {
        EditorGUILayout.BeginHorizontal();

        GUILayout.BeginVertical("HelpBox");
        if (cEvent.useVideo)
        {

            cEvent.useTransparentBG = true;
            cEvent.useRandomSprite = false;
            cEvent.useStackedPicture = false;


            height -= 120;
            //height += 20;

            if (!cEvent.inVideoChain)
            {

                height += 87;

                GUILayout.BeginHorizontal();

                GUILayout.BeginVertical("GroupBox");

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Video Clip");
                cEvent.video.videoClip = (VideoClip)EditorGUILayout.ObjectField(cEvent.video.videoClip, typeof(VideoClip), false);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Loop");
                cEvent.video.loop = EditorGUILayout.Toggle(cEvent.video.loop);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Play back speed");
                cEvent.video.playbackSpeed = EditorGUILayout.Slider(cEvent.video.playbackSpeed, 0f, 10f);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Audio output");
                cEvent.video.audioType = (AudioType)EditorGUILayout.EnumPopup(cEvent.video.audioType);
                EditorGUILayout.EndHorizontal();



                GUILayout.EndVertical();
                GUILayout.EndHorizontal();

            }
            else
            {
                //GUILayout.Label("Video Is actually in a chain");
                //height += 20;
                GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                GUILayout.Label("Overwrite Previous Video ?");
                cEvent.overwritePreviousVideo = EditorGUILayout.Toggle(cEvent.overwritePreviousVideo);
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();


                if (cEvent.overwritePreviousVideo)
                {
                    height += 20;
                    height += 87;
                    GUILayout.BeginVertical("GroupBox");
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label("Video Clip");
                    cEvent.video.videoClip = (VideoClip)EditorGUILayout.ObjectField(cEvent.video.videoClip, typeof(VideoClip), true);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label("Loop");
                    cEvent.video.loop = EditorGUILayout.Toggle(cEvent.video.loop);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label("Play back speed");
                    cEvent.video.playbackSpeed = EditorGUILayout.Slider(cEvent.video.playbackSpeed, 0f, 10f);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label("Audio output");
                    cEvent.video.audioType = (AudioType)EditorGUILayout.EnumPopup(cEvent.video.audioType);
                    EditorGUILayout.EndHorizontal();
                    GUILayout.EndVertical();

                }

            }

        }
        else
        {
            cEvent.overwritePreviousVideo = false;
        }
        GUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }

    public virtual void HandleStackPicture()
    {
        EditorGUILayout.BeginHorizontal();

        GUILayout.BeginVertical("HelpBox");



        if (cEvent.useStackedPicture)
        {
            //cEvent.useTransparentBG = false;
            cEvent.useRandomSprite = false;
            cEvent.useVideo = false;

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("+"))
            {
                cEvent.stackedSprites.Add(new StackedSprite());
                // If i'm using 2 sprites, place them face to face automatically
                if (cEvent.stackedSprites.Count == 2)
                {
                    cEvent.stackedSprites[0].x_pos = -25f;


                    cEvent.stackedSprites[1].x_pos = 25f;
                }
            }
            if (GUILayout.Button("-"))
            {
                cEvent.stackedSprites.Remove(cEvent.stackedSprites[cEvent.stackedSprites.Count - 1]);
            }
            if (GUILayout.Button("reset"))
            {
                cEvent.stackedSprites = new List<StackedSprite>();
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical("GroupBox");

            //GUILayout.Label("INSIDE");

            height -= 85;
            width += 55;
            EditorGUILayout.BeginHorizontal(GUILayout.Width(300));

            EditorGUILayout.EndHorizontal();

            for (int i = 0; i < cEvent.stackedSprites.Count; i++)
            {
                //cEvent.randomSprites.Add((Sprite)EditorGUILayout.ObjectField("Transparent Bg", sprite, typeof(Sprite), false));
                height += 182;
                width += 0;
                cEvent.stackedSprites[i].id = i;
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical("GroupBox");
                GUILayout.Label("                   stacked sprite " + (i + 1));
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical();
                //GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                GUILayout.Label("x position");
                if (GUILayout.Button("left", GUILayout.Width(40)))
                {
                    cEvent.stackedSprites[i].x_pos = -25f;
                }
                if (GUILayout.Button("mid", GUILayout.Width(40)))
                {
                    cEvent.stackedSprites[i].x_pos = 0f;
                }
                if (GUILayout.Button("right", GUILayout.Width(40)))
                {
                    cEvent.stackedSprites[i].x_pos = 25f;
                }
                GUILayout.EndHorizontal();
                cEvent.stackedSprites[i].x_pos = EditorGUILayout.Slider(cEvent.stackedSprites[i].x_pos, -50, 50, GUILayout.Width(190));
                GUILayout.Label("y position");
                cEvent.stackedSprites[i].y_pos = EditorGUILayout.Slider(cEvent.stackedSprites[i].y_pos, -50, 50, GUILayout.Width(190));
                //GUILayout.EndVertical();
                GUILayout.EndVertical();
                GUILayout.BeginVertical();

                GUILayoutOption[] Quad = new GUILayoutOption[] { GUILayout.Width(80), GUILayout.Height(80) };
                cEvent.stackedSprites[i].sprite = (Sprite)EditorGUILayout.ObjectField(cEvent.stackedSprites[i].sprite, typeof(Sprite), false, Quad);

                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                GUILayout.Label("Default Orientation");
                cEvent.stackedSprites[i].spriteOrientation = (SpriteOrientation)EditorGUILayout.EnumPopup(cEvent.stackedSprites[i].spriteOrientation);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.Label("Automatic Rotation", GUILayout.Width(115));
                cEvent.stackedSprites[i].automaticRotation = EditorGUILayout.Toggle(cEvent.stackedSprites[i].automaticRotation);

                if (!cEvent.stackedSprites[i].automaticRotation)
                {
                    GUILayout.Label("Invert Orientation");
                    cEvent.stackedSprites[i].invert = EditorGUILayout.Toggle(cEvent.stackedSprites[i].invert);
                }

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.Label("Use Effect", GUILayout.Width(115));
                cEvent.stackedSprites[i].useEffect = EditorGUILayout.Toggle(cEvent.stackedSprites[i].useEffect);

                if (cEvent.stackedSprites[i].useEffect)
                {
                    GUILayout.Label("Effect");
                    cEvent.stackedSprites[i].effect = (StackedSpriteEffect)EditorGUILayout.EnumPopup(cEvent.stackedSprites[i].effect);

                }

                GUILayout.EndHorizontal();
                if (cEvent.stackedSprites[i].useEffect)
                {
                    height += 20;
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("AutoSkip when effect is finished", GUILayout.Width(200));
                    cEvent.stackedSprites[i].autoSkip = EditorGUILayout.Toggle(cEvent.stackedSprites[i].autoSkip);
                    GUILayout.EndHorizontal();
                    switch (cEvent.stackedSprites[i].effect)
                    {
                        case StackedSpriteEffect.Translation:
                            height += 60;
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Translation Starting point", GUILayout.Width(160));
                            cEvent.stackedSprites[i].translationStart = (int)EditorGUILayout.Slider(cEvent.stackedSprites[i].translationStart, 0, 100);
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Translation Starting ending", GUILayout.Width(160));
                            cEvent.stackedSprites[i].translationEnd = (int)EditorGUILayout.Slider(cEvent.stackedSprites[i].translationEnd, 0, 100);
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Translation speed", GUILayout.Width(160));
                            cEvent.stackedSprites[i].translationSpeed = EditorGUILayout.Slider(cEvent.stackedSprites[i].translationSpeed, 0, 100);
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Translation Delay", GUILayout.Width(160));
                            cEvent.stackedSprites[i].translationDelay = EditorGUILayout.Slider(cEvent.stackedSprites[i].translationDelay, 0, 5);
                            GUILayout.EndHorizontal();


                            break;
                        case StackedSpriteEffect.Shaking:
                            height += 60;
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Shake Frequency");
                            cEvent.stackedSprites[i].shakeFrequency = EditorGUILayout.Slider(cEvent.stackedSprites[i].shakeFrequency, 0, 1, GUILayout.Width(190));


                            GUILayout.EndHorizontal();
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Shake Amplitude");
                            cEvent.stackedSprites[i].shakeAmplitude = EditorGUILayout.Slider(cEvent.stackedSprites[i].shakeAmplitude, 0, 20, GUILayout.Width(190));


                            GUILayout.EndHorizontal();
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Shake Repetition");
                            cEvent.stackedSprites[i].shakeRepetition = (int)EditorGUILayout.Slider(cEvent.stackedSprites[i].shakeRepetition, 0, 30, GUILayout.Width(190));


                            GUILayout.EndHorizontal();

                            break;
                        case StackedSpriteEffect.FadeIn:
                            height += 50;
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Fade duration");
                            cEvent.stackedSprites[i].fadeDuration = EditorGUILayout.Slider(cEvent.stackedSprites[i].fadeDuration, 0, 5, GUILayout.Width(190));


                            GUILayout.EndHorizontal();
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Fade Delay");
                            cEvent.stackedSprites[i].fadeDelay = EditorGUILayout.Slider(cEvent.stackedSprites[i].fadeDelay, 0, 1, GUILayout.Width(190));


                            GUILayout.EndHorizontal();
                            break;
                        case StackedSpriteEffect.FadeOut:
                            height += 50;
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Fade duration");
                            cEvent.stackedSprites[i].fadeDuration = EditorGUILayout.Slider(cEvent.stackedSprites[i].fadeDuration, 0, 5, GUILayout.Width(190));


                            GUILayout.EndHorizontal();
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Fade Delay");
                            cEvent.stackedSprites[i].fadeDelay = EditorGUILayout.Slider(cEvent.stackedSprites[i].fadeDelay, 0, 1, GUILayout.Width(190));


                            GUILayout.EndHorizontal();
                            break;
                        default:
                            break;
                    }

                }
                GUILayout.EndVertical();


                GUILayout.EndHorizontal();

                GUILayout.EndVertical();
                GUILayout.EndHorizontal();

            }

            GUILayout.EndVertical();

            GUILayout.EndHorizontal();



        }
        GUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }


    public virtual void HandleRandomSprite()
    {
        EditorGUILayout.BeginHorizontal();

        GUILayout.BeginVertical("HelpBox");
        if (cEvent.useRandomSprite)
        {
            cEvent.useTransparentBG = false;
            cEvent.useStackedPicture = false;
            cEvent.useVideo = false;

            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical("GroupBox");

            //GUILayout.Label("INSIDE");

            height -= 85;
            //height += 55;
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("+"))
            {
                cEvent.randomSprites.Add(null);
            }
            if (GUILayout.Button("-"))
            {
                cEvent.randomSprites.Remove(cEvent.randomSprites[cEvent.randomSprites.Count - 1]);
            }
            if (GUILayout.Button("reset"))
            {
                cEvent.randomSprites = new List<Sprite>();
            }
            EditorGUILayout.EndHorizontal();

            for (int i = 0; i < cEvent.randomSprites.Count; i++)
            {
                //cEvent.randomSprites.Add((Sprite)EditorGUILayout.ObjectField("Transparent Bg", sprite, typeof(Sprite), false));
                height += 66;
                EditorGUILayout.BeginHorizontal();

                cEvent.randomSprites[i] = (Sprite)EditorGUILayout.ObjectField("random sprite " + (i + 1), cEvent.randomSprites[i], typeof(Sprite), false);

                EditorGUILayout.EndHorizontal();


            }

            GUILayout.EndVertical();

            GUILayout.EndHorizontal();



        }
        GUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }


    public virtual void HandleSoundChoice()
    {
        EditorGUILayout.BeginHorizontal();
        cEvent.useSound = EditorGUILayout.Foldout(cEvent.useSound, "Sound");
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        GUILayout.BeginVertical();
        if (cEvent.useSound)
        {

            height += 70;
            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical("GroupBox");
            if (cEvent.dialogueAudioSound.sound == null)
            {
                cEvent.dialogueAudioSound.sound = new Sound();
            }
            // AudioType
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Audio Type", GUILayout.Width(145));
            cEvent.dialogueAudioSound.sound.audioType = (AudioType)EditorGUILayout.EnumPopup(cEvent.dialogueAudioSound.sound.audioType);
            EditorGUILayout.EndHorizontal();

            if (cEvent.dialogueAudioSound.sound.audioType == AudioType.Music || cEvent.dialogueAudioSound.sound.audioType == AudioType.Sfx)
            {
                height += 120;

                // ClipName
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Clip Name", GUILayout.Width(145));
                cEvent.dialogueAudioSound.sound.clipName = EditorGUILayout.TextField(cEvent.dialogueAudioSound.sound.clipName);
                EditorGUILayout.EndHorizontal();

                // Clip
                EditorGUILayout.BeginVertical();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Clip (empty stop previous dialog music)");
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                cEvent.dialogueAudioSound.sound.clip = (AudioClip)EditorGUILayout.ObjectField(cEvent.dialogueAudioSound.sound.clip, typeof(AudioClip), false);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();

                // Volume
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Volume", GUILayout.Width(145));
                cEvent.dialogueAudioSound.sound.volume = EditorGUILayout.Slider(cEvent.dialogueAudioSound.sound.volume, 0f, 1f);
                EditorGUILayout.EndHorizontal();

                // Pitch
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Pitch", GUILayout.Width(145));
                cEvent.dialogueAudioSound.sound.pitch = EditorGUILayout.Slider(cEvent.dialogueAudioSound.sound.pitch, 0f, 1f);
                EditorGUILayout.EndHorizontal();

                if (cEvent.dialogueAudioSound.sound.audioType == AudioType.Music)
                {
                    cEvent.dialogueAudioSound.sound.loop = false;
                    // Loop Music
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Loop", GUILayout.Width(145));
                    cEvent.dialogueAudioSound.sound.loop = EditorGUILayout.Toggle(cEvent.dialogueAudioSound.sound.loop);
                    EditorGUILayout.EndHorizontal();
                }
                else
                {
                    cEvent.dialogueAudioSound.sound.loop = false;
                    // Loop Sfx (with delay between)
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Loop sfx", GUILayout.Width(145));
                    cEvent.dialogueAudioSound.loopSfx = EditorGUILayout.Toggle(cEvent.dialogueAudioSound.loopSfx);
                    EditorGUILayout.EndHorizontal();
                }

                GUILayout.BeginVertical("HelpBox");
                EditorGUILayout.BeginHorizontal("Delay and loop settings");

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Wait", GUILayout.Width(130));
                cEvent.dialogueAudioSound.wait = EditorGUILayout.Slider(cEvent.dialogueAudioSound.wait, 0f, 1f);
                EditorGUILayout.EndHorizontal();


                EditorGUILayout.EndHorizontal();

                if (cEvent.dialogueAudioSound.loopSfx)
                {
                    height += 35;
                    EditorGUILayout.BeginHorizontal("Delay and loop settings");
                    //EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Delay Between Loops", GUILayout.Width(130));
                    cEvent.dialogueAudioSound.loopDelay = EditorGUILayout.Slider(cEvent.dialogueAudioSound.loopDelay, 0f, 0.09f);
                    //EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal("Delay and loop settings");
                    EditorGUILayout.LabelField("Loop until next button is clicked?", GUILayout.Width(200));
                    cEvent.waitUntilNextorEnd = EditorGUILayout.Toggle(cEvent.waitUntilNextorEnd);

                    EditorGUILayout.EndHorizontal();
                }




                GUILayout.EndVertical();
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Audio is not supported for this type");
                EditorGUILayout.EndHorizontal();
            }

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();




        }
        GUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }
}

