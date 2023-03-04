using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IActions
    {
        float _weaponRange = 1f;
        float _weaponDamage = 5f;

        float _timeBetweenAttackAnimCycles = 0.5f;
        float _lastAttackTimeStamp = 0;

        Health _combatTarget;
        Mover _mover;
        Animator _animator;
        //Health _targetHealth;

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
                return;
            }
            if (_combatTarget != null && !IsInRange())
            {
                _mover.MoveTo(_combatTarget.transform.position);
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
                _lastAttackTimeStamp = Time.time;
                //start attack animation. Animation triggers Hit() event
                _animator.SetTrigger("attack");
                _animator.ResetTrigger("stopAttack");
                //rotate around Y axis to look at the target
                transform.LookAt(new Vector3(_combatTarget.transform.position.x, transform.position.y, _combatTarget.transform.position.z));

            }
        }

        public bool CanAttack(CombatTarget combatTarget)
        {
            //return false is target is null or dead
            if(combatTarget == null || combatTarget.GetComponent<Health>().IsDead()) return false;
            else return true;
        }

        //Player punch attack animation event
        void Hit()
        {
            if(_combatTarget == null) return;

            _combatTarget.takeDamage(_weaponDamage);  
        }

        private bool IsInRange()
        {
            return Vector3.Distance(transform.position, _combatTarget.transform.position) < _weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            _combatTarget = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            _combatTarget = null;
            //stop attack animation
            _animator.ResetTrigger("attack");
            _animator.SetTrigger("stopAttack");
        }

    }
}
