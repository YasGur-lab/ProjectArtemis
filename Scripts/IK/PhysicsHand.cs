using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

/*************************************
Class: PhysicsHand
Author: Alexander Achorn
Created: 30/09/22
Last Modified: 20/01/23
Description: This class controls the hand movements and their relation to the rest of the IK of the players body.

Change Log
**********
Date: 20/01/23
Author: Alexander Achorn
Verified By: Antoine Plouffe
Changes: Setup the basics to allow toggling Head Forward vs Hand Forward.

Date: 27/01/23
Author: Alexander Achorn
Verified By: Antoine Plouffe
Changes: Setup Rules to try and Clamp the bodies rotation from the head
*************************************/

public class PhysicsHand : MonoBehaviour
{
    private enum HandSide
    { Left, Right }
    [Header("Settings Manager")]
    [SerializeField] private VRSettingsController vrSettings;
    [SerializeField] private ActionBasedContinuousMoveProvider movementProvider;
    [SerializeField] private Transform rootObject;
    [SerializeField] private HandSide handSide;
    [SerializeField] private Transform mainCamera;

    [Header("PID")]
    [SerializeField] float frequency = 50f;
    [SerializeField] float damping = 1f;
    [SerializeField] float rotFrequency = 100f;
    [SerializeField] float rotDamping = 0.9f;
    [SerializeField] private Rigidbody playerBody;
    [SerializeField] Transform target;
    [Space]
    [SerializeField] private ActionBasedController controller;
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
        //followTarget = controller.gameObject.transform;
        body = GetComponent<Rigidbody>();
        //body.collisionDetectionMode = CollisionDetectionMode.Continuous;
        //body.interpolation = RigidbodyInterpolation.Interpolate;
        //body.mass = 20.0f;
        body.maxAngularVelocity = float.PositiveInfinity;

        //Inputs Setup
        //controller.selectAction.action.started += Grab;
        //controller.selectAction.action.canceled += Release;

        //Teleport hands
        //body.position = followTarget.position;
        //body.rotation = followTarget.rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (vrSettings)
        {
            if (vrSettings.UsingHandsForward() && (handSide == HandSide.Right && vrSettings.IsRightHanded()) || (handSide == HandSide.Left && !vrSettings.IsRightHanded()))
            {
                if (movementProvider)
                {
                    movementProvider.forwardSource = target;
                }

                Vector3 direction = target.position - mainCamera.position;

                float angleBetweenHeadandHand = Vector3.SignedAngle(direction, mainCamera.forward, Vector3.up);

                if (angleBetweenHeadandHand < 15.0f && angleBetweenHeadandHand > -65.0f)
                {
                    rootObject.forward = Vector3.ProjectOnPlane(target.forward, Vector3.up).normalized;
                }
            }
        }

        PIDMovement();
        PIDRotation();
    }

    private void PIDMovement()
    {
        //PID Algorithum
        float kp = (6f * frequency) * (6f * frequency) * 0.25f;
        float kd = 4.5f * frequency * damping;
        float g = 1/ (1 + kd * Time.fixedDeltaTime + kp * Time.fixedDeltaTime * Time.fixedDeltaTime);
        float ksg = kp * g;
        float kbg = (kd + kp * Time.fixedDeltaTime) * g;

        //Translation
        var posWithOffset = target.TransformPoint(positionOffset);

        Vector3 force = (posWithOffset - transform.position) * ksg + (playerBody.velocity - body.velocity) * kbg;

        body.AddForce(force, ForceMode.Acceleration);
    }

    private void PIDRotation()
    {
        //PID Algorithum
        float kp = (6f * rotFrequency) * (6f * rotFrequency) * 0.25f;
        float kd = 4.5f * rotFrequency * rotDamping;
        float g = 1 / (1 + kd * Time.fixedDeltaTime + kp * Time.fixedDeltaTime * Time.fixedDeltaTime);
        float ksg = kp * g;
        float kbg = (kd + kp * Time.fixedDeltaTime) * g;

        //Rotation
        var rotWithOffset = target.rotation * Quaternion.Euler(rotationOffset);

        Quaternion q = rotWithOffset * Quaternion.Inverse(transform.rotation);
        
        if(q.w < 0)
        {
            q.x = -q.x;
            q.y = -q.y;
            q.z = -q.z;
            q.w = -q.w;
        }

        q.ToAngleAxis(out float angle, out Vector3 axis);
        axis.Normalize();
        axis *= Mathf.Deg2Rad;
        Vector3 torque = ksg * axis * angle + -body.angularVelocity * kbg;

        body.AddTorque(torque, ForceMode.Acceleration);
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
