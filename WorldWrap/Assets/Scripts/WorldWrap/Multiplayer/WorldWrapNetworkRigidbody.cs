using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldWrapNetworkRigidbody : MonoBehaviour
{
    private Rigidbody puppetRigidbody;
    private WorldWrapTransformRelay clientTransformRelay;
    private WorldWrapNetworkManager worldWrapNetworkManager;

    private void Start()
    {
        puppetRigidbody = gameObject.GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject collisionGameObject = collision.collider.gameObject;
        if (CollidedWithClient(collisionGameObject))
        {
            clientTransformRelay.ApplyForce(collision.impulse * Time.fixedDeltaTime);
        }
    }

    private bool CollidedWithClient(GameObject collisionGameObject)
    {   
        return collisionGameObject.GetComponent<Rigidbody>() && worldWrapNetworkManager.IsClient(collisionGameObject);
    }

    public void SetClientTransformRelay(WorldWrapTransformRelay transformRelay)
    {
        clientTransformRelay = transformRelay;
    }

    public void SetNetworkManager(WorldWrapNetworkManager networkManager)
    {
        worldWrapNetworkManager = networkManager;
    }
}
