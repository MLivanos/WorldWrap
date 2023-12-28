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
    private List<Vector3> lastPositions;
    private int playerIndex;
    // To delete
    private GameObject recentGO;

    private void Start()
    {
        puppets = new List<GameObject>();
        puppetTransformRelays = new List<TransformRelay>();
        clientObjects = new List<GameObject>();
        clientRelays = new List<TransformRelay>();
        lastPositions = new List<Vector3>();
        CreateTransformRelayGroup();
    }

    // To delete
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            recentGO = InstantiateOnNetwork(1);
        }
        if(Input.GetKeyDown(KeyCode.X))
        {
            RemoveClient(recentGO);
        }
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
        CreatePuppetObject(puppetTransformRelay);
    }

    public void CreatePuppetObject(TransformRelay puppetTransformRelay)
    {
        GameObject newPuppet = Instantiate(puppetPrefabs[puppetTransformRelay.GetPrefabIndex()]);
        SetupRigidbody(newPuppet, puppetTransformRelay);
        SetupNetworkObjectScript(newPuppet, puppetTransformRelay);
        puppetTransformRelays.Add(puppetTransformRelay);
        puppets.Add(newPuppet);
        puppetTransformRelay.InitializeTransform(puppetTransformRelay.GetPosition(), puppetTransformRelay.GetEulerAngles());
        newPuppet.transform.position = puppetTransformRelay.GetPosition();
        Vector3 rotation = puppetTransformRelay.GetEulerAngles();
        newPuppet.transform.eulerAngles = rotation;
    }

    private void SetupRigidbody(GameObject newPuppet, TransformRelay puppetTransformRelay)
    {
        WorldWrapNetworkRigidbody newPuppetRigidbody = newPuppet.GetComponent<WorldWrapNetworkRigidbody>();
        if (newPuppetRigidbody != null)
        {
            newPuppetRigidbody.SetClientTransformRelay(puppetTransformRelay);
            newPuppetRigidbody.SetNetworkManager(this);
        }
    }

    private void SetupNetworkObjectScript(GameObject newObject, TransformRelay newRelay)
    {
        WorldWrapNetworkObject puppetScript = newObject.AddComponent(typeof(WorldWrapNetworkObject)) as WorldWrapNetworkObject;
        puppetScript.setTransformRelay(newRelay);
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
        lastPositions.Add(Vector3.zero);
        clientRelays.Add(newRelay);
        SetupNetworkObjectScript(clientObjects.Last(), newRelay);
        newRelay.Setup();
    }

    public void AddClientRelay(TransformRelay newRelay)
    {
        lastPositions.Add(Vector3.zero);
        clientRelays.Add(newRelay);
        newRelay.Setup();
    }

    public void ChangePuppetToClient(GameObject newClient)
    {
        clientObjects.Add(newClient);
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
        GameObject newClientObject = Instantiate(clientPrefabs[prefabIndex]);
        clientObjects.Add(newClientObject);
        return newClientObject;
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

    public void ApplyForce(TransformRelay clientRelay, Vector3 force)
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

    public void ReplaceClientWithPuppet(TransformRelay relayToReplace)
    {
        int indexToReplace = clientRelays.IndexOf(relayToReplace);
        GameObject objectToRemove = clientObjects[indexToReplace];
        GameObject newPuppet = clientObjects[indexToReplace];
        TransformRelay newTransformRelay = clientRelays[indexToReplace];
        puppets.Add(newPuppet);
        puppetTransformRelays.Add(newTransformRelay);
        clientObjects.RemoveAt(indexToReplace);
        clientRelays.RemoveAt(indexToReplace);
    }

    public void RemoveClient(TransformRelay relayToRemove, bool deleteClientObject = true)
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

    public void RemovePuppet(TransformRelay relayToRemove)
    {
        int indexToRemove = puppetTransformRelays.IndexOf(relayToRemove);
        Destroy(puppets[indexToRemove]);
        puppetTransformRelays.RemoveAt(indexToRemove);
        puppets.RemoveAt(indexToRemove);
    }

    public void AcceptNewTransform(TransformRelay transformRelay)
    {
        puppetTransformRelays.Add(transformRelay);
    }
}
