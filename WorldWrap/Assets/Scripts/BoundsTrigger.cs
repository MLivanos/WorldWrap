using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BoundsTrigger : TriggerBehavior
{
    float lowerXBound;
    float lowerZBound;
    float upperXBound;
    float upperZBound;

    protected override void Start()
    {
        base.Start();
        lowerXBound = wrapManager.transform.position.x - gameObject.transform.lossyScale.x / 2;
        upperXBound = wrapManager.transform.position.x + gameObject.transform.lossyScale.x / 2;
        lowerZBound = wrapManager.transform.position.z - gameObject.transform.lossyScale.z / 2;
        upperZBound = wrapManager.transform.position.z + gameObject.transform.lossyScale.z / 2;
    }

    private void OnTriggerExit(Collider other)
    {
        Vector3 otherTransform = other.gameObject.transform.position;
        Debug.Log(otherTransform);
        float otherX = otherTransform.x;
        float otherZ = otherTransform.z;
        if (otherX <= lowerXBound)
        {
            otherTransform.x = upperXBound;
        }
        else if (otherX >= upperXBound)
        {
            otherTransform.x = lowerXBound;
        }
        if (otherZ <= lowerZBound)
        {
            otherTransform.z = upperZBound;
        }
        else if (otherZ >= upperZBound)
        {
            otherTransform.z = lowerZBound;
        }
        // NavMeshAgents will glitch if transform is modified directly
        NavMeshAgent agent = other.gameObject.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.Warp(otherTransform);
            return;
        }
        other.gameObject.transform.position = otherTransform;
    }
}
