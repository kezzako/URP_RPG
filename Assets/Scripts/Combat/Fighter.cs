using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour
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
                _mover.StopMoving();
            }
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, _combatTarget.position) < _weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            Debug.Log("attack!!");
            _combatTarget = combatTarget.transform;
        }

        public void Cancel()
        {
            _combatTarget = null;
        }
    }
}
