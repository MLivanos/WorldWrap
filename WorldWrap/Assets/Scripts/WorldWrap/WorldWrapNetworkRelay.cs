using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class WorldWrapNetworkRelay : NetworkBehaviour
{
    [SerializeField] private GameObject transformRelayPrefab;
    private GameObject newClientObject;
    private WorldWrapNetworkManager worldWrapNetworkManager;
    private ulong clientId;

    private void Start()
    {
        if (IsOwner)
        {
            clientId = NetworkManager.Singleton.LocalClientId;
            FindWorldWrapNetworkManager();
        }
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
        worldWrapNetworkManager.SetNetworkRelay(this);
    }

    public void InstantiateOnNetwork(int prefabIndex)
    {
        InstantiateOnNetworkServerRpc(clientId, prefabIndex);
    }

    [ServerRpc]
    private void InstantiateOnNetworkServerRpc(ulong clientID, int prefabIndex)
    {
        TransformRelay newRelay = Instantiate(transformRelayPrefab).GetComponent<TransformRelay>();
        newRelay.SetPrefabIndex(prefabIndex);
        newRelay.gameObject.GetComponent<NetworkObject>().SpawnWithOwnership(clientID);
    }
}