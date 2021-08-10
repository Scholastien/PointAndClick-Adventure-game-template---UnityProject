using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;

[CreateAssetMenu(fileName = "new Mail", menuName = "LewdOwl/Mail/Mail")]
public class Mail : ScriptableObject {
    [Space(20)]
    public bool unlocked;
    public bool alreadyRead;

    [Space(20)]
    public string author;
    public string subject;

    [TextArea(10, 40)]
    [Tooltip("Put \"[index]\" to reference a sprite in attachment")]
    public string content;
    [Tooltip("Sprites")]
    public List<Sprite> attachment;


    [HideInInspector]
    public List<string> tempList;
    [HideInInspector]
    public List<int> integerIndex;


    /// <summary>
    /// The email should be read and according to the tag* in it, display corresponding gameobject* in the right order
    /// 
    /// *string tag = " [n] " where n is the index in the attachment list.
    /// 
    /// *displayed gameobjects will either be UI.Text or UI.Image
    /// that way we can have: text + image + text
    /// </summary>
    /// <returns>
    /// List of displayed object after parcing
    /// </returns>
    public List<GameObject> CreateContentList(Font font)
    {
        List<GameObject> gameObjects = new List<GameObject>();

        ParseMail();

        for (int i = 0; i < tempList.Count; i++)
        {
            int parsedInteger = 0;
            bool isInteger = int.TryParse(tempList[i], out parsedInteger);

            GameObject go = new GameObject();

            if (isInteger)
            {
                go.name = i + " - Image attachment[" + parsedInteger+ "]";
                // Ask for a Image gameobject
                Image imageGo = go.AddComponent<Image>();
                imageGo.sprite = attachment[parsedInteger];



                imageGo.preserveAspect = true;
                
                imageGo.SetNativeSize();

                gameObjects.Add(go);
            }
            else
            {
                String value = tempList[i];
                int startIndex = 0;
                int length = 15;
                go.name = 
                    i + 
                    " - Text " + 
                    value.Substring(
                        startIndex, 
                        length >= tempList[i].Length? tempList[i].Length - 1 : length) + "...";
                // Ask for a text object
                Text textGo = go.AddComponent<Text>();
                textGo.text = tempList[i];
                textGo.font = font;
                textGo.fontSize = 26;
                textGo.alignment = TextAnchor.MiddleLeft;

                textGo.rectTransform.sizeDelta = new Vector2(100, 40);

                // text color hex = #323232
                Color color;
                if (ColorUtility.TryParseHtmlString("#323232", out color))
                    textGo.color = color;

                gameObjects.Add(go);

            }

        }


        

        //tempList = elements;

        return gameObjects;
    }


    public void ParseMail()
    {
        String pattern = @"[\[\.\]]";

        String[] elements = Regex.Split(content, pattern);

        //string[] parsedContent = content.Split();

        //foreach (String elem in elements)
        //    Debug.Log(elem);

        tempList = new List<string>();
        integerIndex = new List<int>();

        foreach (String elem in elements)
        {
            // Save id if element is a number
            int parsedInteger = 0;
            bool isInteger = int.TryParse(elem.ToString(), out parsedInteger);



            // Exclude string that contain only spaces
            bool containOnlySpace = true;
            foreach (char c in elem)
            {
                if (c != ' ')
                {
                    containOnlySpace = false;
                }
            }


            if (!containOnlySpace)
            {
                tempList.Add(elem.ToString());
            }
            if (isInteger)
            {
                integerIndex.Add(tempList.Count - 1);
            }
        }
    }
}
