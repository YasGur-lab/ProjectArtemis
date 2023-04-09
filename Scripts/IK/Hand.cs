using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

/*************************************
Class: Hand
Author: Alexander Achorn
Created: 08/12/2022
Last Modified: 
Description: This class is in charge of the moving a basic IK hand.

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

public class Hand : MonoBehaviour
{
    //Physics Movement
    [Space]
    [SerializeField] private ActionBasedController controller;
    [SerializeField] private float followSpeed = 30f;
    [SerializeField] private float rotateSpeed = 100f;
    [Space]
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private Vector3 rotationOffset;
    [Space]
    [SerializeField] private Transform palm;
    [SerializeField] private float reachDistance = 0.1f, joinDistance = 0.05f;
    [SerializeField] private LayerMask grabbableLayer;

    private Transform followTarget;
    private Rigidbody body;

    private bool isGrabbing;
    private GameObject heldObject;
    private Transform grabPoint;
    private FixedJoint joint1, joint2;

    void Start()
    {
        //Physics Movement
        followTarget = controller.gameObject.transform;
        body = GetComponent<Rigidbody>();
        body.collisionDetectionMode = CollisionDetectionMode.Continuous;
        body.interpolation = RigidbodyInterpolation.Interpolate;
        body.mass = 20.0f;
        body.maxAngularVelocity = 20;

        //Inputs Setup
        controller.selectAction.action.started += Grab;
        controller.selectAction.action.canceled += Release;

        //Teleport hands
        body.position = followTarget.position;
        body.rotation = followTarget.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        PhysicsMove();
    }

    private void PhysicsMove()
    {
        //Position
        var posWithOffset = followTarget.TransformPoint(positionOffset);

        var distance = Vector3.Distance(posWithOffset, transform.position);
        body.velocity = (posWithOffset - transform.position).normalized * followSpeed * distance;

        //Rotation
        var rotWithOffset = followTarget.rotation * Quaternion.Euler(rotationOffset);

        var q = rotWithOffset * Quaternion.Inverse(body.rotation);
        q.ToAngleAxis(out float angle, out Vector3 axis);
        body.angularVelocity = angle * axis * Mathf.Deg2Rad * rotateSpeed;
    }

    private void Grab(InputAction.CallbackContext context)
    {
        if (isGrabbing || heldObject)
        { 
            return; 
        }

        Collider[] grabbableColliders = Physics.OverlapSphere(palm.position, reachDistance, grabbableLayer);
        
        if (grabbableColliders.Length < 1)
        {
            return;
        }

        var objectToGrab = grabbableColliders[0].transform.gameObject;
        var objectBody = objectToGrab.GetComponent<Rigidbody>();

        if (objectBody != null)
        {
            heldObject = objectBody.gameObject;
        }
        else
        {
            objectBody = objectToGrab.GetComponentInParent<Rigidbody>();
            if (objectBody != null)
            {
                heldObject = objectBody.gameObject;
            }
            else
            {
                return;
            }
        }

        StartCoroutine(GrabObject(grabbableColliders[0], objectBody));
    }

    private IEnumerator GrabObject(Collider collider, Rigidbody targetBody)
    {
        isGrabbing = true;

        //Create a grab point
        grabPoint = new GameObject().transform;
        grabPoint.position = collider.ClosestPoint(palm.position);
        grabPoint.parent = heldObject.transform;

        //Move hand to grab point
        followTarget = grabPoint;

        //Wait for hand to reach grab point
        while (grabPoint != null && Vector3.Distance(grabPoint.position, palm.position) > joinDistance && isGrabbing)
        {
            yield return new WaitForEndOfFrame();
        }

        //Freeze hand and object motion
        body.velocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
        targetBody.velocity = Vector3.zero;
        targetBody.angularVelocity = Vector3.zero;

        targetBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        targetBody.interpolation = RigidbodyInterpolation.Interpolate;

        //Attach joints
        joint1 = gameObject.AddComponent<FixedJoint>();
        joint1.connectedBody = targetBody;
        joint1.breakForce = float.PositiveInfinity;
        joint1.breakTorque = float.PositiveInfinity;

        joint1.connectedMassScale = 1;
        joint1.massScale = 1;
        joint1.enableCollision = false;
        joint1.enablePreprocessing = false;

        joint2 = heldObject.AddComponent<FixedJoint>();
        joint2.connectedBody = body;
        joint2.breakForce = float.PositiveInfinity;
        joint2.breakTorque = float.PositiveInfinity;

        joint2.connectedMassScale = 1;
        joint2.massScale = 1;
        joint2.enableCollision = false;
        joint2.enablePreprocessing = false;

        //Reset follow target
        followTarget = controller.gameObject.transform;
    }

    private void Release(InputAction.CallbackContext context)
    {
        if (joint1 != null)
        {
            Destroy(joint1);
        }
        if (joint2 != null)
        {
            Destroy(joint2);
        }
        if (grabPoint != null)
        {
            Destroy(grabPoint.gameObject);
        }

        if (heldObject != null)
        {
            var targetBody = heldObject.GetComponent<Rigidbody>();
            targetBody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            targetBody.interpolation = RigidbodyInterpolation.None;
            heldObject = null;
        }

        isGrabbing = false;
        followTarget = controller.gameObject.transform;
    }
}
