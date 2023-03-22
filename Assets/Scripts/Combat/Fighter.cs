using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IActions
    {
        [SerializeField] GameObject _weaponPrefab = null;
        [SerializeField] Transform _handTransform = null;

        float _weaponRange = 1f;
        float _weaponDamage = 5f;

        float _timeBetweenAttackAnimCycles = 0.5f;
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

        private void Update()
        {
            //do not do fighter actions if no target or if target is dead
            if (_combatTarget == null) return;

            if (_combatTarget.IsDead())
            {
                CancelAttackAnimations();
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
            if(combatTarget == null || combatTarget.GetComponent<Health>().IsDead()) return false;
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
            if(_combatTarget == null) return;

            _combatTarget.takeDamage(_weaponDamage);
        }

        //animation event
        void PunchEnd()
        {
            _isDoingAttackAnimation = false;
        }

        private bool IsInRange()
        {
            return Vector3.Distance(transform.position, _combatTarget.transform.position) < _weaponRange;
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


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _weaponRange);
        }
    }
}
