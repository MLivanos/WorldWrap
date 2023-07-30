using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class TriggerBehavior : MonoBehaviour
{
    protected WrapManager wrapManager;
    protected int wrapLayer;

    protected virtual void Start()
    {
        try
        {
            wrapManager = GameObject.Find("WrapManager").GetComponent<WrapManager>();
        }
        catch
        {
            Exception missingManagerException = new Exception("To use TriggerBehavior, WrapManager object must exist and be called WrapManager");
            Debug.LogException(missingManagerException);
        }
        gameObject.GetComponent<BoxCollider>().isTrigger = true;
        wrapLayer = wrapManager.GetWrapLayer();
    }

    protected bool IsHandlingClientPlayer(GameObject playerObject)
    {
        if (!wrapManager.IsMultiplayer())
        {
            return true;
        }
        NetworkObject playerNetworkObject = playerObject.GetComponent<NetworkObject>();
        return playerNetworkObject.IsOwner;
    }

    protected bool IsCollidingWithPlayer(GameObject playerObject)
    {
        return playerObject.tag == "Player";
    }

}
