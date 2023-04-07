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
    float _damage = 0;

    ObjectPool<Projectile> _projectilePool;

    public event Action<Projectile> CollisionEvent;

    void FixedUpdate()
    {
        if (_target == null) return;
        
        //transform.LookAt(GetAimLocation());
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

    public void SetPool(ObjectPool<Projectile> pool)
    {
        _projectilePool = pool;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Health>(out Health health))
        {
            health.takeDamage(_damage);
        }

        //Return the arrow to the pool after hitting something with a collider.
        CollisionEvent?.Invoke(this);

        
        //_projectilePool.Release(this);
        //if(GameObject.ReferenceEquals(other.gameObject, _target.gameObject))
        //{
        //    //in weapon.cs we subscribe to this and
        //    //trelease the projecile back into the pool.
        //    CollisionEvent?.Invoke(this);

        //    _target.takeDamage(_damage);

        //    //Destroy(this.gameObject);
        //}
    }

    private void OnDestroy()
    {
        Debug.Log("Projectile DESTROYED!!!!!");
    }
}
