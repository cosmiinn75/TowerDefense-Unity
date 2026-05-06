using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Assets.FantasyTowerDefense.Scripts.Demo;
using TMPro;

public class SpawnManager : MonoBehaviour
{
    [Header("Level Settings")]
    public int currentLevel;
    public int waveLevel ;
    [Header("Enemy Settings")]
    public GameObject enemyPrefab;
    public List<EnemyData> availableEnemies; //Types of enemies
    public List<Transform> wayPoints; 
    [Header("Wave Settings")]
    public float timeBetweenWaves = 5.0f;
    public float timeBetweenEnemies = 1.5f;
    public int currentWave = 1;
    [Header("Tracking")]
    public int activeEnemies;// How many enemies are still alive
    public List<GameObject> enemiesLeft;
    [Header("UI Menu")]
    public GameObject waveText;
    void Start() {
        waveText.SetActive(false);
        StartCoroutine(SpawnWave());
      
    }
    
    IEnumerator SpawnWave()
    {
        while (currentWave <= waveLevel)
        {
            if (currentWave == 1)
            {
                waveText.SetActive(true);
                waveText.GetComponent<TextMeshProUGUI>().text = "Wave " + currentWave.ToString();
                waveText.GetComponent<TextFadeAnimation>()?.TriggerAnimation();
                yield return new WaitForSeconds(1f);
            }
            yield return new WaitForSeconds(0.5f);
            activeEnemies = 0;
            List<EnemyData> enemiesToSpawn = null;

            switch (currentLevel) {
                case 1:
                    enemiesToSpawn = LoadLevel1();
                    break;

                case 2:
                    enemiesToSpawn = LoadLevel2();
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


            currentWave++;
            if (currentWave <= waveLevel)
            {
                enemiesLeft.Clear();
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
            enemiesLeft.Add(newEnemy);
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

    private List<EnemyData> LoadLevel1()
    {
        List<EnemyData> list = new List<EnemyData>();

        switch (currentWave)
        {
            case 1:
                // Wave 1: 6 Goblins

                list = FillWave(availableEnemies[0], 6);
                break;

            case 2:
                // Wave 2: 10 Spiders
                list = FillWave(availableEnemies[1], 10);
                break;

            case 3:
                // Wave 3: 4 Bandit Scouts
                list = FillWave(availableEnemies[2], 4);
                break;

            case 4:
                // Wave 4: 4 Goblins and 4 Bandit Scouts
        
                AddEnemies(list, availableEnemies[0], 4);
                AddEnemies(list, availableEnemies[2], 4);
                break;

            case 5:
                // Wave 5: 1 Troll (Boss)
                list = FillWave(availableEnemies[3], 1);
                break;

            default:
                list = new List<EnemyData>();
                break;

        }
        return list;
    }
    private List<EnemyData> LoadLevel2()
    {
        List<EnemyData> list = new List<EnemyData>();

        switch (currentWave)
        {
            case 1:
                // Wave 1: 8 Goblins

                list = FillWave(availableEnemies[0], 8);
                break;

            case 2:
                // Wave 2: 12 wolves
                list = FillWave(availableEnemies[16], 12);
                break;

            case 3:
                // Wave 3: 6 bandit rangers
                list = FillWave(availableEnemies[4], 6);
                break;

            case 4:
                // Wave 4: 15 spiders

                list = FillWave(availableEnemies[1], 15);
                break;

            case 5:
                // Wave 5: 4 bandit scouts + 2 bandit elders
                AddEnemies(list, availableEnemies[2], 2);
                AddEnemies(list, availableEnemies[5], 1);
                AddEnemies(list, availableEnemies[2], 2);
                AddEnemies(list, availableEnemies[5], 1);
                break;
            case 6:
                // Wave 6: 10 wolves + cyclops(boss)
                AddEnemies(list, availableEnemies[16], 10);
                AddEnemies(list, availableEnemies[7], 1);
                break;

            default:
                list = new List<EnemyData>();
                break;

        }
        return list;
    }

}
