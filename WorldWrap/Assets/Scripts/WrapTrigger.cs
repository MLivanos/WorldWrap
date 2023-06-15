using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapTrigger : TriggerBehavior
{

    private void OnTriggerEnter(Collider other)
    {
       wrapManager.LogTriggerEntry(gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        wrapManager.LogTriggerExit(gameObject);
    }
}
