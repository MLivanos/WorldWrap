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
    private WrapManager wrapManager;
    private WorldWrapNetworkRelay networkRelay;
    private Transform transformRelayGroup;
    private List<GameObject> puppets;
    private List<WorldWrapTransformRelay> puppetTransformRelays;
    private List<GameObject> clientObjects;
    private List<WorldWrapTransformRelay> clientRelays;
    private List<Vector3> lastPositions;
    private int playerIndex;

    private void Start()
    {
        puppets = new List<GameObject>();
        puppetTransformRelays = new List<WorldWrapTransformRelay>();
        clientObjects = new List<GameObject>();
        clientRelays = new List<WorldWrapTransformRelay>();
        lastPositions = new List<Vector3>();
        FindWrapManager();
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
        for(int objectIndex = 0; objectIndex < clientRelays.Count; objectIndex++)
        {
            SendPositionUpdate(objectIndex);
        }
    }

    private void FindWrapManager()
    {
        GameObject[] gameObjectsInScene = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject objectInScene in gameObjectsInScene)
        {
            wrapManager = objectInScene.GetComponent<WrapManager>();
            if (wrapManager != null)
            {
                return;
            }
        }
        // TODO: Raise error
    }

    private bool HasPrefabName(string objectName)
    {
        return objectName.StartsWith(puppetName);
    }

    public void AddToPuppets(GameObject newPuppetRelay)
    {
        if (ShouldNotCreatePuppet(newPuppetRelay))
        {
            return;
        }
        WorldWrapTransformRelay puppetTransformRelay = newPuppetRelay.GetComponent<WorldWrapTransformRelay>();
        CreatePuppetObject(puppetTransformRelay);
    }

    public void CreatePuppetObject(WorldWrapTransformRelay puppetTransformRelay)
    {
        GameObject newPuppet = wrapManager.SemanticInstantiate(puppetPrefabs[puppetTransformRelay.GetPrefabIndex()]);
        SetupRigidbody(newPuppet, puppetTransformRelay);
        SetupNetworkObjectScript(newPuppet, puppetTransformRelay);
        puppetTransformRelays.Add(puppetTransformRelay);
        puppets.Add(newPuppet);
        puppetTransformRelay.InitializeTransform(puppetTransformRelay.GetPosition(), puppetTransformRelay.GetEulerAngles());
        newPuppet.transform.position = puppetTransformRelay.GetPosition();
        Vector3 rotation = puppetTransformRelay.GetEulerAngles();
        newPuppet.transform.eulerAngles = rotation;
    }

    private void SetupRigidbody(GameObject newPuppet, WorldWrapTransformRelay puppetTransformRelay)
    {
        WorldWrapNetworkRigidbody newPuppetRigidbody = newPuppet.GetComponent<WorldWrapNetworkRigidbody>();
        if (newPuppetRigidbody != null)
        {
            newPuppetRigidbody.SetClientTransformRelay(puppetTransformRelay);
            newPuppetRigidbody.SetNetworkManager(this);
        }
    }

    private void SetupNetworkObjectScript(GameObject newObject, WorldWrapTransformRelay newRelay)
    {
        WorldWrapNetworkObject puppetScript = newObject.AddComponent(typeof(WorldWrapNetworkObject)) as WorldWrapNetworkObject;
        puppetScript.setTransformRelay(newRelay);
    }

    public void AddToClientObjects(WorldWrapTransformRelay newRelay)
    {
        lastPositions.Add(Vector3.zero);
        clientRelays.Add(newRelay);
        SetupNetworkObjectScript(clientObjects.Last(), newRelay);
        newRelay.Setup();
    }

    public void AddClientRelay(WorldWrapTransformRelay newRelay)
    {
        lastPositions.Add(newRelay.GetPosition());
        clientRelays.Add(newRelay);
    }

    public void ChangePuppetToClient(GameObject oldPuppet, int prefabIndex)
    {
        int oldPuppetIndex = puppets.IndexOf(oldPuppet);
        puppets.RemoveAt(oldPuppetIndex);
        puppetTransformRelays.RemoveAt(oldPuppetIndex);
        GameObject newClient = wrapManager.SemanticInstantiate(clientPrefabs[prefabIndex]);
        newClient.transform.parent = oldPuppet.transform.parent;
        newClient.transform.position = oldPuppet.transform.position;
        newClient.transform.eulerAngles = oldPuppet.transform.eulerAngles;
        Destroy(oldPuppet);
        clientObjects.Add(newClient);
        SetupNetworkObjectScript(clientObjects.Last(), clientRelays.Last());
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
        clientRelays[objectIndex].Move(clientObjects[objectIndex].transform.position - lastPositions[objectIndex] - offset);
        lastPositions[objectIndex] = clientObjects[objectIndex].transform.position;
        clientRelays[objectIndex].SetRotation(clientObjects[objectIndex].transform.eulerAngles);
    }

    private bool ShouldNotCreatePuppet(GameObject newPuppetRelay)
    {
        if (OwnsRelay(newPuppetRelay.GetComponent<WorldWrapTransformRelay>()))
        {
            return true;
        }
        return false;
    }

    private bool OwnsRelay(WorldWrapTransformRelay relay)
    {
        return relay.IsOwned();
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

    private void OffsetChildren(Vector3 movementVector, GameObject objectToMove)
    {
        SendPositionUpdate(movementVector, clientObjects.IndexOf(objectToMove));
        foreach(Transform childTransform in objectToMove.transform)
        {
            if (clientObjects.Contains(childTransform.gameObject))
            {
                OffsetChildren(movementVector, childTransform.gameObject);
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

    public GameObject InstantiateOnNetwork(int prefabIndex)
    {
        networkRelay.InstantiateOnNetwork(prefabIndex);
        GameObject newClientObject = wrapManager.SemanticInstantiate(clientPrefabs[prefabIndex]);
        clientObjects.Add(newClientObject);
        return newClientObject;
    }

    public Vector3 GetPuppetOffset()
    {
        return wrapManager.GetSemanticOffset();
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

    public void Warp(Vector3 movementVector, GameObject objectToMove)
    {
        objectToMove.transform.Translate(movementVector, Space.World);
        OffsetChildren(movementVector, objectToMove);
    }

    public void ApplyForce(WorldWrapTransformRelay clientRelay, Vector3 force)
    {
        int clientIndex = clientRelays.IndexOf(clientRelay);
        if (clientIndex < 0)
        {
            return;
        }
        Rigidbody clientRigidbody = clientObjects[clientIndex].GetComponent<Rigidbody>();
        if(clientRigidbody != null)
        {
            clientRigidbody.AddForce(force, ForceMode.Impulse);
        }
    }

    public bool IsClient(GameObject possibleClient)
    {
        return clientObjects.Contains(possibleClient);
    }

    public void ReplaceClientWithPuppet(WorldWrapTransformRelay relayToReplace)
    {
        int indexToReplace = clientRelays.IndexOf(relayToReplace);
        GameObject objectToRemove = clientObjects[indexToReplace];
        GameObject oldClientObject = clientObjects[indexToReplace];
        WorldWrapTransformRelay newTransformRelay = clientRelays[indexToReplace];
        GameObject newPuppet = Instantiate(puppetPrefabs[newTransformRelay.GetPrefabIndex()]);
        newPuppet.transform.position = oldClientObject.transform.position;
        newPuppet.transform.eulerAngles = oldClientObject.transform.eulerAngles;
        Destroy(oldClientObject);
        puppets.Add(newPuppet);
        puppetTransformRelays.Add(newTransformRelay);
        clientObjects.RemoveAt(indexToReplace);
        clientRelays.RemoveAt(indexToReplace);
        lastPositions.RemoveAt(indexToReplace);
        SetupRigidbody(newPuppet, newTransformRelay);
        SetupNetworkObjectScript(newPuppet, newTransformRelay);
        newTransformRelay.InitializeTransform(newTransformRelay.GetPosition(), newTransformRelay.GetEulerAngles());
        newTransformRelay.OffsetLastPosition(GetPuppetOffset());
    }

    public void RemoveClient(WorldWrapTransformRelay relayToRemove, bool deleteClientObject = true)
    {
        int indexToRemove = clientRelays.IndexOf(relayToRemove);
        GameObject objectToRemove = clientObjects[indexToRemove];
        RemoveClient(objectToRemove, deleteClientObject);
        
    }

    public void RemoveClient(GameObject objectToRemove, bool deleteClientObject = true)
    {
        int indexToRemove = clientObjects.IndexOf(objectToRemove);
        if (deleteClientObject)
        {
            clientRelays[indexToRemove].RemovePuppetsServerRpc();
            clientRelays[indexToRemove].DespawnServerRpc();
        }
        Destroy(clientObjects[indexToRemove]);
        clientObjects.RemoveAt(indexToRemove);
        clientRelays.RemoveAt(indexToRemove);
        lastPositions.RemoveAt(indexToRemove);
    }

    public void RemovePuppet(WorldWrapTransformRelay relayToRemove)
    {
        int indexToRemove = puppetTransformRelays.IndexOf(relayToRemove);
        Destroy(puppets[indexToRemove]);
        puppetTransformRelays.RemoveAt(indexToRemove);
        puppets.RemoveAt(indexToRemove);
    }

    public void AcceptNewTransform(WorldWrapTransformRelay transformRelay)
    {
        puppetTransformRelays.Add(transformRelay);
    }
}
