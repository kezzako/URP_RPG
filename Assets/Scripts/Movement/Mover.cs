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

        float _navSpeed = 0;

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
            if (!_navMeshAgent.isOnNavMesh) return;

            float forwardSpeed = 0;

            float distanceToDest = _navMeshAgent.remainingDistance;

            if (distanceToDest < 0.05) forwardSpeed = 0f;
            else forwardSpeed = _navSpeed;
         
            _animator.SetFloat("forwardSpeed", forwardSpeed);

            //Vector3 velocity = _navMeshAgent.velocity;
            //Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            //_animator.SetFloat("forwardSpeed", localVelocity.z);
        }

        private void OnAnimatorMove()
        {
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            _navMeshAgent.speed = (_animator.deltaPosition / Time.deltaTime).magnitude;
            if (!stateInfo.IsName("Locomotion"))
            {
                _animator.ApplyBuiltinRootMotion();
            }
        }

        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination);
        }

        public void Cancel()
        {
            _navMeshAgent.isStopped = true;
            _navMeshAgent.destination = transform.position;
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
            //_navMeshAgent.speed = speed;
            _navSpeed = speed;
            //_animator.SetFloat("forwardSpeed", speed);
        }

        public struct moverSaveData
        {
            public float[] position;
            public float[] quarternions;
        }

        public JToken CaptureAsJToken()
        {
            moverSaveData saveData = new()
            {
                position = new float[3],
                quarternions = new float[4]
            };

            saveData.position[0] = transform.position.x;
            saveData.position[1] = transform.position.y;
            saveData.position[2] = transform.position.z;

            saveData.quarternions[0] = transform.rotation.x;
            saveData.quarternions[1] = transform.rotation.y;
            saveData.quarternions[2] = transform.rotation.z;
            saveData.quarternions[3] = transform.rotation.w;

            return JToken.FromObject(saveData);
            //Debug.Log("Transform: " + transform.position);
            //return JToken.FromObject(new SerializableVector3(transform.position));
        }

        public void RestoreFromJToken(JToken state)
        {
            _navMeshAgent.enabled = false;

            moverSaveData saveData = new()
            {
                position = new float[3],
                quarternions = new float[4]
            };

            saveData = state.ToObject<moverSaveData>();

            transform.position = new Vector3(
                saveData.position[0], //x
                saveData.position[1], //y
                saveData.position[2]  //z
                );

            transform.rotation = new Quaternion(
                saveData.quarternions[0], //x
                saveData.quarternions[1], //y
                saveData.quarternions[2], //z
                saveData.quarternions[3]  //w
                );

            _navMeshAgent.enabled = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
            //this.gameObject.transform = state.ToObject<Transform>();
        }
    }

}