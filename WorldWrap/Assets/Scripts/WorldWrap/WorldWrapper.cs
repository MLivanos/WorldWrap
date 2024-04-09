using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorldWrapper : MonoBehaviour
{
    
    private GameObject[,] blockMatrix;
    private bool zeroMagnitudeWrapTriggered;
    private WorldWrapNetworkManager worldWrapNetworkManager;
    private List<GameObject> selfWrappers;
    private bool isMultiplayer;

    public bool WrapWorld(Vector3 currentPosition, Vector3 previousPosition)
    {
        zeroMagnitudeWrapTriggered = false;
        GameObject[,] newMatrix = GetTranslations(currentPosition, previousPosition);
        TranslateBlocks(GetBlockPositions(), newMatrix);
        blockMatrix = newMatrix;
        return zeroMagnitudeWrapTriggered;
    }

    public void Setup(GameObject[,] blocks, WorldWrapNetworkManager networkManager)
    {
        selfWrappers = new List<GameObject>();
        blockMatrix = blocks;
        worldWrapNetworkManager = networkManager;
        isMultiplayer = (worldWrapNetworkManager != null);
    }

    private GameObject[,] GetTranslations(Vector3 currentPosition, Vector3 previousPosition)
    {
        GameObject[,] newMatrix = new GameObject[blockMatrix.GetLength(0), blockMatrix.GetLength(1)];
        GameObject[,] oldMatrix = new GameObject[blockMatrix.GetLength(0), blockMatrix.GetLength(1)];
        oldMatrix = DeepCopyMatrix(blockMatrix);
        Vector3 translationVector = currentPosition - previousPosition;
        if (translationVector.magnitude == 0.0f)
        {
            zeroMagnitudeWrapTriggered = true;
            return blockMatrix;
        }
        if (translationVector.x > 0)
        {
            TranslateUp(newMatrix, oldMatrix);
            oldMatrix = DeepCopyMatrix(newMatrix);
        }
        else if (translationVector.x < 0)
        {
            TranslateDown(newMatrix, oldMatrix);
            oldMatrix = DeepCopyMatrix(newMatrix);
        }
        if (translationVector.z > 0)
        {
            TranslateLeft(newMatrix, oldMatrix);
        }
        else if (translationVector.z < 0)
        {
            TranslateRight(newMatrix, oldMatrix);
        }
        return newMatrix;
    }

    private void TranslateBlocks(Vector3[,] oldPositions, GameObject[,] newMatrix)
    {
        Vector3 movementVector = Vector3.zero;
        HashSet<int> objectAlreadyMoved = new HashSet<int>();
        int middleX = (int)oldPositions.GetLength(0) / 2;
        int middleZ = (int)oldPositions.GetLength(1) / 2;
        movementVector = oldPositions[middleX,middleZ] - newMatrix[middleX,middleZ].transform.position;
        TranslateSelfWrappers(movementVector);
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

    private void TranslateSelfWrappers(Vector3 movementVector)
    {
        foreach(GameObject objectWrapping in selfWrappers)
        {
            objectWrapping.transform.Translate(movementVector, Space.World);
        }
    }

    private Vector3[,] GetBlockPositions()
    {
        Vector3[,] blockPositions = new Vector3[blockMatrix.GetLength(0),blockMatrix.GetLength(1)];
        for(int row = 0; row < blockMatrix.GetLength(0); row++)
        {
            for(int column = 0; column < blockMatrix.GetLength(1); column++)
            {
                blockPositions[row,column] = blockMatrix[row,column].transform.position;
            }
        }
        return blockPositions;
    }

    private void TranslateLeft(GameObject[,] newMatrix, GameObject[,] oldMatrix)
    {
        // Wrap rightmost column around
        for(int row=0; row < oldMatrix.GetLength(0); row++)
        {
            newMatrix[row, oldMatrix.GetLength(1)-1] = oldMatrix[row,0];
        }
        for(int row=0; row < oldMatrix.GetLength(0); row++)
        {
            for(int column=0; column < oldMatrix.GetLength(1) - 1; column++)
            {
                newMatrix[row, column] = oldMatrix[row, column+1];
            }
        }
    }

    private void TranslateRight(GameObject[,] newMatrix, GameObject[,] oldMatrix)
    {
        // Wrap leftmost column around
        for(int row=0; row < oldMatrix.GetLength(0); row++)
        {
            newMatrix[row, 0] = oldMatrix[row, oldMatrix.GetLength(1)-1];
        }
        for(int row=0; row < oldMatrix.GetLength(0); row++)
        {
            for(int column=1; column < oldMatrix.GetLength(1); column++)
            {
                newMatrix[row, column] = oldMatrix[row, column - 1];
            }
        }
    }

    private void TranslateUp(GameObject[,] newMatrix, GameObject[,] oldMatrix)
    {
        // Wrap highest row around
        for(int column=0; column < oldMatrix.GetLength(1); column++)
        {
            newMatrix[oldMatrix.GetLength(0)-1, column] = oldMatrix[0, column];
        }
        for(int row=1; row < oldMatrix.GetLength(0); row++)
        {
            for(int column=0; column < oldMatrix.GetLength(1); column++)
            {
                newMatrix[row - 1, column] = oldMatrix[row, column];
            }
        }
    }

    private void TranslateDown(GameObject[,] newMatrix, GameObject[,] oldMatrix)
    {
        // Wrap lowest row around
        for(int column=0; column < oldMatrix.GetLength(1); column++)
        {
            newMatrix[0, column] = oldMatrix[oldMatrix.GetLength(0)-1, column];
        }
        for(int row=0; row < oldMatrix.GetLength(0) - 1; row++)
        {
            for(int column=0; column < oldMatrix.GetLength(1); column++)
            {
                newMatrix[row + 1, column] = oldMatrix[row, column];
            }
        }
    }

    private GameObject[,] DeepCopyMatrix(GameObject[,] matrix)
    {
        GameObject[,] newMatrix = new GameObject[matrix.GetLength(0), matrix.GetLength(1)];
        for(int row = 0; row < matrix.GetLength(0); row++)
        {
            for(int column = 0; column < matrix.GetLength(1); column++)
            {
                newMatrix[row,column] = matrix[row,column];
            }
        }
        return newMatrix;
    }

    private void MoveObject(GameObject objectToMove, Vector3 movementVector)
    {
        NavMeshAgent agent = objectToMove.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            Vector3 agentDestination = agent.destination;
            agent.Warp(objectToMove.transform.position + movementVector);
            agent.destination = agentDestination;
            return;
        }
        if (IsMultiplayerClient(objectToMove))
        {
            worldWrapNetworkManager.Warp(movementVector, objectToMove);
            return;
        }
        objectToMove.transform.Translate(movementVector, Space.World);
    }

    private bool IsMultiplayerClient(GameObject objectToMove)
    {
        WorldWrapNetworkObject networkObject = objectToMove.GetComponent<WorldWrapNetworkObject>();
        if (!networkObject)
        {
            return false;
        }
        return isMultiplayer && networkObject.IsOwned();
    }

    public void AddToSelfWrappers(GameObject selfWrapper)
    {
        selfWrappers.Add(selfWrapper);
    }

    public void RemoveSelfWrap(GameObject selfWrapper)
    {
        selfWrappers.Remove(selfWrapper);
    }
}