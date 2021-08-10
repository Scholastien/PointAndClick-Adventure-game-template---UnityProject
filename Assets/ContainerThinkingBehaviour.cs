using UnityEngine;
using UnityEngine.UI;

public class ContainerThinkingBehaviour : MonoBehaviour
{

    public Transform SpeakerImageHolder;

    public GameObject darkFilter;
    public Sprite matteBlack;


    // Update is called once per frame
    void Update()
    {
        // if there's 2 character
        if (SpeakerImageHolder.childCount == 2)
        {

            // spawn dark filter
            SpawnFilter();

            // child 1 set to last position
            SpeakerImageHolder.GetChild(0).transform.SetAsLastSibling();
        }
    }

    void SpawnFilter()
    {
        darkFilter = new GameObject("DarkFilter");
        darkFilter.transform.parent = SpeakerImageHolder;
        darkFilter.transform.SetAsLastSibling();



        Image image = darkFilter.AddComponent<Image>();
        image.sprite = matteBlack;
        var tempColor = image.color;
        tempColor.a = 0.3f;
        image.color = tempColor;



        RectTransform rt = darkFilter.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0f, 0f);
        rt.anchorMax = new Vector2(1f, 1f);
        rt.sizeDelta = new Vector2(1920f, 1080f);




    }
}
