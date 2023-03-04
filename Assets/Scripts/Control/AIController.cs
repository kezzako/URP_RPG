using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{

    public class AIController : MonoBehaviour
    {
        [SerializeField] float _chaseDistance = 5f;

        //assign Player in the inspector
        [SerializeField] GameObject _player;

        private void Update()
        {
            if (DistanceToPlayer() < _chaseDistance)
            {
                Debug.Log("Chase player" + gameObject.name);
            }
        }

        private float DistanceToPlayer()
        {
            return Vector3.Distance(transform.position, _player.transform.position);
        }
    }

}