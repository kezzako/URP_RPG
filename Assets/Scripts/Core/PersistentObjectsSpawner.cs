using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ROG.Core
{
    public class PersistentObjectsSpawner : MonoBehaviour
    {
        [SerializeField] GameObject[] _persistenObjectPrefabs;

        static bool _hasSpawned = false;

        private void Awake()
        {
            if (_hasSpawned) return;

            SpawnPersistenObjects();

            _hasSpawned = true;
        }

        private void SpawnPersistenObjects()
        {
            foreach(GameObject prefab in _persistenObjectPrefabs)
            {
                Instantiate(prefab);
                DontDestroyOnLoad(prefab);
            }
        }
    }
}


