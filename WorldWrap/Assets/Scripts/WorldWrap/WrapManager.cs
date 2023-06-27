using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WrapManager : MonoBehaviour
{
    [SerializeField] private GameObject[] blocks;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject lureObject;
    [SerializeField] private int wrapLayer;
    [SerializeField] private bool isUsingNavmesh;
    private GameObject[,] blockMatrix;
    private GameObject initialTrigger;
    private GameObject currentTrigger;
    private GameObject previousBlock;
    private GameObject currentBlock;
    private bool isTransitioning;

    private void Start()
    {
        initialTrigger = null;
        currentTrigger = null;
        // Automatically detect matrix structure of blocks
        Vector2[] coordinatesByX;
        Vector2[] coordinatesByZ;
        Dictionary<float, int> xToRow = new Dictionary<float, int>();
        Dictionary<float, int> zToColumn = new Dictionary<float, int>();
        SortCoordinates(out coordinatesByX, out coordinatesByZ);
        SetupMatrix(coordinatesByX, coordinatesByZ, xToRow, zToColumn);
        FillMatrix(xToRow, zToColumn);
        if (isUsingNavmesh)
        {
            CreateNavMeshLure();
        }
    }

    // Given the unorganized array of blocks, organize them into a matrix
    private void SortCoordinates(out Vector2[] coordinatesByX, out Vector2[] coordinatesByZ)
    {
        Vector2[] coordinates = new Vector2[blocks.Length];
        for(int blockIndex = 0; blockIndex < blocks.Length; blockIndex += 1)
        {
            float blockX = blocks[blockIndex].transform.position.x;
            float blockZ = blocks[blockIndex].transform.position.z;
            coordinates[blockIndex] = new Vector2(blockX, blockZ);
        }
        List<Vector2> coordinatesList = new List<Vector2>(coordinates);
        coordinatesByX = coordinatesList.OrderBy(v => v.x).ToArray();
        coordinatesByZ = coordinatesList.OrderBy(v => v.y).ToArray();
    }

    // Detect matrix shape and instantiate it
    private void SetupMatrix(Vector2[] coordinatesByX, Vector2[] coordinatesByZ, Dictionary<float, int> xToRow, Dictionary<float, int> zToColumn)
    {
        int numberOfRows = 1;
        int numberOfColumns = 1;
        float previousX = coordinatesByX[0].x;
        float previousZ = coordinatesByZ[0].y;
        xToRow[previousX] = 0;
        zToColumn[previousX] = 0;
        for(int blockIndex = 0; blockIndex < blocks.Length; blockIndex += 1)
        {
            float x = coordinatesByX[blockIndex].x;
            float z = coordinatesByZ[blockIndex].y;
            if(x != previousX)
            {
                xToRow[x] = numberOfRows;
                numberOfRows += 1;
                previousX = x;
            }
            if(z != previousZ)
            {
                xToRow[z] = numberOfColumns;
                numberOfColumns += 1;
                previousZ = z;
            }
        }
        blockMatrix = new GameObject[numberOfRows,numberOfColumns];
    }

    // Add blocks to their position according to where they exist in the world.
    private void FillMatrix(Dictionary<float, int> xToRow, Dictionary<float, int> zToColumn)
    {
        foreach(GameObject block in blocks)
        {
            int row = xToRow[block.transform.position.x];
            int column = xToRow[block.transform.position.z];
            blockMatrix[row, column] = block;
        }
    }

    private void CreateNavMeshLure()
    {
        Dictionary<float, GameObject> xToLure = new Dictionary<float, GameObject>();
        Dictionary<float, GameObject> zToLure = new Dictionary<float, GameObject>();
        foreach(Transform lurePlane in lureObject.transform)
        {
            if (xToLure.ContainsKey(lurePlane.position.x))
            {
                AddNavMeshLinks(lurePlane.gameObject, xToLure[lurePlane.position.x]);
            }
            else if (zToLure.ContainsKey(lurePlane.position.z))
            {
                AddNavMeshLinks(lurePlane.gameObject, zToLure[lurePlane.position.z]);
            }
            else
            {
                xToLure[lurePlane.position.x] = lurePlane.gameObject;
                zToLure[lurePlane.position.z] = lurePlane.gameObject;
            }
        }
    }

    private void CreateNavMeshPlanes()
    {
        float planeXOffset = blockMatrix[0,1].transform.position.z - blockMatrix[0,0].transform.position.z;
        float planeZOffset = blockMatrix[1,0].transform.position.x - blockMatrix[0,0].transform.position.x;
        // TODO: Check this on non-square maps
        float planeXScale = planeXOffset * blockMatrix.GetLength(0) / 10.0f;
        float planeZScale = planeZOffset * blockMatrix.GetLength(1) / 10.0f;
        float planePosition = blockMatrix[0,0].transform.position.z - planeZOffset / 2.0f - 1.5f;
        GameObject navMeshlure = new GameObject("NavMeshLure");
        GameObject plane1 = CreateNavMeshPlane(planeXScale, 0.0f, planePosition, navMeshlure);
        GameObject plane2 = CreateNavMeshPlane(planeXScale, 180.0f, planePosition, navMeshlure);
        planePosition = blockMatrix[0,0].transform.position.x - planeXOffset / 2.0f - 1.5f;
        GameObject plane3 = CreateNavMeshPlane(planeZScale, 90.0f, planePosition, navMeshlure);
        GameObject plane4 = CreateNavMeshPlane(planeZScale, 270.0f, planePosition, navMeshlure);
        navMeshlure.transform.position = transform.position;
    }

    private GameObject CreateNavMeshPlane(float scale, float rotation, float offset, GameObject navMeshlure)
    {
        GameObject plane  = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.localScale = new Vector3(scale, 1.0f, 0.3f);
        plane.transform.Rotate(0.0f, rotation, 0.0f, Space.World);
        // Create a plane that is at the origin on two axes, offset on the other to be touching the edge of the world
        plane.transform.Translate(plane.transform.TransformVector(Vector3.forward).normalized * offset, Space.World);
        plane.GetComponent<Renderer>().material = (Material)Resources.Load("Translucent1", typeof(Material));
        plane.transform.parent = navMeshlure.transform;
        return plane;
    }

    private void AddNavMeshLinks(GameObject plane1, GameObject plane2, int numberOfLinks = 20)
    {
        Vector3 plane1ToPlane2 = plane2.transform.position - plane1.transform.position;
        Vector3 newLinkPosition;
        // TODO: Replace with more precise figure than *10
        float planeLength = Mathf.Max(plane1.transform.lossyScale.x, plane1.transform.lossyScale.z) * 10;
        float linkIncrement = planeLength / numberOfLinks;
        int longDirection = 0;
        if (plane1.transform.position.z > plane1.transform.position.x)
        {
            longDirection = 2;
        }
        for (int linkNumber = 0; linkNumber < numberOfLinks; linkNumber++)
        {
            newLinkPosition = plane1.transform.position;
            newLinkPosition[longDirection] += -planeLength / 2 + linkNumber * linkIncrement;
            NavMeshLinkData newLink = new NavMeshLinkData();
            newLink.area = 0;
            newLink.bidirectional = true;
            newLink.costModifier = 0.02f;
            newLink.startPosition = newLinkPosition;
            newLink.endPosition = newLinkPosition + plane1ToPlane2;
            NavMesh.AddLink(newLink);
        }
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

    private void WrapWorld()
    {
        GameObject[,] newMatrix = GetTranslations();
        TranslateBlocks(GetBlockPositions(), newMatrix);
        blockMatrix = newMatrix;
        initialTrigger = null;
    }

    public void LogBlockEntry(GameObject enterBlock)
    {
        currentBlock = enterBlock;
    }

    public void LogBlockExit(GameObject exitBlock)
    {
        previousBlock = exitBlock;
    }

    public void HandleObjectBlockEnter(GameObject resident)
    {

    }

    private void PrintMatrix(GameObject[,] mat)
    {
        for(int row = 0; row < mat.GetLength(0); row++)
        {
            Debug.Log(string.Format("{0}, {1}, {2}", mat[row,0],mat[row,1],mat[row,2]));
        }
    }

    private GameObject[,] GetTranslations()
    {
        GameObject[,] newMatrix = new GameObject[blockMatrix.GetLength(0), blockMatrix.GetLength(1)];
        Vector3 translationVector = currentBlock.transform.position - previousBlock.transform.position;
        if (translationVector.magnitude == 0.0f)
        {
            return blockMatrix;
        }
        if (translationVector.x > 0)
        {
            TranslateUp(newMatrix);
        }
        else if (translationVector.x < 0)
        {
            TranslateDown(newMatrix);
        }
        if (translationVector.z > 0)
        {
            TranslateLeft(newMatrix);
        }
        else if (translationVector.z < 0)
        {
            TranslateRight(newMatrix);
        }
        return newMatrix;
    }

    private void TranslateBlocks(Vector3[,] oldPositions, GameObject[,] newMatrix)
    {
        Vector3 movementVector;
        HashSet<int> objectAlreadyMoved = new HashSet<int>();
        for(int row = 0; row < oldPositions.GetLength(0); row++)
        {
            for(int column = 0; column < oldPositions.GetLength(1); column++)
            {
                movementVector = oldPositions[row,column] - newMatrix[row,column].transform.position;
                TranslateObjects(newMatrix[row,column], movementVector, objectAlreadyMoved);
                newMatrix[row,column].transform.position = oldPositions[row,column];
            }
        }
    }

    private void TranslateObjects(GameObject block, Vector3 movementVector, HashSet<int> objectAlreadyMoved)
    {
        BlockTrigger triggerScript = block.GetComponentInChildren<BlockTrigger>();
        List<GameObject> residentsOfBlock = triggerScript.getResidents();
        GameObject[] oldResidents = new GameObject[residentsOfBlock.Count];
        int oldResidentCounter = 0;
        foreach(GameObject resident in residentsOfBlock)
        {
            if (resident == null)
            {
                oldResidents[oldResidentCounter] = resident;
                oldResidentCounter ++;
                continue;
            }
            int key = resident.GetInstanceID();
            if (resident.transform.parent == null && !objectAlreadyMoved.Contains(key))
            {
                objectAlreadyMoved.Add(key);
                MoveObject(resident, movementVector);
            }
        }
        foreach(GameObject oldResident in oldResidents)
        {
            if (oldResident == null)
            {
                break;
            }
            triggerScript.removeResident(oldResident);
        }
    }

    private Vector3[,] GetBlockPositions()
    {
        Vector3[,] blockPositions = new Vector3[blockMatrix.GetLength(0),blockMatrix.GetLength(0)];
        for(int row = 0; row < blockMatrix.GetLength(0); row++)
        {
            for(int column = 0; column < blockMatrix.GetLength(1); column++)
            {
                blockPositions[row,column] = blockMatrix[row,column].transform.position;
            }
        }
        return blockPositions;
    }

    private void TranslateLeft(GameObject[,] newMatrix)
    {
        // Wrap rightmost column around
        for(int row=0; row < blockMatrix.GetLength(0); row++)
        {
            newMatrix[row, blockMatrix.GetLength(1)-1] = blockMatrix[row,0];
        }
        for(int row=0; row < blockMatrix.GetLength(0); row++)
        {
            for(int column=0; column < blockMatrix.GetLength(0) - 1; column++)
            {
                newMatrix[row, column] = blockMatrix[row, column+1];
            }
        }
    }

    private void TranslateRight(GameObject[,] newMatrix)
    {
        // Wrap leftmost column around
        for(int row=0; row < blockMatrix.GetLength(0); row++)
        {
            newMatrix[row, 0] = blockMatrix[row, blockMatrix.GetLength(1)-1];
        }
        for(int row=0; row < blockMatrix.GetLength(0); row++)
        {
            for(int column=1; column < blockMatrix.GetLength(1); column++)
            {
                newMatrix[row, column] = blockMatrix[row, column - 1];
            }
        }
    }

    private void TranslateUp(GameObject[,] newMatrix)
    {
        // Wrap lowest row around
        for(int column=0; column < blockMatrix.GetLength(1); column++)
        {
            newMatrix[blockMatrix.GetLength(0)-1, column] = blockMatrix[0, column];
        }
        for(int row=1; row < blockMatrix.GetLength(0); row++)
        {
            for(int column=0; column < blockMatrix.GetLength(0); column++)
            {
                newMatrix[row - 1, column] = blockMatrix[row, column];
            }
        }
    }

    private void TranslateDown(GameObject[,] newMatrix)
    {
        // Wrap lowest row around
        for(int column=0; column < blockMatrix.GetLength(1); column++)
        {
            newMatrix[0, column] = blockMatrix[blockMatrix.GetLength(0)-1, column];
        }
        for(int row=0; row < blockMatrix.GetLength(0) - 1; row++)
        {
            for(int column=0; column < blockMatrix.GetLength(0); column++)
            {
                newMatrix[row + 1, column] = blockMatrix[row, column];
            }
        }
    }

    private void MoveObject(GameObject objectToMove, Vector3 movementVector)
    {
        NavMeshAgent agent = objectToMove.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.Warp(objectToMove.transform.position + movementVector);
            return;
        }
        objectToMove.transform.Translate(movementVector, Space.World);
    }

    public int GetWrapLayer()
    {
        return wrapLayer;
    }
}