using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using RPG.Core;
using GameDevTV.Saving;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IActions, IJsonSaveable
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

            //Debug.Log("mover: " + _navMeshAgent.isStopped + " obj: " + gameObject.name);
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
            //Debug.Log("stop mover" + gameObject.name);
        }

        public void StartMover()
        {
            _navMeshAgent.isStopped = false;
        }

        //Do not call this directly from other classes! Use StartMoveAction() instead!
        public void MoveTo(Vector3 destination)
        {
            _navMeshAgent.destination = destination;
            _navMeshAgent.isStopped = false;
        }

        public void SetNavSpeed(float speed)
        {
            _navMeshAgent.speed = speed;
        }

        public JToken CaptureAsJToken()
        {
            var newArr = new float[3]
            {
                transform.position.x,
                transform.position.y,
                transform.position.z
            };

            return JToken.FromObject(newArr);
            //Debug.Log("Transform: " + transform.position);
            //return JToken.FromObject(new SerializableVector3(transform.position));
        }

        public void RestoreFromJToken(JToken state)
        {
            _navMeshAgent.enabled = false;

            var jsonArr = state.ToObject<float[]>();
            Vector3 vect3 = new Vector3(jsonArr[0], jsonArr[1], jsonArr[2]);
            transform.position = vect3;

            _navMeshAgent.enabled = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
            //this.gameObject.transform = state.ToObject<Transform>();
        }
    }

}