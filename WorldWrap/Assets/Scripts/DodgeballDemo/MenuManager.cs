using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private TMP_Text topText;
    [SerializeField] private Button easyButton;
    [SerializeField] private Button replayButton;
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
    private bool hasStarted;

    private void Start()
    {
        gameMode = -1;
        rotationDirection = 1;
        hasStarted = false;
        SetupObjects();
        ConnectButtons();
        SetupCamera();
        replayButton.gameObject.SetActive(false);
        player.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        RotateCamera();
        if (hasStarted)
        {
            CheckForGameOver();
        }
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
        hasStarted = true;
        gameManager.SetDifficulty(gameMode);
        gameManager.Play();
        closeText();
        player.SetActive(true);
        mainCamera.transform.eulerAngles = Vector3.zero;
        mainCamera.transform.parent = player.transform;
        mainCamera.transform.localPosition = playerCameraPosition;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
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
            child.gameObject.SetActive(false);
        }
    }

    private void ConnectButtons()
    {
        easyButton.onClick.AddListener(SetEasy);
        mediumButton.onClick.AddListener(SetMedium);
        hardButton.onClick.AddListener(SetHard);
        playButton.onClick.AddListener(playGame);
        replayButton.onClick.AddListener(Replay);
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

    private void DisplayWinScreen()
    {
        DisplayGameOverWidgets();
        topText.text = "Congratulations,\nYou Win!\nPlay Again?";
    }

    private void DisplayLossScreen()
    {
        DisplayGameOverWidgets();
        topText.text = "Sorry,\nYou Lose.\nTry Again?";
    }

    private void DisplayGameOverWidgets()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        topText.gameObject.SetActive(true);
        replayButton.gameObject.SetActive(true);
    }

    private void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void CheckForGameOver()
    {
        if (gameManager.HasWon())
        {
            DisplayWinScreen();
        }
        if (gameManager.HasLost())
        {
            DisplayLossScreen();
        }
    }
}
