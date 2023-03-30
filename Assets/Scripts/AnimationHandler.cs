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
        //no need during pause
        if (Time.timeScale > 0)
        {
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            float navmeshSpeed = (_animator.deltaPosition / Time.deltaTime).magnitude;

            //on the 1st frame exiting pause navmeshSpeed returns NaN, resulting in errors
            if (!float.IsNaN(navmeshSpeed))
            {
                _navMeshAgent.speed = navmeshSpeed;
            }

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
}
