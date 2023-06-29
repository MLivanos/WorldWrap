using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button easyButton;
    [SerializeField] private Button mediumButton;
    [SerializeField] private Button hardButton;
    [SerializeField] private Button playButton;
    [SerializeField] private Vector3 menuCameraPosition;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float maxAngle;
    [SerializeField] private float minAngle;
    private Vector3 playerCameraPosition;
    private GameObject mainCamera;
    private GameObject player;
    private GameManager gameManager;
    private float rotationDirection;
    private int gameMode;

    private void Start()
    {
        gameMode = -1;
        rotationDirection = 1;
        SetupObjects();
        ConnectButtons();
        SetupCamera();
        player.SetActive(false);
    }

    private void Update()
    {
        RotateCamera();
    }

    private void RotateCamera()
    {
        if (mainCamera.transform.eulerAngles.y > maxAngle || mainCamera.transform.eulerAngles.y < minAngle)
        {
            rotationDirection *= -1;
        }
        mainCamera.transform.RotateAround(mainCamera.transform.position, Vector3.up, rotationDirection * rotationSpeed * Time.deltaTime);
    }

    private void playGame()
    {
        if (gameMode == -1)
        {
            return;
        }
        gameManager.SetDifficulty(gameMode);
        gameManager.Play();
        closeText();
        player.SetActive(true);
        mainCamera.transform.eulerAngles = Vector3.zero;
        mainCamera.transform.parent = player.transform;
        mainCamera.transform.localPosition = playerCameraPosition;
    }

    private void SetEasy()
    {
        gameMode = 0;
    }

    private void SetMedium()
    {
        gameMode = 1;
    }

    private void SetHard()
    {
        gameMode = 2;
    }

    private void closeText()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(!child.gameObject.activeSelf);
        }
    }

    private void ConnectButtons()
    {
        easyButton.onClick.AddListener(SetEasy);
        mediumButton.onClick.AddListener(SetMedium);
        hardButton.onClick.AddListener(SetHard);
        playButton.onClick.AddListener(playGame);
    }

    private void SetupCamera()
    {
        playerCameraPosition = mainCamera.transform.localPosition;
        mainCamera.transform.parent = null;
        mainCamera.transform.position = menuCameraPosition;
    }

    private void SetupObjects()
    {
        GameObject[] gameObjectsInScene = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject objectInScene in gameObjectsInScene)
        {
            if (objectInScene.name == "GameManager")
            {
                gameManager = objectInScene.GetComponent<GameManager>();
            }
            if (objectInScene.name == "Player")
            {
                player = objectInScene;
                mainCamera = player.transform.GetChild(0).gameObject;
            }
        }
    }
}
