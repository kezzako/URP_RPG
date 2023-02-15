using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using RPG.Movement;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Mover _mover;
        bool _wantsToRun = false;

        private void Awake()
        {
            _mover = GetComponent<Mover>();
        }

        private void Update()
        {
            if (_wantsToRun)
                _mover.MoveToCursor();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
;            if (context.started)
                _wantsToRun = true;
            else if (context.performed)
                _wantsToRun = true;
            else if (context.canceled)
                _wantsToRun = false;
        }

    }

}
