using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    // Movement variables
    [SerializeField] private float rotationSensitivity;
    [SerializeField] private float speed;
    private Rigidbody playerRigidbody;
    private float xRotation;
    private float yRotation;
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
        SetupScreenMovement();
        playerRigidbody = gameObject.GetComponent<Rigidbody>();
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

    private void SetupScreenMovement()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UpdatePosition()
    {
        //float rotationSpeed = Input.GetAxisRaw("Horizontal") * Time.deltaTime * rotationSensitivity;
        yRotation += Input.GetAxisRaw("Mouse X") * Time.deltaTime * rotationSensitivity;
        xRotation -= Input.GetAxisRaw("Mouse Y") * Time.deltaTime * rotationSensitivity;
        xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f);
        mainCamera.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
        playerRigidbody.velocity = transform.TransformDirection(Vector3.forward) * speed * Input.GetAxisRaw("Vertical");
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
                hit.rigidbody.constraints = RigidbodyConstraints.FreezePosition;
                hit.rigidbody.gameObject.transform.localPosition = heldObjectPosition;
                heldObject = hit.rigidbody.gameObject;
                isHoldingObject = true;
            }
        }
    }

    private void ThrowObject()
    {
        heldObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
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
