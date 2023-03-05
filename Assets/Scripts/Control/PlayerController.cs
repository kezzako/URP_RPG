using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using RPG.Movement;
using System;
using RPG.Combat;
using RPG.Core;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Mover _mover;
        Fighter _fighter;
        Health _health;

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

                //Debug.Log(target);

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
                    _mover.StartMoveAction(hit.point);
                }
                return true;
            }
            //if no target or too close to the target
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        }

        //public void OnClick(InputAction.CallbackContext context)
        //{
        //    if (context.started)
        //    {
        //        _wantsToRun = true; //start moving
        //    }
        //    //enter here once the hold period expires
        //    else if (context.performed)
        //    {
        //        _wantsToRun = true; //keep moving on hold
        //    }
        //    else if (context.canceled)
        //    {
        //        _wantsToRun = false; //stop moving on button release
        //    }
        //}
    }

}
