using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    // Movement variables
    [SerializeField] private float rotationSensitivity;
    [SerializeField] private float speed;
    // Camera variables
    private Camera mainCamera;
    private GameObject mainCameraGameObject;
    private Vector3 mainCameraFPPosition;
    private bool isFirstPerson;
    // Interaction variables
    [SerializeField] private Vector3 heldObjectPosition;
    [SerializeField] private float grabbingRange;
    [SerializeField] private float throwStrength;
    private GameObject heldObject;
    private bool isHoldingObject;

    private void Start()
    {
        SetupCamera();
        isHoldingObject = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown("v"))
        {
            ChangeCameraView();
        }
        if (Input.GetKeyDown("e"))
        {
            Interact();
        }
        UpdatePosition();
    }

    private void SetupCamera()
    {
        mainCamera = gameObject.GetComponentInChildren<Camera>();
        mainCameraGameObject = mainCamera.gameObject;
        mainCameraFPPosition = mainCameraGameObject.transform.localPosition;
        isFirstPerson = true;
    }

    private void UpdatePosition()
    {
        float rotationSpeed = Input.GetAxisRaw("Horizontal") * Time.deltaTime * rotationSensitivity;
        float movementSpeed = Input.GetAxisRaw("Vertical") * Time.deltaTime * speed;
        
        transform.Rotate(0, rotationSpeed, 0); 
        transform.Translate(Vector3.forward * Time.deltaTime * movementSpeed);
    }

    private void Interact()
    {
        if (isHoldingObject)
        {
            ThrowObject();
        }
        else
        {
            PickUpObject();
        }
        
    }

    private void PickUpObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, grabbingRange))
        {
            if (hit.rigidbody != null && hit.rigidbody.tag == "Grabbable")
            {
                hit.rigidbody.gameObject.transform.parent = gameObject.transform;
                hit.rigidbody.gameObject.transform.localPosition = heldObjectPosition;
                heldObject = hit.rigidbody.gameObject;
                isHoldingObject = true;
            }
        }
    }

    private void ThrowObject()
    {
        heldObject.transform.parent = null;
        Rigidbody objectRigidBody = heldObject.GetComponent<Rigidbody>();
        objectRigidBody.AddForce(throwStrength * transform.TransformDirection(Vector3.forward), ForceMode.Impulse);
        isHoldingObject = false;
    }

    private void ChangeCameraView()
    {
        if (isFirstPerson)
        {
            // Establish third-person view
            mainCameraGameObject.transform.parent = null;
            mainCameraGameObject.transform.position = new Vector3(0, 80, 0);
            mainCameraGameObject.transform.eulerAngles = new Vector3(90, 0, 0);
        }
        else
        {
            // Establish first-person view
            mainCameraGameObject.transform.parent = gameObject.transform;
            mainCameraGameObject.transform.localPosition = mainCameraFPPosition;
            mainCameraGameObject.transform.rotation = gameObject.transform.rotation;
        }
        isFirstPerson = !isFirstPerson;
    }
}
