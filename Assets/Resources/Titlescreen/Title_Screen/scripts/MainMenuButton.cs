using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace TitleScreen
{
    [RequireComponent(typeof(Image))]
    public class MainMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public MainMenuGroup menuGroup;

        public Image background;

        public GameObject fieldPrefab;
        public Vector3 positionInParent;

        public UnityEvent onButtonSelected;
        public UnityEvent onButtonDeselected;

        public void OnPointerClick(PointerEventData eventData)
        {
            menuGroup.OnButtonSelected(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            menuGroup.OnButtonEnter(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            menuGroup.OnButtonExit(this);
        }

        // Start is called before the first frame update
        void Start()
        {
            background = GetComponent<Image>();
            menuGroup.Subscribe(this);
        }


        public void Select()
        {
            if(onButtonSelected != null)
            {
                onButtonSelected.Invoke();
            }
        }

        public void Deselect()
        {
            if (onButtonDeselected != null)
            {
                onButtonDeselected.Invoke();
            }
        }


    }
}