using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BoundsTrigger : TriggerBehavior
{
    private float lowerXBound;
    private float lowerZBound;
    private float upperXBound;
    private float upperZBound;

    protected override void Start()
    {
        base.Start();
        lowerXBound = wrapManager.transform.position.x - gameObject.transform.lossyScale.x / 2;
        upperXBound = wrapManager.transform.position.x + gameObject.transform.lossyScale.x / 2;
        lowerZBound = wrapManager.transform.position.z - gameObject.transform.lossyScale.z / 2;
        upperZBound = wrapManager.transform.position.z + gameObject.transform.lossyScale.z / 2;
        Debug.Log(lowerXBound);
        Debug.Log(upperXBound);
        Debug.Log(lowerZBound);
        Debug.Log(upperZBound);
    }

    private void OnTriggerExit(Collider other)
    {
        Vector3 otherPosition = other.gameObject.transform.position;
        float otherX = otherPosition.x;
        float otherZ = otherPosition.z;
        if (otherX <= lowerXBound)
        {
            otherPosition.x = upperXBound;
        }
        else if (otherX >= upperXBound)
        {
            otherPosition.x = lowerXBound;
        }
        if (otherZ <= lowerZBound)
        {
            otherPosition.z = upperZBound;
        }
        else if (otherZ >= upperZBound)
        {
            otherPosition.z = lowerZBound;
        }
        // NavMeshAgents will glitch if transform is modified directly
        NavMeshAgent agent = other.gameObject.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.Warp(otherPosition);
            return;
        }
        other.gameObject.transform.position = otherPosition;
    }

    public Vector2 getXBounds()
    {
        return new Vector2(lowerXBound, upperXBound);
    }

    public Vector2 getZBounds()
    {
        return new Vector2(lowerZBound, upperZBound);
    }
}
