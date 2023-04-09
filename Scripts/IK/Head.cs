using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/*************************************
Class: Head
Author: Alexander Achorn
Created: 30/09/22
Last Modified: 20/01/23
Description: This class controls the head movement and its relation to the rest of the IK of the players body.

Change Log
**********
Date: 20/01/23
Author: Alexander Achorn
Verified By: Antoine Plouffe
Changes: Ported over my PID loop to fix some issues with the body it relation to the head. Also setup the basics to allow toggling Head Forward vs Hand Forward.

Date: 
Author: 
Verified By: 
Changes: 
*************************************/

public class Head : MonoBehaviour
{
    [Header("Settings Manager")]
    [SerializeField] private VRSettingsController vrSettings;
    [SerializeField] private ActionBasedContinuousMoveProvider movementProvider;

    [Header("PID")]
    [SerializeField] float frequency = 50f;
    [SerializeField] float damping = 1f;
    [SerializeField] float rotFrequency = 100f;
    [SerializeField] float rotDamping = 0.9f;
    [SerializeField] private Rigidbody playerBody;
    [SerializeField] Transform target;
    [Space]
    [SerializeField] private Transform rootObject, followObject;
    [SerializeField] private Vector3 positionOffset, rotationOffset, headBodyOffset;

    private Rigidbody body;

    void Start()
    {
        body = GetComponent<Rigidbody>();
        body.maxAngularVelocity = float.PositiveInfinity;
    }

    void FixedUpdate()
    {
        rootObject.position = transform.position + headBodyOffset;

        if(vrSettings)
        {
            if(!vrSettings.UsingHandsForward())
            {
                if (movementProvider)
                {
                    movementProvider.forwardSource = target;
                }

                rootObject.forward = Vector3.ProjectOnPlane(followObject.forward, Vector3.up).normalized;
            }
        }
        else
        {
            rootObject.forward = Vector3.ProjectOnPlane(followObject.forward, Vector3.up).normalized;
        }

        PIDMovement();
        PIDRotation();
    }

    private void PIDMovement()
    {
        //PID Algorithum
        float kp = (6f * frequency) * (6f * frequency) * 0.25f;
        float kd = 4.5f * frequency * damping;
        float g = 1 / (1 + kd * Time.fixedDeltaTime + kp * Time.fixedDeltaTime * Time.fixedDeltaTime);
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

        if (q.w < 0)
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

    public Vector3 GetHeadBodyOffset()
    {
        return headBodyOffset;
    }

    public void SetHeadBodyOffset(Vector3 offset)
    {
        headBodyOffset = offset;
    }
}
