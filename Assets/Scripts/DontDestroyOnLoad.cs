using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    bool _isDontDestroyOnLoad = false;

    private void Awake()
    {
        if (!_isDontDestroyOnLoad)
        {
            DontDestroyOnLoad(this);
            _isDontDestroyOnLoad = true;
        }
    }
}
