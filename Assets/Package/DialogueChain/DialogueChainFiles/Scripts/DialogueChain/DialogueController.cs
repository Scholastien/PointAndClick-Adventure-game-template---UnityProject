using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class DialogueController : MonoBehaviour
{
    #region Declarations
    public static DialogueController instance;

    //User Customizable options
    public Canvas dialogueCanvas;
    //Default bg that allow to see through the dialogue canvas
    public Sprite transparentBg;
    public Sprite matteBlack;
    public Material blurryMaterial;
    public List<GameObject> containers = new List<GameObject>();
    public List<Sprite> boxImages = new List<Sprite>();
    public List<Sprite> speakerBoxImages = new List<Sprite>();
    public bool chainDataReset = true;

    public DialogueChain currentDialogueChain;

    Vector3 originalImageScale;
    Vector3 originalImagePos;
    Vector3 originalSpeakerPos;
    DialogueContainer currentContainer = null;
    GameObject loadedContainer = null;
    Canvas loadedCanvas;
    ChainEvent currentEvent;

    [HideInInspector] public bool buttonPressed;
    [HideInInspector] public bool isRunning;
    [HideInInspector] public bool isWriting;
    [HideInInspector] public bool isFading;
    [HideInInspector] public bool isWaiting;
    [HideInInspector] public bool finishWriting;
    [HideInInspector] public float tempTextDelay;
    #endregion

    private void Awake()
    {
        //Singleton
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            isRunning = false;
        }
    }

    //Putting the loading and saving methods here get around the fact that unity assets keep their changes after leaving playmode.
    private void OnDisable()
    {
        if (chainDataReset)
        {
            SaveChainData.Load("tempChainData.dat");
        }
    }
    private void Start()
    {
        if (chainDataReset)
        {
            SaveChainData.Save("tempChainData.dat");
        }
    }

    #region Handling Button Presses
    //Waits for the dialogue advancement button to be pressed
    public IEnumerator RunNextEventAfterUserConfirms(DialogueChain chain, ChainEvent cEvent)
    {

        NavigationManager.instance.needToOpenTheNavBar = true;

        if (TransitionManager.instance.maskAnimationState != AnimationState.Done)
        {
            yield return new WaitUntil(() => TransitionManager.instance.maskAnimationState == AnimationState.Ending);
            yield return new WaitForSeconds(TransitionManager.instance.transitionTimeEnd);
            TransitionManager.instance.maskAnimationState = AnimationState.Done;
        }


        if (currentEvent.useSound)
        {
            Debug.Log("<color=green> Use Audio </color> " + currentEvent.dialogueAudioSound.sound.clipName);

            AudioManager.instance.dialogueAudio.AddNewDialogueSound(currentEvent.dialogueAudioSound);

        }

        buttonPressed = false;
        //Debug.Log("button next");

        string buttonString = "";

        while (isWaiting || isFading)
        {
            yield return new WaitForEndOfFrame();
        }

        if (isWriting)
        {
            do
            {
                yield return new WaitForEndOfFrame();
                if (FinishTextButtonPressed() != null && FinishTextButtonPressed() != "")
                {
                    buttonPressed = true;
                    buttonString = FinishTextButtonPressed();
                }
            } while (!buttonPressed && isWriting);
            if (isWriting)
            {
                do
                {
                    yield return new WaitForEndOfFrame();
                } while (Input.GetAxis(buttonString) != 0 && isWriting);

                finishWriting = true;
            }
        }

        buttonPressed = false;
        buttonString = "";
        yield return new WaitForEndOfFrame();

        do
        {
            yield return new WaitForEndOfFrame();
            switch (currentEvent.cEventType)
            {
                case ChainEventType.Audio:
                    break;
                case ChainEventType.Check:
                    break;
                case ChainEventType.IntAdjustment:
                    break;
                case ChainEventType.ItemManagement:
                    break;
                case ChainEventType.SecondaryInput:
                    break;
                case ChainEventType.SetTrigger:
                    break;
                case ChainEventType.Start:
                    break;
                case ChainEventType.UserInput:
                    break;
                case ChainEventType.VariableModifier:
                    break;
                case ChainEventType.SetLocation:
                    break;
                default:
                    //Debug.Log("Default input");
                    if (AdvanceButtonPressed() != null && AdvanceButtonPressed() != "")
                    {
                        buttonPressed = true;
                        buttonString = AdvanceButtonPressed();
                    }
                    break;
            }
        } while (!buttonPressed);
        do
        {
            yield return new WaitForEndOfFrame();
        } while (Input.GetAxis(buttonString) != 0 || GameManager.instance.managers.dialogueManager.GetBtnPressed());


        CloseDialogue();


        cEvent.previousChainEvent = currentEvent;



        chain.RunEvent(cEvent);
    }

    public static string AdvanceButtonPressed()
    {
        if (!DialogueManager.instance.blockNext && TransitionManager.instance.IsAnimCompleted())
        {
            for (int i = 0; i < DialogueChainPreferences.inputsToAdvanceDialogue.Length; i++)
            {
                if ((Input.GetAxis(DialogueChainPreferences.inputsToAdvanceDialogue[i]) > 0))
                {
                    return DialogueChainPreferences.inputsToAdvanceDialogue[i];
                }
            }
        }

        return "";
    }
    public static string FinishTextButtonPressed()
    {
        for (int i = 0; i < DialogueChainPreferences.inputsToAdvanceDialogueQuickly.Length; i++)
        {
            if ((Input.GetAxis(DialogueChainPreferences.inputsToAdvanceDialogueQuickly[i]) > 0))
            {
                return DialogueChainPreferences.inputsToAdvanceDialogueQuickly[i];
            }
        }

        return "";
    }
    #endregion

    #region Handling Dialogue Boxes
    //The method called when a chain event is a dialogue or user input. It will call the appropriate methods to instantiate dialogue boxes and write text on them.
    public void ShowDialogue(ChainEvent cEvent)
    {
        isRunning = true;
        finishWriting = false;
        isWriting = false;
        isFading = false;
        isWaiting = true;
        currentEvent = cEvent;


        Invoke("ContinueDialogue", cEvent.dialogueWaitTime);
    }

    void ContinueDialogue()
    {

        isWaiting = false;
        currentContainer = new DialogueContainer();
        GetContainer(currentEvent.dialogueContainer);
        currentContainer.container.SetActive(true);

        float speakerTextToBoxDiff = currentContainer.speakerNameBox.GetComponent<RectTransform>().rect.width - currentContainer.speakerNameText.GetComponent<RectTransform>().rect.width;


        #region CallExternalManagers

        ///////
        //////////
        //////////
        //////////
        //////////
        //////////
        //////////
        //////////
        //////////
        //////////
        ///



        VideoChainHandler();




        #endregion






        if (currentEvent.dialoguefadeTime > 0)
        {
            StartCoroutine("FadeDialogue", currentEvent.dialoguefadeTime);
        }

        if (currentEvent.showImage)
        {

            currentContainer.speakerImage.enabled = true;

            if (currentEvent.useCustomPlayerImage)
            {
                currentEvent.speakerImage = DialogueChainPreferences.GetPlayerDialogueAvatars()[currentEvent.playerImageIndex];
            }

            if (currentEvent.useTransparentBG)
            {
                Debug.Log("<color=purple>Transparent BG</color>");
                currentContainer.speakerImage.GetComponent<Image>().sprite = instance.transparentBg;
                // TODO Correct this
            }
            if (currentEvent.useBlurryBG)
            {
                // Enable a transparent layer on top of the used background
                //currentContainer.speakerImage
                GameObject go = Instantiate(currentContainer.speakerImage.gameObject);
                go.transform.SetParent(currentContainer.speakerImage.transform);
                go.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
                go.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
                //apply the mate black background
                Image blurry_img = go.GetComponent<Image>();
                blurry_img.sprite = matteBlack;
                // apply the blurry mat to the layer
                blurry_img.material = blurryMaterial;
            }

            if (currentEvent.useRandomSprite)
            {
                int index = UnityEngine.Random.Range(0, currentEvent.randomSprites.Count);


                currentEvent.speakerImage = currentEvent.randomSprites[index];
            }
            if (currentEvent.useStackedPicture)
            {
                foreach (StackedSprite stacked in currentEvent.stackedSprites)
                {
                    GameObject go = StackedImageHandler(stacked, currentContainer.speakerImage.transform);
                }
            }

            if (currentEvent.autoFadeAtStart)
            {
                BackgroundFadingHandler(currentContainer.speakerImage.gameObject);
            }

            currentContainer.speakerImage.GetComponent<Image>().sprite = currentEvent.useTransparentBG ? transparentBg : currentEvent.speakerImage;

            originalImageScale = currentContainer.speakerImage.GetComponent<RectTransform>().localScale;

            if (currentEvent.flipImage)
            {
                currentContainer.speakerImage.rectTransform.localScale = new Vector3(originalImageScale.x * -1, originalImageScale.y, originalImageScale.z);

                if (currentContainer.speakerImage.GetComponent<LayoutElement>().ignoreLayout)
                {
                    Vector3 tempPos = currentContainer.speakerImage.rectTransform.anchoredPosition;
                    tempPos.x += currentContainer.speakerImage.rectTransform.rect.width;
                    currentContainer.speakerImage.GetComponent<RectTransform>().anchoredPosition = tempPos;
                }
                else
                {
                    currentContainer.speakerImage.transform.parent.GetComponent<HorizontalLayoutGroup>().padding.left += (int)currentContainer.speakerImage.rectTransform.rect.width;
                }
            }
        }
        else
        {
            currentContainer.speakerImage.enabled = false;
        }

        if (currentEvent.noSpeaker || currentEvent.speaker == "")
        {
            currentContainer.speakerNameBox.SetActive(false);
        }
        else
        {
            currentContainer.speakerNameBox.SetActive(true);
            currentContainer.speakerNameText.text = DialogueChainPreferences.GetCharacterNameWithString(currentEvent.speaker);
            Color color = VariablesManager.instance.charactersCollection.GetCharacterColor(currentEvent.speaker);
            currentContainer.speakerNameText.color = new Color(color.r, color.g, color.b, color.a);
            originalSpeakerPos = currentContainer.speakerNameBox.GetComponent<RectTransform>().anchoredPosition;

            if (!currentEvent.leftSide)
            {
                Vector2 containerPos = currentContainer.container.GetComponent<RectTransform>().anchoredPosition;
                containerPos.x = loadedCanvas.GetComponent<RectTransform>().rect.width - currentContainer.container.GetComponent<LayoutElement>().preferredWidth - containerPos.x;
                currentContainer.container.GetComponent<RectTransform>().anchoredPosition = containerPos;

                currentContainer.dialogueBox.transform.parent.GetComponent<VerticalLayoutGroup>().childAlignment = TextAnchor.LowerRight;
                currentContainer.speakerNameBox.GetComponent<RectTransform>().anchoredPosition = new Vector3(currentContainer.dialogueBox.transform.parent.GetComponent<RectTransform>().rect.width - (currentContainer.speakerNameText.GetComponent<RectTransform>().rect.width + speakerTextToBoxDiff) + originalSpeakerPos.x, originalSpeakerPos.y, originalSpeakerPos.z);

                if (currentEvent.showImage)
                {
                    Vector2 pos = currentContainer.speakerImage.GetComponent<RectTransform>().anchoredPosition;
                    int mod = -1;
                    if (currentEvent.flipImage)
                    {
                        mod = 1;
                    }
                    pos.x = (currentContainer.container.GetComponent<LayoutElement>().preferredWidth - currentContainer.speakerImage.GetComponent<RectTransform>().anchoredPosition.x + currentEvent.speakerImage.rect.width * mod);
                    currentContainer.speakerImage.GetComponent<RectTransform>().anchoredPosition = pos;
                }
                if (!currentEvent.noSpeaker)
                {
                    Vector2 pos = currentContainer.speakerNameBox.GetComponent<RectTransform>().anchoredPosition;
                    pos.x = (currentContainer.container.GetComponent<LayoutElement>().preferredWidth - currentContainer.speakerNameBox.GetComponent<RectTransform>().anchoredPosition.x - currentContainer.speakerNameText.GetComponent<Text>().preferredWidth - pos.x);
                    currentContainer.speakerNameBox.GetComponent<RectTransform>().anchoredPosition = pos;
                }
            }
            else
            {
                //verticallayout group on dialogueBox
                currentContainer.dialogueBox.transform.parent.GetComponent<VerticalLayoutGroup>().childAlignment = TextAnchor.LowerLeft;
            }
        }

        if (currentEvent.dialogue != "")
        {
            currentContainer.dialogueBox.SetActive(true);
        }
        else
        {
            currentContainer.container.SetActive(true);
            if (currentEvent.cEventType == ChainEventType.UserInput)
            {
                currentContainer.dialogueBox.SetActive(true);
                currentContainer.dialogueBoxImage.color = new Color(1, 1, 1, 0);
                currentContainer.speakerNameText.enabled = false;
                currentContainer.dialogueText.enabled = false;
            }
            else
            {
                currentContainer.dialogueBoxImage.color = new Color(1, 1, 1, 0);


                currentContainer.dialogueBox.SetActive(false);
                ///
                ///
                ///
                ///
                ///
                ///
                ///
                ///
                currentContainer.speakerNameBox.SetActive(false);
            }
        }

        if (currentDialogueChain == null)
        {
            StartCoroutine(TypeOutDialogue(Dialogue.DialogueSyntaxFix(currentEvent.dialogue), GameManager.instance.managers.optionManager.textSpeed, currentEvent));
        }
        else
        {
            StartCoroutine(TypeOutDialogue(Dialogue.DialogueSyntaxFix(currentEvent.dialogue), GameManager.instance.managers.optionManager.textSpeed, currentEvent));
        }

        if (currentEvent.cEventType == ChainEventType.UserInput)
        {
            StartCoroutine(ShowInputs(currentEvent, currentContainer));
        }
        else
        {
            if (currentContainer.inputPanel != null)
            {
                currentContainer.inputPanel.SetActive(false);
            }
        }
    }

    //Instantiates the correct dialogue container prefab chosen by the dialogue chain event
    void GetContainer(ContainerType cType)
    {



        Scene s = SceneManager.GetSceneByName("Dialogue");



        GameObject[] gameObjects = s.GetRootGameObjects();
        //Debug.Log(gameObjects.Length);

        loadedCanvas = Instantiate(dialogueCanvas, Vector3.zero, Quaternion.identity) as Canvas;
        DialogueSpawnBehaviour dialogueSpawn = GameManager.instance.managers.dialogueManager.dialogueSpawn;


        loadedCanvas.transform.SetParent(dialogueSpawn.gameObject.transform);
        loadedContainer = Instantiate(containers[(int)cType], loadedCanvas.transform) as GameObject;

        SetupContainer();


    }

    //Gets the sprite associated with the boximage enum
    public Sprite GetBoxSprite(BoxImage bType)
    {
        return boxImages[(int)bType];
    }
    public Sprite GetSpeakerBoxSprite(BoxImage bType)
    {
        return speakerBoxImages[(int)bType];
    }

    //Sets these variables equal to their appropriate gameobjects
    void SetupContainer()
    {
        currentContainer.container = loadedContainer;

        currentContainer.speakerImage = loadedContainer.GetComponent<ContainerUI>().dialogueContainer.speakerImage;
        currentContainer.speakerNameBox = loadedContainer.GetComponent<ContainerUI>().dialogueContainer.speakerNameBox;
        currentContainer.speakerNameText = loadedContainer.GetComponent<ContainerUI>().dialogueContainer.speakerNameText;
        currentContainer.dialogueBox = loadedContainer.GetComponent<ContainerUI>().dialogueContainer.dialogueBox;
        currentContainer.dialogueText = loadedContainer.GetComponent<ContainerUI>().dialogueContainer.dialogueText;
        currentContainer.inputPanel = loadedContainer.GetComponent<ContainerUI>().dialogueContainer.inputPanel;
        currentContainer.dialogueBoxImage = loadedContainer.GetComponent<ContainerUI>().dialogueContainer.dialogueBoxImage;

        //currentContainer.speakerImage = loadedContainer.transform.Find("SpeakerImage").GetComponent<Image>();
        //currentContainer.speakerNameBox = loadedContainer.transform.Find("SpeakerNameBox").gameObject;
        //currentContainer.speakerNameText = loadedContainer.transform.Find("SpeakerNameBox").transform.Find("SpeakerName").GetComponent<Text>();
        //currentContainer.dialogueBox = loadedContainer.transform.Find("DialogueBox").gameObject;
        //currentContainer.dialogueText = loadedContainer.transform.Find("DialogueBox").transform.Find("Dialogue").GetComponent<Text>();
        //currentContainer.inputPanel = loadedContainer.transform.Find("InputArea").gameObject;

        currentContainer.dialogueText.text = "";
        currentContainer.speakerNameText.text = "";
    }

    //Types out the dialogue one character at a time depending on a textDelay variable taken from the current dialogue chain
    public IEnumerator TypeOutDialogue(string text, float delay, ChainEvent dEvent)
    {
        if (DialogueManager.instance.skip)
        {
            delay = 0.1f;
        }

        if (delay == 0)
        {
            //If the delay is 0, the text immediately fills the box
            currentContainer.dialogueText.text = text;
        }
        else
        {
            //Set the dialogue box text to the final text to break it into lines. This stops the end of the line from starting to type a word and then moving it to the next line.
            isWriting = true;
            currentContainer.dialogueText.text = text;
            if (dEvent.cEventType == ChainEventType.UserInput)
            {
                UserInput(dEvent, currentContainer);
            }

            Canvas.ForceUpdateCanvases();

            string[] lines = new string[currentContainer.dialogueText.cachedTextGenerator.lines.Count];
            for (int i = 0; i < currentContainer.dialogueText.cachedTextGenerator.lines.Count; i++)
            {
                int startIndex = currentContainer.dialogueText.cachedTextGenerator.lines[i].startCharIdx;
                int endIndex = (i == currentContainer.dialogueText.cachedTextGenerator.lines.Count - 1) ? currentContainer.dialogueText.text.Length
                    : currentContainer.dialogueText.cachedTextGenerator.lines[i + 1].startCharIdx;
                int length = endIndex - startIndex;
                lines[i] = (currentContainer.dialogueText.text.Substring(startIndex, length));
            }

            //Set the preferred box size to that of its final size so it doesn't change size while typing.
            float width = currentContainer.dialogueBox.GetComponent<RectTransform>().sizeDelta.x;
            //Debug.Log("preferredWidth on dialogueBox");
            currentContainer.dialogueBox.GetComponent<LayoutElement>().preferredWidth = width;
            float height = currentContainer.dialogueBox.GetComponent<RectTransform>().sizeDelta.y;
            //Debug.Log("preferredHeight on dialogueBox");
            currentContainer.dialogueBox.GetComponent<LayoutElement>().preferredHeight = height;

            //Reset the box to have no text or buttons
            if (dEvent.cEventType == ChainEventType.UserInput)
            {
                foreach (Transform child in currentContainer.inputPanel.transform)
                {
                    Destroy(child.gameObject);
                }
            }
            currentContainer.dialogueText.text = "";
            Canvas.ForceUpdateCanvases();

            //Write the text to the box one line at a time and one character at a time
            for (int i = 0; i < lines.Length; i++)
            {
                int characterCount = 0;
                while (characterCount < lines[i].Length)
                {
                    if (finishWriting)
                    {
                        currentContainer.dialogueText.text = text;
                        isWriting = false;
                        finishWriting = false;
                        yield break;
                    }

                    currentContainer.dialogueText.text += lines[i][characterCount++];

                    if (currentContainer.dialogueText.text.Length % delay == 0)

                        yield return new WaitForEndOfFrame();
                }

                //Enter the line breaks at the end of each line so that a word doesn't start typing on one line and drop to another.
                if (i < lines.Length - 1)
                {
                    currentContainer.dialogueText.text += "\n";
                }
            }
            if (!currentEvent.waitUntilNextorEnd)
            {
                AudioManager.instance.StartCoroutine("DestroyDialogueSFX");
            }
            isWriting = false;
        }
    }

    //Waits for the dialogue text to be finished writing before showing the possible user inputs
    public IEnumerator ShowInputs(ChainEvent dEvent, DialogueContainer container)
    {
        while (isWriting)
        {
            bool buttonPressed = false;
            string buttonString = "";

            do
            {
                yield return new WaitForEndOfFrame();
                if (FinishTextButtonPressed() != null && FinishTextButtonPressed() != "")
                {
                    buttonPressed = true;
                    buttonString = FinishTextButtonPressed();
                }
            } while (!buttonPressed && isWriting);
            while (isWriting && Input.GetAxis(buttonString) != 0)
            {
                yield return new WaitForEndOfFrame();
            }

            if (isWriting)
            {
                finishWriting = true;
                yield return new WaitForEndOfFrame();
            }
        }

        UserInput(dEvent, currentContainer);
    }

    //If the dialogue box has user inputs this adds prefab buttons for each one
    public void UserInput(ChainEvent dEvent, DialogueContainer container)
    {
        container.inputPanel.SetActive(true);

        for (int i = 0; i < dEvent.inputButtons.Count; i++)
        {
            // Current path
            //Assets/Resources/Prefab/Essentials/UI/Dialogue/Player Input/DialogueButton.prefab
            GameObject dialogueButton = Instantiate(Resources.Load("Prefab/Essentials/UI/Dialogue/Player Input/DialogueButton")) as GameObject;
            dialogueButton.transform.SetParent(container.inputPanel.transform);
            dialogueButton.transform.localScale = Vector3.one;
            int loopBack = i >= DialogueChainPreferences.characterBeforeInputText.Length ? 0 : i;

            string translated = LanguageManager.instance.GetTranslation(DialogueChainPreferences.characterBeforeInputText[loopBack] + dEvent.inputButtons[i].buttonText);

            dialogueButton.transform.GetChild(0).GetComponent<Text>().text = translated;
            dialogueButton.GetComponent<Button>().onClick.AddListener(delegate { currentDialogueChain.GetNextEventFromInput(dialogueButton.transform.GetSiblingIndex()); });
        }
        for (int i = 0; i < dEvent.secondaryInputButtons.Count; i++)
        {
            GameObject dialogueButton = Instantiate(Resources.Load("Prefab/Essentials/UI/Dialogue/Player Input/DialogueButton")) as GameObject;
            dialogueButton.transform.SetParent(container.inputPanel.transform);
            dialogueButton.transform.localScale = Vector3.one;
            int loopBack = i + dEvent.inputButtons.Count >= DialogueChainPreferences.characterBeforeInputText.Length ? 0 : i + dEvent.inputButtons.Count;
            dialogueButton.transform.GetChild(0).GetComponent<Text>().text = DialogueChainPreferences.characterBeforeInputText[loopBack] + dEvent.secondaryInputButtons[i].buttonText;
            dialogueButton.GetComponent<Button>().onClick.AddListener(delegate { currentDialogueChain.GetNextEventFromInput(dialogueButton.transform.GetSiblingIndex()); });
        }

    }

    //Fades the dialogue box as directed by ChainEvent dialogueFade
    IEnumerator FadeDialogue(float fadeTime)
    {
        isFading = true;

        Color tempImage = currentContainer.speakerImage.GetComponent<Image>().color;
        float o1 = tempImage.a;
        tempImage.a = 0;
        Color tempName = currentContainer.speakerNameBox.GetComponent<Image>().color;
        float o2 = tempName.a;
        tempName.a = 0;
        Color tempDialogue = currentContainer.dialogueBox.GetComponent<Image>().color;
        float o3 = tempDialogue.a;
        tempDialogue.a = 0;
        Color tempDialogueText = currentContainer.dialogueText.GetComponent<Text>().color;
        float o4 = tempDialogueText.a;
        tempDialogueText.a = 0;
        Color tempNameText = currentContainer.speakerNameText.GetComponent<Text>().color;
        float o5 = tempNameText.a;
        tempNameText.a = 0;

        float runningTime = 0;
        while (runningTime <= fadeTime)
        {
            currentContainer.speakerImage.GetComponent<Image>().color = tempImage;
            currentContainer.speakerNameBox.GetComponent<Image>().color = tempName;
            currentContainer.dialogueBox.GetComponent<Image>().color = tempDialogue;
            currentContainer.dialogueText.GetComponent<Text>().color = tempDialogueText;
            currentContainer.speakerNameText.GetComponent<Text>().color = tempNameText;

            tempImage.a = Mathf.Lerp(0, o1, 1f - Mathf.Cos((runningTime / fadeTime) * Mathf.PI * 0.5f));
            tempName.a = Mathf.Lerp(0, o2, 1f - Mathf.Cos((runningTime / fadeTime) * Mathf.PI * 0.5f));
            tempDialogue.a = Mathf.Lerp(0, o3, 1f - Mathf.Cos((runningTime / fadeTime) * Mathf.PI * 0.5f));
            tempDialogueText.a = Mathf.Lerp(0, o4, 1f - Mathf.Cos((runningTime / fadeTime) * Mathf.PI * 0.5f));
            tempNameText.a = Mathf.Lerp(0, o5, 1f - Mathf.Cos((runningTime / fadeTime) * Mathf.PI * 0.5f));

            runningTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        isFading = false;
    }


    //Destroys the dialogue container instance (When next is pressed or dialogue end).
    public void CloseDialogue()
    {

        StopCoroutine("FadeDialogue");
        if (currentContainer != null)
        {
            currentContainer = null;
            if (loadedCanvas != null)
            {
                Destroy(loadedContainer.gameObject);
                Destroy(loadedCanvas.gameObject);


                // Disable sounds
                AudioManager.instance.deletableDialogSfx = true;
                AudioManager.instance.StartCoroutine("DestroyDialogueSFX");

                if (currentEvent.isEnd)
                {
                    EventManager.Instance.EndOfDialogue();
                    // destroy vid only if it's the end of the dialogue
                    if (VideoPlayerManager.instance.videoToPlay.IsPlaying())
                    {
                        VideoPlayerManager.instance.videoToPlay.Stop(false, VideoPlayerManager.instance.rawImage, true);
                    }
                    else
                    {
                        VideoPlayerManager.instance.videoToPlay.Stop(false, VideoPlayerManager.instance.rawImage, true);
                    }
                }

                //VideoChainHandler();
            }
        }
    }

    public void BackgroundFadingHandler(GameObject original)
    {
        Debug.LogWarning("Entering fade handler");


        // Put the sprite in the original image
        original.GetComponent<Image>().sprite = currentEvent.speakerImage;

        //clone it
        GameObject clone = Instantiate(original);
        clone.transform.parent = original.transform.parent;

        // resize the clone
        RectTransform rt = clone.GetComponent<RectTransform>();
        rt.offsetMin = new Vector2(0, 0);
        rt.offsetMax = new Vector2(0, 0);

        // put the previous sprite in the cloned image
        clone.GetComponent<Image>().sprite = currentEvent.previousChainEvent.speakerImage;

        // Set both of them at the 2 first position of the parent, original first
        clone.transform.SetAsFirstSibling(); // <- clone need to fade out for us to be able to see the original
        original.transform.SetAsFirstSibling();

        // Fade out the clone
        DialogueManager.instance.CallForAutoFade(clone, currentEvent.autoFadeDuration, currentEvent.SkipWhenFadeDone);



    }






    // Create GameObject and initialize it with a StackedSprite
    public GameObject StackedImageHandler(StackedSprite stackedSprite, Transform parent)
    {
        // Creating GameObject
        GameObject go = new GameObject("stackedSprite_" + stackedSprite.id);
        go.transform.SetParent(parent);



        CanvasScaler canvasScaler = loadedCanvas.gameObject.GetComponent<CanvasScaler>();

        Image img = go.AddComponent<Image>();
        img.preserveAspect = true;
        if (stackedSprite.useEffect)
        {
            if (stackedSprite.effect == StackedSpriteEffect.FadeIn || stackedSprite.effect == StackedSpriteEffect.FadeOut)
            {
                Color start = img.color;
                img.color = new Color(start.r, start.g, start.b, 0);
            }

        }
        img.sprite = stackedSprite.sprite;
        img.SetNativeSize();


        // Applying StackedSpite parameters
        Tuple<float, float> screenPos = stackedSprite.CalculatePosOnCanvas(canvasScaler.referenceResolution.x, canvasScaler.referenceResolution.y);
        RectTransform rt = go.GetComponent<RectTransform>();

        rt.pivot = new Vector2(0.5f, screenPos.Item2);
        rt.anchorMin = new Vector2(0.5f, 0f);
        rt.anchorMax = new Vector2(0.5f, 0f);
        rt.anchoredPosition = new Vector2(screenPos.Item1, 0);
        rt.localScale = new Vector3(1f, 1f, 1f);

        Quaternion orientationInvert = new Quaternion(0f, 180f, 0f, 0f);
        Quaternion orientationStay = new Quaternion(0f, 0f, 0f, 0f);

        // Invert
        if (stackedSprite.invert)
        {
            rt.rotation = orientationInvert;
        }
        else
        {
            rt.rotation = orientationStay;
        }

        // Applying Effects
        if (stackedSprite.useEffect)
        {
            switch (stackedSprite.effect)
            {
                case StackedSpriteEffect.Translation:
                    stackedSprite.Translation(go);
                    break;
                case StackedSpriteEffect.Shaking:
                    stackedSprite.Shaking(go);
                    break;
                case StackedSpriteEffect.FadeIn:
                    stackedSprite.FadeIn(go);
                    break;
                case StackedSpriteEffect.FadeOut:
                    stackedSprite.FadeOut(go);
                    break;
                default:
                    break;
            }
        }


        return go;

    }


    public void SkipAfterEffect(StackedSprite stackedSprite)
    {
        if (stackedSprite.autoSkip)
        {
            GameManager.instance.managers.dialogueManager.NextEvent();
        }
    }


    public void TransitionHandler(ChainEvent cevent)
    {
        if(cevent.transitionType != TransitionType.None && cevent.isEnd)
        {
            TransitionManager.instance.SetNextTransition(cevent.transitionType);
        }
    }


    // Start a new video
    public void VideoChainHandler()
    {
        if (DialogueManager.instance.debug_ConversationLog)
            Debug.Log(currentEvent.cEventType + " : is end = " + currentEvent.isEnd + "  / inVideoChain : " + currentEvent.inVideoChain + "  / useVideo : " + currentEvent.useVideo);

        // Video Should be destroyed if one is playing and not in a dialogue chain (don't use vid)
        if (VideoPlayerManager.instance.videoToPlay.IsPlaying())
        {

            if (!currentEvent.useVideo)
            {
                VideoPlayerManager.instance.videoToPlay.Stop(false, VideoPlayerManager.instance.rawImage, true);
            }

            // Video destroyed if the dialogue end on a statut box and a video still running
            else if (!(currentEvent.cEventType == ChainEventType.Dialogue || currentEvent.cEventType == ChainEventType.UserInput) && currentEvent.isEnd)
            {
                VideoPlayerManager.instance.videoToPlay.Stop(false, VideoPlayerManager.instance.rawImage, true);
            }
        }


        // if in a dialogue chain: do nothing exept if it overwrite the previous vid
        if (currentEvent.inVideoChain)
        {
            if (currentEvent.overwritePreviousVideo)
            {
                // Delete previous video
                VideoPlayerManager.instance.videoToPlay.Stop(false, VideoPlayerManager.instance.rawImage, true);


                // Make a new video
                VideoPlayerManager.instance.videoToPlay = currentEvent.video;
                VideoPlayerManager.instance.videoToPlay.InitVideoPlayer(VideoPlayerManager.instance.videoPlayer);
                VideoPlayerManager.instance.videoToPlay.Play(true);
            }
        }
        // else
        else
        {
            // if playing vid: play vid
            if (currentEvent.useVideo)
            {
                // Make a new video
                VideoPlayerManager.instance.videoToPlay = currentEvent.video;
                VideoPlayerManager.instance.videoToPlay.InitVideoPlayer(VideoPlayerManager.instance.videoPlayer);
                VideoPlayerManager.instance.videoToPlay.Play(true);
            }
        }
    }







    #endregion

    public static bool ButtonPressed()
    {
        return instance.buttonPressed;
    }
    public static bool IsRunning()
    {
        return instance.isRunning;
    }
    public static bool IsWriting()
    {
        return instance.isWriting;
    }
    public static bool IsFading()
    {
        return instance.isFading;
    }
    public static bool IsWaiting()
    {
        return instance.isWaiting;
    }
}


//Used to assign gameobjects that will be accessed often when showing dialogue.
[System.Serializable]
public class DialogueContainer
{
    public GameObject container;
    public Image speakerImage;
    public GameObject speakerNameBox;
    public Text speakerNameText;
    public GameObject dialogueBox;
    public Text dialogueText;
    public GameObject inputPanel;
    public GameObject UI;
    public Image dialogueBoxImage;
}