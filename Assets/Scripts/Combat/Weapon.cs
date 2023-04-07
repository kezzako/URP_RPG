using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

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

        ObjectPool<Projectile> _projectilePool;
        [SerializeField] int _maxPoolSize = 10;

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            if (_weaponPrefab != null)
            {
                DestroyOldWeapon(rightHand, leftHand);

                GameObject weapon = Instantiate(_weaponPrefab, (_isRightHanded ? rightHand : leftHand));

                if (_animOverrController != null)
                {
                    animator.runtimeAnimatorController = _animOverrController;
                }
            }

            if (HasProjectile())
            {
                _projectilePool = new ObjectPool<Projectile>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, false, 10, _maxPoolSize);
            }
        }

        //requires the weapon to be the 4th child of the hand, so pretty bad. To change!!
        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            GameObject oldWeapon = null;

            //check for weapon in right hand
            if (rightHand.childCount > 3)
            {
                oldWeapon = rightHand.GetChild(3).gameObject;
            }

            //check for weapon in left hand
            if (leftHand.childCount > 3 && oldWeapon == null)
            {
                oldWeapon = leftHand.GetChild(3).gameObject;
            }
            if (oldWeapon == null) return;

            Destroy(oldWeapon);
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
            //Projectile projectileInstance = Instantiate(_projectile, spawnPos, spawnRot);
            Projectile projectileInstance = _projectilePool.Get();

            projectileInstance.transform.position = spawnPos;
            
            projectileInstance.SetDamage(_damage);

            //give the info of who the arrow is supposed to hit/follow.
            //if it's a homing arrow that follows the target, we need target info
            projectileInstance.SetTarget(target);

            //set the orientation of the arrow to be towards the target
            projectileInstance.transform.LookAt(projectileInstance.GetAimLocation());

            //give pool reference so the arrow can return itself to the pool
            projectileInstance.SetPool(_projectilePool);

            projectileInstance.CollisionEvent += HandleProjectileTargetCollision;
        }

        public bool HasProjectile()
        {
            return _projectile != null;
        }

        void HandleProjectileTargetCollision(Projectile projectile)
        {
            projectile.CollisionEvent -= HandleProjectileTargetCollision;
            _projectilePool.Release(projectile);
        }

        Projectile CreatePooledItem()
        {
            return Instantiate(_projectile);
        }

        // Called when an item is returned to the pool using Release
        void OnReturnedToPool(Projectile projectile)
        {
            Debug.Log("returned to pool");
            projectile.gameObject.SetActive(false);
        }

        // Called when an item is taken from the pool using Get
        void OnTakeFromPool(Projectile projectile)
        {
            projectile.gameObject.SetActive(true);
        }

        // If the pool capacity is reached then any items returned will be destroyed.
        // We can control what the destroy behavior does, here we destroy the GameObject.
        void OnDestroyPoolObject(Projectile projectile)
        {
            Destroy(projectile.gameObject);
        }
    }
}
