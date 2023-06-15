using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpManager : MonoBehaviour
{
    [SerializeField] private GameObject[] blocks;
    private GameObject[,] blockMatrix;
    private GameObject initialBlock;
    private GameObject currentBlock;
    private bool isTransitioning;

    private void Start()
    {
        initialBlock = null;
        currentBlock = null;
        // Automatically detect matrix structure of blocks
        Vector2[] coordinatesByX;
        Vector2[] coordinatesByZ;
        Dictionary<float, int> xToRow = new Dictionary<float, int>();
        Dictionary<float, int> zToColumn = new Dictionary<float, int>();
        SortCoordinates(out coordinatesByX, out coordinatesByZ);
        SetupMatrix(coordinatesByX, coordinatesByZ, xToRow, zToColumn);
        FillMatrix(xToRow, zToColumn);
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

    public void LogEntry(GameObject entryBlock)
    {
        if(initialBlock == null)
        {
            initialBlock = entryBlock;
        }
        else
        {
            isTransitioning = true;
        }
        currentBlock = entryBlock;
    }

    public void LogExit(GameObject exitBlock)
    {
        // If we are moving from one block to another, do nothing
        if (isTransitioning)
        {
            isTransitioning = false;
            return;
        }
        // Initiate wrap
        if (!GameObject.ReferenceEquals(currentBlock, initialBlock))
        {
            GameObject[,] newMatrix = new GameObject[blockMatrix.GetLength(0), blockMatrix.GetLength(1)];
            int translationNumber = GetTranslationNumber();
            switch (translationNumber)
            {
                case 3:
                    TranslateLeft(newMatrix);
                    break;
                case 2:
                    TranslateRight(newMatrix);
                    break;
                case 1:
                    TranslateUp(newMatrix);
                    break;
                case 0:
                    TranslateDown(newMatrix);
                    break;
                default:
                    Exception missingManagerException = new Exception("Incorrect translation code");
                    Debug.LogException(missingManagerException);
                    break;
            }
            TranslateBlocks(GetBlockPositions(), newMatrix);
            blockMatrix = newMatrix;
        }
    }

    private void PrintMatrix(GameObject[,] mat)
    {
        for(int row = 0; row < mat.GetLength(0); row++)
        {
            Debug.Log(string.Format("{0}, {1}, {2}", mat[row,0],mat[row,1],mat[row,2]));
        }
    }

    private int GetTranslationNumber()
    {
        return 2;
    }

    private void TranslateBlocks(Vector3[,] oldPositions, GameObject[,] newMatrix)
    {
        for(int row = 0; row < oldPositions.GetLength(0); row++)
        {
            for(int column = 0; column < oldPositions.GetLength(1); column++)
            {
                newMatrix[row,column].transform.position = oldPositions[row,column];
            }
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
            newMatrix[row, 0] = blockMatrix[row,blockMatrix.GetLength(1)-1];
        }
        for(int row=0; row < blockMatrix.GetLength(0); row++)
        {
            for(int column=0; column < blockMatrix.GetLength(0) - 1; column++)
            {
                newMatrix[row, column + 1] = blockMatrix[row, column];
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
}