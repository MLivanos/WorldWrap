using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class WorldWrapNetworkManager : MonoBehaviour
{
    [SerializeField] private int maxNumberOfPlayers;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject puppetPrefab;
    [SerializeField] private string puppetName;
    Dictionary<int, TransformRelay> relayLookup;
    private GameObject[] puppets;
    private TransformRelay[] puppetTransformRelays;
    private GameObject clientPlayerObject;
    private TransformRelay clientPlayerRelay;
    private Vector3 lastPosition;
    private int numberOfPuppetsFound;
    private bool instantiated;

    private void Start()
    {
        relayLookup = new Dictionary<int, TransformRelay>();
        puppets = new GameObject[maxNumberOfPlayers];
        puppetTransformRelays = new TransformRelay[maxNumberOfPlayers];
        numberOfPuppetsFound = 0;
    }

    private void FixedUpdate()
    {
        if (!instantiated)
        {
            return;
        }
        for(int puppetIndex = 0; puppetIndex < numberOfPuppetsFound; puppetIndex++)
        {
            UpdatePuppetPosition(puppetIndex);
        }
        SendPositionUpdate();
    }

    private bool HasPrefabName(string objectName)
    {
        return objectName.StartsWith(puppetName) && char.IsDigit(objectName[^1]);
    }

    private void AddToPuppets(GameObject newPuppetRelay, int puppetIndex)
    {
        if (puppetIndex >= maxNumberOfPlayers)
        {
            Debug.LogError("Error: Number of players detected (" + (puppetIndex + 1) +
            ") exceeds maximum number of players (" + maxNumberOfPlayers + ")");
            return;
        }
        TransformRelay puppetTransformRelay = newPuppetRelay.GetComponent<TransformRelay>();
        GameObject newPuppet = Instantiate(puppetPrefab);
        newPuppet.tag = "WorldWrapPuppet";
        relayLookup[newPuppet.GetInstanceID()] = puppetTransformRelay;
        puppetTransformRelays[puppetIndex] = puppetTransformRelay;
        puppets[puppetIndex] = newPuppet;
        newPuppet.transform.position = puppetTransformRelay.GetPosition();
        newPuppet.transform.eulerAngles = puppetTransformRelay.GetRotation();
        numberOfPuppetsFound++;
    }

    public void AddToPuppets(string senderName, GameObject newPuppetRelay)
    {
        if (senderName == clientPlayerRelay.gameObject.name)
        {
            return;
        }
        for (int puppetIndex = 0; puppetIndex < maxNumberOfPlayers; puppetIndex++)
        {
            if (!puppets[puppetIndex])
            {
                AddToPuppets(newPuppetRelay, puppetIndex);
                break;
            }
        }
    }

    private void UpdatePuppetPosition(int puppetIndex)
    {
        puppets[puppetIndex].transform.Translate(puppetTransformRelays[puppetIndex].GetMovement());
        puppets[puppetIndex].transform.eulerAngles = puppetTransformRelays[puppetIndex].GetRotation();
    }

    private void SendPositionUpdate()
    {
        clientPlayerRelay.Move(clientPlayerObject.transform.position - lastPosition);
        clientPlayerRelay.SetRotation(clientPlayerObject.transform.eulerAngles);
        lastPosition = clientPlayerObject.transform.position;
    }

    public void CreatePlayerObject(TransformRelay relay)
    {
        clientPlayerObject = Instantiate(playerPrefab);
        clientPlayerRelay = relay;
        relay.Move(clientPlayerObject.transform.position);
        relay.SetRotation(clientPlayerObject.transform.eulerAngles);
        lastPosition = clientPlayerObject.transform.position;
        instantiated = true;
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
                AddToPuppets(objectInScene, numberOfPuppetsFound);
            }
        }
    }

    public int GetNumberOfPuppets()
    {
        return numberOfPuppetsFound;
    }

    public void OffsetTransform(Vector3 movementVector)
    {
        clientPlayerRelay.Warp(movementVector);
    }
}
