using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using RPG.Movement;
using System;
using RPG.Combat;
using RPG.Core;
using UnityEngine.EventSystems;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Mover _mover;
        Fighter _fighter;
        Health _health;

        float _runSpeed = 5.66f;

        private void Awake()
        {
            _mover = GetComponent<Mover>();
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
        }

        private void Update()
        {
            if (_health.IsDead()) return;

            if (DoCombat()) return;
            if (DoMovement()) return;
        }

        //returns true if hovering over a valid combat target
        public bool DoCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            
            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.gameObject.GetComponent<CombatTarget>();

                //skip iteration if no target
                if (target == null) continue;

                //skip loop iteration if not a valid target to attack
                if (!_fighter.CanAttack(target.gameObject)) continue;

                if (Mouse.current.leftButton.IsPressed())
                {
                    GetComponent<Fighter>().Attack(target.gameObject);
                }

                return true; //return true even if just hovering over a valid target
            }
            return false;
        }

        //returns true if hovering over a valid movement point
        public bool DoMovement()
        {
            bool hasHit = Physics.Raycast(GetMouseRay(), out RaycastHit hit);

            if (hasHit)
            {
                if (Mouse.current.leftButton.IsPressed())
                {
                    _mover.SetNavSpeed(_runSpeed);

                    _mover.StartMoveAction(hit.point);
                }
                return true;
            }

            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        }
    }

}
