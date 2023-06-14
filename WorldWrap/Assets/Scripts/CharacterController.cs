using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float rotationSensitivity;
    [SerializeField] private float speed;

    void Start()
    {
        
    }

    void Update()
    {
        float rotationSpeed = Input.GetAxisRaw("Horizontal") * Time.deltaTime * rotationSensitivity;
        float movementSpeed = Input.GetAxisRaw("Vertical") * Time.deltaTime * speed;
        
        transform.Rotate(0, rotationSpeed, 0); 
        transform.Translate(Vector3.forward * Time.deltaTime * movementSpeed);
    }
}
