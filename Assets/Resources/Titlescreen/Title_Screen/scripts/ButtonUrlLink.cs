using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonUrlLink : MonoBehaviour
{
    public void GoToUrl(string url)
    {
        Application.OpenURL(url);
    }

}
