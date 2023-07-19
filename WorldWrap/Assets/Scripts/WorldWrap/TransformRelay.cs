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
    private Vector3 lastPosition;
    private Vector3 lastRotation;
    private string puppetName;

    private void Start()
    {
        FindWorldWrapNetworkManager();
        NameSelf();
        if (IsOwner)
        {
            worldWrapNetworkManager.CreatePlayerObject(this);
            worldWrapNetworkManager.FindPuppets();
            AddToPuppetsServerRpc();
        }
    }

    public Vector3 GetPosition()
    {
        return puppetPosition.Value;
    }

    public Vector3 GetMovement()
    {
        Vector3 movement = puppetPosition.Value - lastPosition;
        lastPosition = puppetPosition.Value;
        return movement;
    }

    public Vector3 GetEulerAngles()
    {
        return puppetRotation.Value;
    }

    public Vector3 GetRotation()
    {
        return puppetRotation.Value - lastRotation;
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
        puppetName = worldWrapNetworkManager.GetPuppetName();
    }

    private void NameSelf()
    {
        gameObject.name = puppetName + worldWrapNetworkManager.GetNumberOfPuppets();
    }

    public void Warp(Vector3 movementVector)
    {
        puppetPosition.Value -= movementVector;
    }

    public void InitializeTransform(Vector3 initialPosition, Vector3 initialRotation)
    {
        if(IsOwner)
        {
            puppetPosition.Value = initialPosition;
            puppetRotation.Value = initialRotation;
        }
        lastPosition = initialPosition;
        lastRotation = initialRotation;
    }

    [ClientRpc]
    private void AddToPuppetsClientRpc(string senderName)
    {
        worldWrapNetworkManager.AddToPuppets(senderName, gameObject);
    }

    [ServerRpc]
    private void AddToPuppetsServerRpc()
    {
        AddToPuppetsClientRpc(gameObject.name);
    }

}