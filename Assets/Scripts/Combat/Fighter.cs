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
        Transform _combatTarget;
        Mover _mover;

        private void Awake()
        {
            _mover = GetComponent<Mover>();
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
        }
    }
}
