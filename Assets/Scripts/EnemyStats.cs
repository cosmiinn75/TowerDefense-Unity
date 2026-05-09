
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using Assets.FantasyTowerDefense.Scripts.Creature;
public class EnemyStats : MonoBehaviour
{
    public float currentHealth;
    public float currentSpeed;
    public EnemyData config; // Type of enemy
    private SpawnManager spawner;
    public bool reachedEnd = false;
    [Header("Resistances")]
    public bool hasArmor;
    public bool hasMagicResistance;
    public bool hasSlowResistance;
    public bool hasPoisonResistance;
    public bool hasStunResistance;
    [Header("Resistance UI")]
    public GameObject armorIcon;
    public GameObject magicIcon;
    public GameObject slowIcon;
    public GameObject poisonIcon;
    public GameObject stunIcon;


    private void Start()
    {
        spawner = FindFirstObjectByType<SpawnManager>();
        hasArmor = config.isArmored;
        hasMagicResistance = config.hasMagicResistance;
        hasSlowResistance = config.hasSlowResistance;
        hasPoisonResistance = config.hasPoisonResistance;
        hasStunResistance = config.hasStunResistance;

            if (armorIcon != null) armorIcon.SetActive(hasArmor);
            if (magicIcon != null) magicIcon.SetActive(hasMagicResistance);
            if (slowIcon != null) slowIcon.SetActive(hasSlowResistance);
            if (poisonIcon != null) poisonIcon.SetActive(hasPoisonResistance);
            if (stunIcon != null) stunIcon.SetActive(hasStunResistance);
        
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



            var king = DamageKingTower.Instance._kingMonster;
            if(king == null || king.State >= CreatureState.Dead)
            {
                return;
            }
            if (!reachedEnd)
            {
                
                CurrencyManager.Instance.AddGold(config.goldReward);
            }
            else
            {
                DamageKingTower.Instance.TakeDamage(20);
            }
        }

  

    }
    public void KillKingTower()
    {
        DamageKingTower.Instance.TakeDamage(100);
    }
}
