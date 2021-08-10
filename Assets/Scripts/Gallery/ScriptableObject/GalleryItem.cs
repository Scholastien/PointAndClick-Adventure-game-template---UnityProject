using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A gallery item is a props that can be unlocked by the player through progression
/// <summary>
/// It state should be saved persitently (unlocked)
/// Depending on the UI choice the player made, the gallery either:
///   - Display a narrative gallery: The full scene with dialogue for immersion
///   - Display a standalone gallery: with only pics and videos relative to a specific event
/// </summary>
[CreateAssetMenu(fileName = "new GalleryItem", menuName = "LewdOwl/Gallery/GalleryItem")]
public class GalleryItem : ScriptableObject {

    [Header("State")]
    // Persistently saved, if true -> unlocked
    public bool unlocked = false;
    [Tooltip("Start at 1")]
    public int index = 0;


    [Header("Narrative")]
    #region Narrative
    public Sprite previewPicture;
    // DialogueChain to play
    public DialogueChain dialogueChain;
    #endregion

    [Header("Textless")]
    #region Dialogueless
    // Video collection
    public List<Video> videos;
    // Sprite collection
    public List<Sprite> sprites;
    #endregion


    public void UnlockGalleryItem()
    {
        unlocked = true;

        // Ask to the PersistentSave a save
        PersistentData.instance.Save();
    }
}
