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
    private NetworkVariable<int> prefabIndex = new NetworkVariable<int>(default);
    private WorldWrapNetworkManager worldWrapNetworkManager;
    private Vector3 lastPosition;
    private Vector3 lastRotation;
    private ulong clientId;
    private string puppetName;

    private void Awake()
    {
        FindWorldWrapNetworkManager();
        NameSelf();
        if(IsOwner)
        {
            SetClientIdServerRpc(NetworkManager.Singleton.LocalClientId);
        }
    }

    private void Start()
    {
        if (IsOwner)
        {
            worldWrapNetworkManager.AddToClientObjects(this);
        }
    }

    public void Setup()
    {
        AddToPuppetsServerRpc();
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
        Vector3 rotation = puppetRotation.Value - lastRotation;
        lastRotation = puppetRotation.Value;
        return rotation;
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

    public int GetPrefabIndex()
    {
        return prefabIndex.Value;
    }

    public void SetPrefabIndex(int indexNumber)
    {
        prefabIndex.Value = indexNumber;
    }

    public void ApplyForce(Vector3 force)
    {
        ApplyForceServerRpc(force);
    }

    public void ChangeOwnership(ulong newClientID)
    {
        ChangeOwnershipServerRpc(newClientID);
        clientId = newClientID;
    }

    public ulong GetClientID()
    {
        return gameObject.GetComponent<NetworkObject>().OwnerClientId;
    }

    public void ChangePuppetToClient(GameObject puppet)
    {
        worldWrapNetworkManager.ChangePuppetToClient(puppet);
    }

    [ClientRpc]
    private void AddToPuppetsClientRpc(string senderName)
    {
        worldWrapNetworkManager.AddToPuppets(senderName, gameObject);
    }

    [ClientRpc]
    private void ApplyForceClientRpc(Vector3 force, ClientRpcParams clientRpcParams = default)
    {
        worldWrapNetworkManager.ApplyForce(this, force);
    }

    [ClientRpc]
    private void RemovePuppetsClientRpc()
    {
        if (IsOwner)
        {
            return;
        }
        worldWrapNetworkManager.RemovePuppet(this);
    }

    [ClientRpc]
    private void ChangeOwnershipClientRpc()
    {
        if (IsOwner)
        {
            worldWrapNetworkManager.ReplaceClientWithPuppet(this);
        }
    }

    [ClientRpc]
    private void AddToClientsClientRpc()
    {
        if (IsOwner)
        {
            worldWrapNetworkManager.AddClientRelay(this);
        }
    }

    [ServerRpc]
    private void AddToPuppetsServerRpc()
    {
        AddToPuppetsClientRpc(gameObject.name);
    }

    [ServerRpc]
    public void RemovePuppetsServerRpc()
    {
        RemovePuppetsClientRpc();
    }

    [ServerRpc]
    private void SetClientIdServerRpc(ulong clientIdNumber)
    {
        clientId = clientIdNumber;
    }

    [ServerRpc(RequireOwnership = false)]
    private void ApplyForceServerRpc(Vector3 force)
    {
        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[]{clientId}
            }
        };
        ApplyForceClientRpc(force, clientRpcParams);
    }

    [ServerRpc]
    public void DespawnServerRpc()
    {
        OnNetworkDespawn();
    }

    [ServerRpc(RequireOwnership = false)]
    private void ChangeOwnershipServerRpc(ulong newClientID)
    {
        clientId = newClientID;
        ChangeOwnershipClientRpc();
        GetComponent<NetworkObject>().ChangeOwnership(newClientID);
        AddToClientsClientRpc();
    }
}