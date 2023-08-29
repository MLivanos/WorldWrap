using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldWrapNetworkObject : MonoBehaviour
{
    private TransformRelay relay;
    private ulong clientID;

    public ulong getClientID()
    {
        return clientID;
    }

    public void setClientID(ulong id)
    {
        clientID = id;
    }

    public TransformRelay getTransformRelay()
    {
        return relay;
    }

    public void setTransformRelay(TransformRelay newRelay)
    {
        relay = newRelay;
    }
}
