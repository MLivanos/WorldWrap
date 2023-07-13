using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class WorldWrapNetworkManager : MonoBehaviour
{
    [SerializeField] private int maxNumberOfPlayers;
    [SerializeField] private GameObject puppetPrefab;
    [SerializeField] private string puppetName;
    private GameObject[] puppets;
    private TransformRelay[] puppetTransformRelays;
    private GameObject clientPlayerObject;
    private TransformRelay clientPlayerRelay;
    private Vector3 lastPosition;

    private void Start()
    {
        puppets = new GameObject[maxNumberOfPlayers];
        puppetTransformRelays = new TransformRelay[maxNumberOfPlayers];
        int numberOfPuppetsFound = 0;
        GameObject[] gameObjectsInScene = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject objectInScene in gameObjectsInScene)
        {
            if(hasPrefabName(objectInScene.name))
            {
                AddToPuppets(objectInScene, numberOfPuppetsFound);
                numberOfPuppetsFound ++;
            }
        }
        clientPlayerObject = Instantiate(puppetPrefab);
        clientPlayerRelay = clientPlayerObject.GetComponent<TransformRelay>();
        lastPosition = clientPlayerObject.transform.position;
    }

    private void Update()
    {
        for(int puppetIndex = 0; puppetIndex < maxNumberOfPlayers; puppetIndex++)
        {
            if (!puppets[puppetIndex])
            {
                break;
            }
            UpdatePuppetPosition(puppetIndex);
        }
        SendPositionUpdate();
    }

    private bool hasPrefabName(string objectName)
    {
        return objectName.StartsWith(puppetName) && char.IsDigit(objectName[^1]);
    }

    private void AddToPuppets(GameObject newPuppetRelay, int puppetIndex)
    {
        if (puppetIndex >= maxNumberOfPlayers - 1)
        {
            Debug.LogError("Error: Number of players detected (" + puppetIndex +
            ") exceeds maximum number of players (" + maxNumberOfPlayers + ")");
            return;
        }
        TransformRelay puppetTransformRelay = newPuppetRelay.GetComponent<TransformRelay>();
        puppets[puppetIndex] = newPuppetRelay;
        GameObject newPuppet = Instantiate(puppetPrefab);
        newPuppet.transform.position = puppetTransformRelay.GetPosition();
        newPuppet.transform.rotation = puppetTransformRelay.GetRotation();
    }

    public void AddToPuppets(GameObject newPuppetRelay)
    {
        for (int puppetIndex = 0; puppetIndex < maxNumberOfPlayers; puppetIndex++)
        {
            if (puppets[puppetIndex])
            {
                AddToPuppets(newPuppetRelay, puppetIndex);
                break;
            }
        }
    }

    private void UpdatePuppetPosition(int puppetIndex)
    {
        puppets[puppetIndex].transform.position = puppetTransformRelays[puppetIndex].GetPosition();
        puppets[puppetIndex].transform.rotation = puppetTransformRelays[puppetIndex].GetRotation();
    }

    private void SendPositionUpdate()
    {
        clientPlayerRelay.Move(clientPlayerObject.transform.position - lastPosition);
        clientPlayerRelay.SetRotation(clientPlayerObject.transform.rotation);
        clientPlayerRelay.RelayToServer();
    }
}
