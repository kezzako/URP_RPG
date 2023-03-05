using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using RPG.Core;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IActions
    {
        NavMeshAgent _navMeshAgent;
        Animator _animator;
        Health _health;

        //bool _wantsToRun = false;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _health = GetComponent<Health>();
        }

        private void Update()
        {
            //disable nav mesh agent if we are dead
            _navMeshAgent.enabled = !_health.IsDead();

            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = _navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            _animator.SetFloat("forwardSpeed", localVelocity.z);
        }

        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination);
        }

        public void Cancel()
        {
            _navMeshAgent.isStopped = true;
        }

        //Do not call this directly from other classes! Use StartMoveAction() instead!
        public void MoveTo(Vector3 destination)
        {
            _navMeshAgent.destination = destination;
            _navMeshAgent.isStopped = false;
        }

    }

}