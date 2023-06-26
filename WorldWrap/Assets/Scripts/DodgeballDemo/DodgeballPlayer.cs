using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeballPlayer : DodgeballActor
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
    // Interaction variables
    [SerializeField] private float grabbingRange;

    private void Start()
    {
        SetupCamera();
        SetupScreenMovement();
        playerRigidbody = gameObject.GetComponent<Rigidbody>();
        isHoldingObject = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
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
    }

    private void SetupScreenMovement()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UpdatePosition()
    {
        yRotation += Input.GetAxisRaw("Mouse X") * Time.deltaTime * rotationSensitivity;
        xRotation -= Input.GetAxisRaw("Mouse Y") * Time.deltaTime * rotationSensitivity;
        xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f);
        mainCamera.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
        playerRigidbody.velocity = transform.TransformDirection(Vector3.forward) * speed * Input.GetAxisRaw("Vertical");
        playerRigidbody.velocity += transform.TransformDirection(Vector3.right) * speed * Input.GetAxisRaw("Horizontal");
    }

    private void Interact()
    {
        RaycastHit hit;
        Transform outlook = mainCamera.transform;
        if (isHoldingObject)
        {
            ThrowObject();
        }
        // If object in view
        else if (Physics.Raycast(outlook.position, outlook.TransformDirection(Vector3.forward), out hit, grabbingRange))
        {
            if (hit.rigidbody != null && hit.rigidbody.tag == "Dodgeball")
            {
                PickupObject(hit.rigidbody.gameObject);
            }
        }
    }
}
