using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using Vector3 = UnityEngine.Vector3;

/*************************************
Class: Climber
Author: Antoine Plouffe
Created: 01/09/22
Last Modified: 07/03/2023
Description: This class is in charge of the Climbing mechanic.

Change Log
**********
Date: 07/03/23
Author: Antoine Plouffe
Verified By: Alexander Achorn
Changes: Updating the CharacterController to be perpendicular to the cliff slope (WIP).

Date: 07/04/23
Author: Alexander Achorn
Verified By: 
Changes: Removed third case of something random disabling the jumper all of which caused issues
*************************************/

public class Climber : MonoBehaviour
{
    public static event Action m_ClimbActive;
    public static event Action m_ClimbInActive;
    public CharacterController m_Character;

    [SerializeField] GravityController m_GravityController;

    [SerializeField] GameObject m_TeleportationRay;

    public InputActionProperty m_VelocityRight;
    public InputActionProperty m_VelocityLeft;

    private bool m_RightActive = false;
    private bool m_LeftActive = false;

    [SerializeField] ActionBasedContinuousMoveProvider m_ContinuousMoveProvider;

    private Vector3 m_VelocityAfterHandDeactivated = new(0.0f, 0.0f, 0.0f);
    
    [SerializeField] private GameObject m_Head;

    //IsGrounded
    [SerializeField] private IsGroundedRaycast m_IsGroundedRaycast;
    [SerializeField] private GameObject m_From;

    //VR Settings To Get PlayerHeight
    [SerializeField] VRSettingsController m_VRSettings;


    void Start()
    {
        m_Character = GetComponent<CharacterController>();

        XRDirectClimbInteractor.m_ClimbHandActivated += HandActivated;
        XRDirectClimbInteractor.m_ClimbHandDeactivated += HandDeactivated;
    }

    private void OnDestroy()
    {
        XRDirectClimbInteractor.m_ClimbHandActivated -= HandActivated;
        XRDirectClimbInteractor.m_ClimbHandDeactivated -= HandDeactivated;
    }

    void Update()
    {
        if (m_Character.isGrounded)
        {
            m_VelocityAfterHandDeactivated = Vector3.zero;
        }
    }

    void FixedUpdate()
    {
        if (m_RightActive || m_LeftActive) Climb();

        if (m_VelocityAfterHandDeactivated != Vector3.zero)
            m_Character.Move(m_Character.transform.rotation * -m_VelocityAfterHandDeactivated * Time.fixedDeltaTime);
    }

    void Climb()
    {
        Vector3 velocity = m_LeftActive ? m_VelocityLeft.action.ReadValue<Vector3>() : m_VelocityRight.action.ReadValue<Vector3>();
        m_Character.Move(m_Character.transform.rotation * -velocity * Time.fixedDeltaTime);
    }
    
    private void HandActivated(string controllerName)
    {
        if (controllerName == "LeftHand Controller")
        {
            m_VelocityAfterHandDeactivated = Vector3.zero;
            m_RightActive = false;
            m_LeftActive = true;
            m_ContinuousMoveProvider.useGravity = false;
        }
        else
        {
            m_VelocityAfterHandDeactivated = Vector3.zero;
            m_RightActive = true;
            m_LeftActive = false;
            m_ContinuousMoveProvider.useGravity = false;
        }
        m_ClimbActive?.Invoke();
    }

    private void HandDeactivated(string controllerName)
    {
        if (m_RightActive && controllerName == "RightHand Controller")
        {
            m_RightActive = false;
            m_ContinuousMoveProvider.useGravity = true;
            m_ClimbInActive?.Invoke();

            m_VelocityAfterHandDeactivated = m_VelocityRight.action.ReadValue<Vector3>();
        }
        else if (m_LeftActive && controllerName == "LeftHand Controller")
        {
            m_LeftActive = false;
            m_ContinuousMoveProvider.useGravity = true;
            m_ClimbInActive?.Invoke();

            m_VelocityAfterHandDeactivated = m_VelocityLeft.action.ReadValue<Vector3>();
        }
    }

    public bool IsClimbing()
    {
        if (m_RightActive || m_LeftActive) return true;
        else return false;
    }
}
