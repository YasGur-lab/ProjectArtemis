using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

/*************************************
Class: GunFlare
Author: Antoine Plouffe
Created: 08/02/2023
Last Modified: 08/02/2023
Description: This class is in charge of the GunFlare.

Change Log
**********
Date: 
Author: Antoine Plouffe
Verified By:
Changes: Created.

Date: 
Author: 
Verified By: 
Changes: 
*************************************/

public class FlareGun : MonoBehaviour
{
    public bool m_ShootRequested;
    [SerializeField] private Transform m_Muzzle;
    [SerializeField] InputActionReference m_RightInput;
    [SerializeField] InputActionReference m_LeftInput;
    private float m_CurrentTimeBeforeReload;
    [SerializeField][Range(0.0f, 100.0f)] private float m_TimeBeforeReload;
    private bool m_HasShoot;

    [SerializeField] private Flare m_Flare;

    [SerializeField] Equipment m_Equipment;

    [SerializeField] XRDirectInteractor[] m_Interactors;

    void OnEnable()
    {
        m_RightInput.action.started += RightGunflareActivated;
        m_LeftInput.action.started += LeftGunflareActivated;
        m_CurrentTimeBeforeReload = m_TimeBeforeReload;
    }

    void OnDisable()
    {
        m_RightInput.action.started -= RightGunflareActivated;
        m_LeftInput.action.started -= LeftGunflareActivated;
        m_CurrentTimeBeforeReload = m_TimeBeforeReload;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_HasShoot)
        {
            m_CurrentTimeBeforeReload -= Time.deltaTime;
            if (m_CurrentTimeBeforeReload <= 0.0f)
            {
                m_HasShoot = false;
                m_CurrentTimeBeforeReload = m_TimeBeforeReload;
            }
        }

        if (m_ShootRequested)
        {
            m_ShootRequested = false;
            if (!m_HasShoot)
            {
                m_HasShoot = true;
                Flare flare = Instantiate(m_Flare);
                flare.Init(m_Muzzle.transform, m_Muzzle.transform.forward.normalized);
            }
        }
    }
    private void RightGunflareActivated(InputAction.CallbackContext obj)
    {
        if (m_Equipment.IsGunFlareAttachedToSocket()) return;
        if (!m_Interactors[0].hasSelection) return;

        if (!m_HasShoot)
        {
            m_HasShoot = true;
            Flare flare = Instantiate(m_Flare);
            flare.Init(m_Muzzle.transform, m_Muzzle.transform.forward.normalized);
        }
    }

    private void LeftGunflareActivated(InputAction.CallbackContext obj)
    {
        if (m_Equipment.IsGunFlareAttachedToSocket()) return;
        if (!m_Interactors[1].hasSelection) return;

        if (!m_HasShoot)
        {
            m_HasShoot = true;
            Flare flare = Instantiate(m_Flare);
            flare.Init(m_Muzzle.transform, m_Muzzle.transform.forward.normalized);
        }
    }

    void OnTriggerEnter(Collider coll)
    {
    }
}
