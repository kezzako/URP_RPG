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

        public void Spawn(Transform handTransform, Animator animator)
        {
            if (_weaponPrefab != null && handTransform != null)
            {
                Instantiate(_weaponPrefab, handTransform);
             
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
