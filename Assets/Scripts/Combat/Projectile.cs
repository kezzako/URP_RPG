using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Health _target = null;
    [SerializeField] float _speed = 15f;
    float _damage = 0;

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

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Health>(out Health health))
        {
            CollisionEvent?.Invoke(this);
            health.takeDamage(_damage);
        }

        //if(GameObject.ReferenceEquals(other.gameObject, _target.gameObject))
        //{
        //    //in weapon.cs we subscribe to this and
        //    //trelease the projecile back into the pool.
        //    CollisionEvent?.Invoke(this);

        //    _target.takeDamage(_damage);

        //    //Destroy(this.gameObject);
        //}
    }
}
