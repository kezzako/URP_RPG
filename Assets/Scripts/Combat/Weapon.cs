using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController _animOverrController = null;
        [SerializeField] GameObject _weaponPrefab = null;
        [SerializeField] float _range = 1f;
        [SerializeField] float _damage = 5f;
        [SerializeField] bool _isRightHanded = true;

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            if (_weaponPrefab != null)
            {
                Instantiate(_weaponPrefab, (_isRightHanded ? rightHand : leftHand));

                if (_animOverrController != null)
                {
                    animator.runtimeAnimatorController = _animOverrController;
                }
            }
        }

        public float GetRange()
        {
            return _range;
        }

        public float GetDamage()
        {
            return _damage;
        }

    }
}
