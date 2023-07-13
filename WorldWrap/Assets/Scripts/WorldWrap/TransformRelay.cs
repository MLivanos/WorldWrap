using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransformRelay : NetworkBehaviour
{
    private Vector3 puppetTransform;
    private Quaternion puppetRotation;

    private void Start()
    {
        if (!IsOwner)
        {
            RelayInstantiation();
        }
    }

    public Vector3 GetPosition()
    {
        return puppetTransform;
    }

    public Quaternion GetRotation()
    {
        return puppetRotation;
    }

    public void Move(Vector3 movementVector)
    {
        puppetTransform += movementVector;
    }

    public void SetRotation(Quaternion rotationQuaternion)
    {
        puppetRotation = rotationQuaternion;
    }

    public void RelayToServer()
    {
        // Send serverRPC
    }

    private void RelayInstantiation()
    {
        GameObject[] gameObjectsInScene = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject objectInScene in gameObjectsInScene)
        {
            if(objectInScene.name == "WorldwrapNetworkManager")
            {
                objectInScene.GetComponent<WorldWrapNetworkManager>().AddToPuppets(gameObject);
            }
        }
    }
}
