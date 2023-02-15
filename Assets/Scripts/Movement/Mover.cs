using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

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
            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = _navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            _animator.SetFloat("forwardSpeed", localVelocity.z);
        }

        public void MoveTo(Vector3 destination)
        {
            _navMeshAgent.destination = destination;
        }

    }

}