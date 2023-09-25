using UnityEngine;
using Unity.Netcode;

public class WorldWrapNetworkObject : MonoBehaviour
{
    private TransformRelay relay;

    public ulong GetClientID()
    {
        return relay.GetClientID();
    }

    public TransformRelay getTransformRelay()
    {
        return relay;
    }

    public void setTransformRelay(TransformRelay newRelay)
    {
        relay = newRelay;
    }

    private void OnTransformParentChanged()
    {
        if (transform.parent == null)
        {
            return;
        }
        WorldWrapNetworkObject parentNetworkObject = transform.parent.gameObject.GetComponent<WorldWrapNetworkObject>();
        if (parentNetworkObject != null && parentNetworkObject.GetClientID() != relay.GetClientID())
        {
            relay.ChangeOwnership(parentNetworkObject.GetClientID());
        }
    }
}
