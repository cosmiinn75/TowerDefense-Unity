using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Unity.VisualScripting;

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
        while (true)
        {
            waveIsInProgess = true;
            activeEnemies = 0;
            for(int i = 0; i < enemiesPerWave; i++)
            {
                SpawnEnemy();
                activeEnemies++;
                yield return new WaitForSeconds(timeBetweenEnemies);
            }
            while(activeEnemies > 0)
            {
                yield return new WaitForSeconds(0.05f); // Checks if there are enemies left
            }

            waveIsInProgess = false;
            currentWave++;
            enemiesPerWave += 2;

            yield return new WaitForSeconds(timeBetweenWaves);

        }


    }

    void SpawnEnemy()
    {
        int randomIndex = Random.Range(0, availableEnemies.Count);
        EnemyData selectedData = availableEnemies[randomIndex]; // Selects random enemy type

        //Spawns enemy at spawner locations
        GameObject newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

        EnemyStats stats = newEnemy.GetComponent<EnemyStats>();
        
        stats.InitializeData(selectedData); // His stats are now as the random one 
        newEnemy.GetComponent<FollowPath>().pathwayPoints = wayPoints; 
        ApplyWaveBuff(stats);
    }

   
    void ApplyWaveBuff(EnemyStats stats)
    {
        float healthMultiplier = 1f + (currentWave - 1) * 0.1f;
        stats.currentHealth *= healthMultiplier; // Ups enemy's health 
        float speedMultiplier = 1f + (currentWave - 1) * 0.1f;
        stats.currentSpeed *= speedMultiplier; // Increases enemy's speed
        
    }
    public void EnemyDied()
    {
        activeEnemies--;
    }
}
