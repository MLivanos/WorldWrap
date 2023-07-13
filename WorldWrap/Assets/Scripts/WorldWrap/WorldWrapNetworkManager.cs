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
    private GameObject clientPlayerObject;
    private Vector3 lastPosition;
    public Vector3[] movementVectors;

    private void Start()
    {
        puppets = new GameObject[maxNumberOfPlayers];
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
        lastPosition = clientPlayerObject.transform.position;
    }

    private void Update()
    {
        foreach(GameObject puppet in puppets)
        {
            UpdatePuppetPosition(puppet);
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
        puppets[puppetIndex] = newPuppetRelay;
        GameObject newPuppet = Instantiate(puppetPrefab);
        // Put in position
    }

    private void UpdatePuppetPosition(GameObject puppet)
    {

    }

    private void SendPositionUpdate()
    {

    }
}
