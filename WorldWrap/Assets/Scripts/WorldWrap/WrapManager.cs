using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

public class WrapManager : MonoBehaviour
{
    [SerializeField] private GameObject[] blocks;
    [SerializeField] private GameObject lureObject;
    [SerializeField] private GameObject worldWrapNetworkManagerObject;
    [SerializeField] private bool isUsingNavmesh;
    [SerializeField] private bool isMultiplayer;
    private WorldWrapNetworkManager worldWrapNetworkManager;
    private WorldWrapper wrapper;
    private BoundsTrigger bounds;
    private List<GameObject> selfWrappers;
    private GameObject[,] blockMatrix;
    private Vector3 referenceBlockInitialPosition;
    private GameObject initialTrigger;
    private GameObject currentTrigger;
    private GameObject previousBlock;
    private GameObject currentBlock;
    private bool isTransitioning;
    private bool zeroMagnitudeWrapTriggered;

    private void Start()
    {
        selfWrappers = new List<GameObject>();
        initialTrigger = null;
        currentTrigger = null;
        WrapManagerSetup setupHelper = gameObject.AddComponent(typeof(WrapManagerSetup)) as WrapManagerSetup;
        wrapper = gameObject.AddComponent(typeof(WorldWrapper)) as WorldWrapper;
        setupHelper.Setup(blocks,lureObject);
        blockMatrix = setupHelper.GetBlockMatrix();
        referenceBlockInitialPosition = setupHelper.SetReferenceBlock();
        bounds = setupHelper.FindBounds();
        if (isUsingNavmesh)
        {
            setupHelper.CreateNavMeshLure();
        }
        if (isMultiplayer)
        {
            worldWrapNetworkManager = worldWrapNetworkManagerObject.GetComponent<WorldWrapNetworkManager>();
        }
        wrapper.Setup(blockMatrix, worldWrapNetworkManager);
        CheckBounds();
        Destroy(setupHelper);
    }

    private void Update()
    {
        if (zeroMagnitudeWrapTriggered)
        {
            zeroMagnitudeWrapTriggered = false;
        }
    }

    private void CheckBounds()
    {
        if (bounds)
        {
            return;
        }
        if (isMultiplayer)
        {
            Exception missingManagerException = new Exception("Error: No BoundsTrigger found. Please surround your world with a boundsTrigger.");
            Debug.LogException(missingManagerException);
        }
        Debug.LogWarning("Warning: Cannot use SemanticWrap without a BoundsTrigger. We strongly reccomend surrounding your world with a BoundsTrigger.");
    }

    public Vector3 GetSemanticOffset()
    {
        return referenceBlockInitialPosition - blocks[0].transform.position;
    }

    public GameObject SemanticInstantiate(GameObject objectToInstantiate)
    {
        GameObject newObject = Instantiate(objectToInstantiate);
        Vector3 semanticOffset = -1*GetSemanticOffset();
        newObject.transform.Translate(semanticOffset);
        newObject.transform.position = bounds.GetNewPosition(newObject.transform.position);
        return newObject;
    }

    public void LogTriggerEntry(GameObject entryBlock)
    {
        if(initialTrigger == null)
        {
            initialTrigger = entryBlock;
        }
        else
        {
            isTransitioning = true;
        }
        currentTrigger = entryBlock;
    }

    public void LogTriggerExit(GameObject exitBlock)
    {
        // If we are moving from one block to another, do nothing
        if (isTransitioning)
        {
            isTransitioning = false;
            return;
        }
        // Initiate wrap
        if (ShouldWrap())
        {
            WrapWorld();
        }
        initialTrigger = null;
        currentTrigger = null;
    }

    /* Some colliders contain multiple contact points, causing wraps to happen twice.
    All these checks ensure only one wrap happens and only when it is supposed to */
    private bool ShouldWrap()
    {
        bool shouldWrap = !GameObject.ReferenceEquals(currentTrigger, initialTrigger) &&
        !GameObject.ReferenceEquals(currentBlock, previousBlock) && currentTrigger!= null &&
        initialTrigger != null;
        return shouldWrap;
    }

    public void WrapWorld()
    {
        zeroMagnitudeWrapTriggered = wrapper.WrapWorld(currentBlock.transform.position, previousBlock.transform.position);
        initialTrigger = null;
        previousBlock = currentBlock;
    }

    public void LogBlockEntry(GameObject enterBlock)
    {
        if (previousBlock == null)
        {
            previousBlock = enterBlock;
        }
        currentBlock = enterBlock;
        if (zeroMagnitudeWrapTriggered)
        {
            WrapWorld();
        }
    }

    public void SetLureObject(GameObject navMeshLure)
    {
        lureObject = navMeshLure;
    }

    public void SetBlocksLength(int length)
    {
        blocks = new GameObject[length];
    }

    public void AddBlock(GameObject block)
    {
        int nextBlockIndex = -1;
        for(int index = 0; index < blocks.Length; index++)
        {
            if(!blocks[index])
            {
                nextBlockIndex = index;
                break;
            }
        }
        blocks[nextBlockIndex] = block;
    }

    // RENAME: UsingNavMesh in v1.0.0
    public void SetIsUsingNavMesh(bool isUsing)
    {
        isUsingNavmesh = isUsing;
    }

    public void UsingMultiplayer(bool usingMultiplayer)
    {
        isMultiplayer = true;
    }

    public bool IsMultiplayer()
    {
        return isMultiplayer;
    }

    public void SetNetworkManager(GameObject networkManager)
    {
        worldWrapNetworkManagerObject = networkManager;
    }

    public void SetIsMultiplayer(bool multiplayer)
    {
        isMultiplayer = multiplayer;
    }

    public void AddToSelfWrappers(GameObject selfWrapper)
    {
        wrapper.AddToSelfWrappers(selfWrapper);
    }

    public void RemoveSelfWrap(GameObject selfWrapper)
    {
        wrapper.RemoveSelfWrap(selfWrapper);
    }
}