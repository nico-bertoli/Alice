using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Renderers gizmos lines between child objects 
/// </summary>
public class WayPointsPathRenderer : MonoBehaviour
{
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;

        for(int i = 1; i < transform.childCount; i++) {
            Gizmos.DrawLine(transform.GetChild(i-1).position, transform.GetChild(i).position);
        }
        Gizmos.DrawLine(transform.GetChild(transform.childCount-1).position, transform.GetChild(0).position);
    }
}
