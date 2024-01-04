using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

public class WrapManagerSetup : MonoBehaviour
{
    private GameObject[,] blockMatrix;
    private GameObject[] blocks;
    private GameObject lureObject;

    public void Setup(GameObject[] blocksArray, GameObject lureObject_)
    {
        blocks = blocksArray;
        lureObject = lureObject_;
    }

    public GameObject[,] GetBlockMatrix()
    {
        Vector2[] coordinatesByX;
        Vector2[] coordinatesByZ;
        Dictionary<float, int> xToRow = new Dictionary<float, int>();
        Dictionary<float, int> zToColumn = new Dictionary<float, int>();
        FindBounds();
        SortCoordinates(out coordinatesByX, out coordinatesByZ);
        SetupMatrix(coordinatesByX, coordinatesByZ, xToRow, zToColumn);
        FillMatrix(xToRow, zToColumn);
        return blockMatrix;
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
        int numberOfRows = 1, numberOfColumns = 1;
        float previousX = coordinatesByX[0].x;
        float previousZ = coordinatesByZ[0].y;
        xToRow[previousX] = 0;
        zToColumn[previousZ] = 0;
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
                zToColumn[z] = numberOfColumns;
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
            int column = zToColumn[block.transform.position.z];
            blockMatrix[row, column] = block;
        }
    }

    public void CreateNavMeshLure()
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

    private void AddNavMeshLinks(GameObject plane1, GameObject plane2, int numberOfLinks = 20)
    {
        Vector3 plane1ToPlane2 = plane2.transform.position - plane1.transform.position;
        Vector3 newLinkPosition;
        float planeLength = Mathf.Max(plane1.transform.lossyScale.x, plane1.transform.lossyScale.z) * 10;
        float linkIncrement = planeLength / numberOfLinks;
        int longDirection = 0;
        if (Math.Abs(plane1.transform.position.z) < Math.Abs(plane1.transform.position.x))
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

    public Vector3 SetReferenceBlock()
    {
        try
        {
            return blocks[0].transform.position;
        }
        catch
        {
            Exception missingManagerException = new Exception("Error: No blocks detected in WrapManager's blocks list. Did you forget to add the blocks?");
            Debug.LogException(missingManagerException);
            return Vector3.zero;
        }
    }

    public BoundsTrigger FindBounds()
    {
        GameObject[] gameObjectsInScene = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject objectInScene in gameObjectsInScene)
        {
            BoundsTrigger bounds = objectInScene.GetComponent<BoundsTrigger>();
            if (bounds)
            {
                return bounds;
            }
        }
       return null;
    }
}
