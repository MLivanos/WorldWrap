using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
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