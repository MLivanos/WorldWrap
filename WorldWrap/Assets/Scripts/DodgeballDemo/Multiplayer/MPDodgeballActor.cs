using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MPDodgeballActor : NetworkBehaviour
{
    [SerializeField] protected Vector3 heldObjectPosition;
    [SerializeField] protected float throwStrength;
    [SerializeField] protected int health;
    protected GameObject heldObject;
    protected bool isHoldingObject;
    
    public void decrementHealth()
    {
        health --;
    }

    public bool IsDead()
    {
        return health <= 0;
    }

    public int GetHealth()
    {
        return health;
    }

    protected void PickupObject(GameObject ball)
    {
        if (isHoldingObject)
        {
            return;
        }
        ball.transform.parent = transform;
        ball.transform.localPosition = heldObjectPosition;
        heldObject = ball;
        isHoldingObject = true;
        Rigidbody ballRidigBody = ball.GetComponent<Rigidbody>();
        ballRidigBody.constraints = RigidbodyConstraints.FreezePosition;
    }

    protected void ThrowObject(float spread = 0.0f)
    {
        heldObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        heldObject.transform.parent = null;
        Rigidbody objectRigidBody = heldObject.GetComponent<Rigidbody>();
        Vector3 throwDirection = Quaternion.Euler(0, Random.Range(-spread, spread), 0) * transform.TransformDirection(Vector3.forward);
        objectRigidBody.AddForce(throwStrength * throwDirection, ForceMode.Impulse);
        Dodgeball dodgeballScript = heldObject.GetComponent<Dodgeball>();
        dodgeballScript.SetActive(true);
        isHoldingObject = false;
    }

    public bool IsHoldingObject()
    {
        return isHoldingObject;
    }

}
