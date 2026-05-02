using System;
using System.Linq;
using Assets.FantasyTowerDefense.Scripts.Creature;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.FantasyTowerDefense.Scripts.Demo
{
    public abstract class Tower : MonoBehaviour
    {
        public Transform Source;

        [Header("Params")]
        public int Damage;
        public float Range;
        public float FireInterval;
        public float ReloadTime;
        public float Cost;

        [Header("Prefabs")]
        public Projectile ProjectilePrefab;
        private float _fireTime;
        protected State _state;

        protected enum State
        {
            Ready,
            Empty,
            Loaded
        }
        private Monster _target;
        protected abstract void Rotate(Transform target);

        protected abstract void Fire(Action fire);

        protected abstract void Reload();

        public void Update()
        {
        
            
                if (_target != null)
                {

                    if (!_target.enabled || _target.State >= CreatureState.Dead)
                    {
                        _target = null;

                    }
                }

                if (_target == null)
                {
                    _target = Monster.Instances.Where(i => i.enabled && i.State < CreatureState.Dead).OrderBy(i => Vector2.Distance(i.transform.position, transform.position)).FirstOrDefault();
                if (_target.CompareTag("isKingTower")){
                    _target = null;
                }

                }

                if (_target != null && Vector2.Distance(_target.transform.position, transform.position) <= Range)
                {
                    Rotate(_target.transform);

                    if (_state == State.Ready)
                    {
                        Fire(_target);
                    }
                }


                if (_state == State.Empty && Time.time - _fireTime > ReloadTime)
                {
                    Reload();
                    _state = State.Loaded;
                }

                if (_state == State.Loaded && Time.time - _fireTime > FireInterval)
                {
                    _state = State.Ready;
                }



                if (_target != null && Vector2.Distance(_target.transform.position, transform.position) > Range)
                {
                    _target = null;
                }

            }
        

        private void Fire(Monster target)
        {
            _state = State.Empty;
            _fireTime = Time.time;

            Fire(() => Instantiate(ProjectilePrefab, Source.position, Quaternion.identity, transform).Initialize(this, target));
        }
    }


}