using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockTrigger : TriggerBehavior
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            wrapManager.LogBlockEntry(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            wrapManager.LogBlockExit(gameObject);
        }
    }
}
