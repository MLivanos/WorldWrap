using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockTrigger : TriggerBehavior
{

    private void OnTriggerEnter(Collider other)
    {
       wrapManager.LogBlockEntry(gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        wrapManager.LogBlockExit(gameObject);
    }
}
