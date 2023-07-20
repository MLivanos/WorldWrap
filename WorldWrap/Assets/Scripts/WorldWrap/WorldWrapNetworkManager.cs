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
    private List<GameObject> puppets;
    private List<TransformRelay> puppetTransformRelays;
    private GameObject clientPlayerObject;
    private TransformRelay clientPlayerRelay;
    private Vector3 lastPosition;
    private int numberOfPuppetsFound;
    private bool instantiated;

    private void Start()
    {
        puppets = new List<GameObject>();
        puppetTransformRelays = new List<TransformRelay>();
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

    private void AddToPuppets(GameObject newPuppetRelay)
    {
        if (ShouldNotCreatePuppet(newPuppetRelay))
        {
            return;
        }
        TransformRelay puppetTransformRelay = newPuppetRelay.GetComponent<TransformRelay>();
        GameObject newPuppet = Instantiate(puppetPrefab);
        newPuppet.tag = "WorldWrapPuppet";
        puppetTransformRelays.Add(puppetTransformRelay);
        puppets.Add(newPuppet);
        puppetTransformRelay.InitializeTransform(puppetTransformRelay.GetPosition(), puppetTransformRelay.GetEulerAngles());
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
        AddToPuppets(newPuppetRelay);
    }

    private void UpdatePuppetPosition(int puppetIndex)
    {
        Vector3 movement = puppetTransformRelays[puppetIndex].GetMovement();
        puppets[puppetIndex].transform.Translate(movement, Space.World);
        puppets[puppetIndex].transform.Rotate(puppetTransformRelays[puppetIndex].GetRotation());
    }

    private void SendPositionUpdate()
    {
        clientPlayerRelay.Move(clientPlayerObject.transform.position - lastPosition);
        clientPlayerRelay.SetRotation(clientPlayerObject.transform.eulerAngles);
        lastPosition = clientPlayerObject.transform.position;
    }

    private void SendPositionUpdate(Vector3 offset)
    {
        clientPlayerRelay.Move(clientPlayerObject.transform.position - lastPosition - offset);
        clientPlayerRelay.SetRotation(clientPlayerObject.transform.eulerAngles);
        lastPosition = clientPlayerObject.transform.position;
    }

    private bool ShouldNotCreatePuppet(GameObject newPuppetRelay)
    {
        if (newPuppetRelay.GetComponent<TransformRelay>() == clientPlayerRelay)
        {
            return true;
        }
        return false;
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

    public Vector3 GetInitialPosition()
    {
        return clientPlayerObject.transform.position;
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
        int numberOfPuppets = 0;
        GameObject[] gameObjectsInScene = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject objectInScene in gameObjectsInScene)
        {
            if(HasPrefabName(objectInScene.name))
            {
                numberOfPuppets++;
            }
        }
        return numberOfPuppets;
    }

    public void OffsetTransform(Vector3 movementVector)
    {
        clientPlayerRelay.Warp(movementVector);
    }

    public void WarpPlayer(Vector3 movementVector)
    {
        clientPlayerObject.transform.Translate(movementVector, Space.World);
        SendPositionUpdate(movementVector);
    }
}
