using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockTrigger : TriggerBehavior
{
    private List<GameObject> objectsInsideBox = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            wrapManager.LogBlockEntry(gameObject);
        }
        objectsInsideBox.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            wrapManager.LogBlockExit(gameObject);
        }
        objectsInsideBox.Remove(other.gameObject);
    }
}
