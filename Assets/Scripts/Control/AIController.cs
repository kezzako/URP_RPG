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

        Vector3 guardPosition; //the position we are guarding. Return to this position after we are done moving

        private void Awake()
        {
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
            _mover = GetComponent<Mover>();
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
                _fighter.Attack(_player);
            }
            else
            {
                _mover.StartMoveAction(guardPosition); //will also cancel the fighter action
            }
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