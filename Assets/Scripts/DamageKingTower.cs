using Assets.FantasyTowerDefense.Scripts.Creature;
using Assets.FantasyTowerDefense.Scripts.Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageKingTower : MonoBehaviour
{
    public static DamageKingTower Instance;
    private Monster _kingMonster;

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
            StartCoroutine(Wait());
            Time.timeScale = 0;
            Debug.Log("GAME OVER");
        }
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
    }
}