using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BikeRidingMinigame;

public class MailReaderBehaviour : MonoBehaviour
{
    public Font font;


    public Mail mailToRead;

    public int current_width = 1146;

    [Header("Scene Referenced GameObject")]
    public ScrollRect scrollRect;
    public Transform scrollListContent;
    public Scrollbar scrollbar;
    public Text Subject;
    public Text From;
    public List<GameObject> ContentList;
    public Button BackButton;
    public GameObject mailList;

    // Use this for initialization
    public void Start()
    {
        CreateContent();
    }

    public void CreateContent()
    {
        Subject.text = mailToRead.subject;
        Subject.font = font;
        From.text = mailToRead.author;
        From.font = font;



        ContentList = mailToRead.CreateContentList(font);

        // Clean the content
        foreach (Transform child in scrollListContent)
        {
            Destroy(child.gameObject);
        }

        // Add the content to the list
        foreach (GameObject go in ContentList)
        {
            // Setting the parent
            go.transform.parent = scrollListContent;


            Image imageGo = go.GetComponent<Image>();
            // Adapt Image ratio to parent width
            if (imageGo != null)
            {
                Debug.Log("height " + imageGo.gameObject.name + ": " + imageGo.sprite.rect.height);
                Debug.Log("width " + imageGo.gameObject.name + ": " + imageGo.sprite.rect.width);
                //Debug.Log("<color=red>width " + imageGo.gameObject.name + "</color>: " + imageGo.rectTransform.rect.width);

                if(imageGo.sprite.rect.width > current_width)
                {
                    //
                    Debug.Log("<color=red>Adapt</color> : " + imageGo.name);
                    int processedHeight = AdaptImageHeightToWidth(
                        (int)imageGo.sprite.rect.width,
                        (int)imageGo.sprite.rect.height,
                        current_width);
                    Debug.Log("<color=purple>Processed height</color> : " + processedHeight);
                    imageGo.rectTransform.sizeDelta = new Vector2(current_width, processedHeight);
                }
            }
        }

        

        scrollbar.value = 1;

        scrollRect.verticalNormalizedPosition = 1;
    }



    public int AdaptImageHeightToWidth(int sprite_width, int sprite_height, int matched_width)
    {
        return matched_width * sprite_height / sprite_width;
    }


    public void GoBackToMailList()
    {
        ComputerManager.instance.LoadApp(1);
    }

}
