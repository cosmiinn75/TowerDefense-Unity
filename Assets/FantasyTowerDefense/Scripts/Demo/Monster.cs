using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.FantasyTowerDefense.Scripts.Common.Tween;
using Assets.FantasyTowerDefense.Scripts.Creature;
using Assets.FantasyTowerDefense.Scripts.Fx;
using TMPro;
using Unity.VisualScripting;
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
        private EnemyStats enemyStats;
        public int Health;
        public int Damage;
        public int Speed;

        [Header("Element Effect")]
        public Color poisonColor = new Color(0.3f, 0.8f, 0.2f); //Green
        public Color iceColor = new Color(0.2f, 0.6f, 1.0f); //Blue
        public Color lightningColor = Color.white; //Stun color

        public enum ElementState { None , Poisoned , Frosted, Stunned};
        [HideInInspector] public ElementState currentElementalState = ElementState.None;
        private Coroutine activeEffectCoroutine;
        public CreatureState State { get; private set; } = CreatureState.Run;

        [Header("Stun Settings")]
        [HideInInspector] public bool wasStunned = false;
        public float stunCooldownDuration = 3.0f;


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
            enemyStats = GetComponent<EnemyStats>();
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


                if (Vector2.Distance(transform.position, targetPosition) < 0.3)
                {
                    transform.position = targetPosition;

                    _checkpoint++;

                    if (_checkpoint == _checkpoints.Count)
                    {

                        var stats = GetComponent<EnemyStats>();
                        stats.reachedEnd = true;

                        SpawnManager spawner = FindFirstObjectByType<SpawnManager>();
                        if(spawner != null && spawner.currentWave >= spawner.waveLevel)
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
            TriggerBlinkEffect();
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

        public void ApplyElementEffect(string elementType, float duration)
        {
            if (elementType == "Lightning" && wasStunned)
            {
                return;
            }

            if (activeEffectCoroutine != null)
            {
                StopCoroutine(activeEffectCoroutine);
                ResetMonsterStats();
            }
            switch (elementType)
            {
                case "Poison":
                    currentElementalState = ElementState.Poisoned;
                    activeEffectCoroutine = StartCoroutine(ApplyPoisonEffect(duration));
                    break;
                case "Ice":

                    currentElementalState = ElementState.Frosted;
                    activeEffectCoroutine = StartCoroutine(ApplyIceEffect(duration));
                    break;
                case "Lightning":
                    currentElementalState = ElementState.Stunned;
                    activeEffectCoroutine =StartCoroutine(ApplyStunEffect(duration));
                    break;
            }
        }

        private void ResetMonsterStats()
        {
          
            _speed = Speed;

            var renderers = GetComponentsInChildren<SpriteRenderer>();
            foreach (var r in renderers)
            {
                if (r.transform.IsChildOf(Hud) || r.gameObject.name == "HUD" || r.gameObject.CompareTag("isResist"))
                {
                    continue;
                }
                r.color = Color.white;
            }
        }

        private IEnumerator ApplyPoisonEffect(float duration)
        {
            var renderers = GetComponentsInChildren<SpriteRenderer>();

            foreach (var r in renderers)
            {
                if (r.transform.IsChildOf(Hud) || r.gameObject.name == "HUD" || r.gameObject.CompareTag("isResist"))
                {
                    continue;
                }
                r.color = poisonColor;
            }

            float damagePerTick = 50f;
            float interval = 1f;
            float elapsed = 0f;
            if(enemyStats.hasPoisonResistance)
            {
                Debug.Log("Poison res");
                damagePerTick = 20f;
            }
            while (elapsed < duration)
            {
             
                yield return new WaitForSeconds(interval);
                elapsed += interval;

              
                GetDamage((int)damagePerTick);
            }


            foreach (var r in renderers)
            {
                if (r.transform.IsChildOf(Hud) || r.gameObject.name == "HUD" || r.gameObject.CompareTag("isResist"))
                {
                    continue;
                }
                r.color = Color.white;
            }

            currentElementalState = ElementState.None;
            activeEffectCoroutine = null;
        }

        private IEnumerator ApplyIceEffect(float duration)
        {
            var renderers = GetComponentsInChildren<SpriteRenderer>();

            float modifiedSpeed = _speed;
            float originalSpeed = _speed;
            if (!enemyStats.hasSlowResistance)
            {
               modifiedSpeed = _speed * 0.6f;
                Debug.Log("slow res");
            }
            else
            {
                modifiedSpeed = _speed * 0.8f;
            }
            _speed = (int)modifiedSpeed;
            foreach (var r in renderers)
            {
                if (r.transform.IsChildOf(Hud) || r.gameObject.name == "HUD" || r.gameObject.CompareTag("isResist"))
                {
                    continue;
                }
                r.color = iceColor;
            }

            yield return new WaitForSeconds(duration);


            _speed = (int)originalSpeed;

            foreach (var r in renderers)
            {
                if (r.transform.IsChildOf(Hud) || r.gameObject.name == "HUD" || r.gameObject.CompareTag("isResist"))
                {
                    continue;
                }
                r.color = Color.white;
            }

            currentElementalState = ElementState.None;
            activeEffectCoroutine = null;
        }

        private IEnumerator ApplyStunEffect(float duration)
        {
            if (!enemyStats.hasStunResistance)
            {
                var renderers = GetComponentsInChildren<SpriteRenderer>();
                float tempSpeed = _speed;
                _speed = 0;
                wasStunned = true;

                float elapsed = 0f;
                while (elapsed < duration)
                {
                    foreach (var r in renderers)
                    {
                        if (r.transform.IsChildOf(Hud) || r.gameObject.name == "HUD" || r.gameObject.CompareTag("isResist"))
                        {
                            continue;
                        }

                        r.color = (elapsed % 0.2f < 0.1f) ? Color.yellow : new Color(0.8f, 0.9f, 1f);
                    }

                    elapsed += 0.1f;
                    yield return new WaitForSeconds(0.1f);
                }

                _speed = (int)tempSpeed;

                foreach (var r in renderers)
                {
                    if (r.transform.IsChildOf(Hud) || r.gameObject.name == "HUD" || r.gameObject.CompareTag("isResist"))
                    {
                        continue;
                    }
                    r.color = Color.white;
                }
            }
                currentElementalState = ElementState.None;
                activeEffectCoroutine = null;

                StartCoroutine(StunCooldownRoutine());
           
        }

        private IEnumerator StunCooldownRoutine()
        {
            yield return new WaitForSeconds(stunCooldownDuration);
            wasStunned = false;
        }
        private IEnumerator Blink()
        {
            _baseMaterial ??= new Material(Shader.Find("Sprites/Default"));
            _blinkMaterial ??= new Material(Shader.Find("GUI/Text Shader"));

            var renderers = GetComponentsInChildren<SpriteRenderer>();

            foreach (var r in renderers)
            {
                if (r.gameObject.CompareTag("isResist"))
                {
                    continue;
                }
                r.material = _blinkMaterial;
            }

            yield return new WaitForSeconds(0.1f);

            foreach (var r in renderers)
            {
                if (r.gameObject.CompareTag("isResist"))
                {
                    continue;
                }
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

        public void TriggerBlinkEffect()
        {
            if(currentElementalState == ElementState.None)
            {
                StartCoroutine(nameof(Blink));
            }
        }
    }
}
