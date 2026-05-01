using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.FantasyTowerDefense.Scripts.Common.Tween;
using Assets.FantasyTowerDefense.Scripts.Creature;
using Assets.FantasyTowerDefense.Scripts.Fx;
using UnityEngine;
using Random = UnityEngine.Random;
namespace Assets.FantasyTowerDefense.Scripts.Demo
{
    public class Monster : MonoBehaviour
    {
        public string Id;

        public GameObject Front;
        public GameObject Back;
        public Animator Animator;

        public Transform Center;
        public Transform[] FrontWeapons;
        public Transform[] BackWeapons;
        public Transform Hud;
        public SpriteRenderer HealthBar;

        public int Health;
        public int Damage;
        public int Speed;


        public CreatureState State { get; private set; } = CreatureState.Run;

        public int _health;
        private int _damage;
        private int _speed;

        private List<Transform> _checkpoints;
        private int _checkpoint;
        private float _offset;

        private static readonly int IsFrontHash = Animator.StringToHash("IsFront");
        private static readonly int StateHash = Animator.StringToHash("State");

        public static List<Monster> Instances = new();

        public void Initialize(List<Transform> checkpoints)
        {
            _checkpoints = checkpoints;
            _checkpoint = 0;
            _offset = Random.Range(-0.75f, 0.75f);

            if (!gameObject.CompareTag("isKingTower"))
            {
                transform.position = checkpoints[_checkpoint].position + new Vector3(_offset, _offset);
            }
        }

        public void Awake()
        {
            Instances.Add(this);
        }

        public void Start()
        {
            _health = Health;
            _damage = Damage;
            _speed = Speed;
        }

        public void OnDestroy()
        {
            Instances.Remove(this);
        }

        private Vector2 _direction;


        public void Update()
        {

            if (!gameObject.CompareTag("isKingTower"))
            {
                var target = _checkpoints[_checkpoint];

                var targetPosition = target.transform.position + new Vector3(_offset, _offset);


                if (Vector2.Distance(transform.position, targetPosition) < 0.1)
                {
                    transform.position = targetPosition;

                    _checkpoint++;

                    if (_checkpoint == _checkpoints.Count)
                    {

                        var stats = GetComponent<EnemyStats>();
                        stats.reachedEnd = true;

                        SpawnManager spawner = FindFirstObjectByType<SpawnManager>();
                        if(spawner != null && spawner.currentWave >= 5)
                        {
                            if(DamageKingTower.Instance != null)
                            {
                                DamageKingTower.Instance.TakeDamage(10000);
                            }
                        }
                        Destroy(gameObject);
                        return;
                    }

                    target = _checkpoints[_checkpoint];
                    targetPosition = target.transform.position + new Vector3(_offset, _offset);
                }

                var direction = targetPosition - transform.position;

                if (direction.x != 0) _direction.x = direction.x;
                if (direction.y != 0) _direction.y = direction.y;

                RotateTo(direction.x == 0 ? _direction : direction);
                transform.position += _speed * Time.deltaTime * direction.normalized;
                Animator.SetInteger(StateHash, (int)State);
            }
        }

        public void RotateTo(Vector2 direction)
        {
            var scale = transform.localScale;

            scale.x = Mathf.Sign(direction.x) * Mathf.Abs(scale.x);

            Front.transform.localScale = scale;
            Back.transform.localScale = scale;

            Front.SetActive(direction.y < 0 || direction.y == 0 && direction.x != 0);
            Back.SetActive(!Front.activeSelf);

            scale = Hud.localScale;
            scale.x = Mathf.Sign(direction.x);

            Hud.localScale = scale;

            Animator.SetBool(IsFrontHash, Front.activeSelf);
        }

        public void GetDamage(int damage)
        {
            if (_health == 0) return;

            _health = Mathf.Clamp(_health - damage, 0, Health);

            HealthBar.size = new Vector2((float)_health / Health, 0.2f);

            Hit();

            if (_health == 0)
            {
                Die();
            }
        }

        public void Hit()
        {
            StartCoroutine(nameof(Blink));
            GetComponent<ScaleSpring>().enabled = true;
        }

        public void Die()
        {
            enabled = false;
            State = CreatureState.Dead;
            Animator.SetInteger(StateHash, (int)State);
            Hud.gameObject.SetActive(false);
            StartCoroutine(nameof(Destroy));
        }

        private static Material _baseMaterial;
        private static Material _blinkMaterial;

        private IEnumerator Blink()
        {
            _baseMaterial ??= new Material(Shader.Find("Sprites/Default"));
            _blinkMaterial ??= new Material(Shader.Find("GUI/Text Shader"));

            var renderers = GetComponentsInChildren<SpriteRenderer>();

            foreach (var r in renderers)
            {
                r.material = _blinkMaterial;
            }

            yield return new WaitForSeconds(0.1f);

            foreach (var r in renderers)
            {
                r.material = _baseMaterial;
            }
        }

        private IEnumerator Destroy()
        {
            yield return new WaitForSeconds(0.2f);

            FxManager.Instance.CreateDeath(Center);

            var spriteRenderers = GetComponentsInChildren<SpriteRenderer>().ToList();
            var opacity = spriteRenderers.Select(i => i.color.a).ToList();

            for (var i = 0; i < 10; i++)
            {
                for (var j = 0; j < spriteRenderers.Count; j++)
                {
                    var spriteRenderer = spriteRenderers[j];
                    var color = spriteRenderer.color;

                    color.a -= opacity[j] / 10;
                    spriteRenderer.color = color;
                }

                yield return new WaitForSeconds(0.025f);
            }

            yield return new WaitForSeconds(0.5f);

            Destroy(gameObject);
        }



        public void SetStartingHealth(int newHealth)
        {
            Health = newHealth;
            _health = newHealth;


            if (HealthBar != null)
            {
                HealthBar.size = new Vector2(1f, 0.2f);
            }
        }
    }
}
