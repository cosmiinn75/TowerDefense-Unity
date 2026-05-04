
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.FantasyTowerDefense.Scripts.Demo
{
    public class Projectile : MonoBehaviour
    {
        public float Speed;
        public GameObject ExplosionPrefab;
        public AnimationCurve Trajectory = new(new Keyframe(0f, 0f, 0f, 0f), new Keyframe(1f, 0f, 0f, 0f));
        private Tower _source;
        private Monster _target;
        private float _damage;
        private float _createTime;
        [Header("Element Settings")]
        public string elementType;
        [Header("Resistances")] 
        public bool affectedByArmor;
        public bool affectedByMagic;
        public void Initialize(Tower tower, Monster target)
        {
            _createTime = Time.time;
            _source = tower;
            _target = target;
        }

        public void Update()
        {
            if (_target)
            {
                var direction = _target.Center.position - _source.Source.position;
                var timeNormalized = Speed * (Time.time - _createTime) / direction.magnitude;

                if (timeNormalized < 1)
                {
                    var height = Trajectory.Evaluate(timeNormalized);

                    transform.position = _source.Source.position + direction * timeNormalized + new Vector3(0, height, 0);
                    transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, _target.Center.position - transform.position));
                }
                else
                {
                    transform.position = _target.Center.position;
                    Bang();
                }
            }
            else
            {
                Bang();
            }
        }

        private void Bang()
        {
            enabled = false;

            foreach (var sr in GetComponentsInChildren<SpriteRenderer>())
            {
                sr.enabled = false;
            }

            foreach (var ps in GetComponentsInChildren<ParticleSystem>())
            {
                ps.Stop(true);
            }

            var explosion = Instantiate(ExplosionPrefab, transform.position, Quaternion.identity, transform);

            Destroy(gameObject, 1);
            
            if(_target?.GetComponent<Monster>() == null) {
                return;
            }
            var monster = _target?.GetComponent<Monster>();

            if (monster)
            {
                var enemyStats = monster.GetComponent<EnemyStats>();
                float finalDamage = _source.Damage;
                if (affectedByArmor && enemyStats.hasArmor)
                {
                     finalDamage = _source.Damage * 0.5f;
                    Debug.Log("Armor res");
              
                }
                else if(affectedByMagic && enemyStats.hasMagicResistance)
                {
                    finalDamage = _source.Damage * 0.5f;
                    Debug.Log("Magic res");
                }
                monster.GetDamage((int)finalDamage);
                if (!string.IsNullOrEmpty(elementType))
                {
                    if(elementType == "Ice")
                    {
                        monster.ApplyElementEffect("Ice", 3.0f);
                    }
                    else if(elementType == "Poison")
                    {
                        monster.ApplyElementEffect("Poison", 4.0f);
                    }
                    else if(elementType == "Lightning")
                    {
                        monster.ApplyElementEffect("Lightning", 1f);
                    }



                }
            }
        }
    }
}