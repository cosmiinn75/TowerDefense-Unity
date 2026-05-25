using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Assets.FantasyTowerDefense.Scripts.Demo;
using TMPro;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    [Header("Level Settings")]
    public int currentLevel;
    public int waveLevel;
    [Header("Enemy Settings")]
    public GameObject enemyPrefab;
    public List<EnemyData> availableEnemies; //Types of enemies
    public List<Transform> wayPoints;
    [Header("Wave Settings")]
    private float timeBetweenWaves = 5.0f;
    public float defaultTimeBetweenEnemies = 1.5f;
    public float timeBetweenEnemies;
    public int currentWave = 1;
    public float secondsBeforeFirstWave;
    [Header("Tracking")]
    public int activeEnemies;// How many enemies are still alive
    [Header("UI Menu")]
    public GameObject waveText;
    public GameObject bossHealthUI;

    void Start() {
        waveText.SetActive(false);
        StartCoroutine(SpawnWave());
        timeBetweenEnemies = defaultTimeBetweenEnemies;
        
    }

    IEnumerator SpawnWave()
    {
        while (currentWave <= waveLevel)
        {
 
            UpdateWaveUI();

            if (currentWave == 1)
            {
                yield return new WaitForSeconds(secondsBeforeFirstWave);
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
            }

            activeEnemies = 0;
            timeBetweenEnemies = defaultTimeBetweenEnemies;
            List<EnemyData> enemiesToSpawn = LoadLevelData(currentLevel);

            foreach (var enemy in enemiesToSpawn)
            {
                SpawnEnemy(enemy);
                activeEnemies++;
                yield return new WaitForSeconds(timeBetweenEnemies);
            }

    
            while (activeEnemies > 0)
            {
                yield return new WaitForSeconds(0.1f);
            }

            currentWave++;

            if (currentWave <= waveLevel)
            {     
                yield return new WaitForSeconds(timeBetweenWaves);
            }
            else
            {
           
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.gameOver = true;
                    GameManager.Instance.win = true;
                }
                StartCoroutine(WaitAndEnd());
                yield break; 
            }
        }
    }

    private void UpdateWaveUI()
    {
        if (waveText != null)
        {
            waveText.SetActive(true);
            TextMeshProUGUI tmp = waveText.GetComponent<TextMeshProUGUI>();
            if (tmp != null) tmp.text = "Wave " + currentWave.ToString();


            if (waveText.TryGetComponent<TextFadeAnimation>(out var anim))
            {
                anim.TriggerAnimation();
            }
        }
    }

    private List<EnemyData> LoadLevelData(int level)
    {
        return level switch
        {
            1 => LoadLevel1(),
            2 => LoadLevel2(),
            3 => LoadLevel3(),
            4 => LoadLevel4(),
            5 => LoadLevel5(),
            6 => LoadLevel6(),
            7 => LoadLevel7(),
            8 => LoadLevel8(),
            9 => LoadLevel9(),
            10 => LoadLevel10(),
            _ => new List<EnemyData>(),
        };
    }
    void SpawnEnemy(EnemyData enemyData)
    {
        if (enemyData != null)
        {
            GameObject newEnemy = Instantiate(enemyData.enemyPrefab, transform.position, Quaternion.identity);
            EnemyStats stats = newEnemy.GetComponent<EnemyStats>();

            stats.InitializeData(enemyData); // His stats are now as the random one
            var monsterLogic = newEnemy.GetComponent<Monster>();

            if (stats != null && (enemyPrefab.name.Contains("King") || stats.config.name.Contains("King"))) { 
                if(bossHealthUI != null)
                {
                    bossHealthUI.SetActive(true);
                    Slider slider = bossHealthUI.GetComponent<Slider>();
                    if (slider != null) {

                        slider.maxValue = stats.currentHealth;
                        slider.value = stats.currentHealth;

                        stats.OnHealthChanged += (currentHp) =>
                        {
                            slider.value = currentHp;
                        
                        };


                        stats.OnDeath += () => { bossHealthUI.SetActive(false); };

                    }

                } 
            }


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
    private void AddEnemies(List<EnemyData> list, EnemyData enemyType, int count)
    {
        for (int i = 0; i < count; i++)
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
    private List<EnemyData> LoadLevel8()
    {
        List<EnemyData> list = new List<EnemyData>();

        switch (currentWave)
        {
            case 1: // 15 Skeletons + 10 Mages
                timeBetweenEnemies = 0.8f;
                list = FillWave(availableEnemies[12], 15); // Skeleton
                AddEnemies(list, availableEnemies[10], 10); // Mage
                break;

            case 2: // 15 Wargs + 5 Bandit Leaders
                timeBetweenEnemies = 0.6f;
                list = FillWave(availableEnemies[14], 15); // Warg
                AddEnemies(list, availableEnemies[6], 5); // Bandit Leader
                break;

            case 3: // 12 Witches + 15 Mech Spiders
                timeBetweenEnemies = 0.8f;
                list = FillWave(availableEnemies[15], 12); // Witch
                AddEnemies(list, availableEnemies[11], 15); // Mech Spider
                break;

            case 4: // 2 Warriors + 20 Goblins
                timeBetweenEnemies = 1.0f;
                list = FillWave(availableEnemies[13], 2); // Warrior 
                AddEnemies(list, availableEnemies[0], 20); // Goblin
                break;

            case 5: // 4 Cyclops + 6 Trolls
                timeBetweenEnemies = 1.2f;
                list = FillWave(availableEnemies[17], 4); // Cyclops  
                AddEnemies(list, availableEnemies[3], 6); // Troll
                break;

            case 6: // 10 Witches + 10 Mages + 10 Bandit Elders
                timeBetweenEnemies = 0.7f;
                list = FillWave(availableEnemies[15], 10); // Witch
                AddEnemies(list, availableEnemies[10], 10); // Mage
                AddEnemies(list, availableEnemies[5], 10); // Bandit Elder
                break;

            case 7: // 3 Warriors + 8 Bandit Leaders + 5 Cyclops
                timeBetweenEnemies = 1.0f;
                list = FillWave(availableEnemies[13], 3); // Warrior
                AddEnemies(list, availableEnemies[6], 8); // Bandit Leader
                AddEnemies(list, availableEnemies[17], 5); // Cyclops
                break;

            default:
                list = new List<EnemyData>();
                break;
        }

        return list;
    }

    private List<EnemyData> LoadLevel9()
    {
        List<EnemyData> list = new List<EnemyData>();

        switch (currentWave)
        {
            case 1:
                // Wave 1: 15 Mages + 10 Skeletons
                timeBetweenEnemies = 1.2f;
                list = FillWave(availableEnemies[10], 15);   // Mage (Index 10)
                AddEnemies(list, availableEnemies[12], 10);  // Skeleton (Index 12)
                break;

            case 2:
                // Wave 2: 20 Wargs + 5 Trolls
                timeBetweenEnemies = 0.6f;
                list = FillWave(availableEnemies[14], 20);   // Warg (Index 14)
                AddEnemies(list, availableEnemies[3], 5);    // Troll (Index 3)
                break;

            case 3:
                // Wave 3: 15 Mech Spiders + 10 Witches
                timeBetweenEnemies = 0.7f;
                list = FillWave(availableEnemies[11], 15);   // Mech Spider (Index 11)
                AddEnemies(list, availableEnemies[15], 10);  // Witch (Index 15)
                break;

            case 4:
                // Wave 4: 4 Cyclops + 6 Bandit Leaders
                timeBetweenEnemies = 1.2f;
                list = FillWave(availableEnemies[17], 4);     // Cyclops (Index 17)
                AddEnemies(list, availableEnemies[6], 6);    // Bandit Leader (Index 6)
                break;

            case 5:
                // Wave 5: 25 Goblins + 15 Bandit Elders
                timeBetweenEnemies = 0.5f;
                list = FillWave(availableEnemies[0], 25);    // Goblin (Index 0)
                AddEnemies(list, availableEnemies[5], 15);   // Bandit Elder (Index 5)
                break;

            case 6:
                // Wave 6: 15 Witches + 10 Mages
                timeBetweenEnemies = 0.7f;
                list = FillWave(availableEnemies[15], 7);   // Witch (Index 15)
                AddEnemies(list, availableEnemies[10], 10);  // Mage (Index 10)
                AddEnemies(list, availableEnemies[15], 8);
                break;

            case 7:
                // Wave 7: 4 Warriors + 10 Enemy Archers
                timeBetweenEnemies = 1.0f;
                list = FillWave(availableEnemies[13], 4);    // Warrior (Index 13)
                AddEnemies(list, availableEnemies[8], 10);   // Enemy Archer (Index 8)
                break;

            case 8:
                // Wave 8: 5 Warriors + 8 Cyclops + 10 Bandit Leaders
                list = FillWave(availableEnemies[13], 5);    // Warrior (Index 13)
                AddEnemies(list, availableEnemies[17], 8);     // Cyclops (Index 17)
                AddEnemies(list, availableEnemies[6], 10);   // Bandit Leader (Index 6)
                break;

            default:
                list = new List<EnemyData>();
                break;
        }

        return list;
    }
    private List<EnemyData> LoadLevel10()
    {
        List<EnemyData> list = new List<EnemyData>();

        switch (currentWave)
        {
            case 1:
                // Wave 1: 3 Wargs -> 3 Mages -> 3 Wargs -> 3 Mages (repetat de 2 ori)
                timeBetweenEnemies = 1.0f;
                for (int i = 0; i < 2; i++)
                {
                    AddEnemies(list, availableEnemies[14], 3); // Warg
                    AddEnemies(list, availableEnemies[10], 3); // Mage
                }
                break;

            case 2:
                // Wave 2: 1 Troll -> 5 Skeletons (repetat de 4 ori, total: 4 Trolls + 20 Skeletons)
                timeBetweenEnemies = 0.8f;
                for (int i = 0; i < 4; i++)
                {
                    AddEnemies(list, availableEnemies[3], 1);  // Troll
                    AddEnemies(list, availableEnemies[12], 5); // Skeleton
                }
                break;

            case 3:
                // Wave 3: 3 Bandit Rangers -> 2 Bandit Elders -> 1 Bandit Leader (repetat de 4 ori) + 3 Rangers la final
                timeBetweenEnemies = 0.9f;
                for (int i = 0; i < 4; i++)
                {
                    AddEnemies(list, availableEnemies[4], 3); // Bandit Ranger
                    AddEnemies(list, availableEnemies[5], 2); // Bandit Elder
                    AddEnemies(list, availableEnemies[6], 1); // Bandit Leader
                }
                AddEnemies(list, availableEnemies[4], 3); // Cei 3 Rangers rămași
                break;

            case 4:
                // Wave 4: 3 Mech Spiders -> 2 Witches (repetat de 5 ori) + 2 Mech Spiders
                timeBetweenEnemies = 0.7f;
                for (int i = 0; i < 5; i++)
                {
                    AddEnemies(list, availableEnemies[11], 3); // Mech Spider
                    AddEnemies(list, availableEnemies[15], 2); // Witch
                }
                AddEnemies(list, availableEnemies[11], 2);
                break;

            case 5:
                // Wave 5: Spărgătorul de economie (4 Warriors, 8 Cyclops) intercalat
                timeBetweenEnemies = 1.2f;
                for (int i = 0; i < 4; i++)
                {
                    AddEnemies(list, availableEnemies[13], 1); // Warrior
                    AddEnemies(list, availableEnemies[17], 2); // Cyclops
                }
                break;

            case 6:
                // Wave 6: 5 Wargs -> 3 Witches -> 5 Wargs -> 3 Witches (repetat de 5 ori)
                timeBetweenEnemies = 0.5f;
                for (int i = 0; i < 5; i++)
                {
                    AddEnemies(list, availableEnemies[14], 5); // Warg
                    AddEnemies(list, availableEnemies[15], 3); // Witch
                }
                break;

            case 7:
                // Wave 7: 2 Warriors -> 4 Mages -> 4 Enemy Archers (repetat de 3 ori)
                timeBetweenEnemies = 0.8f;
                for (int i = 0; i < 3; i++)
                {
                    AddEnemies(list, availableEnemies[13], 2); // Warrior
                    AddEnemies(list, availableEnemies[10], 4); // Mage
                    AddEnemies(list, availableEnemies[8], 4);  // Enemy Archer
                }
                break;

            case 8:
                // Wave 8: 2 Bandit Leaders -> 2 Cyclops -> 1 Troll (repetat de 4 ori)
                timeBetweenEnemies = 1.0f;
                for (int i = 0; i < 4; i++)
                {
                    AddEnemies(list, availableEnemies[6], 2);  // Bandit Leader
                    AddEnemies(list, availableEnemies[17], 2); // Cyclops
                    AddEnemies(list, availableEnemies[3], 1);  // Troll
                }
                break;

            case 9:
                // Wave 9: 1 Warrior -> 2 Witches -> 3 Mech Spiders (repetat de 5 ori)
                timeBetweenEnemies = 0.7f;
                for (int i = 0; i < 5; i++)
                {
                    AddEnemies(list, availableEnemies[19], 1); // Warrior
                    AddEnemies(list, availableEnemies[15], 2); // Witch
                    AddEnemies(list, availableEnemies[11], 3); // Mech Spider
                }
                break;

            case 10:
                // Wave 10: REGELE ESTE TRIMIS ULTIMUL.
                // Întâi trimitem armata lui de elită intercalată pentru a distrage atenția turnurilor și a consuma cooldown-urile.
                //timeBetweenEnemies = 2f;
                //for (int i = 0; i < 4; i++)
                //{
                //    AddEnemies(list, availableEnemies[19], 1); // Warrior
                //    AddEnemies(list, availableEnemies[22], 1); // Cyclops
                //}

                AddEnemies(list, availableEnemies[9], 1);
                break;

            default:
                list = new List<EnemyData>();
                break;
        }

        return list;
    }
    public IEnumerator WaitAndEnd()
    {
     
        yield return new WaitForSecondsRealtime(2f);
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OpenWinLosePanel();
        }
        Debug.Log("Am oprit timpul");
        Time.timeScale = 0f;
    }
}
