using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;

namespace RPG.Control
{

    public class AIController : MonoBehaviour
    {
        [SerializeField] float _chaseDistance = 3f;
        [SerializeField] float _suspicionTime = 3f;
        [SerializeField] PatrolPath _patrolPath; //assign  in the editor the patrol path you want the guard to follow
        float _waypointTolerance = 0.5f; //the distance in which we are considered at the waypoint

        GameObject _player;
        Fighter _fighter;
        Health _health;
        Mover _mover;
        ActionScheduler _actionScheduler;

        Vector3 _guardPosition; //the position we are guarding. Always end up returning here
        float _lastSawPlayerTimestamp = 0;
        int _currentWaypointIndex = 0;

        private void Awake()
        {
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
            _mover = GetComponent<Mover>();
            _actionScheduler = GetComponent<ActionScheduler>();
        }

        private void Start()
        {
            _player = GameObject.FindWithTag("Player");

            _guardPosition = transform.position;
        }

        private void Update()
        {
            if (_health.IsDead()) return;

            if (InAttackRangeOfPlayer(_chaseDistance) && _fighter.CanAttack(_player))
            {
                _lastSawPlayerTimestamp = Time.time;
                AttackBehavior();
            }
            //if suspicionTime has not elapsed yet, we stand there
            else if ((Time.time - _lastSawPlayerTimestamp) < _suspicionTime)
            {
                SuspicionBehavior();
            }
            //if suspicionTime has elapsed
            else
            {
                PatrolBehavior();
                Debug.Log("patrol");
            }
        }

        private void PatrolBehavior()
        {
            Vector3 nextPosition = _guardPosition;

            if(_patrolPath != null)
            {
                if (AtWaypoint())
                {
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }

            _mover.StartMoveAction(nextPosition); //will also cancel the fighter action
        }

        private Vector3 GetCurrentWaypoint()
        {
            return _patrolPath.GetWaypoint(_currentWaypointIndex);
        }

        private void CycleWaypoint()
        {
            _currentWaypointIndex = _patrolPath.GetNextIndex(_currentWaypointIndex);
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            Debug.Log("At way point? " + distanceToWaypoint);
            Debug.Log("At way point? " + (distanceToWaypoint <= _waypointTolerance));

            return (distanceToWaypoint <= _waypointTolerance);
        }

        private void SuspicionBehavior()
        {
            //cancel current action, which means do nothing
            _actionScheduler.CancelCurrentAction();
        }

        private void AttackBehavior()
        {
            _fighter.Attack(_player);
        }

        //returns if distance between us and player is smaller than range
        private bool InAttackRangeOfPlayer(float range)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);
            return (distanceToPlayer < range);
        }

        //Called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _chaseDistance);
        }
    }

}