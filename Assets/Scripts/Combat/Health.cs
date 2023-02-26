using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        float _maxHealth = 100f;
        [SerializeField] float _currentHealth;

        private void Awake()
        {
            _currentHealth = _maxHealth;
        }

        public void takeDamage(float damage)
        {
            _currentHealth -= damage;
            if(_currentHealth < 0)
            {
                _currentHealth = 0;
                Die();
            }
            Debug.Log("Health: " + _currentHealth);
        }

        private void Die()
        {
            Debug.Log("Dead!!!");
        }
    }
}