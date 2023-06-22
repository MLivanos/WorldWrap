using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeballActor : MonoBehaviour
{
    protected int health;
    protected GameObject heldObject;
    protected bool isHoldingObject;
    [SerializeField] private float throwStrength;
    
    public void decrementHealth()
    {
        health --;
    }

    public bool isDead()
    {
        return health <= 0;
    }

    protected void ThrowObject()
    {
        heldObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        heldObject.transform.parent = null;
        Rigidbody objectRigidBody = heldObject.GetComponent<Rigidbody>();
        objectRigidBody.AddForce(throwStrength * transform.TransformDirection(Vector3.forward), ForceMode.Impulse);
        isHoldingObject = false;
    }
}
