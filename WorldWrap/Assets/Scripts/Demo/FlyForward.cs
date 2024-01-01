using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyForward : MonoBehaviour
{
    [SerializeField] private float speed;

    private void Start()
    {
        Rigidbody objectRigidBody = gameObject.GetComponent<Rigidbody>();
        objectRigidBody.velocity = transform.TransformDirection(Vector3.forward) * speed;
    }

}
