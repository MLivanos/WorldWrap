using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private GameObject treePrefab;
    [SerializeField] private GameObject globalBounds;
    [SerializeField] private float ballQuantity;
    [SerializeField] private float treeQuantity;
    private float globalXMin;
    private float globalXMax;
    private float globalZMin;
    private float globalZMax;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        BoundsTrigger boundsTrigger = globalBounds.GetComponent<BoundsTrigger>();
        Vector2 xBounds = boundsTrigger.getXBounds();
        Vector2 zBounds = boundsTrigger.getZBounds();
        globalXMin = xBounds[0];
        globalXMax = xBounds[1];
        globalZMin = zBounds[0];
        globalZMax = zBounds[1];
    }

    private void Update()
    {
        if (Input.GetKeyDown("b"))
        {
            AddBalls();
        }
        if (Input.GetKeyDown("t"))
        {
            AddTree();
        }
    }

    private void AddBalls()
    {
        for(int ballNumber = 0; ballNumber < ballQuantity; ballNumber++)
        {
            InstantiateNewBall();
        }
    }

    private void AddTree()
    {
        foreach(Transform blockTransform in globalBounds.transform)
        {
            for(int treeNumber = 0; treeNumber < treeQuantity; treeNumber++)
            {
                GameObject centerCube = blockTransform.Find("Cube").gameObject;
                ParentNewTree(centerCube);
            }
        }
    }

    private Vector3 GetRandomPosition(float xMin, float xMax, float zMin, float zMax, float yPosition)
    {
        return new Vector3(Random.Range(xMin, xMax), yPosition, Random.Range(zMin, zMax));
    }

    private void InstantiateNewBall()
    {
        GameObject newBall = Instantiate(ballPrefab, GetRandomPosition(globalXMin, globalXMax, globalZMin, globalZMax, 2.0f), Quaternion.identity);
        newBall.GetComponent<Renderer>().material.color = Random.ColorHSV();
    }

    private void ParentNewTree(GameObject centerCube)
    {
        GameObject newTree = Instantiate(treePrefab, Vector3.zero, Quaternion.identity);
        newTree.transform.parent = centerCube.transform;
        newTree.GetComponent<Renderer>().material.color = Random.ColorHSV();
        float cubeSize = 0.5f;
        Vector3 newPos = GetRandomPosition(-cubeSize, cubeSize, -cubeSize, cubeSize, 0.75f);
        newTree.transform.localPosition = newPos;
    }

}
