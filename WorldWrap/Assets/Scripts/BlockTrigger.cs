using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockTrigger : TriggerBehavior
{
    // Residents refers to the gameobjects inside the block
    private List<GameObject> residents = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            wrapManager.LogBlockEntry(gameObject);
        }
        if (other.gameObject.layer != wrapManager.GetWrapLayer())
        {
            residents.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            wrapManager.LogBlockExit(gameObject);
        }
        residents.Remove(other.gameObject);
    }

    public List<GameObject> getResidents()
    {
        return residents;
    }
}
