using Assets.FantasyTowerDefense.Scripts.Creature;
using Assets.FantasyTowerDefense.Scripts.Demo;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DamageKingTower : MonoBehaviour
{
    public static DamageKingTower Instance;
    public Monster _kingMonster;
    public event Action OnKingDeath;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
        }
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

        AudioManager.Instance?.PlaySFX(AudioManager.Instance.damageKingClip);

        if (_kingMonster.State == CreatureState.Dead)
        {
            GameManager.Instance.win = false;
            GameManager.Instance.gameOver = true;


            OnKingDeath?.Invoke();

            SpawnManager spawner = FindFirstObjectByType<SpawnManager>();
            spawner?.StartCoroutine(spawner.WaitAndEnd());
        }
    }

}