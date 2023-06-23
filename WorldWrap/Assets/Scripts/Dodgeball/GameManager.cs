using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int difficulty;
    [SerializeField] private int numberOfEnemies;
    [SerializeField] private float dodgeballSpawnFrequency;
    [SerializeField] private float[] enemyThrowStrengthByDifficulty;
    [SerializeField] private float[] enemySpreadByDifficulty;
    [SerializeField] private float[] enemySpeedByDifficulty;
    [SerializeField] private int[] enemyHealthByDifficulty;
    private GameObject[] enemies;
    private DodgeballPlayer player;
    private bool isGameOn;
    private bool startingNewGame;
    
    private void Start()
    {
        enemies = new GameObject[numberOfEnemies];
        isGameOn = false;
        startingNewGame = true;
        player = GameObject.Find("Player").GetComponent<DodgeballPlayer>();
    }

    private void Update()
    {
        if (isGameOn)
        {
            CheckForWin();
            CheckForLoss();
        }
        if (startingNewGame)
        {
            CreateEnemies();
            startingNewGame = false;
            isGameOn = true;
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
        isGameOn = false;
        Debug.Log("The Game Is Over: You Win!");
    }

    private void CheckForLoss()
    {
        if (player.IsDead())
        {
            Debug.Log("The Game Is Over: You Lose!");
            isGameOn = false;
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
        GameObject enemy = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        NavMeshAgent agent = enemy.AddComponent(typeof(NavMeshAgent)) as NavMeshAgent;
        DodgeballEnemy enemyScript = enemy.AddComponent(typeof(DodgeballEnemy)) as DodgeballEnemy;
        Rigidbody enemyRigidBody = enemy.AddComponent(typeof(Rigidbody)) as Rigidbody;
        enemyRigidBody.mass = 10.0f;
        enemyRigidBody.isKinematic = true;
        agent.baseOffset = 0.95f;
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
}
