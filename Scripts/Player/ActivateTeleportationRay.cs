using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

/*************************************
Class: ActivateTeleportationRay
Author: Antoine Plouffe
Created: 01/09/2022
Last Modified: 02/12/2022
Description: This class is in charge of handling the teleportation.

Change Log
**********
Date: 02/12/2022
Author: Antoine Plouffe
Verified By: Alexander Achorn
Changes: Created.

Date: 
Author: 
Verified By: 
Changes: 
*************************************/

public class ActivateTeleportationRay : MonoBehaviour
{
    [SerializeField] private GameObject m_RightTeleportation;
    [SerializeField] private InputActionProperty m_RightActivate;
    [SerializeField] private InputActionProperty m_RightCancel;

    [SerializeField] private InputActionProperty m_LeftActivate;
    [SerializeField] private InputActionProperty m_LeftCancel;

    [SerializeField] private XRDirectInteractor m_RightInteractor;
    [SerializeField] private XRDirectInteractor m_LeftInteractor;
    private void OnEnable()
    {
        m_RightInteractor.selectEntered.AddListener(TakeInput);
        m_RightInteractor.selectExited.AddListener(StopInput);
        m_LeftInteractor.selectEntered.AddListener(TakeInput);
        m_LeftInteractor.selectExited.AddListener(StopInput);
    }

    private void OnDisable()
    {
        m_RightInteractor.selectEntered.RemoveListener(TakeInput);
        m_RightInteractor.selectExited.RemoveListener(StopInput);
        m_LeftInteractor.selectEntered.RemoveListener(TakeInput);
        m_LeftInteractor.selectExited.RemoveListener(StopInput);
    }

    private void TakeInput(SelectEnterEventArgs arg0)
    {
        m_RightTeleportation.SetActive(true);
    }

    private void StopInput(SelectExitEventArgs arg0)
    {
        m_RightTeleportation.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        m_RightTeleportation.SetActive(m_RightCancel.action.ReadValue<float>() == 0.0f &&
                                       m_RightActivate.action.ReadValue<float>() > 0.1f);

        m_RightTeleportation.transform.GetChild(0).gameObject.SetActive(m_RightCancel.action.ReadValue<float>() == 0.0f &&
                                                                        m_RightActivate.action.ReadValue<float>() > 0.1f);
    }
}
    