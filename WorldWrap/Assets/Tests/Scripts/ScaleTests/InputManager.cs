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
    private float xMin;
    private float xMax;
    private float zMin;
    private float zMax;


    private void Start()
    {
        BoundsTrigger boundsTrigger = globalBounds.GetComponent<BoundsTrigger>();
        Vector2 xBounds = boundsTrigger.getXBounds();
        Vector2 zBounds = boundsTrigger.getZBounds();
        xMin = xBounds[0];
        xMax = xBounds[1];
        zMin = zBounds[0];
        zMax = zBounds[1];
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
            Instantiate(ballPrefab, GetRandomPosition(), Quaternion.identity);
        }
    }

    private void AddTree()
    {
        
    }

    private Vector3 GetRandomPosition()
    {
        return new Vector3(Random.Range(xMin, xMax), 2.0f, Random.Range(zMin, zMax));
    }

}
