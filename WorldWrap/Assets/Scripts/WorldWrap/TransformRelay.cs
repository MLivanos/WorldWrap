using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class TransformRelay : NetworkBehaviour
{
    private NetworkVariable<Vector3> puppetPosition = new NetworkVariable<Vector3>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<Vector3> puppetRotation = new NetworkVariable<Vector3>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private WorldWrapNetworkManager worldWrapNetworkManager;
    private string puppetName;

    private void Start()
    {
        FindWorldWrapNetworkManager();
        NameSelf();
        if (IsOwner)
        {
            worldWrapNetworkManager.CreatePlayerObject(this);
        }
        else
        {
            AddToPuppetsClientRpc();
        }
    }

    public Vector3 GetPosition()
    {
        return puppetPosition.Value;
    }

    public Vector3 GetRotation()
    {
        return puppetRotation.Value;
    }

    public void Move(Vector3 movementVector)
    {
        puppetPosition.Value += movementVector;
    }

    public void SetRotation(Vector3 rotationVector)
    {
        puppetRotation.Value = rotationVector;
    }

    private void FindWorldWrapNetworkManager()
    {
        GameObject[] gameObjectsInScene = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject objectInScene in gameObjectsInScene)
        {
            if(objectInScene.name == "WorldWrapNetworkManager")
            {
                worldWrapNetworkManager = objectInScene.GetComponent<WorldWrapNetworkManager>();
                break;
            }
        }
        worldWrapNetworkManager.FindPuppets();
        puppetName = worldWrapNetworkManager.GetPuppetName();
    }

    private void NameSelf()
    {
        gameObject.name = puppetName + worldWrapNetworkManager.GetNumberOfPuppets();
    }

    [ClientRpc]
    private void AddToPuppetsClientRpc()
    {
        FindWorldWrapNetworkManager();
        worldWrapNetworkManager.AddToPuppets(gameObject);
    }
}
