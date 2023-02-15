using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using RPG.Movement;
using System;
using RPG.Combat;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Mover _mover;
        // bool _wantsToRun = false;

        private void Awake()
        {
            _mover = GetComponent<Mover>();
        }

        private void Update()
        {
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
                //exit loop if no CombatTarget exists
                if (target == null) continue;

                if (Mouse.current.leftButton.IsPressed())
                {
                    GetComponent<Fighter>().Attack();
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
                _mover.MoveTo(hit.point);
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
