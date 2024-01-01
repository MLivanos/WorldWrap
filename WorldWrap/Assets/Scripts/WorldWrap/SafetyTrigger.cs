using System;
using UnityEngine;

public class SafetyTrigger : TriggerBehavior
{
    private void OnTriggerExit(Collider other)
    {
        if (IsCollidingWithPlayer(other.gameObject))
        {
            wrapManager.WrapWorld();
        }
    }
}