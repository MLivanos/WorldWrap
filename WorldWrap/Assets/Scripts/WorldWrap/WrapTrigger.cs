using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapTrigger : TriggerBehavior
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            wrapManager.LogTriggerEntry(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            wrapManager.LogTriggerExit(gameObject);
        }
    }
}
