using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public float currentHealth;
    public float currentSpeed;
    public EnemyData config; // Type of enemy
    private SpawnManager spawner;
    private void Start()
    {
        spawner = FindFirstObjectByType<SpawnManager>(); 
    }
    public void InitializeData(EnemyData data)
    {
        config = data;
        currentHealth = data.health;
        currentSpeed = data.speed;
        GetComponent<SpriteRenderer>().color = data.enemyColor;
    }
    private void OnDestroy()
    {
        if(spawner != null  && gameObject.scene.isLoaded)
        {
            spawner.EnemyDied();
        }
    }

}
