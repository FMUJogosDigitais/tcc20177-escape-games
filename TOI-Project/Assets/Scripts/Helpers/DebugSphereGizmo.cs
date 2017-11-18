using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSphereGizmo : MonoBehaviour {

    public float radius = .2f;
    public Color color = new Color(1f, .5f, 0, 1);

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawWireSphere(transform.position, radius);

    }

}
