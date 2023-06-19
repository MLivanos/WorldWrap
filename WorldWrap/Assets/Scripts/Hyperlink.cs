using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hyperlink : MonoBehaviour
{
    [SerializeField] string url;

    public void OpenLink()
    {
        Application.OpenURL(url);
    }

}
