using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using UnityEngine;

public class WorldWrapNetworkManager : MonoBehaviour
{
    [SerializeField] private GameObject[] clientPrefabs;
    [SerializeField] private GameObject[] puppetPrefabs;
    [SerializeField] private GameObject transformRelayPrefab;
    [SerializeField] private string puppetName;
    [SerializeField] private bool firstPrefabIsPlayer;
    private WorldWrapNetworkRelay networkRelay;
    private Transform transformRelayGroup;
    private List<GameObject> puppets;
    private List<TransformRelay> puppetTransformRelays;
    private List<GameObject> clientObjects;
    private List<TransformRelay> clientRelays;
    private Vector3 lastPosition;
    private int playerIndex;

    private void Start()
    {
        puppets = new List<GameObject>();
        puppetTransformRelays = new List<TransformRelay>();
        clientObjects = new List<GameObject>();
        clientRelays = new List<TransformRelay>();
        CreateTransformRelayGroup();
    }

    private void FixedUpdate()
    {
        UpdateAllPuppets();
        SendAllUpdates();
    }

    private void UpdateAllPuppets()
    {
        for(int puppetIndex = 0; puppetIndex < puppets.Count; puppetIndex++)
        {
            UpdatePuppetPosition(puppetIndex);
        }
    }

    private void SendAllUpdates()
    {
        for(int objectIndex = 0; objectIndex < clientObjects.Count; objectIndex++)
        {
            SendPositionUpdate(objectIndex);
        }
    }

    private bool HasPrefabName(string objectName)
    {
        return objectName.StartsWith(puppetName);
    }

    private void AddToPuppets(GameObject newPuppetRelay)
    {
        if (ShouldNotCreatePuppet(newPuppetRelay))
        {
            return;
        }
        TransformRelay puppetTransformRelay = newPuppetRelay.GetComponent<TransformRelay>();
        GameObject newPuppet = Instantiate(puppetPrefabs[puppetTransformRelay.GetPrefabIndex()]);
        newPuppet.tag = "WorldWrapPuppet";
        puppetTransformRelays.Add(puppetTransformRelay);
        puppets.Add(newPuppet);
        puppetTransformRelay.InitializeTransform(puppetTransformRelay.GetPosition(), puppetTransformRelay.GetEulerAngles());
        newPuppet.transform.position = puppetTransformRelay.GetPosition();
        newPuppet.transform.eulerAngles = puppetTransformRelay.GetRotation();
    }

    public void AddToPuppets(string senderName, GameObject newPuppetRelay)
    {
        if (senderName == clientRelays[playerIndex].gameObject.name)
        {
            return;
        }
        AddToPuppets(newPuppetRelay);
    }

    public void AddToClientObjects(TransformRelay newRelay)
    {
        GameObject newClientObject = Instantiate(clientPrefabs[newRelay.GetPrefabIndex()]);
        clientObjects.Add(newClientObject);
        clientRelays.Add(newRelay);
        newRelay.Setup();
    }

    private void UpdatePuppetPosition(int puppetIndex)
    {
        Vector3 movement = puppetTransformRelays[puppetIndex].GetMovement();
        puppets[puppetIndex].transform.Translate(movement, Space.World);
        puppets[puppetIndex].transform.Rotate(puppetTransformRelays[puppetIndex].GetRotation());
    }

    private void SendPositionUpdate(int objectIndex)
    {
        SendPositionUpdate(Vector3.zero, objectIndex);
    }

    private void SendPositionUpdate(Vector3 offset, int objectIndex)
    {
        clientRelays[objectIndex].Move(clientObjects[objectIndex].transform.position - lastPosition - offset);
        clientRelays[objectIndex].SetRotation(clientObjects[objectIndex].transform.eulerAngles);
        lastPosition = clientObjects[objectIndex].transform.position;
    }

    private bool ShouldNotCreatePuppet(GameObject newPuppetRelay)
    {
        if (OwnsRelay(newPuppetRelay.GetComponent<TransformRelay>()))
        {
            return true;
        }
        return false;
    }

    private bool OwnsRelay(TransformRelay relay)
    {
        foreach(TransformRelay ownedRelay in clientRelays)
        {
            if (relay == ownedRelay)
            {
                return true;
            }
        }
        return false;
    }

    private void CreateTransformRelayGroup()
    {
        transformRelayGroup = (new GameObject("TransformRelays")).transform;
        GameObject[] gameObjectsInScene = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject objectInScene in gameObjectsInScene)
        {
            if(HasPrefabName(objectInScene.name))
            {
                objectInScene.transform.parent = transformRelayGroup.transform;
                AddToPuppets(objectInScene);
            }
        }
    }

    public void SetNetworkRelay(WorldWrapNetworkRelay relay)
    {
        networkRelay = relay;
        FindPuppets();
        if (firstPrefabIsPlayer)
        {
            InstantiateOnNetwork(0);
        }
    }

    public void InstantiateOnNetwork(int prefabIndex)
    {
        networkRelay.InstantiateOnNetwork(prefabIndex);
    }

    public string GetPuppetName()
    {
        return puppetName;
    }

    public void FindPuppets()
    {
        GameObject[] gameObjectsInScene = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject objectInScene in gameObjectsInScene)
        {
            if(HasPrefabName(objectInScene.name))
            {
                AddToPuppets(objectInScene);
            }
        }
    }

    public int GetNumberOfPuppets()
    {
        return puppets.Count + clientObjects.Count;
    }

    public GameObject GetTransformRelayPrefab()
    {
        return transformRelayPrefab;
    }

    public void OffsetTransform(Vector3 movementVector)
    {
        clientRelays[playerIndex].Warp(movementVector);
    }

    public void WarpPlayer(Vector3 movementVector)
    {
        clientObjects[playerIndex].transform.Translate(movementVector, Space.World);
        SendPositionUpdate(movementVector, playerIndex);
    }
}
