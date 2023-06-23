using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private GameObject player;
    private bool isGameOn;
    
    private void Start()
    {
        enemies = new GameObject[numberOfEnemies];
        isGameOn = false;
        CreateEnemies();
    }

    private void Update()
    {
        if (isGameOn)
        {
            CheckForWin();
            CheckForLoss();
        }
    }

    private void CheckForWin()
    {
        foreach(GameObject enemy in enemies)
        {
            if (enemy.activeInHierarchy)
            {
                return;
            }
        }
        isGameOn = false;
        Debug.Log("The Game Is Over: You Win!");
    }

    private void CheckForLoss()
    {
        DodgeballPlayer playerScript = player.GetComponent<DodgeballPlayer>();
        if (playerScript.isDead())
        {
            Debug.Log("The Game Is Over: You Lose!");
            isGameOn = false;
        }
    }

    private void CreateEnemies()
    {
        for (int enemyID = 0; enemyID < numberOfEnemies; enemyID ++)
        {
            // CREATE ENEMY
            // enemy = ...
            // PLACE ENEMY
            // enemy.SetRandomLocation();
            /* SetEnemyStats(enemy);
            enemies[enemyID] = enemy;
            */
        }
    }

    private void SetEnemyStats(GameObject enemy)
    {
        int health = enemyHealthByDifficulty[difficulty];
        float throwStrength = enemyThrowStrengthByDifficulty[difficulty];
        float spread = enemySpreadByDifficulty[difficulty];
        float speed = enemySpeedByDifficulty[difficulty];
        DodgeballEnemy enemyScript = enemy.GetComponent<DodgeballEnemy>();
        // EDIT STATS
        /*
        enemyScript.SetHealth(health);
        enemyScript.SetThrowStrength(health);
        enemyScript.SetSpread(health);
        enemyScript.SetSpeed(health);
        */
    }
}
