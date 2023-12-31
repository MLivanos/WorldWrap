using UnityEngine;
using Unity.Netcode;

public class WorldWrapNetworkObject : MonoBehaviour
{
    private WorldWrapTransformRelay relay;

    public ulong GetClientID()
    {
        return relay.GetClientID();
    }

    public WorldWrapTransformRelay getTransformRelay()
    {
        return relay;
    }

    public void setTransformRelay(WorldWrapTransformRelay newRelay)
    {
        relay = newRelay;
    }

    public bool IsOwned()
    {
        return relay.IsOwned();
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
            relay.ChangePuppetToClient(gameObject);
        }
    }
}
