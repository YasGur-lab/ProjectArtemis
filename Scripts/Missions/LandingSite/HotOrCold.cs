using System;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Windows;

/*************************************
Class: HotOrCold
Author: Antoine Plouffe
Created: 01/09/2023
Last Modified: 02/12/2022
Description: This class is in charge of the Icelocator minigame

Change Log
**********
Date: 02/09/2022
Author: Antoine Plouffe
Verified By: Alexander Achorn
Changes: Created.

Date: 05/04/2023
Author: Antoine Plouffe
Verified By: 
Changes: Moved most of the code related to the device to IceLocatorDevice script

Date: 07/04/23
Author: Alexander Achorn
Verified By: 
Changes: Simplifying and removing bloat
*************************************/

public class HotOrCold : MonoBehaviour
{
    [Header("Ice")]
    [SerializeField] float m_DistanceBuffer;
    [SerializeField] float m_MaxDistance;

    [Header("Device")]
    [SerializeField] GameObject m_Device;
    [SerializeField] GameObject m_DeviceAntenna;

    [Header("Viper")]
    [SerializeField] GameObject m_Viper;

    //Slider
    float m_InitialDistanceFromDevice;

    public float DegreeBetweenDeviceAndIce()
    {
        Vector3 tempFrom = transform.position - m_Device.transform.position;
        Vector3 tempTo = m_Device.transform.position - m_DeviceAntenna.transform.position;
        return Vector3.Angle(tempFrom, tempTo);
    }

    public float GetIceDistanceFromViperForSlider()
    {
        Vector3 viperTempPos = m_Viper.transform.position;
        viperTempPos.y = 0.0f;
        Vector3 iceTempPos = transform.position;
        iceTempPos.y = 0.0f;

        float distanceFromViper = Vector3.Distance(viperTempPos, iceTempPos);
        if(m_InitialDistanceFromDevice == 0)
        {
            return distanceFromViper;
        }

        distanceFromViper = m_InitialDistanceFromDevice - distanceFromViper;
        return distanceFromViper;
    }

    public float GetIceDistanceFromDevice()
    {
        Vector3 TempPos = m_Device.transform.position;
        TempPos.y = 0.0f;
        Vector3 iceTempPos = transform.position;
        iceTempPos.y = 0.0f;

        float distance = Vector3.Distance(TempPos, iceTempPos);
        if (m_InitialDistanceFromDevice == 0)
        {
            return distance;
        }

        distance = m_InitialDistanceFromDevice - distance;
        return distance;
    }

    public float GetIceDistancePerc()
    {
        Vector3 TempPos = m_Device.transform.position;
        TempPos.y = 0.0f;
        Vector3 iceTempPos = transform.position;
        iceTempPos.y = 0.0f;

        float distance = Vector3.Distance(TempPos, iceTempPos);

        return distance;
    }

    public float GetDistanceBuffer()
    {
        return m_DistanceBuffer;
    }

    public float GetMaxDistance()
    {
        return m_MaxDistance;
    }

    public void SetInitialIceDistanceFromDevice(float distance)
    {
        m_InitialDistanceFromDevice = distance;
    }
}
