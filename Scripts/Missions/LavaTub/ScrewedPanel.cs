using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/*************************************
Class: ScrewedPanel
Author: Antoine Plouffe
Created: 01/02/2023
Last Modified: 02/02/2023
Description: This class is in charge of handling the ScrewedPanel.

Change Log
**********
Date: 01/02/2023
Author: Antoine Plouffe
Verified By: Alexander Achorn
Changes: Created.

Date: 07/03/2023
Author: Antoine Plouffe
Verified By: Alexander Achorn
Changes: Disabling Battery and Battery Socket when the screwed panel is on.

Date: 07/04/23
Author: Alexander Achorn
Verified By: 
Changes: Hopefully fixed error caused when things get enabled in an odd order
*************************************/

public class ScrewedPanel : MissionObject
{
    [SerializeField] private Transform[] m_Screws;
    [SerializeField] private Transform[] m_ScrewSockets;
    [SerializeField] private WirePanel_MiniGame m_WirePanel;
    [SerializeField] private GameObject m_OldBattery;
    [SerializeField] private GameObject[] m_Wires;
    [SerializeField] private Transform m_BatteryEndSocket;
    [SerializeField] private XRSocketInteractor m_BatterySocketInteractor;
    private int m_NumberOfScrewsOff;

    [SerializeField] Mission2Audio missionAudio;

    // Update is called once per frame
    void Update()
    {
        if (m_WirePanel.isActiveAndEnabled == true)
        {
            if (!AreScrewsOn() && !m_WirePanel.GetMiniGameStatus())
            {
                isComplete = true;

                m_BatterySocketInteractor.socketActive = true;
                m_OldBattery.SetActive(true);

                foreach (var wire in m_Wires)
                {
                    wire.SetActive(true);
                }

                if (gameObject.activeSelf)
                {
                    gameObject.SetActive(false);
                    missionAudio.m_wiresExposed = true;
                }

            }
        }
    }

    public bool AreScrewsOn()
    {
        foreach (var socket in m_ScrewSockets)
        {
            if (socket.GetComponent<XRSocketInteractor>().hasSelection)
            {
                return true;
            }
        }
        return false;
    }

    public void MoveScrewsOntoPanel()
    {
        for (int i = 0; i < m_Screws.Length; i++)
        {
            m_Screws[i].transform.position = m_ScrewSockets[i].transform.position;
        }
    }

    public int GetNumberOfScrewsOff()
    {
        int numberOfScrewsOff = 0;
        foreach (var socket in m_ScrewSockets)
        {
            if (socket.GetComponent<XRSocketInteractor>().hasSelection)
            {
                numberOfScrewsOff++;
            }
        }
        return numberOfScrewsOff;
    }
}
