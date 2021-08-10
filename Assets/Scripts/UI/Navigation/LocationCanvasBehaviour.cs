using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Canvas))]
public class LocationCanvasBehaviour : MonoBehaviour
{

    public GameObject darkVeil;
    public Image darkVeil_img;

    public Location currentLocation;

    public Room currentRoom;

    public List<GameObject> sceneRooms;

    public Canvas canvas;

    private void Awake()
    {
        currentLocation = GameManager.instance.managers.navigationManager.currentLocation;
        currentRoom = GameManager.instance.managers.navigationManager.currentRoom;

        CreateDarkVeil();

        canvas = gameObject.GetComponent<Canvas>();

    }

    // Use this for initialization
    void Start()
    {
        SetDisplayedRoom();
        DisplayCurrentRoom();
    }

    // Update is called once per frame
    void Update()
    {
        currentLocation = GameManager.instance.managers.navigationManager.currentLocation;
        currentRoom = GameManager.instance.managers.navigationManager.currentRoom;
        DisplayCurrentRoom();
    }

    public void SetDisplayedRoom()
    {
        int i = 0;
        foreach (Room room in currentLocation.rooms)
        {
            if (gameObject.transform.GetChild(i).gameObject.GetComponent<RoomBehaviour>() != null)
            {
                sceneRooms.Add(gameObject.transform.GetChild(i).gameObject);

                sceneRooms[i].GetComponent<RoomBehaviour>().Init();
            }
            i++;
        }
    }

    public void DisplayCurrentRoom()
    {
        DisableAllRooms();
        EnableCurrentRoom();
    }

    public void DisableAllRooms()
    {
        foreach (GameObject go in sceneRooms)
        {
            go.SetActive(false);
        }
    }

    public void EnableCurrentRoom()
    {
        foreach (GameObject go in sceneRooms)
        {
            if (go.GetComponent<RoomBehaviour>().room == currentRoom)
            {
                go.SetActive(true);
                Sprite sprite = currentRoom.give_DaytimeSprite(GameManager.instance.managers.timeManager.currentTime);
                if (sprite != null)
                    go.GetComponent<Image>().sprite = sprite;
            }
        }
    }

    // Dark veil that will fade out after a amount of time
    // This will hide the room background to allow Quest manager to give reward like dialogues that could show the room bg for a few frames.
    public void CreateDarkVeil()
    {
        darkVeil = new GameObject();
        darkVeil.name = "Dark Veil to fade";
        darkVeil.transform.parent = this.transform;
        darkVeil.transform.SetAsLastSibling();

        RectTransform rt = darkVeil.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 0);
        rt.anchorMax = new Vector2(1, 1);
        rt.offsetMin = new Vector2(0, 0);
        rt.offsetMax = new Vector2(0, 0);

        darkVeil_img = darkVeil.AddComponent<Image>();
        darkVeil_img.color = new Color(0, 0, 0, 1);
        darkVeil_img.raycastTarget = false;

        StartCoroutine(FadeOut(darkVeil_img));

    }

    // Fade out the dark veil
    public IEnumerator FadeOut(Image image)
    {
        float fadeDuration = 0.5f;

        yield return new WaitForSeconds(0.1f);

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


    #region ScreenShoot

    public void ChangeTargetDisplay(int id) // Either 1 or 2 depending of what need to be done
    {
        canvas.targetDisplay = id;
    }

    #endregion
}
