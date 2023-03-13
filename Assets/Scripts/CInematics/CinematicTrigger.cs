using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        private bool _hasPlayed = false;

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player") && !_hasPlayed)
            {
                GetComponent<PlayableDirector>().Play();
                _hasPlayed = true;
            }
        }
    }
}

