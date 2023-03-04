using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float _maxHealth;
        [SerializeField] float _currentHealth;
        Animator _animator;

        bool _isDead = false;

        public bool IsDead()
        {
            return _isDead;
        }

        private void Awake()
        {
            _currentHealth = _maxHealth;
            _animator = GetComponent<Animator>();

        }

        public void takeDamage(float damage)
        {
            _currentHealth -= damage;
            if(_currentHealth <= 0)
            {
                _currentHealth = 0;
                Die();
            }
            Debug.Log("Health: " + _currentHealth);
        }

        private void Die()
        {
            if (!_isDead) 
            { 
            _animator.SetTrigger("die");
            _isDead = true;   
            }
        }
    }
}