using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastLimiter : MonoBehaviour {


    public bool DebugMode;

    //Limitador dos movimentos
    public Vector3 sceneLimitsMax = new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);
    public Vector3 sceneLimitsMin = new Vector3(-Mathf.Infinity, -Mathf.Infinity, -Mathf.Infinity);


    private void LateUpdate()
    {
        UpdateRayCasts();
    }

    private void UpdateRayCasts()
    {
        //var rayUp = new Ray(transform.position, transform.InverseTransformDirection(transform.up));
        //var rayDown = new Ray(transform.position, transform.InverseTransformDirection(-transform.up));
        //var rayRight = new Ray(transform.position, transform.InverseTransformDirection(transform.right));
        //var rayLeft = new Ray(transform.position, transform.InverseTransformDirection(-transform.right));
        //var rayForward = new Ray(transform.position, transform.InverseTransformDirection(transform.forward));
        //var rayBackward = new Ray(transform.position, transform.InverseTransformDirection(-transform.forward));

        var rayUp = new Ray(transform.position, transform.up);
        var rayDown = new Ray(transform.position, -transform.up);
        var rayRight = new Ray(transform.position, transform.right);
        var rayLeft = new Ray(transform.position, -transform.right);
        var rayForward = new Ray(transform.position, transform.forward);
        var rayBackward = new Ray(transform.position, -transform.forward);

        RaycastHit hitUp, hitDown, hitRight, hitLeft, hitForward, hitBackward;


        //Colisão acima
        if (Physics.Raycast(rayUp, out hitUp))
        {
            sceneLimitsMax.y = hitUp.point.y;
        }
        else
        {
            sceneLimitsMax.y = Mathf.Infinity;
        }

        //Colisão abaixo
        if (Physics.Raycast(rayDown, out hitDown))
        {
            sceneLimitsMin.y = hitDown.point.y;
        }
        else
        {
            sceneLimitsMin.y = -Mathf.Infinity;
        }

        //Colisão a direita
        if (Physics.Raycast(rayRight, out hitRight))
        {
            sceneLimitsMax.x = hitRight.point.x;
        }

        else
        {
            sceneLimitsMax.x = Mathf.Infinity;
        }

        //Colisão a esquerda
        if (Physics.Raycast(rayLeft, out hitLeft))
        {
            sceneLimitsMin.x = hitLeft.point.x;
        }
        else
        {
            sceneLimitsMin.x = -Mathf.Infinity;
        }

        //Colisão a frente
        if (Physics.Raycast(rayForward, out hitForward))
        {
            sceneLimitsMax.z = hitForward.point.z;
        }
        else
        {
            sceneLimitsMax.z = Mathf.Infinity;
        }

        //Colisão atrás
        if (Physics.Raycast(rayBackward, out hitBackward))
        {
            sceneLimitsMin.z = hitBackward.point.z;
        }
        else
        {
            sceneLimitsMin.z = -Mathf.Infinity;
        }

        if (DebugMode)
        {
            Debug.DrawRay(rayUp.origin, rayUp.direction, Color.green);
            Debug.DrawRay(rayDown.origin, rayDown.direction, Color.magenta);
            Debug.DrawRay(rayForward.origin, rayForward.direction, Color.blue);
            Debug.DrawRay(rayBackward.origin, rayBackward.direction, Color.yellow);
            Debug.DrawRay(rayRight.origin, rayRight.direction, Color.red);
            Debug.DrawRay(rayLeft.origin, rayLeft.direction, Color.cyan);
        }
    }

}
