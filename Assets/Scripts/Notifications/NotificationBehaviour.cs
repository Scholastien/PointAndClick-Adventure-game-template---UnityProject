using NotificationSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class NotificationBehaviour : MonoBehaviour {
    
    public bool spawnAnim = true;
    public bool despawnAnim = false;

    public RectTransform childPanel;
    public TextMeshProUGUI title;
    public TextMeshProUGUI value;
    public TextMeshProUGUI verb;

    public float width, height;

    public bool clicked = false;

    public Button button;

	void Awake () {
        button = GetComponent<Button>();
        spawnAnim = true;
        despawnAnim = false;
        clicked = false;
        RectTransform rt = GetComponent<RectTransform>();
        width = rt.rect.width;
        height = rt.rect.height;

        button.onClick.AddListener(() => Click());
        
    }

    #region ClickBehaviour
    private void Click()
    {
        clicked = true;
    }
    #endregion
}
