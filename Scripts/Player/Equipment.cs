using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/*************************************
Class: Jumper
Author: Antoine Plouffe
Created: 01/11/22
Last Modified: 19/01/23
Description: This class is in charge of the player's equipment.

Change Log
**********
Date: 17/01/23
Author: Antoine Plouffe
Verified By: Alexander Achorn
Changes: Adding a function to swap the fake tablet with the real one.

Date: 07/04/23
Author: Alexander Achorn
Verified By: 
Changes: Simplifying and removing bloat
*************************************/

public class Equipment : MonoBehaviour, ISaveable
{
    [Header("Tools")]
    [SerializeField] GameObject m_IceLocator;
    [SerializeField] GameObject m_Drill;
    [SerializeField] GameObject m_FlareGun;
    [Header("Tool Setting")]
    [SerializeField] bool m_IceLocIsEquiped = false;
    [SerializeField] bool m_DrillIsEquiped = false;
    [SerializeField] bool m_FlareIsEquiped = false;
    [Header("Other Setting")]
    [SerializeField] XRDirectInteractor[] m_Interactors;
    [SerializeField] SuitUp suitState;
    [SerializeField] private XRSocketInteractor m_TabletSocket;
    [SerializeField] private XRSocketInteractor m_FlareSocket;
    [SerializeField] private XRSocketInteractor m_ToolSocket;
    [SerializeField] private Camera m_MainCamera;

    [SerializeField] private bool m_IsTabletOn = false;

    [SerializeField] GameObject m_Tablet;
    private GameObject m_Player;

    void Start()
    {
        //m_Tablet = GameObject.Find("Tablet");
        m_Player = GameObject.Find("XR Origin");

        if (m_IsTabletOn)
        {
            if(m_Tablet)
                m_Tablet.transform.position = m_TabletSocket.attachTransform.position;
        }

        if (m_DrillIsEquiped && m_IceLocIsEquiped)
        {
            m_IceLocIsEquiped = false;
        }

        if(m_IceLocator && !m_IceLocIsEquiped)
        {
            m_IceLocator.SetActive(false);
        }
        if (m_Drill && !m_DrillIsEquiped)
        {
            m_Drill.SetActive(false);
        }
        if (m_FlareGun && !m_FlareIsEquiped)
        {
            m_FlareGun.SetActive(false);
        }

        if(m_IceLocator && m_IceLocIsEquiped)
        {
            m_ToolSocket.startingSelectedInteractable = m_IceLocator.GetComponent<XRGrabInteractable>();
        }
        else if(m_Drill && m_DrillIsEquiped)
        {
            m_ToolSocket.startingSelectedInteractable = m_Drill.GetComponent<XRGrabInteractable>();
        }

    }

    void Update()
    {

        if (DistanceFromPlayer(m_Tablet.transform.position) > 4.0f)
        {
            m_Tablet.transform.position = m_TabletSocket.attachTransform.position;
        }

        if (m_FlareGun)
        {
            if (DistanceFromPlayer(m_FlareGun.transform.position) > 4.0f)
            {
                m_FlareGun.transform.position = m_FlareSocket.attachTransform.position;
            }
        }

        if (m_IceLocator && m_IceLocIsEquiped)
        {
            if (DistanceFromPlayer(m_IceLocator.transform.position) > 4.0f)
            {
                m_IceLocator.transform.position = m_ToolSocket.attachTransform.position;
            }
        }

        if (m_Drill && m_DrillIsEquiped)
        {
            if (DistanceFromPlayer(m_Drill.transform.position) > 4.0f)
            {
                m_Drill.transform.position = m_ToolSocket.attachTransform.position;
            }
        }
    }

    public void SwapTablet()
    {
        foreach (var controller in m_Interactors)
        {
            if (controller.hasSelection)
            {
                controller.interactionManager.SelectEnter(controller.GetComponent<IXRSelectInteractor>(), m_Tablet.GetComponent<XRGrabInteractable>());
                Destroy(controller.GetOldestInteractableSelected().transform.gameObject);
            }
        }
    }

    public bool IsSpacesuitOn()
    {
            return suitState.isComplete;
    }
    public bool IsGunFlareAttachedToSocket()
    {
        if(m_FlareIsEquiped)
        {
            return m_FlareSocket.hasSelection;
        }

        return true;
    }

    public bool IsDrillAttachedToSocket()
    {
        if (m_DrillIsEquiped || m_IceLocIsEquiped)
        {
            return m_ToolSocket.hasSelection;
        }

        return true;
    }

    private float DistanceFromPlayer(Vector3 pos)
    {
        return Vector3.Distance(m_Player.transform.position, pos);
    }

    public object SaveState()
    {
        //SetSpacesuitOn();
        return new SaveData()
        {
            m_IsTabletOn = this.m_IsTabletOn
        };
    }


    public void LoadState(object state)
    {
        SaveData saveData = (SaveData)state;

        m_IsTabletOn = saveData.m_IsTabletOn;
    }

    [Serializable]
    private struct SaveData
    {
        public bool m_IsTabletOn;
    }
}
