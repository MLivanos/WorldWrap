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
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject otherGameObject = other.gameObject;
        float otherX = otherGameObject.transform.position.x;
        float otherZ = otherGameObject.transform.position.z;
        if (InsideBounds(otherX, otherZ))
        {
            SelfWrap newSelfWrap = otherGameObject.AddComponent(typeof(SelfWrap)) as SelfWrap;
            newSelfWrap.SetBounds(this);
            wrapManager.AddToSelfWrappers(newSelfWrap.gameObject);
            return;
        }
        Vector3 otherPosition = GetNewPosition(otherGameObject.transform.position);
        // NavMeshAgents will glitch if transform is modified directly
        NavMeshAgent agent = other.gameObject.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            bool agentIsStopped = agent.velocity == Vector3.zero;
            Vector3 agentDestination = agent.destination;
            agent.Warp(otherPosition);
            agent.destination = agentDestination;
            agent.isStopped = agentIsStopped;
            return;
        }
        other.gameObject.transform.position = otherPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        SelfWrap otherSelfWrap = other.gameObject.GetComponent<SelfWrap>();
        if (otherSelfWrap)
        {
            wrapManager.RemoveSelfWrap(other.gameObject);
            Destroy(otherSelfWrap);
        }
    }

    public Vector3 GetNewPosition(Vector3 currentPosition)
    {
        float xDistance = upperXBound - lowerXBound;
        float zDistance = upperZBound - lowerXBound;
        Vector3 otherPosition = currentPosition;
        // euclideanPosition +/-=(1+|euclideanPosition/axisLength|)*axisLength
        // TODO: Test all corrections
        if (currentPosition.x <= lowerXBound)
        {
            otherPosition.x = otherPosition.x + ((int)Mathf.Abs(otherPosition.x/xDistance)+1)*xDistance;
        }
        else if (currentPosition.x >= upperXBound)
        {
            otherPosition.x = otherPosition.x - ((int)Mathf.Abs(otherPosition.x/xDistance)+1)*xDistance;
        }
        if (currentPosition.z <= lowerZBound)
        {
            otherPosition.z = otherPosition.z + ((int)Mathf.Abs(otherPosition.z/zDistance)+1)*zDistance;
        }
        else if (currentPosition.z >= upperZBound)
        {
            otherPosition.z = otherPosition.z - ((int)Mathf.Abs(otherPosition.z/zDistance)+1)*zDistance;
        }
        return otherPosition;
    }

    public Vector2 getXBounds()
    {
        return new Vector2(lowerXBound, upperXBound);
    }

    public Vector2 getZBounds()
    {
        return new Vector2(lowerZBound, upperZBound);
    }

    public bool InsideBounds(float otherX, float otherZ)
    {
        return otherX > lowerXBound && otherX < upperXBound && otherZ > lowerZBound && otherZ < upperZBound;
    }
}