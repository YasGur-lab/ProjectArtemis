using System;
using UnityEngine;

/*************************************
Class: Foot
Author: Alexander Achorn
Created: 08/12/2022
Last Modified: 
Description: This class is in charge of the moving an IK foot.

Change Log
**********
Date: 09/01/2022
Author: Alexander Achorn
Verified By:
Changes: Added Change Log

Date: 
Author: 
Verified By: 
Changes: 
*************************************/

public class Foot : MonoBehaviour
{
    [SerializeField] private LayerMask terrainLayer;
    [SerializeField] private Transform body;
    [SerializeField] private Foot otherFoot;
    [SerializeField] private float speed = 5, stepDistance = .3f, stepLength = .3f, stepHeight = .3f;
    [SerializeField] private Vector3 footPosOffset, footRotOffset;

    private float footSpacing, lerp;
    private Vector3 oldPos, currentPos, newPos;
    private Vector3 oldRot, currentRot, newRot;
    //private Vector3 oldNorm, currentNorm, newNorm;
    private bool isFirstStep = true;
    
    void Start()
    {
        footSpacing = transform.localPosition.x;
        currentPos = newPos = oldPos = transform.position;
        currentRot = newRot = oldRot = body.forward;
        //currentNorm = newNorm = oldNorm = transform.up;
        lerp = 1;
    }

    void Update()
    {
        transform.position = currentPos + footPosOffset;
        transform.rotation = Quaternion.LookRotation(currentRot) * Quaternion.Euler(footRotOffset);
        //transform.rotation = Quaternion.LookRotation(currentNorm) * Quaternion.Euler(footRotOffset);

        Ray ray = new Ray(body.position + (body.right * footSpacing) + (Vector3.up * 2), Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, 10, terrainLayer.value))
        {
            if (isFirstStep || (Vector3.Distance(newPos, hit.point) > stepDistance && !otherFoot.IsMoving() 
                && !IsMoving()))
            {
                lerp = 0;
                int direction = body.InverseTransformPoint(hit.point).z > body.InverseTransformPoint(newPos).z
                    ? 1
                    : -1;
                newPos = hit.point + (body.forward * (direction * stepLength));
                newRot = body.forward;
                //newNorm = hit.normal;
            }
        }

        if (lerp < 1)
        {
            Vector3 tempPos = Vector3.Lerp(oldPos, newPos, lerp);
            tempPos.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

            currentPos = tempPos;
            currentRot = Vector3.Lerp(oldRot, newRot, lerp);
            //currentNorm = Vector3.Lerp(oldNorm, newNorm, lerp);
            lerp += Time.deltaTime * speed;
        }
        else
        {
            oldPos = newPos;
            oldRot = newRot;
            //oldNorm = newNorm;
        }

        isFirstStep = false;
    }

    public bool IsMoving()
    {
        return lerp < 1;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(newPos, 0.1f);
    }
}
