using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyForward : MonoBehaviour
{
    [SerializeField] private float speed;
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody objectRigidBody = gameObject.GetComponent<Rigidbody>();
        objectRigidBody.velocity = transform.TransformDirection(Vector3.forward) * speed;
    }

}
