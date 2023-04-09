using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using Vector3 = UnityEngine.Vector3;

/*************************************
Class: Jumper
Author: Antoine Plouffe
Created: 01/11/22
Last Modified: 17/01/23
Description: This class is in charge of the Jump mechanic.

Change Log
**********
Date: 17/01/23
Author: Antoine Plouffe
Verified By: Alexander Achorn
Changes: Making the jump activation on direct input instead of activating it based on player's controller's velocity. This is for the testing phase and see which version works best.

Date: 07/03/23
Author: Antoine Plouffe
Verified By: Alexander Achorn
Changes: Making the jump in the forward direction only. Disabling movements input when jumping.

Date: 29/03/23
Author: Charles Weir
Verified By: 
Changes: Added headset forward to jump
*************************************/

public class Jumper : MonoBehaviour
{
    //Input
    [SerializeField] InputActionReference m_Input;
    private bool m_LeftActive = false;

    //Character controller
    CharacterController m_Controller;


    //main camera for headset forward
    Camera m_Camera;

    //JumpHandling
    bool m_JumpActivated = false;
    [SerializeField][Range(0.0f, 100.0f)] float m_Force;
    [SerializeField][Range(0.0f, 90)] int m_Angle = 45;

    //Velocity
    Vector3 m_LeftControllerVelocity = new(0.0f, 0.0f, 0.0f);
    public InputActionProperty m_VelocityLeft;

    //IsGrounded
    [SerializeField][Range(0.0f, 5.0f)] float m_DistanceToGround = 0.51f;
    [SerializeField] private IsGroundedRaycast m_IsGroundedRaycast;
    [SerializeField] private GameObject m_From;
    [SerializeField][Range(0.0f, 1.0f)] float m_MaxTimerBeforeIsGroundedActive = 0.2f;
    float m_CurrentTimerBeforeIsGroundedActive;
    bool m_IsGrounded = true;

    //Climbing
    Climber m_Climber;

    //Teleportation Ray
    [SerializeField] GameObject m_TeleportationRay;

    //Player's movements
    ActionBasedContinuousMoveProvider m_ActionBasedMovements;
    private HandlingInputs m_HandlingInputs;

    //Disabling
    //private bool m_IsDisabled;
    [SerializeField] XRDirectInteractor m_LeftInteractor;

    //// Start is called before the first frame update
    void Start()
    {
        m_HandlingInputs = GetComponent<HandlingInputs>();
        m_IsGrounded = false;
        m_Climber = GetComponent<Climber>();
        m_Controller = GetComponent<CharacterController>();
        m_Camera = Camera.main;
        m_ActionBasedMovements = GetComponent<ActionBasedContinuousMoveProvider>();
        m_Input.action.started += JumpActivated;
    }

    void Update()
    {
        if (m_Climber.IsClimbing()) return;

        if (m_JumpActivated)
        {
            m_CurrentTimerBeforeIsGroundedActive += Time.deltaTime;

            if (m_CurrentTimerBeforeIsGroundedActive > m_MaxTimerBeforeIsGroundedActive)
            {
                m_IsGrounded = m_IsGroundedRaycast.IsGroundedByRaycast(m_From, m_DistanceToGround);
            }
        }

        if (m_IsGrounded)
        {
            m_LeftActive = false;
            m_LeftControllerVelocity = Vector3.zero;

            m_CurrentTimerBeforeIsGroundedActive = 0.0f;

            if (m_JumpActivated)
            {
                m_HandlingInputs.EnableInputs("XRI LeftHand Locomotion");
                m_HandlingInputs.EnableInputs("XRI RightHand Locomotion");
                m_TeleportationRay.SetActive(true);
                m_JumpActivated = false;
            }
        }
    }

    //// Update is called once per frame
    void FixedUpdate()
    {
        if (m_LeftActive)
        {
            if (m_Climber.IsClimbing() || m_LeftInteractor.hasSelection) return;
            StartCoroutine(Jump());
        }
    }
    
    IEnumerator Jump()
    {
        yield return new WaitForSeconds(0.0f);
        m_JumpActivated = true;

        if (m_TeleportationRay.activeSelf)
            m_TeleportationRay.SetActive(false);

        float radians = m_Angle * Mathf.Deg2Rad;
        Vector3 forwardVector = m_Controller.transform.forward;
        Vector3 upVector = Vector3.up;
        Vector3 movementVector = forwardVector * Mathf.Cos(radians) + upVector * Mathf.Sin(radians);
        Vector3 jumpDir = movementVector.normalized * m_Force;
        Vector3 temp = Vector3.Lerp(Vector3.zero, jumpDir, Time.fixedDeltaTime);
        m_Controller.Move(temp);
    }

    private void JumpActivated(InputAction.CallbackContext obj)
    {
        if(m_LeftInteractor)
        {
            if (!m_Climber.IsClimbing() && !m_LeftInteractor.hasSelection)
            {
                m_LeftControllerVelocity = Vector3.zero;
                m_HandlingInputs.DisableInputs("XRI LeftHand Locomotion");
                m_HandlingInputs.DisableInputs("XRI RightHand Locomotion");
                m_LeftActive = true;
                m_IsGrounded = false;
            }
        }
    }
}