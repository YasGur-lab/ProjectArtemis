using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

/*************************************
Class: Drill
Author: Antoine Plouffe
Created: 01/02/2023
Last Modified: 02/02/2023
Description: This class is in charge of the Drill.

Change Log
**********
Date: 01/02/2023
Author: Antoine Plouffe
Verified By: Alexander Achorn
Changes: Created.

Date: 06/03/2022
Author: Charlie Weir
Verified By: Alexander Achorn
Changes: Added Haptic Feedback

Date: 
Author: 
Verified By: 
Changes: 
*************************************/

public class Drill : MonoBehaviour
{
    [SerializeField] private XRSocketInteractor m_TipSocket;
    [SerializeField] InputActionReference m_RightInput;
    [SerializeField] InputActionReference m_LeftInput;
    private bool m_IsActive;
    private GameObject m_CurrentScrew;
    [SerializeField][Range(0.0f, 100.0f)] private float m_RotationSpeed;
    [SerializeField] private float m_VibrationDuration = 0.068f;
    [SerializeField] private float m_VibrationIntensity = 0.68f;

    private float m_VibratorTimer = 0.0f;

    [SerializeField] XRDirectInteractor[] m_Interactors;

    void OnEnable()
    {
        m_RightInput.action.started += RightDrillActivated;
        m_RightInput.action.canceled += DrillDeactivated;
        m_LeftInput.action.started += LeftDrillActivated;
        m_LeftInput.action.canceled += DrillDeactivated;
    }

    void OnDisable()
    {
        m_RightInput.action.started -= RightDrillActivated;
        m_RightInput.action.canceled -= DrillDeactivated;
        m_LeftInput.action.started -= LeftDrillActivated;
        m_LeftInput.action.canceled -= DrillDeactivated;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_IsActive)
        {
            Rotate(m_TipSocket.transform.GetChild(0).transform);
            var m_Interactor = GetComponent<XRGrabInteractable>().interactorsSelecting[0];
            if (m_Interactor != null)
            {
                m_VibratorTimer += Time.deltaTime;
                if(m_VibratorTimer>=m_VibrationDuration)
                {
                    m_VibratorTimer = 0;
                    ((XRDirectClimbInteractor)m_Interactor).SendHapticImpulse(m_VibrationIntensity, m_VibrationDuration);
                }
                
            }
        }
    }
    private void RightDrillActivated(InputAction.CallbackContext obj)
    {
        if (!m_Interactors[0].hasSelection) return;

        if (m_CurrentScrew && !m_TipSocket.hasSelection)
        {
            if (m_CurrentScrew.GetComponent<Screw>().GetFirstEnter() == false)
            {
                m_CurrentScrew.GetComponent<Screw>().FirstEnter();
                m_TipSocket.GetComponent<XRSocketInteractor>().StartManualInteraction(m_CurrentScrew.GetComponent<IXRSelectInteractable>());
                m_CurrentScrew.GetComponent<Rigidbody>().isKinematic = false;
             
            }
        }
        if (m_TipSocket.hasSelection) m_IsActive = true;
    }

    private void LeftDrillActivated(InputAction.CallbackContext obj)
    {
        if (!m_Interactors[1].hasSelection) return;

        if (m_CurrentScrew && !m_TipSocket.hasSelection)
        {
            if (m_CurrentScrew.GetComponent<Screw>().GetFirstEnter() == false)
            {
                m_CurrentScrew.GetComponent<Screw>().FirstEnter();
                m_TipSocket.GetComponent<XRSocketInteractor>().StartManualInteraction(m_CurrentScrew.GetComponent<IXRSelectInteractable>());
                m_CurrentScrew.GetComponent<Rigidbody>().isKinematic = false;

            }
        }
        if (m_TipSocket.hasSelection) m_IsActive = true;
    }

    private void DrillDeactivated(InputAction.CallbackContext obj)
    {
        m_IsActive = false;
    }

    public void Rotate(Transform socket)
    {
        socket.Rotate(new Vector3(0, m_RotationSpeed, 0));
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.GetComponentInParent<Screw>())
        {
            if (!Equals(m_CurrentScrew, coll.gameObject))
            {
                m_CurrentScrew = coll.GetComponentInParent<Screw>().gameObject;
                m_CurrentScrew.GetComponent<Screw>().IsTriggeredCollider(true);
                m_CurrentScrew.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }

    IEnumerator ResetCurrentScrewCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        m_CurrentScrew = null;
    }

    public void ResetCurrentScrew()
    {
        StartCoroutine(ResetCurrentScrewCoroutine());
    }
}
