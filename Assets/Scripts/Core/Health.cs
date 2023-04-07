using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using GameDevTV.Saving;

namespace RPG.Core
{
    public class Health : MonoBehaviour, IJsonSaveable
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
        }

        private void Die()
        {
            if (!_isDead) 
            { 
                _animator.SetTrigger("die");
                _isDead = true;
                GetComponent<ActionScheduler>().CancelCurrentAction();
                GetComponent<Collider>().enabled = false;
            }
        }

        public JToken CaptureAsJToken()
        {
            return JToken.FromObject(_currentHealth);
        }

        public void RestoreFromJToken(JToken state)
        {
            _currentHealth = state.ToObject<float>();

            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                Die();
            }

            //if we save, kill a guy, then load we need to revive the guy so
            //he doesn't stay in "dead" animation state
            if(_currentHealth > 0 && _isDead)
            {
                _isDead = false;
                _animator.ResetTrigger("die");
                _animator.SetTrigger("revive");
            }
        }

    }
}