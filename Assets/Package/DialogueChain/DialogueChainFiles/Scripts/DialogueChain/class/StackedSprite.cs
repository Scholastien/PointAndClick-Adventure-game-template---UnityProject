using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class StackedSprite
{
    // picture to display
    public Sprite sprite;

    public float y_pivot = 0.25f;

    // X and Y position on the canvas (range = -50 to 50)
    public float x_pos;
    public float y_pos;

    // Rotate automatic with x position
    public SpriteOrientation spriteOrientation = SpriteOrientation.Left;
    public bool automaticRotation = true;
    public bool invert;

    // Effect
    public bool useEffect;
    public StackedSpriteEffect effect;
    public bool autoSkip = false;
    #region Fade
    public float fadeDuration = 0.5f;
    public float fadeDelay = 0f;
    #endregion
    #region Shake
    public bool vertical = false;
    public int shakeRepetition = 5;
    public float shakeAmplitude = 15f;
    public float shakeFrequency = 0.5f;
    #endregion
    #region Translate
    public float translationStart = 50;
    public float translationEnd = 50;
    public float translationDelay = 5f;
    public float translationSpeed = 5f;
    #endregion


    // Dimension of the sprite
    private float width;
    private float height;

    public int id { get; set; }

    public StackedSprite()
    {
        x_pos = 0f;
        y_pos = -50f;
        automaticRotation = true;
        invert = true;
    }


    // Transform the relative position to fit the screen dimension
    public Tuple<float, float> CalculatePosOnCanvas(float width, float height)
    {
        // Transform xpos and ypos to percentage
        float x = x_pos;
        float y = y_pos + 50;

        if (x > 0 && automaticRotation)
        {
            switch (spriteOrientation)
            {
                case SpriteOrientation.Right:
                    invert = true;
                    break;
                case SpriteOrientation.Left:
                    invert = false;
                    break;
                default:
                    break;
            }
        }
        else if (x <= 0 && automaticRotation)
        {
            switch (spriteOrientation)
            {
                case SpriteOrientation.Right:
                    invert = false;
                    break;
                case SpriteOrientation.Left:
                    invert = true;
                    break;
                default:
                    break;
            }
        }

        float x_value = width * x / 100;
        float y_value = y / 100;
        //Debug.Log(y_value);



        Tuple<float, float> result = new Tuple<float, float>(x_value, y_value);

        return result;
    }


    #region Effect
    // move game object from A to B
    public void Translation(GameObject go)
    {
        DialogueManager.instance.StartCoroutine(Translate(go.GetComponent<RectTransform>(), this));
    }
    //shake game object
    public void Shaking(GameObject go)
    {
        DialogueManager.instance.StartCoroutine(Shake(go.transform, this));
    }
    // Change SpriteAlpha from 0 to 100 with an operation delay
    public void FadeIn(GameObject go)
    {
        Image image = go.GetComponent<Image>();
        DialogueManager.instance.StartCoroutine(Fade(image, 0, this));
    }


    // Change SpriteAlpha from 100 to 0 with an operation delay
    public void FadeOut(GameObject go)
    {
        Image image = go.GetComponent<Image>();
        DialogueManager.instance.StartCoroutine(Fade(image, 1, this));
    }

    public IEnumerator Translate(RectTransform t, StackedSprite stackedSprite)
    {


        var wait = new WaitForSeconds(1 / (translationSpeed));

        // Translation between 0 and 100
        // Canvas width 1920 
        // middle = 0 , far left = -960, far right = 960

        // % on the screen ratio = TranslationValue * 1920 / 100
        // apply canvas position on it = % on the screen ratio - 960
        int startPos = (int)(translationStart * 1920 / 100);
        Debug.Log("Start pos : " + startPos);

        Vector2 start = new Vector2(startPos - 960, t.anchoredPosition.y);


        int endPos = (int)(translationEnd * 1920 / 100);
        Debug.Log("End pos : " + endPos);

        Vector2 end = new Vector2(endPos - 960, t.anchoredPosition.y);
        t.anchoredPosition = start;

        yield return new WaitForSeconds(translationDelay);

        //float startTime = Time.time;


        //while (t.anchoredPosition.x != end.x)
        //{
        //    float distCovered = (Time.time - startTime) * translationSpeed;
        //    float fracJourney = distCovered / (startPos - endPos - 1920);
        //    t.localPosition = Vector3.Lerp(start, end, fracJourney);



        //}


        while (t.anchoredPosition.x != end.x)
        {
            if (t.anchoredPosition.x < end.x)
            {
                t.anchoredPosition = new Vector2(t.anchoredPosition.x + (int)(translationSpeed), t.anchoredPosition.y);
                if (t.anchoredPosition.x >= end.x)
                    break;
            }
            else if (t.anchoredPosition.x > end.x)
            {
                t.anchoredPosition = new Vector2(t.anchoredPosition.x - (int)(translationSpeed), t.anchoredPosition.y);
                if (t.anchoredPosition.x <= end.x)
                    break;
            }
            else
            {
                break;
            }
            yield return wait;

            if (t == null)
                break;
        }
        if (stackedSprite.autoSkip)
        {
            Debug.Log("<color=yellow>" + stackedSprite.sprite.name + "stackedSprite.autoSkip : " + stackedSprite.autoSkip + "</color>");
            DialogueController.instance.SkipAfterEffect(stackedSprite);
        }
    }

    public IEnumerator Shake(Transform t, StackedSprite stackedSprite)
    {

        if (t != null)
        {
            Vector3 OriginalPos = t.position;
            var wait = new WaitForSeconds(shakeFrequency / shakeRepetition);

            float amp = shakeAmplitude;

            //Mathf.Sin(Time.time * shakeSpeed) * shakeAmout;
            for (int i = 0; i < shakeRepetition; i++)
            {
                if (t == null)
                    break;

                //t.position = OriginalPos + UnityEngine.Random.insideUnitSphere * shakeAmplitude;

                Vector2 modifier = new Vector2((UnityEngine.Random.insideUnitSphere * shakeAmplitude).x, (UnityEngine.Random.insideUnitSphere * shakeAmplitude).y);

                float amp_x = modifier.x;
                float amp_y = vertical ? modifier.y : 0f;

                t.position = new Vector3(OriginalPos.x + amp_x, OriginalPos.y + amp_y);
                amp *= -1;
                yield return wait;
            }
            if (t != null)
                t.position = OriginalPos;
        }
        yield return new WaitForSeconds(0f);

        if (stackedSprite.autoSkip)
        {
            Debug.Log("<color=yellow>" + stackedSprite.sprite.name + "stackedSprite.autoSkip : " + stackedSprite.autoSkip + "</color>");
            DialogueController.instance.SkipAfterEffect(stackedSprite);
        }
    }


    // Fade a picture With and alpha value to start (0 or 1 / transparent to opaque OR opaque to transparent)
    public IEnumerator Fade(Image image, int alphaStart, StackedSprite stackedSprite)
    {
        var wait = new WaitForSeconds(fadeDuration / 10);

        if (image != null)
        {

            // fade in after delay
            if (alphaStart == 0)
            {
                Color start = image.color;
                for (float i = 0; i <= fadeDuration; i += Time.deltaTime)
                {
                    // increase alpha
                    float alpha = i / fadeDuration;
                    if (image != null)
                        image.color = new Color(start.r, start.g, start.b, alpha);
                    else
                        break;
                    yield return null;
                }
                if (image != null)
                    image.color = new Color(start.r, start.g, start.b, 1);
            }

            // fade out after delay
            else
            {
                Color start = image.color;
                for (float i = fadeDuration; i >= 0; i -= Time.deltaTime)
                {
                    // decrease alpha
                    float alpha = i / fadeDuration;
                    if (image != null)
                        image.color = new Color(start.r, start.g, start.b, alpha);
                    else
                        break;
                    yield return null;
                }
                if (image != null)
                    image.color = new Color(start.r, start.g, start.b, 0);
            }

            //Debug.LogError("<color=red> Current autoskip </color> "+ stackedSprite.autoSkip);

            if (stackedSprite.autoSkip)
            {
                Debug.Log("<color=yellow>" + stackedSprite.sprite.name + "stackedSprite.autoSkip : " + stackedSprite.autoSkip + "</color>");
                DialogueController.instance.SkipAfterEffect(stackedSprite);
            }
        }
    }

    #endregion
}





public enum SpriteOrientation
{
    Right,
    Left
}

public enum StackedSpriteEffect
{
    Translation,
    Shaking,
    FadeIn,
    FadeOut
}