using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleText : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown("t"))
        {
            ToggleTextVisibility();
        }
    }

    private void ToggleTextVisibility()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(!child.gameObject.activeSelf);
        }
    }
}
