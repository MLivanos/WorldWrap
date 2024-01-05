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
            Vector3 agentDestination = agent.destination;
            agent.Warp(otherPosition);
            agent.destination = agentDestination;
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
        float xOffset = calculateWrapDirection(otherPosition.x, upperXBound, lowerXBound) * calculateWrapMagnitude(otherPosition.x, xDistance);
        float zOffset = calculateWrapDirection(otherPosition.z, upperZBound, lowerZBound) * calculateWrapMagnitude(otherPosition.z, zDistance);
        return otherPosition + new Vector3(xOffset, 0, zOffset);
    }

    private float calculateWrapMagnitude(float axisPosition, float axisDistance)
    {
        // euclideanPosition +/-=(1+|euclideanPosition/axisLength|)*axisLength
        return ((int)Mathf.Abs(axisPosition/axisDistance)+1)*axisDistance;
    }

    private float calculateWrapDirection(float axisPosition, float upperPosition, float lowerPosition)
    {
        float axisProportion = Mathf.Floor((axisPosition - lowerPosition)/(upperPosition-lowerPosition));
        return -1 * Mathf.Clamp(axisProportion, -1, 1);
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
