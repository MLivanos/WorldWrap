using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPDBGame : MonoBehaviour
{
    [SerializeField] private int dodgeballIndex;
    [SerializeField] private GameObject networkManagerGameObject;
    private WorldWrapNetworkManager networkManager;
    private GameObject recentGO;

    void Start()
    {
        networkManager = networkManagerGameObject.GetComponent<WorldWrapNetworkManager>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            recentGO = networkManager.InstantiateOnNetwork(dodgeballIndex);
        }
        if(Input.GetKeyDown(KeyCode.X))
        {
            networkManager.RemoveClient(recentGO);
        }
    }
}
