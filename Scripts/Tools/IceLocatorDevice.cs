using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;
using Image = UnityEngine.UI.Image;
using System;

/*************************************
Class: IceLocatorDevice
Author: Alexander Achorn
Created: 05/03/2023
Last Modified: 05/04/2023
Description: This script is in charge of handling the ice locator device.

Change Log
**********
Date: 05/04/2023
Author: Alexander Achorn
Verified By: Antoine Plouffe
Changes: Created

Date: 05/04/2023
Author: Antoine Plouffe
Verified By: 
Changes: Moved most of the code related to the device from HotOrCold script

Date: 07/04/23
Author: Alexander Achorn
Verified By: 
Changes: Restored intended functionality
*************************************/

[System.Serializable]
public class IceLocatorDevice : MissionObject
{
    [Header("Viper")]
    [SerializeField] ViperMovement m_Viper;

    [Header("Ice")]
    [SerializeField] HotOrCold m_Ice;
    public float m_FinalPercOfIce;

    [Header("Inputs")]
    [SerializeField] InputActionReference m_RightInput;
    [SerializeField] InputActionReference m_LeftInput;
    [SerializeField] XRDirectInteractor[] m_Interactors;

    [Header("Device")]
    [SerializeField] GameObject m_Indicator;
    [SerializeField] GameObject m_DeviceAntenna;
    [SerializeField][Range(100.0f, 140.0f)] float m_PercOfIceBuffer = 100.0f;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI m_PercOfIceText;
    [SerializeField] Slider m_Slider;

    void OnEnable()
    {
        m_RightInput.action.started += RightLocatorActivated;
        m_LeftInput.action.started += LeftLocatorActivated;
    }

    void OnDisable()
    {
        m_RightInput.action.started -= RightLocatorActivated;
        m_LeftInput.action.started -= LeftLocatorActivated;
    }

    void Start()
    {
        if(m_Ice)
        {
            m_Slider.maxValue = m_Ice.GetIceDistanceFromDevice();
            m_Ice.SetInitialIceDistanceFromDevice(m_Ice.GetIceDistanceFromDevice() + (m_PercOfIceBuffer / 100));
            m_PercOfIceText.text = " ";
        }
    }

    private void Update()
    {
        if (m_Viper.isTriggered && !isTriggered)
        {
            isTriggered = true;
        }

        if (!isComplete && isTriggered)
        {
            SetIndicatorColour();
            SetSliderValue();
        }
    }

    private void RightLocatorActivated(InputAction.CallbackContext obj)
    {
        if (!m_Interactors[0].hasSelection) return;

        if (!isComplete && isTriggered && !m_Viper.finalDestChosen)
        {
            m_Viper.SetDestination(RaycastDestination());

            m_FinalPercOfIce = CalculatingPercOfIceInArea();
            if(m_FinalPercOfIce > 50.0f)
            {
                isComplete = true;
                m_Viper.finalDestChosen = true;
                m_Viper.m_PercOfIceFound = m_FinalPercOfIce;
            }
        }
    }

    private void LeftLocatorActivated(InputAction.CallbackContext obj)
    {
        if (!m_Interactors[1].hasSelection) return;

        if (!isComplete && isTriggered && !m_Viper.finalDestChosen)
        {
            m_Viper.SetDestination(RaycastDestination());

            m_FinalPercOfIce = CalculatingPercOfIceInArea();
            if (m_FinalPercOfIce > 50.0f)
            {
                isComplete = true;
                m_Viper.finalDestChosen = true;
                m_Viper.m_PercOfIceFound = m_FinalPercOfIce;
            }
        }
    }

    private Vector3 RaycastDestination()
    {
        RaycastHit hit;
        Vector3 StartPos = transform.position;
        Vector3 Endpos = m_DeviceAntenna.transform.position;
        Vector3 Dir = Endpos - StartPos;
        Dir.Normalize();

        if (Physics.Raycast(StartPos, Dir, out hit))
        {
            if (hit.collider.gameObject.tag == "Terrain")
            {
                return hit.point;
            }
        }

        return m_Viper.gameObject.transform.position;
    }

    public float CalculatingPercOfIceInArea()
    {
        float distBuffer = m_Ice.GetDistanceBuffer();
        float distFromIce = m_Ice.GetIceDistancePerc();
        float percent = 0.0f;

        if (distFromIce <= distBuffer)
        {
            percent = 1.0f;
        }
        else if (distFromIce > m_Ice.GetMaxDistance())
        {
            percent = 0.0f;
        }
        else
        {
            float distFromBuffer = distFromIce - distBuffer;
            float maxDistToGet0Perc = m_Ice.GetMaxDistance() - distBuffer;
            percent = 1.0f - (distFromBuffer / maxDistToGet0Perc);
        }
        percent *= 100;

        return Mathf.Floor(percent);
    }

    void SetIndicatorColour()
    {
        if (m_Ice.DegreeBetweenDeviceAndIce() >= 173)
        {
            //Sound Bip Bip
            m_Indicator.GetComponent<Image>().color = Color.blue;
        }
        else if (m_Ice.DegreeBetweenDeviceAndIce() < 173 && m_Ice.DegreeBetweenDeviceAndIce() >= 155)
        {
            m_Indicator.GetComponent<Image>().color = Color.green;
        }
        else if (m_Ice.DegreeBetweenDeviceAndIce() < 155 && m_Ice.DegreeBetweenDeviceAndIce() >= 120)
        {
            m_Indicator.GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            m_Indicator.GetComponent<Image>().color = Color.red;
        }
    }

    void SetSliderValue()
    {
        m_Slider.value = m_Ice.GetIceDistanceFromDevice();
    }

    public void SetPercOfIceText(string text)
    {
        m_PercOfIceText.text = text;
    }
}
