using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyForward : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody objectRigidBody;

    private void Start()
    {
        objectRigidBody = gameObject.GetComponent<Rigidbody>();
        objectRigidBody.velocity = transform.TransformDirection(Vector3.forward) * speed;
    }

    public void ChangeVelocity(float newSpeed)
    {
        speed = newSpeed;
        objectRigidBody.velocity = transform.TransformDirection(Vector3.forward) * speed;
    }

}
