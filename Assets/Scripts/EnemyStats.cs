
using JetBrains.Annotations;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public float currentHealth;
    public float currentSpeed;
    public EnemyData config; // Type of enemy
    private SpawnManager spawner;
    public bool reachedEnd = false;
    private void Start()
    {
        spawner = FindFirstObjectByType<SpawnManager>();
    }
    public void InitializeData(EnemyData data)
    {
        config = data;
        currentHealth = data.health;
        currentSpeed = data.speed;

    }
    private void OnDestroy()
    {
        if (spawner != null && gameObject.scene.isLoaded)
        {
            spawner.EnemyDied();

            if (!reachedEnd)
            {
                CurrencyManager.Instance.AddGold((int)config.goldReward);
                Debug.Log("Am adaugat bani");
            }
            else
            {
                DamageKingTower.Instance.TakeDamage(20);
                Debug.Log("-hp pentru rege");
            }
        }

  

    }
    public void KillKingTower()
    {
        DamageKingTower.Instance.TakeDamage(100);
    }
}
