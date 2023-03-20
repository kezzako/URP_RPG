using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {

        enum DestinationIdentifier
        {
            A, B, C, D, E, F
        }

        [SerializeField] int _sceneToLoad = -1;
        [SerializeField] Transform _spawnPoint;
        [SerializeField] DestinationIdentifier _destinationPortal;
        [SerializeField] float _fadeOutTime = 1.5f;
        [SerializeField] float _fadeInTime = 0.75f;


        //SavingWrapper _wrapper;

        //private void Start()
        //{
        //    _wrapper = FindObjectOfType<SavingWrapper>();
        //    Debug.Log(_wrapper);
        //}
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Portal triggered");

            if(other.tag == "Player")
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            if(_sceneToLoad < 0)
            {
                Debug.LogError("\"Scene To Load\" is not set");
                yield break;
            }

            Debug.Log("Transition!!");

            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();
            Debug.Log("Fader: " + fader);
            yield return fader.FadeOut(_fadeOutTime);
            print("Save");
            SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
            DontDestroyOnLoad(wrapper);

            wrapper.Save();
            yield return SceneManager.LoadSceneAsync(_sceneToLoad);
            print("load");
            wrapper.Load();

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            yield return fader.FadeIn(_fadeInTime);

            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            //moving player with transform.position doesn't work well because navmesh is also updating it
            player.GetComponent<NavMeshAgent>().Warp(otherPortal._spawnPoint.position);

            //player.transform.position = otherPortal._spawnPoint.position;
            player.transform.rotation = otherPortal._spawnPoint.rotation;


        }

        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if (portal._destinationPortal != _destinationPortal) continue;

                return portal;
            }

            return null;
        }

        private void OnDestroy()
        {
            print("Portal destroyed");
        }
    }
}
