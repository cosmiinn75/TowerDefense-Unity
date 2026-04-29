using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Unity.VisualScripting;
using Assets.FantasyTowerDefense.Scripts.Demo;
using UnityEditor;
using JetBrains.Annotations;

public class SpawnManager : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject enemyPrefab;
    public List<EnemyData> availableEnemies; //Types of enemies
    public List<Transform> wayPoints; 
    [Header("Wave Settings")]
    public float timeBetweenWaves = 5.0f;
    public float timeBetweenEnemies = 2.0f;
    public int currentWave = 1;
    public int enemiesPerWave = 5;
    [Header("Tracking")]
    public int activeEnemies; // How many enemies are still alive
    private bool waveIsInProgess = false;
    
    void Start() {
        StartCoroutine(SpawnWave());
    }
    
    IEnumerator SpawnWave()
    {
        while (currentWave <= 5)
        {
            waveIsInProgess = true;
            activeEnemies = 0;
            List<EnemyData> enemiesToSpawn = null;
            switch (currentWave)
            {
                case 1:
                    enemiesToSpawn = new List<EnemyData>{ availableEnemies[0] , availableEnemies[0]  , availableEnemies[0] , availableEnemies[0] };
                    break;
                case 2:
                    enemiesToSpawn = new List<EnemyData> { availableEnemies[0], availableEnemies[1] , availableEnemies[0] , availableEnemies[0] , availableEnemies[0] };
                    break;
                case 3:
                    enemiesToSpawn = new List<EnemyData> { availableEnemies[1], availableEnemies[0], availableEnemies[1], availableEnemies[0], availableEnemies[0] };
                    break;
                case 4:
                    enemiesToSpawn = new List<EnemyData> { availableEnemies[0], availableEnemies[0], availableEnemies[1], availableEnemies[2], availableEnemies[0] };
                    break;
                case 5:
                    enemiesToSpawn = new List<EnemyData> { availableEnemies[2], availableEnemies[1], availableEnemies[0], availableEnemies[1], availableEnemies[0] };
                    break;
                default:
                    break;


                }

           foreach(var enemy in enemiesToSpawn) { 
                SpawnEnemy(enemy);
                activeEnemies++;
                yield return new WaitForSeconds(timeBetweenEnemies);
            }
            while(activeEnemies > 0)
            {
                yield return new WaitForSeconds(0.05f); // Checks if there are enemies left
            }

            waveIsInProgess = false;
            currentWave++;
            enemiesPerWave ++;

            yield return new WaitForSeconds(timeBetweenWaves);

        }


    }

    void SpawnEnemy(EnemyData enemyData)
    {
        if (enemyData != null)
        {
            GameObject newEnemy = Instantiate(enemyData.enemyPrefab, transform.position, Quaternion.identity);
            EnemyStats stats = newEnemy.GetComponent<EnemyStats>();

            stats.InitializeData(enemyData); // His stats are now as the random one 
            var monsterLogic = newEnemy.GetComponent<Monster>();

            if (monsterLogic != null)
            {
                monsterLogic.Initialize(wayPoints);
                monsterLogic.Speed = (int)stats.currentSpeed;
                monsterLogic.SetStartingHealth((int)stats.currentHealth);
            }
        }


    }


    public void EnemyDied()
    {
        activeEnemies--;
    }

    void ApplyWaveBuff(EnemyStats stats)
    {

        float healthMultiplier = 1f + (currentWave - 1) * 0.1f;
        stats.currentHealth *= healthMultiplier; // Ups enemy's health 
        float speedMultiplier = 1f + (currentWave - 1) * 0.1f;
        stats.currentSpeed *= speedMultiplier; // Increases enemy's speed

    }

}
