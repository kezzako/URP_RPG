using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;

namespace RPG.Control
{

    public class AIController : MonoBehaviour
    {
        [SerializeField] float _chaseDistance = 3f;
        GameObject _player;
        Fighter _fighter;
        Health _health;
        Mover _mover;
        ActionScheduler _actionScheduler;

        Vector3 guardPosition; //the position we are guarding. Always end up returning here
        float lastSawPlayerTimestamp = 0;
        [SerializeField] float _suspicionTime = 3f;

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

            guardPosition = transform.position;
        }

        private void Update()
        {
            if (_health.IsDead()) return;

            if (InAttackRangeOfPlayer(_chaseDistance) && _fighter.CanAttack(_player))
            {
                lastSawPlayerTimestamp = Time.time;
                AttackBehavior();
            }
            //if suspicionTime has not elapsed yet, we stand there
            else if ((Time.time - lastSawPlayerTimestamp) < _suspicionTime)
            {
                SuspicionBehavior();
            }
            //if suspicionTime has elapsed, go back to guardPosition
            else
            {
                _mover.StartMoveAction(guardPosition); //will also cancel the fighter action
            }
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