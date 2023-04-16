using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    Health _target = null;
    [SerializeField] float _speed = 15f;
    [SerializeField] bool _isAHomingProjectile = false;
    [SerializeField] GameObject hitEffect = null;
    float _damage = 0; //damage is set when instantiated by the weapon

    public event Action<Projectile> CollisionEvent;

    void FixedUpdate()
    {
        if (_target == null) return;

        //if homing, turn the projectile towards the target.
        //if the target dies midway, just keep going straight
        if (_isAHomingProjectile && !_target.IsDead())
        {
            transform.LookAt(GetAimLocation());
        }

        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    //call this after instiantiating
    public void SetTarget(Health target)
    {
        _target = target;
    }

    //call this after instiantiating
    public void SetDamage(float damage)
    {
        _damage = damage;
    }

    public Vector3 GetAimLocation()
    {
        CapsuleCollider targetCapsule = _target.GetComponent<CapsuleCollider>();
        if (targetCapsule == null) return _target.transform.position;

        return _target.transform.position + Vector3.up * targetCapsule.height / 2;
    }

    //public void SetPool(ObjectPool<Projectile> pool)
    //{
    //    //_projectilePool = pool;
    //}

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Health>(out Health health))
        {
            health.takeDamage(_damage);
        }

        if(hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, transform.rotation);
        }
        float oldSpeed = _speed;
        _speed = 0;
        //Return the arrow to the pool after hitting something with a collider.
        //CollisionEvent?.Invoke(this);

        StartCoroutine(ReutunToPoolAfterTime(1f, oldSpeed));
        

    }

    IEnumerator ReutunToPoolAfterTime(float time, float oldSpeed)
    {
        yield return new WaitForSeconds(time);
        CollisionEvent?.Invoke(this);
        _speed = oldSpeed;
    }
}
