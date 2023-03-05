using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        const float waypointGizmoRadius = 0.4f;
        Color[] Colors = { Color.gray, Color.blue, Color.red, Color.yellow, Color.green, Color.white, Color.black };

        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Gizmos.color = Colors[i];
                Gizmos.DrawSphere(transform.GetChild(i).position, waypointGizmoRadius);
            }


        }
    }
}