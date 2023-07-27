using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldWrapNetworkRigidbody : MonoBehaviour
{
    private Rigidbody puppetRigidbody;
    private TransformRelay clientTransformRelay;

    private void Start()
    {
        puppetRigidbody = gameObject.GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.impulse);
    }

    public void SetClientTransformRelay(TransformRelay transformRelay)
    {
        clientTransformRelay = transformRelay;
    }
}
