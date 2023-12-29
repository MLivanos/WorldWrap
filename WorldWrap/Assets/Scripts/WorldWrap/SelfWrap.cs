using System;
using UnityEngine;

public class SelfWrap : MonoBehaviour
{
    private float lowerXBound;
    private float lowerZBound;
    private float upperXBound;
    private float upperZBound;
    private BoundsTrigger boundsTrigger;

    private void Update()
    {
        if (!boundsTrigger.InsideBounds(transform.position.x, transform.position.z))
        {
            Debug.Log("!");
            transform.position = boundsTrigger.GetNewPosition(transform.position);
        }
    }

    public void SetBounds(BoundsTrigger bounds)
    {
        boundsTrigger = bounds;
        SetXBounds(bounds.getXBounds());
        SetZBounds(bounds.getZBounds());
    }

    private void SetXBounds(Vector2 xBounds)
    {
        lowerXBound = xBounds[0];
        upperXBound = xBounds[1];
    }

    private void SetZBounds(Vector2 zBounds)
    {
        lowerZBound = zBounds[0];
        upperZBound = zBounds[1];
    }
}