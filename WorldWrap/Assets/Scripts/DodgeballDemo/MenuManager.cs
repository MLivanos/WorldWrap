using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button easyButton;
    [SerializeField] private Button mediumButton;
    [SerializeField] private Button hardButton;
    [SerializeField] private Button playButton;
    private GameObject mainCamera;
    private GameObject player;
    private GameManager gameManager;
    int gameMode;

    private void Start()
    {
        gameMode = -1;
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
        easyButton.onClick.AddListener(SetEasy);
        mediumButton.onClick.AddListener(SetMedium);
        hardButton.onClick.AddListener(SetHard);
        playButton.onClick.AddListener(playGame);
        mainCamera.transform.parent = null;
        player.SetActive(false);
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
        mainCamera.transform.parent = player.transform;
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
}
