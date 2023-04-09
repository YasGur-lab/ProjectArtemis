using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*************************************
Class: CoreSample
Author: Alexander Achorn
Created: 08/04/2023
Last Modified: 08/04/2023
Description: This script is use to handle the Vipers Core Sample.

Change Log
**********
Date: 08/04/2023
Author: Alexander Achorn
Verified By: 
Changes: Created

Date: 
Author: 
Verified By: 
Changes: 
*************************************/

public class CoreSample : MissionObject
{
    [SerializeField] ViperMovement m_viper;

    float m_PercOfIceFound;

    private void Start()
    {
        m_PercOfIceFound = m_viper.m_PercOfIceFound;
    }
}
