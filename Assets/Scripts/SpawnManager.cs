using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Assets.FantasyTowerDefense.Scripts.Demo;
using TMPro;

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
    [Header("Tracking")]
    public int activeEnemies; // How many enemies are still alive
   // private bool waveIsInProgess = false;
    [Header("UI Menu")]
    public GameObject waveText;
    void Start() {
        waveText.SetActive(false);
        StartCoroutine(SpawnWave());
      
    }
    
    IEnumerator SpawnWave()
    {
        while (currentWave <= 5)
        {
            if (currentWave == 1)
            {
                waveText.SetActive(true);
                waveText.GetComponent<TextMeshProUGUI>().text = "Wave " + currentWave.ToString();
                waveText.GetComponent<TextFadeAnimation>()?.TriggerAnimation();
                yield return new WaitForSeconds(1f);
            }
            yield return new WaitForSeconds(0.5f);
            // waveIsInProgess = true;
            activeEnemies = 0;
            List<EnemyData> enemiesToSpawn = null;
            switch (currentWave)
            {
                case 1:
                    // Wave 1: 6 Goblins
                    enemiesToSpawn = FillWave(availableEnemies[0], 6);
                    break;

                case 2:
                    // Wave 2: 10 Spiders
                    enemiesToSpawn = FillWave(availableEnemies[1], 10);
                    break;

                case 3:
                    // Wave 3: 4 Bandit Scouts
                    enemiesToSpawn = FillWave(availableEnemies[2], 4);
                    break;

                case 4:
                    // Wave 4: 4 Goblins and 4 Bandit Scouts
                    enemiesToSpawn = new List<EnemyData>();
                    AddEnemies(enemiesToSpawn, availableEnemies[0], 4);
                    AddEnemies(enemiesToSpawn, availableEnemies[2], 4);
                    break;

                case 5:
                    // Wave 5: 1 Troll (Boss)
                    enemiesToSpawn = FillWave(availableEnemies[3], 1);
                    break;

                default:
                    enemiesToSpawn = new List<EnemyData>();
                    break;
            }

            foreach (var enemy in enemiesToSpawn) { 
                SpawnEnemy(enemy);
                activeEnemies++;
                yield return new WaitForSeconds(timeBetweenEnemies);
            }
            while(activeEnemies > 0)
            {
                yield return new WaitForSeconds(0.05f); // Checks if there are enemies left
            }

            //waveIsInProgess = false;
            currentWave++;
            if (currentWave <= 5)
            {
                waveText.SetActive(true);
                waveText.GetComponent<TextMeshProUGUI>().text = "Wave " + currentWave.ToString();
                waveText.GetComponent<TextFadeAnimation>()?.TriggerAnimation();
            }
            else
            {
                yield return new WaitForSeconds(1.0f);
                Time.timeScale = 0;
            }

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
    private List<EnemyData> FillWave(EnemyData enemyType, int count)
    {
        List<EnemyData> waveList = new List<EnemyData>();

        for (int i = 0; i < count; i++)
        {
            waveList.Add(enemyType);
        }
        return waveList;
    }
    private void AddEnemies(List <EnemyData> list , EnemyData enemyType , int count)
    {
        for(int i= 0; i < count; i++)
        {
            list.Add(enemyType);
        }
    }

}
