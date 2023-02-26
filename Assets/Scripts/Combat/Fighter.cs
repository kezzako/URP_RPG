using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IActions
    {
        [SerializeField] float _weaponRange = 2f;
        float _timeBetweenAttackAnimCycles = 0.5f;
        float _lastAttackTimeStamp = 0;

        Transform _combatTarget;
        Mover _mover;
        Animator _animator;

        private void Awake()
        {
            _mover = GetComponent<Mover>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (_combatTarget == null) return;

            if (_combatTarget != null && !GetIsInRange())
            {
                _mover.MoveTo(_combatTarget.position);
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
                //start attack animation
                _animator.SetTrigger("attack");
                //rotate around Y axis to look at the target
                transform.LookAt(new Vector3(_combatTarget.position.x, transform.position.y, _combatTarget.position.z));
            }
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, _combatTarget.position) < _weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            _combatTarget = combatTarget.transform;
        }

        public void Cancel()
        {
            _combatTarget = null;
            //stop attack animation
            _animator.ResetTrigger("attack");
        }

        //Player animation event
        void Hit()
        {

        }
    }
}
