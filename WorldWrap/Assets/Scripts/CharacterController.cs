using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float rotationSensitivity;
    [SerializeField] private float speed;
    private Camera mainCamera;
    private GameObject mainCameraGameObject;
    private bool isFirstPerson;

    private void Start()
    {
        mainCamera = gameObject.GetComponentInChildren<Camera>();
        mainCameraGameObject = mainCamera.gameObject;
        isFirstPerson = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown("v"))
        {
            ChangeCameraView();
        }
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        float rotationSpeed = Input.GetAxisRaw("Horizontal") * Time.deltaTime * rotationSensitivity;
        float movementSpeed = Input.GetAxisRaw("Vertical") * Time.deltaTime * speed;
        
        transform.Rotate(0, rotationSpeed, 0); 
        transform.Translate(Vector3.forward * Time.deltaTime * movementSpeed);
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
            mainCameraGameObject.transform.localPosition = new Vector3(0, 0.25f, 0.5f);
            mainCameraGameObject.transform.rotation = gameObject.transform.rotation;
        }
        isFirstPerson = !isFirstPerson;
    }
}
