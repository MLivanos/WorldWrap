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
            return;
        }
        Vector3 otherPosition = GetNewPosition(otherGameObject.transform.position);
        // NavMeshAgents will glitch if transform is modified directly
        NavMeshAgent agent = other.gameObject.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.Warp(otherPosition);
            return;
        }
        other.gameObject.transform.position = otherPosition;
    }

    public Vector3 GetNewPosition(Vector3 currentPosition)
    {
        Vector3 otherPosition = currentPosition;
        if (currentPosition.x <= lowerXBound)
        {
            otherPosition.x = upperXBound;
        }
        else if (currentPosition.x >= upperXBound)
        {
            otherPosition.x = lowerXBound;
        }
        if (currentPosition.z <= lowerZBound)
        {
            otherPosition.z = upperZBound;
        }
        else if (currentPosition.z >= upperZBound)
        {
            otherPosition.z = lowerZBound;
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
