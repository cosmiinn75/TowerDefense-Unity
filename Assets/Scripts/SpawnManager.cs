using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Assets.FantasyTowerDefense.Scripts.Demo;
using TMPro;
using Unity.VisualScripting;

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
    private float timeBetweenWaves = 5.0f;
    public float defaultTimeBetweenEnemies = 1.5f;
    public float timeBetweenEnemies ;
    public int currentWave = 1;
    public float secondsBeforeFirstWave;
    [Header("Tracking")]
    public int activeEnemies;// How many enemies are still alive
    public List<GameObject> enemiesLeft = new List<GameObject>();
    [Header("UI Menu")]
    public GameObject waveText;
    void Start() {
        waveText.SetActive(false);
        StartCoroutine(SpawnWave());
        timeBetweenEnemies = defaultTimeBetweenEnemies;
      
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
                yield return new WaitForSeconds(secondsBeforeFirstWave);
            }
            yield return new WaitForSeconds(0.5f);

            activeEnemies = 0;
            timeBetweenEnemies = defaultTimeBetweenEnemies;
            List<EnemyData> enemiesToSpawn = null;

            switch (currentLevel) {
                case 1:
                    enemiesToSpawn = LoadLevel1();
                    break;

                case 2:
                    enemiesToSpawn = LoadLevel2();
                    break;
                case 3:
                    enemiesToSpawn = LoadLevel3();
                    break;
                case 4:
                    enemiesToSpawn = LoadLevel4();
                    break;
                case 5:
                    enemiesToSpawn = LoadLevel5();
                    break;
                case 6:
                    enemiesToSpawn = LoadLevel6();
                    break;
                case 7:
                    enemiesToSpawn = LoadLevel7();
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
                timeBetweenEnemies = 1f;
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
                list = FillWave(availableEnemies[4], 1);
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
                timeBetweenEnemies = 1f;
                list = FillWave(availableEnemies[16], 12);
                break;

            case 3:
                // Wave 3: 6 bandit rangers
                list = FillWave(availableEnemies[4], 6);
                break;

            case 4:
                // Wave 4: 15 spiders
                timeBetweenEnemies = 0.5f;
                list = FillWave(availableEnemies[1], 15);
                break;

            case 5:
                // Wave 5: 4 bandit scouts + 2 bandit elders
                AddEnemies(list, availableEnemies[2], 4);
                AddEnemies(list, availableEnemies[5], 2);
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

    private List<EnemyData> LoadLevel3()
    {
        List<EnemyData> list = new List<EnemyData>();

        switch (currentWave)
        {
            case 1: // 12 Goblins
                list = FillWave(availableEnemies[0], 12);
                break;
            case 2: // 6 Wargs
                timeBetweenEnemies = 1f;
                list = FillWave(availableEnemies[14], 6);
                break;
            case 3: //8 Skeletons + 4 Spiders
                timeBetweenEnemies = 1f;
                list = FillWave(availableEnemies[12], 8);
                AddEnemies(list, availableEnemies[1], 4);
                break;
            case 4: //6 Bandit Rangers
                timeBetweenEnemies = 1f;
                list = FillWave(availableEnemies[4], 6);
                break;
            case 5: // 3 Wargs + 1 Bandit Leader(Boss)
                timeBetweenEnemies = 1f;
                list = FillWave(availableEnemies[14], 3);
                AddEnemies(list, availableEnemies[6], 1);
                break;

            default:
                list = new List<EnemyData>();
                break;
        }



        return list;
    }
    private List<EnemyData> LoadLevel4()
    {
        List<EnemyData> list = new List<EnemyData>();

        switch (currentWave)
        {
            case 1: // 20 Goblins
                timeBetweenEnemies = 0.8f;
                list = FillWave(availableEnemies[0], 20);
                break;
            case 2: // 15 Wolves
                timeBetweenEnemies = 0.5f;
                list = FillWave(availableEnemies[16], 15);
                break;
            case 3: //10 Enemy Archers + 10 Skeletons
                timeBetweenEnemies = 1f;
                list = FillWave(availableEnemies[8], 10);
                AddEnemies(list, availableEnemies[12], 10);
                break;
            case 4: //15 Bandit Rangers
                timeBetweenEnemies = 1f;
                list = FillWave(availableEnemies[4], 15);
                break;
            case 5: // 3 Bandit Elder + 6 Skeletons
                timeBetweenEnemies = 1f;
                list = FillWave(availableEnemies[5], 3);
                AddEnemies(list, availableEnemies[12], 6);
                break;

            default:
                list = new List<EnemyData>();
                break;
        }



        return list;
    }
    private List<EnemyData> LoadLevel5()
    {
        List<EnemyData> list = new List<EnemyData>();

        switch (currentWave)
        {
            case 1: // 12 Goblins
                timeBetweenEnemies = 0.8f;
                list = FillWave(availableEnemies[0], 12);
                break;
            case 2: // 10 Skeletons
                list = FillWave(availableEnemies[12], 10);
                break;
            case 3: // 12 Wargs
                timeBetweenEnemies = 0.8f;
                list = FillWave(availableEnemies[14], 8);
               // AddEnemies(list, availableEnemies[14], 4);
                break;
            case 4: //5 Mages + 10 Spiders
        
                list = FillWave(availableEnemies[10], 5);
                AddEnemies(list, availableEnemies[1], 10);
                break;
            case 5: // 5 Mech Spiders + 3 Bandit Elders
                list = FillWave(availableEnemies[11], 5);
                AddEnemies(list, availableEnemies[5], 3);
         
                break;
            case 6: // 8 Wargs + 5 Enemy Archer
                timeBetweenEnemies = 0.9f;
                list = FillWave(availableEnemies[14], 8);
                AddEnemies(list, availableEnemies[8], 5);
                break;
            case 7: // Troll + Bandit Leader
                timeBetweenEnemies = 3f;
                list = FillWave(availableEnemies[3], 1);
                AddEnemies(list, availableEnemies[6], 1);
                break;
            default:
                list = new List<EnemyData>();
                break;
        }



        return list;
    }
    private List<EnemyData> LoadLevel6()
    {
        List<EnemyData> list = new List<EnemyData>();

        switch (currentWave)
        {
            case 1: // 12 Skeletons
                timeBetweenEnemies = 0.8f;
                list = FillWave(availableEnemies[12], 12);
                break;
            case 2: // 10 Wargs
                timeBetweenEnemies = 0.4f;
                list = FillWave(availableEnemies[14], 10);
                break;
            case 3: // 6 Mages + 10 Goblins
                timeBetweenEnemies = 0.7f;
                list = FillWave(availableEnemies[10], 6);
                AddEnemies(list, availableEnemies[0], 10);
                break;
            case 4: //6 Mech Spiders + 4 Witches    
                timeBetweenEnemies = 0.9f;
                list = FillWave(availableEnemies[11], 6);
                AddEnemies(list, availableEnemies[15], 4);
                break;
            case 5: // 5 Bandit Elders + 6 Wargs
                timeBetweenEnemies = 0.6f;
                list = FillWave(availableEnemies[5], 5);
                AddEnemies(list, availableEnemies[14], 6);

                break;
            case 6: //2  Bandit Leaders +  5 Mages
                timeBetweenEnemies = 0.8f;
                list = FillWave(availableEnemies[6], 2);
                AddEnemies(list, availableEnemies[10], 5);
                break;
            case 7: // 2 Cyclops + 2 TrollNoRes
                timeBetweenEnemies = 1f;
                list = FillWave(availableEnemies[17], 2);
                AddEnemies(list, availableEnemies[18], 2);
                break;
            default:
                list = new List<EnemyData>();
                break;
        }



        return list;
    }
    private List<EnemyData> LoadLevel7()
    {
        List<EnemyData> list = new List<EnemyData>();

        switch (currentWave)
        {
            case 1: //12 Bandit Rangers
                timeBetweenEnemies = 1f;
                list = FillWave(availableEnemies[4], 15);
                break;
            case 2: // 2 Trolls + 12 Goblins
                
                list = FillWave(availableEnemies[3], 2);
                AddEnemies(list, availableEnemies[0], 12);
                break;
            case 3: // 10 Mech Spider + 15 Wargs
                timeBetweenEnemies = 1.2f;
                list = FillWave(availableEnemies[11], 10);
                AddEnemies(list, availableEnemies[14], 15);
                break;
            case 4: // 5 Bandit Leader + 4 Mech Spider   
                timeBetweenEnemies = 1f;
                list = FillWave(availableEnemies[6], 5);
                AddEnemies(list, availableEnemies[11], 4);
                break;
            case 5: // 4 Trolls + 3 Bandit Leaders + 5 Mech Spiders

                list = FillWave(availableEnemies[3], 4);
                AddEnemies(list, availableEnemies[6], 3);
                AddEnemies(list, availableEnemies[11], 5);

                break;
         
            default:
                list = new List<EnemyData>();
                break;
        }



        return list;
    }
    public IEnumerator WaitAndEnd()
    {
        yield return new WaitForSecondsRealtime(1f);
        Debug.Log("Am oprit timpul");
        Time.timeScale = 0f;
    }
}
