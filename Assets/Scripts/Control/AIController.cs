using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using GameDevTV.Saving;
using Newtonsoft.Json.Linq;

namespace RPG.Control
{

    public class AIController : MonoBehaviour, IJsonSaveable
    {
        [SerializeField] PatrolPath _patrolPath; //assign  in the editor the patrol path you want the guard to follow

        GameObject _player;
        Fighter _fighter;
        Health _health;
        Mover _mover;
        ActionScheduler _actionScheduler;
        
        Vector3 _guardPosition;             //the position we are guarding. Always end up returning here
        [SerializeField] float _chaseDistance = 3f;          //radius within which will chase player
        [SerializeField] float _chaseSpeed = 5f;             //speed at which will chase player
        [SerializeField] float _patrolSpeed = 3f;            //speed when we are not chasing player (patrol, suspicion)
        [SerializeField] float _suspicionTime = 3f;          //seconds we stop after player escapes chase
        float _lastSawPlayerTimestamp = float.MinValue;
        int _currentWaypointIndex = 0;      
        float _waypointTolerance = 0.5f;    //the distance in which we are considered at the waypoint
        float _waypointDwellTime = 3f;      //seconds we stop at every waypoint during patrol
        float _arrivedAtWaypointTimestamp = float.MinValue;

        float _timeSinceStart = 0; //use this to control time because when we load a scene we want out time reset too

        public struct saveables
        {
            public float lastSawPlayerTimestamp { get; set; }
            public float arrivedAtWaypointTimestamp { get; set; }
            public int currentWaypointIndex { get; set; }
        }

        saveables _saveables;

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
            _timeSinceStart += Time.deltaTime;

            if (_health.IsDead()) return;

            if (InAttackRangeOfPlayer() && _fighter.CanAttack(_player))
            {
                _lastSawPlayerTimestamp = _timeSinceStart;
                AttackBehavior();
            }
            //if suspicionTime has not elapsed yet, we stand there
            else if ((_timeSinceStart - _lastSawPlayerTimestamp) < _suspicionTime)
            {
                SuspicionBehavior();
            }
            //if suspicionTime has elapsed
            else
            {
                PatrolBehavior();
            }
        }

        private void PatrolBehavior()
        {
            Vector3 nextPosition = _guardPosition;
            _mover.SetNavSpeed(_patrolSpeed);
            if (_patrolPath != null)
            {
                //if at waypoint and waypoint dwelltime elapsed, set next waypoint
                if (AtWaypoint() && ((_timeSinceStart - _arrivedAtWaypointTimestamp) > _waypointDwellTime))
                {
                    CycleWaypoint();
                    _arrivedAtWaypointTimestamp = _timeSinceStart;
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

            return (distanceToWaypoint <= _waypointTolerance);
        }

        private void SuspicionBehavior()
        {
            _mover.SetNavSpeed(_patrolSpeed);
            //cancel current action, which means do nothing
            _actionScheduler.CancelCurrentAction();
        }

        private void AttackBehavior()
        {
            _mover.SetNavSpeed(_chaseSpeed);
            _fighter.Attack(_player);
        }

        //returns if distance between us and player is smaller than chaseDistance + weapon range
        private bool InAttackRangeOfPlayer()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);
            return (distanceToPlayer < _chaseDistance + _fighter.GetCurrentWeapon().GetRange());
        }

        //Called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            if (_fighter != null)
            {
                Gizmos.DrawWireSphere(transform.position, _chaseDistance + _fighter.GetCurrentWeapon().GetRange());
            }
        }

        public JToken CaptureAsJToken()
        {
            _saveables.currentWaypointIndex = _currentWaypointIndex;
            _saveables.arrivedAtWaypointTimestamp = _arrivedAtWaypointTimestamp;
            _saveables.lastSawPlayerTimestamp = _lastSawPlayerTimestamp;
            return JToken.FromObject(_saveables);
        }

        public void RestoreFromJToken(JToken state)
        {
            _saveables = state.ToObject<saveables>();

            _currentWaypointIndex = _saveables.currentWaypointIndex;
            _arrivedAtWaypointTimestamp = _saveables.arrivedAtWaypointTimestamp;
            _lastSawPlayerTimestamp = _saveables.lastSawPlayerTimestamp;
        }
    }

}