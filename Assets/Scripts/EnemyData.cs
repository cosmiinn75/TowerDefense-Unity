using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData" , menuName ="EnemyData")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public float speed;
    public float health;
    public Sprite enemyIcon;
    public float goldReward;
    public Color enemyColor = Color.white;
}
