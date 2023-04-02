using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] Weapon _weapon = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Fighter fighter = other.GetComponent<Fighter>();
            fighter.EquipWeapon(_weapon);
        }
    }
}
