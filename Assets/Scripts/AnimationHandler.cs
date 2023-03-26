using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimationHandler : MonoBehaviour
{
    NavMeshAgent _navMeshAgent;
    Animator _animator;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    private void OnAnimatorMove()
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        _navMeshAgent.speed = (_animator.deltaPosition / Time.deltaTime).magnitude;

        if (!stateInfo.IsName("Locomotion") //&&
        //    !stateInfo.IsName("AttackRightPunch") &&
        //    !stateInfo.IsName("AttackLeftPunch") &&
        //    !stateInfo.IsName("AttackRightPunch2") &&
        //    !stateInfo.IsName("AttackLeftPunch2")
            )
        {
            _animator.ApplyBuiltinRootMotion();
        }
    }
}
