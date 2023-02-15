using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using RPG.Control;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour
    {
        NavMeshAgent _navMeshAgent;
        Animator _animator;

        //bool _wantsToRun = false;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            //if (_wantsToRun)
            //    MoveToCursor();

            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = _navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            _animator.SetFloat("forwardSpeed", localVelocity.z);
        }

        public void MoveToCursor()
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;
            bool hasHit = Physics.Raycast(ray, out hit);

            float distanceToMove = Vector3.Distance(hit.point, transform.position);

            //don't move if distance is too small because the animations become weird
            if (hasHit && distanceToMove > 0.5)
            {
                MoveTo(hit.point);
            }
        }

        public void MoveTo(Vector3 destination)
        {
            _navMeshAgent.destination = destination;
        }

        //public void OnMove(InputAction.CallbackContext context)
        //{
        //    if (context.started)
        //        _wantsToRun = true;
        //    else if (context.performed)
        //        _wantsToRun = true;
        //    else if (context.canceled)
        //        _wantsToRun = false;
        //}

    }

}