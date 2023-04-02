using RPG.Core;
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
        [SerializeField] Projectile _projectile = null;

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

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target)
        {
            Vector3 spawnPos = (_isRightHanded ? rightHand : leftHand).position;
            Quaternion spawnRot = (_isRightHanded ? rightHand : leftHand).rotation;
            Projectile projectileInstance = Instantiate(_projectile, spawnPos, spawnRot);

            projectileInstance.SetDamage(_damage);
            projectileInstance.SetTarget(target);
        }

        public bool HasProjectile()
        {
            return _projectile != null;
        }

    }
}
