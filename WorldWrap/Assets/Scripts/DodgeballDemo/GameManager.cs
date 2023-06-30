using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int difficulty;
    [SerializeField] private int numberOfEnemies;
    [SerializeField] private Vector3 initialDodgeballPosition;
    [SerializeField] private int initialNumberOfDodgeballs;
    [SerializeField] private int maximumNumberOfDodgeballs;
    [SerializeField] private float dodgeballSpawnFrequency;
    [SerializeField] private float[] enemyThrowStrengthByDifficulty;
    [SerializeField] private float[] enemySpreadByDifficulty;
    [SerializeField] private float[] enemySpeedByDifficulty;
    [SerializeField] private int[] enemyHealthByDifficulty;
    [SerializeField] GameObject dodgeBallPrefab;
    [SerializeField] GameObject enemyPrefab;
    private GameObject[] enemies;
    private List<GameObject> aliveDodgeballs;
    private BoundsTrigger bounds;
    private DodgeballPlayer player;
    private bool gameWon;
    private bool isGameOn;
    private bool startingNewGame;
    private bool isSpawning;
    
    private void Start()
    {
        enemies = new GameObject[numberOfEnemies];
        aliveDodgeballs = new List<GameObject>();;
        isGameOn = false;
        startingNewGame = false;
        gameWon = false;
        isSpawning = false;
        GameObject[] gameObjectsInScene = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject objectInScene in gameObjectsInScene)
        {
            if (objectInScene.name == "Player")
            {
                player = objectInScene.GetComponent<DodgeballPlayer>();
            }
            if (objectInScene.name == "GlobalBounds")
            {
                bounds = objectInScene.GetComponent<BoundsTrigger>();
            }
        }
    }

    private void Update()
    {
        if (startingNewGame)
        {
            CreateEnemies();
            InstantiateDodgeballGroup(initialDodgeballPosition, new Vector3(0.0f,0.0f,0.5f), 6);
            startingNewGame = false;
        }
        if (isGameOn)
        {
            CheckForWin();
            CheckForLoss();
            CleanupDodgeballs();
        }
        if (ShouldSpawnDodgeball())
        {
            StartCoroutine(SpawnDodgeball());
        }
    }

    private void CheckForWin()
    {
        foreach(GameObject enemy in enemies)
        {
            if (enemy)
            {
                return;
            }
        }
        gameWon = true;
        isGameOn = false;
    }

    private void CheckForLoss()
    {
        if (player.IsDead())
        {
            isGameOn = false;
            gameWon = false;
        }
    }

    private void CreateEnemies()
    {
        for (int enemyID = 0; enemyID < numberOfEnemies; enemyID ++)
        {
            GameObject newEnemy = AddNewEnemy(enemyID);
            SetEnemyStats(newEnemy);
        }
    }

    private GameObject AddNewEnemy(int enemyID)
    {
        GameObject enemy = Instantiate(enemyPrefab);
        DodgeballEnemy enemyScript = enemy.GetComponent<DodgeballEnemy>();
        enemyScript.PlaceRandomly();
        SetEnemyStats(enemy);
        enemies[enemyID] = enemy;
        return enemy;
    }

    private void SetEnemyStats(GameObject enemy)
    {
        int health = enemyHealthByDifficulty[difficulty];
        float throwStrength = enemyThrowStrengthByDifficulty[difficulty];
        float spread = enemySpreadByDifficulty[difficulty];
        float speed = enemySpeedByDifficulty[difficulty];
        DodgeballEnemy enemyScript = enemy.GetComponent<DodgeballEnemy>();
        enemyScript.SetHealth(health);
        enemyScript.SetThrowStrength(throwStrength);
        enemyScript.SetSpread(spread);
        enemyScript.SetSpeed(speed);
    }

    private IEnumerator SpawnDodgeball()
    {
        isSpawning = true;
        Vector2 xBounds = bounds.getXBounds();
        Vector2 zBounds = bounds.getZBounds();
        Vector3 randomPosition = new Vector3(UnityEngine.Random.Range(xBounds[0], xBounds[1]), 1.0f, UnityEngine.Random.Range(zBounds[0], zBounds[1]));
        InstantiateDodgeball(randomPosition);
        yield return new WaitForSeconds(5.0f / dodgeballSpawnFrequency);
        isSpawning = false;
    }

    private void InstantiateDodgeball(Vector3 position)
    {
        GameObject ball = Instantiate(dodgeBallPrefab, position, Quaternion.identity);
        aliveDodgeballs.Add(ball);
    }

    private void InstantiateDodgeballGroup(Vector3 startingPosition, Vector3 increment, int numberOfBalls)
    {
        for(int ballNumber = 0; ballNumber < numberOfBalls; ballNumber++)
        {
            InstantiateDodgeball(startingPosition + increment*ballNumber);
        }
    }

    private bool ShouldSpawnDodgeball()
    {
        return isGameOn && !isSpawning && aliveDodgeballs.Count < maximumNumberOfDodgeballs;
    }

    private void CleanupDodgeballs()
    {
        aliveDodgeballs.RemoveAll(ball => ball == null || ball.transform.position.y < -1.0f);
    }

    public void SetDifficulty(int level)
    {
        difficulty = level;
    }

    public void Play()
    {
        startingNewGame = true;
        isGameOn = true;
    }

    public bool HasWon()
    {
        return !isGameOn && gameWon;
    }

    public bool HasLost()
    {
        return !isGameOn && !gameWon;
    }
}
