using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*************************************
Class: ViperMovement
Author: Alexander Achorn
Created: 17/02/2022
Last Modified: 05/04/2023
Description: This script is use to set the viper destination and trigger the ice % calculation when arrived at destination

Change Log
**********
Date: 05/03/2023
Author: Alexander Achorn
Verified By: Antoine Plouffe
Changes: Created

Date: 
Author: Antoine Plouffe
Verified By: 05/04/2023
Changes: Updated script to move to a specific location based on the device trigger target. Also Viper follows the slope of the terrain and send calculation for % of ice.

Date: 07/04/23
Author: Alexander Achorn
Verified By: 
Changes: Simplifying and removed bloat returning original player follow functionality and a score and distace that should have always been based on the device not the viper
*************************************/

[System.Serializable]
public class ViperMovement: MissionObject, ISaveable
{
    [Header("References")]
    [SerializeField] NavMeshAgent m_NavAgent;
    [SerializeField] Transform m_Player;
    [SerializeField] GameObject m_Viper;

    [Header("Settings")]
    [SerializeField] float m_RotationSpeed;
    public float m_PercOfIceFound;

    bool playerWalkedAway = false;
    public bool finalDestChosen = false;

    void Update()
    {
        if (m_NavAgent.remainingDistance <= 0.1f && !isComplete && isTriggered && playerWalkedAway && !finalDestChosen)
        {
            SetDestination(m_Player.localToWorldMatrix.GetPosition());
        }

        if (m_NavAgent.remainingDistance > 0.1f && !isComplete && isTriggered && playerWalkedAway)
        {
            RaycastHit slopeHit;
            if (Physics.Raycast(m_Viper.transform.position, Vector3.down, out slopeHit, 5.0f))
            {
                if (slopeHit.collider.gameObject.tag == "Terrain")
                {
                    Quaternion newRot = Quaternion.FromToRotation(m_Viper.transform.forward, slopeHit.normal) * m_Viper.transform.rotation;

                    m_Viper.transform.rotation = Quaternion.Lerp(m_Viper.transform.rotation, newRot, Time.deltaTime * m_RotationSpeed);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!playerWalkedAway)
            {
                playerWalkedAway = true;
            }
        }
    }

    public void SetDestination(Vector3 pos)
    {
        if (!isComplete && isTriggered)
        {
            m_NavAgent.destination = pos;
        }
    }

    public object SaveState()
    {
        return new SaveData()
        {
            m_PercOfIceFound = this.m_PercOfIceFound,
        };
    }

    public void LoadState(object state)
    {
        SaveData saveData = (SaveData)state;
        m_PercOfIceFound = saveData.m_PercOfIceFound;
    }


    [Serializable]
    private struct SaveData
    {
        public float m_PercOfIceFound;
    }
}
