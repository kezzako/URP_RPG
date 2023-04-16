using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using System;
using GameDevTV.Saving;
using Newtonsoft.Json.Linq;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IActions, IJsonSaveable
    {
        [SerializeField] Transform _rightHandTransform = null;
        [SerializeField] Transform _leftHandTransform = null;

        [SerializeField] Weapon _defaultWeapon = null;
        Weapon _currentWeapon = null;

        float _timeBetweenAttackAnimCycles = 1;
        float _lastAttackTimeStamp = float.MinValue;

        bool _isDoingAttackAnimation = false;

        Health _combatTarget;
        Mover _mover;
        Animator _animator;

        private void Awake()
        {
            _mover = GetComponent<Mover>();
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            if (_currentWeapon == null)
            {
                //only equip weapon if null, because the saving system might already have equipped one
                EquipWeapon(_defaultWeapon);
            }
        }

        private void Update()
        {
            //do not do fighter actions if no target or if target is dead
            if (_combatTarget == null) return;

            if (_combatTarget.IsDead())
            {
                CancelAttackAnimations();
                //when we kill the enemy we immediately return to locomotion with some speed
                //with this we return to idle animation
                _mover.Cancel();
                return;
            }
            if (!IsInRange())// && !_isDoingAttackAnimation)
            {
                CancelAttackAnimations();

                if (!_isDoingAttackAnimation)
                {
                    _mover.MoveTo(_combatTarget.transform.position);
                }
            }
            else
            {
                _mover.Cancel();
                AttackBehavior();
            }
        }

        private void AttackBehavior()
        {
            if (Time.time - _lastAttackTimeStamp > _timeBetweenAttackAnimCycles)
            {
                _isDoingAttackAnimation = true;

                _lastAttackTimeStamp = Time.time;
                //start attack animation. Animation triggers Hit() event
                _animator.SetTrigger("attack");
                _animator.ResetTrigger("stopAttack");
                //rotate around Y axis to look at the target
                transform.LookAt(new Vector3(_combatTarget.transform.position.x, transform.position.y, _combatTarget.transform.position.z));
            }
        }

        public bool CanAttack(GameObject combatTarget)
        {
            //return false is target is null or dead
            if (combatTarget == null || combatTarget.GetComponent<Health>().IsDead()) return false;
            else return true;
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            _combatTarget = combatTarget.GetComponent<Health>();
        }

        //Punch attack animation event
        void Hit()
        {
            if (_combatTarget == null) return;

            _combatTarget.takeDamage(_currentWeapon.GetDamage());
        }

        //animation event
        void PunchEnd()
        {
            _isDoingAttackAnimation = false;
        }

        //BowShotWithAnimEvent animation event
        void Shoot()
        {
            if (_combatTarget == null) return;
            if (_currentWeapon.HasProjectile())
            {
                _currentWeapon.LaunchProjectile(_rightHandTransform, _leftHandTransform, _combatTarget);
            }
        }

        public bool IsInRange()
        {
            return Vector3.Distance(transform.position, _combatTarget.transform.position) < _currentWeapon.GetRange();
        }

        public void EquipWeapon(Weapon weapon)
        {
            _currentWeapon = weapon;

            if (weapon != null && _rightHandTransform != null)
            {
                //Instantiate(_weaponPrefab, _rightHandTransform);
                weapon.Spawn(_rightHandTransform, _leftHandTransform, _animator);
            }
        }

        public void Cancel()
        {
            _combatTarget = null;
            _mover.Cancel();
            CancelAttackAnimations();
        }

        public void CancelAttackAnimations()
        {
            _isDoingAttackAnimation = false;
            _animator.ResetTrigger("attack");
            _animator.SetTrigger("stopAttack");
        }

        public Weapon GetCurrentWeapon()
        {
            return _currentWeapon;
        }

        public bool isTargetNotNull()
        {
            return (_combatTarget != null);
        }

        private void OnDrawGizmosSelected()
        {
            if (_currentWeapon != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(transform.position, _currentWeapon.GetRange());
            }
        }

        public JToken CaptureAsJToken()
        {
            return JToken.FromObject(_currentWeapon.name);
        }

        public void RestoreFromJToken(JToken state)
        {
            string weaponName = state.ToObject<string>();
            Weapon weapon = Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }
    }
}
