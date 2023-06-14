using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpManager : MonoBehaviour
{
    [SerializeField] private GameObject[] blocks;
    private GameObject[][] blockMatrix;

    private void Start()
    {
        OrganizeBlockMatrix();
    }

    // Given the unorganized array of blocks, organize them into a matrix
    private void OrganizeBlockMatrix()
    {
        Vector2[] coordinates = new Vector2[blocks.Length];
        for(int blockIndex = 0; blockIndex < blocks.Length; blockIndex += 1)
        {
            float blockX = blocks[blockIndex].transform.position.x;
            float blockZ = blocks[blockIndex].transform.position.z;
            coordinates[blockIndex] = new Vector2(blockX, blockZ);
        }
        List<Vector2> coordinatesList = new List<Vector2>(coordinates);
        Vector2[] coordinatesByX = coordinatesList.OrderBy(v => v.x).ToArray();
        Vector2[] coordinatesByZ = coordinatesList.OrderBy(v => v.y).ToArray();
        int numberOfRows = 1;
        int numberOfColumns = 1;
        float previousX = coordinatesByX[0].x;
        float previousZ = coordinatesByZ[0].y;
        for(int blockIndex = 0; blockIndex < blocks.Length; blockIndex += 1)
        {
            float x = coordinatesByX[blockIndex].x;
            float z = coordinatesByZ[blockIndex].y;
            if(x != previousX)
            {
                numberOfRows += 1;
                previousX = x;
            }
            if(z != previousZ)
            {
                numberOfColumns += 1;
                previousZ = z;
            }
        }
        Debug.Log(numberOfRows);
        Debug.Log(numberOfColumns);
    }

    public void LogEntry(string triggerName)
    {
        Debug.Log(string.Format("WarpTrigger {0} has been entered", triggerName));
    }
    public void LogExit(string triggerName)
    {
        Debug.Log(string.Format("WarpTrigger {0} has been exited", triggerName));
    }
}