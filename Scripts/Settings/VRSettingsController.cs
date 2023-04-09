using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;
using UnityEngine.Rendering;
using UnityEngine.XR.Interaction.Toolkit;

/*************************************
Class: VRSettingsController
Author: Alexander Achorn
Created: 20/01/23
Last Modified: 20/01/23
Description: Manages all VR related prefrence and comfort settings.

Change Log
**********
Date: 20/01/23
Author: Alexander Achorn
Verified By: Antoine Plouffe
Changes: Setup the basics for toggling Head vs Hands forward.

Date: 
Author: 
Verified By: 
Changes: 
*************************************/

public class VRSettingsController : MonoBehaviour, ISaveable
{
    [Serializable]
    private struct SaveData
    {
        public bool m_FreeLocomotion;
    
        public bool m_ControllerRotation;
        public bool m_SnapRotation;
        public float m_SnapDegrees;
        
        public bool m_HandsForward;
        public bool m_RightHanded;

        public float m_playerHeight;
    }

    public bool m_FreeLocomotion = true;

    public bool m_ControllerRotation = true;
    public bool m_SnapRotation = false;
    public float m_SnapDegrees = 5.0f; //Num of degrees

    public bool m_HandsForward = false;
    public bool m_RightHanded = true;

    float m_playerHeight = 1.68f;

    public XROrigin origin;
    public GameObject xrOrigin;
    public Head head;
    Vector3 defaultHeadOffset;

    bool changedFreeLoco = false;
    bool changedControllerRot = false;
    bool changedSnapRot = false;

    // Start is called before the first frame update
    void Start()
    {
        if(head)
        {
            defaultHeadOffset = head.GetHeadBodyOffset();
        } 
    }

    // Update is called once per frame
    void Update()
    {
        if(!m_FreeLocomotion && changedFreeLoco)
        {
            //Disable Movement Stick
            origin.GetComponent<ActionBasedContinuousMoveProvider>().enabled = false;
            changedFreeLoco = false;
        }
        else if(changedFreeLoco)
        {
            origin.GetComponent<ActionBasedContinuousMoveProvider>().enabled = true;
            changedFreeLoco = false;
        }

        if (!m_ControllerRotation && changedControllerRot)
        {
            origin.GetComponent<ActionBasedContinuousTurnProvider>().enabled = false;
            changedControllerRot = false;
        }
        else if(changedControllerRot)
        {
            origin.GetComponent<ActionBasedContinuousTurnProvider>().enabled = true;
            origin.GetComponent<ActionBasedSnapTurnProvider>().enabled = false;
            m_SnapRotation = false;
            changedControllerRot = false;
        }

        if(!m_SnapRotation && changedSnapRot)
        {
            origin.GetComponent<ActionBasedSnapTurnProvider>().enabled = false;
            changedSnapRot = false;
        }
        else if(changedSnapRot)
        {
            origin.GetComponent<ActionBasedContinuousTurnProvider>().enabled = false;
            m_ControllerRotation = false;
            origin.GetComponent<ActionBasedSnapTurnProvider>().enabled = true;
            changedSnapRot = false;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            SetPlayerHeight();
        }
    }

    //Setters
    public void SetFreeLocomotion(bool state)
    {
        m_FreeLocomotion = state;
        changedFreeLoco = true;
    }

    public void SetControllerRotation(bool state)
    {
        m_ControllerRotation = state;
        changedControllerRot = true;
    }

    public void SetSnapRotation(bool state)
    {
        m_SnapRotation = state;
        changedSnapRot = true;
    }

    public void SetSnapDegrees(float degrees)
    {
        m_SnapDegrees = degrees;
        origin.GetComponent<ActionBasedSnapTurnProvider>().turnAmount = m_SnapDegrees;
    }

    public void SetHandsForward(bool state)
    {
        m_HandsForward = state;
    }

    public void SetRightHanded(bool state)
    {
        m_FreeLocomotion = state;
    }

    //Getters
    public bool UsingFreeLocomotion()
    {
        return m_FreeLocomotion;
    }

    public bool UsingControllerRotation()
    {
        return m_ControllerRotation;
    }

    public bool UsingSnapRotation()
    {
        return m_SnapRotation;
    }

    public float GetSnapDegrees()
    {
        return m_SnapDegrees;
    }

    public bool UsingHandsForward()
    {
        return m_HandsForward;
    }

    public bool IsRightHanded()
    {
        return m_RightHanded;
    }


    //Save & Load
    public object SaveState()
    {
        return new SaveData()
        {
            m_FreeLocomotion = this.m_FreeLocomotion,

            m_ControllerRotation = this.m_ControllerRotation,
            m_SnapRotation = this.m_SnapRotation,
            m_SnapDegrees = this.m_SnapDegrees,

            m_HandsForward = this.m_HandsForward,
            m_RightHanded = this.m_RightHanded,

            m_playerHeight = this.m_playerHeight,
        };
    }

    public void LoadState(object state)
    {
        SaveData saveData = (SaveData)state;

        m_FreeLocomotion = saveData.m_FreeLocomotion;

        m_ControllerRotation = saveData.m_ControllerRotation;
        m_SnapRotation = saveData.m_SnapRotation;
        m_SnapDegrees = saveData.m_SnapDegrees;

        m_HandsForward = saveData.m_HandsForward;
        m_RightHanded = saveData.m_RightHanded;

        m_playerHeight = saveData.m_playerHeight;

    }

    public void SetPlayerHeight()
    {
        m_playerHeight = origin.CameraInOriginSpaceHeight;

        if(m_playerHeight != 1.68)
        {
            float newScale = (m_playerHeight / 1.68f) * 1;

            if(newScale > 0)
            {
                xrOrigin.transform.localScale = new Vector3(newScale, newScale, newScale);

                Vector3 newHeadScale = defaultHeadOffset;

                newHeadScale = new Vector3(newHeadScale.x * newScale, newHeadScale.y * newScale, newHeadScale.z * newScale);

                head.SetHeadBodyOffset(newHeadScale);
            }
        }
    }

    public float GetPlayerHeight()
    {
        return m_playerHeight;
    }
}
