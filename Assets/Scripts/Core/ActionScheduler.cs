using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{

    public class ActionScheduler : MonoBehaviour
    {

        IActions _currentAction;

        public void StartAction(IActions action)
        {
            if (_currentAction == action) return;
            if (_currentAction != null)
            {
                _currentAction.Cancel();
                Debug.Log("Cancelling " + _currentAction);
            }
            _currentAction = action;
        }

        public void CancelCurrentAction()
        {
            StartAction(null);
        }

    }
}