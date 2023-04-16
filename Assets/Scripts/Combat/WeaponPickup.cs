using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using System;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] Weapon _weapon = null;
    [SerializeField] float respawnTime = 5;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Fighter fighter = other.GetComponent<Fighter>();
            fighter.EquipWeapon(_weapon);
            StartCoroutine(HideForSeconds(respawnTime));
        }

        IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }
    }

    private void ShowPickup(bool showPickup)
    {
        GetComponent<Collider>().enabled = showPickup;
        GetComponent<MeshRenderer>().enabled = showPickup;
    }

}
