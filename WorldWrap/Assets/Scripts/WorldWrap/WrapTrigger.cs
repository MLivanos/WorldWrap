using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapTrigger : TriggerBehavior
{

    private void OnTriggerEnter(Collider other)
    {
        if (IsCollidingWithPlayer(other.gameObject))
        {
            wrapManager.LogTriggerEntry(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsCollidingWithPlayer(other.gameObject))
        {
            wrapManager.LogTriggerExit(gameObject);
        }
    }
}
