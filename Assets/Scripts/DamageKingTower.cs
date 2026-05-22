using Assets.FantasyTowerDefense.Scripts.Creature;
using Assets.FantasyTowerDefense.Scripts.Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageKingTower : MonoBehaviour
{
    public static DamageKingTower Instance;
    public Monster _kingMonster;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        _kingMonster = GetComponent<Monster>();

        if (_kingMonster != null)
        {
            _kingMonster.Initialize(new List<Transform>());
            _kingMonster.enabled = true;
        }
    }

    public void TakeDamage(int healtLost)
    {
        if (_kingMonster == null) return;

        _kingMonster.GetDamage(healtLost);


        if (_kingMonster.State == CreatureState.Dead)
        {
            GameManager.Instance.gameOver = true;
            GameManager.Instance.win = false;

            SpawnManager spawner = FindFirstObjectByType<SpawnManager>();
            List<GameObject> enemiesLeft = new List<GameObject>(spawner.enemiesLeft);
            if (spawner != null)
            {
                enemiesLeft = spawner.enemiesLeft;
                spawner.StopAllCoroutines(); 
            }
            foreach(var enemy in enemiesLeft)
            {
                if(enemy != null)
                {
                    var script = enemy.GetComponent<Monster>();
                    if(script != null)
                    {
                        script.Die();
                    }
                }
            }
         
            if (spawner != null)
            {
   
                spawner.StartCoroutine(spawner.WaitAndEnd());
            }
        
            spawner.enemiesLeft.Clear();
           

        }
    }
 
}