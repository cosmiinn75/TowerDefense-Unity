using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData" , menuName ="EnemyData")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public float speed;
    public float health;
    public GameObject enemyPrefab;
    public int goldReward;
    public Color enemyColor = Color.white;
    public bool isArmored;
    public bool hasMagicResistance;
    public bool hasSlowResistance;
    public bool hasPoisonResistance;
    public bool hasStunResistance;
}
