using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class WorldWrapNetworkManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject puppetPrefab;
    [SerializeField] private string puppetName;
    private List<GameObject> puppets;
    private List<TransformRelay> puppetTransformRelays;
    private List<GameObject> clientObjects;
    private List<TransformRelay> clientRelays;
    private Vector3 lastPosition;
    private bool instantiated;
    private int playerIndex;

    private void Start()
    {
        puppets = new List<GameObject>();
        puppetTransformRelays = new List<TransformRelay>();
        clientObjects = new List<GameObject>();
        clientRelays = new List<TransformRelay>();
    }

    private void FixedUpdate()
    {
        if (!instantiated)
        {
            return;
        }
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
    }

    public void AddToPuppets(string senderName, GameObject newPuppetRelay)
    {
        if (senderName == clientRelays[playerIndex].gameObject.name)
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
        if (newPuppetRelay.GetComponent<TransformRelay>() == clientRelays[playerIndex])
        {
            return true;
        }
        return false;
    }

    public void CreatePlayerObject(TransformRelay relay)
    {
        playerIndex = clientObjects.Count;
        clientObjects.Add(Instantiate(playerPrefab));
        clientRelays.Add(relay);
        relay.Move(clientObjects.Last().transform.position);
        relay.SetRotation(clientObjects.Last().transform.eulerAngles);
        lastPosition = clientObjects.Last().transform.position;
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
        clientRelays[playerIndex].Warp(movementVector);
    }

    public void WarpPlayer(Vector3 movementVector)
    {
        clientObjects[playerIndex].transform.Translate(movementVector, Space.World);
        SendPositionUpdate(movementVector, playerIndex);
    }
}
