using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Mover _mover;

    private void Awake()
    {
        _mover = GetComponent<Mover>();
    }


    public void MoveToCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        bool hasHit = Physics.Raycast(ray, out hit);

        float distanceToMove = Vector3.Distance(hit.point, transform.position);

        //don't move if distance is too small because the animations become weird
        if (hasHit && distanceToMove > 0.5)
        {
            _mover.MoveTo(hit.point);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
