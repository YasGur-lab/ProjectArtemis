using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.XR.CoreUtils;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.XR.Interaction.Toolkit;

/*************************************
Class: WirePanel_MiniGame
Author: Antoine Plouffe
Created: 01/02/2023
Last Modified: 02/02/2023
Description: This class is in charge of the WirePanel MiniGame.

Change Log
**********
Date: 01/02/2023
Author: Antoine Plouffe
Verified By: Alexander Achorn
Changes: Created.

Date: 07/03/2023
Author: Antoine Plouffe
Verified By: Alexander Achorn
Changes: Moves screws back onto the panel when the minigame is complete.

Date: 
Author: 
Verified By: 
Changes: 
*************************************/

public class WirePanel_MiniGame : MissionObject
{
    [SerializeField][Range(0.0f, 10.0f)] float m_PullingDistance = 1.0f;
    [SerializeField] private LineRenderer[] m_Wire;

    [SerializeField] private Transform[] m_WireStart;
    [SerializeField] private Transform[] m_WireEnd;

    [SerializeField] private Transform[] m_StartSocket;
    [SerializeField] private Transform[] m_EndSocket;

    [SerializeField] private GameObject[] m_GrabbableEndPoint;

    [SerializeField] private GameObject m_Battery;
    [SerializeField] private Transform m_BatteryEndSocket;
    [SerializeField] private XRSocketInteractor m_BatterySocketInteractor;

    [SerializeField] private Transform[] m_CurrentSocket;
    [SerializeField] private Transform[] m_HeadingTowardsSocket;

    private bool[] m_WireAreReadyToBeReconnected;

    [SerializeField] private int m_NumberOfSocketWhoHasSelection;

    private bool m_MiniGameCompleted;

    [SerializeField] GameObject m_ScrewedPanel;
    [SerializeField] GameObject m_Robot;

    //Battery
    [SerializeField] GameObject m_NewBattery;
    [SerializeField] Transform m_NewBatterySocket;
    private bool m_BatterySwitched;

    [SerializeField] Mission2Audio missionAudio;

    //WireHandling
    [SerializeField] bool[] m_HoldingWires;
    bool m_WireArePositionned;
    bool m_InteractionLayerReset;
    float m_TimerBeforeHoldingWireFunctionIsActivated = 0.0f;
    float m_MaxTimerBeforeHoldingWireFunctionIsActivated = 1.0f;

    int[] m_StoredInteractionLayers;

    //Player
    private GameObject m_Player;

    public MiniGameStages m_Stage;
    public enum MiniGameStages
    {
        PlayerHasArrivedInLavaTubeScene,
        PlayerIsInFrontOfAsagumo,
        DrillTaken,
        FirstScrewOff,
        ScrewsAreOff,
        PlayerIsGrabbingFirstWire,
        PlayerIsGrabbingSecondWire,
        WiresAreDisconnected,
        BatteryWasReplaced,
        ScrewsAreBackOn,
        Complete,
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Player = FindObjectOfType<XROrigin>().gameObject;
        m_Stage = MiniGameStages.PlayerHasArrivedInLavaTubeScene;
        m_HoldingWires = new bool[m_Wire.Length];
        m_StoredInteractionLayers = new int[m_Wire.Length];
        m_TimerBeforeHoldingWireFunctionIsActivated = m_MaxTimerBeforeHoldingWireFunctionIsActivated;

        for (int i = 0; i < m_Wire.Length; i++)
        {
            m_StoredInteractionLayers[i] = m_GrabbableEndPoint[i].GetComponent<XRGrabInteractable>().interactionLayers;
        }


        //m_RobotFixedTransform.rotation = new Quaternion(-90, 0, 0, 0);
        for (int i = 0; i < m_Wire.Length; i++)
        {
            m_Wire[i].SetPosition(0, m_WireStart[i].position);
        }
        
        m_WireAreReadyToBeReconnected = new bool[3];
        
        if (m_Battery.GetComponent<XRGrabInteractable>().enabled)
            m_Battery.GetComponent<XRGrabInteractable>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_MiniGameCompleted)
        {
            for (int i = 0; i < m_Wire.Length; i++)
            {
                m_Wire[i].SetPosition(1, m_WireEnd[i].position);
            }
            SwitchBattery();


            if(!m_ScrewedPanel.GetComponent<ScrewedPanel>().AreScrewsOn() && !m_WireArePositionned)
            {
                //Debug.Log("Screws off?: " + m_ScrewedPanel.GetComponent<ScrewedPanel>().AreScrewsOn());
                m_TimerBeforeHoldingWireFunctionIsActivated -= Time.deltaTime;
                if (m_TimerBeforeHoldingWireFunctionIsActivated < 0.0f)
                {
                    //Debug.Log("WireArePositionned: " + m_WireArePositionned);
                    m_WireArePositionned = true;
                }
            }

            for (int i = 0; i < m_HoldingWires.Length; i++)
            {
                if (m_HoldingWires[i] == true)
                {
                    //Debug.Log("Wire currently holding: " + i);
                    if (DistanceFromCurrentSocketPosition(i) > m_PullingDistance && !m_BatterySwitched)
                    {
                        //Debug.Log("Detaching wire: " + i);
                        m_GrabbableEndPoint[i].GetComponent<XRGrabInteractable>().interactionLayers = 0;
                        SetWireToBeReconnected(i);
                    }
                }
            }

            if (m_BatterySwitched && !m_InteractionLayerReset)
            {
                for (int i = 0; i < m_Wire.Length; i++)
                {
                    m_GrabbableEndPoint[i].GetComponent<XRGrabInteractable>().interactionLayers = m_StoredInteractionLayers[i];
                    SwapTargetedSocket(i);
                }
                m_InteractionLayerReset = true;
            }
        }

        if (m_Stage == MiniGameStages.PlayerHasArrivedInLavaTubeScene)
        {
            if (Vector3.Distance(m_Player.transform.position, transform.position) < 1.0f)
            {
                m_Stage = MiniGameStages.PlayerIsInFrontOfAsagumo;
            }
            //We need to find the Asagumo quickly. It isn’t responding. 
        }
        else if (m_Stage == MiniGameStages.PlayerIsInFrontOfAsagumo)
        {
            //Oh my..! What happened to it. Let’s try to replace the defective battery with the spare one. 
            //Do you see the 4 screws on its panel? You need to remove them in order to access the wires and battery.
            //To do so, grab your drill from its storage location on your left side.

            //switch to MiniGameStages.TakesTheDrill is happening in the ClimbingInteractor by calling : DrillHasBeenTaken()
        }
        else if (m_Stage == MiniGameStages.DrillTaken)
        {
            //To unscrew a screw, just place the drill's tip on the screw head and turn on the drill.

            if (m_ScrewedPanel.GetComponent<ScrewedPanel>().GetNumberOfScrewsOff() == 1)
                m_Stage = MiniGameStages.FirstScrewOff;
        }
        else if (m_Stage == MiniGameStages.FirstScrewOff)
        {
            //Grab the screw with your other hand and place it in the bucket next to you.
            //Do the same with the remaining screws.

            if (m_ScrewedPanel.GetComponent<ScrewedPanel>().AreScrewsOn() == false)
                m_Stage = MiniGameStages.ScrewsAreOff;
        }
        else if (m_Stage == MiniGameStages.ScrewsAreOff)
        { 
            //Great, now you have a clear view of the interior.
            //In order to take out the faulty battery, you must detach the connected wires. Carefully remove them individually by pulling them out.
            
            if(PlayerHasTakenWire()) m_Stage = MiniGameStages.PlayerIsGrabbingFirstWire;
        }
        else if (m_Stage == MiniGameStages.PlayerIsGrabbingFirstWire)
        {
            //That’s it!Pull, pull, pull!

            if (!PlayerHasTakenWire())
            {
                //m_DialogueForReleasingFirstGrabbedWire
                //You’ve done it! Repeat the same process with the remaining two.
            }

            if (PlayerHasTakenWire()) m_Stage = MiniGameStages.PlayerIsGrabbingSecondWire; //&& m_DialogueForReleasingFirstGrabbedWire


        }
        else if (m_Stage == MiniGameStages.PlayerIsGrabbingSecondWire)
        {
            if (!PlayerHasTakenWire())
            {
                //m_DialogueForReleasingSecondGrabbedWire
                ////Last one!
            }
            if (AreWiresReadyToBeReconnected()) m_Stage = MiniGameStages.WiresAreDisconnected; //&& m_DialogueForReleasingSecondGrabbedWire
        }
        else if (m_Stage == MiniGameStages.WiresAreDisconnected)
        {
            //Now replace the defective battery with the replacement one.
            if (m_BatterySwitched) m_Stage = MiniGameStages.BatteryWasReplaced;
        }
        else if (m_Stage == MiniGameStages.BatteryWasReplaced)
        {
            //Finally, secured the panel by screwing all four screws.
            if (m_ScrewedPanel.GetComponent<ScrewedPanel>().AreScrewsOn()) m_Stage = MiniGameStages.ScrewsAreBackOn;
        }
        else if (m_Stage == MiniGameStages.ScrewsAreBackOn)
        {
            //That will do it! The Asagumo is back and running! Let’s go back to the Lander.
            m_Stage = MiniGameStages.Complete;
        }
    }

    private bool PlayerHasTakenWire()
    {
        foreach (var wire in m_HoldingWires)
        {
            if (wire == true)
            {
                return true;
            }
        }
        return false;
    }

    public void DrillHasBeenTaken()
    {
        m_Stage = MiniGameStages.DrillTaken;
    }

    public void HoldingWire(int i)
    {
        if (m_WireArePositionned)
        {
            //Debug.Log("setting HoldingWire to true");
            m_HoldingWires[i] = true;
        }
    }

    public void ResetWirePosition(int i)
    {
        m_HoldingWires[i] = false;

        if (DistanceFromCurrentSocketPosition(i) < m_PullingDistance)
            m_GrabbableEndPoint[i].transform.position = m_CurrentSocket[i].position;
        else if (DistanceFromCurrentSocketPosition(i) > m_PullingDistance)
        {
            m_GrabbableEndPoint[i].transform.position = m_HeadingTowardsSocket[i].position;
        }
    }

    void SwapTargetedSocket(int i)
    {
        (m_HeadingTowardsSocket[i], m_CurrentSocket[i]) = (m_CurrentSocket[i], m_HeadingTowardsSocket[i]);
    }


    private float DistanceFromEndPoint(int i)
    {
        return Vector3.Distance(m_GrabbableEndPoint[i].transform.position, m_HeadingTowardsSocket[i].transform.position);
    }

    private float DistanceFromCurrentSocketPosition(int i)
    {
        return Vector3.Distance(m_GrabbableEndPoint[i].transform.position, m_CurrentSocket[i].transform.position);
    }

    public void IsGameComplete()
    {
        StartCoroutine(IsGameCompleteDelay());
    }

    IEnumerator IsGameCompleteDelay()
    {
        yield return new WaitForSeconds(2f);
        if (AreWiresReconnected() && IsBatteryConnected() && !m_MiniGameCompleted)
        {
            //robot is on
            m_MiniGameCompleted = true;
            if (m_ScrewedPanel.activeSelf == false)
            {
                m_ScrewedPanel.SetActive(true);
                m_ScrewedPanel.GetComponent<ScrewedPanel>().MoveScrewsOntoPanel();
            }

            isComplete = true;
            missionAudio.m_fixed = true;
        }
    }

    bool AreWiresReconnected()
    {
        if (AreWiresReadyToBeReconnected())
        {
            for (int i = 0; i < m_EndSocket.Length; i++)
            {
                if (!m_EndSocket[i].GetComponent<XRSocketInteractor>().hasSelection)
                {
                    if (m_Battery)
                    {
                        if (!m_Battery.GetComponent<XRGrabInteractable>().enabled)
                            m_Battery.GetComponent<XRGrabInteractable>().enabled = true;
                        ResetGravity();
                        missionAudio.m_removeBat = true;

                    }

                    return false;
                }
            }
            return true;
        }
        return false;
    }

    public void SwitchBattery()
    {
        if (m_BatterySocketInteractor.hasSelection &&
            m_BatterySocketInteractor.GetOldestInteractableSelected().transform.gameObject == m_NewBattery)
        {
            if (!m_BatterySwitched)
            {
                for (int i = 0; i < m_EndSocket.Length; i++)
                {
                    m_NewBattery.transform.GetChild(i).transform.GetChild(0).transform.gameObject.SetActive(true);
                    m_EndSocket[i] = m_NewBattery.transform.GetChild(i).transform.GetChild(0);
                }

                m_BatterySwitched = true;
                m_Stage = MiniGameStages.BatteryWasReplaced;

            }
        }
    }

    public void ResetGravity()
    {
        if(m_Battery.GetComponent<Rigidbody>().isKinematic)
            m_Battery.GetComponent<Rigidbody>().isKinematic = false;
    }
    bool IsBatteryConnected()
    {
        if (Vector3.Distance(m_BatteryEndSocket.transform.position, m_NewBatterySocket.position) > 0.1f)
            return false;
        return true;
    }

    public void SetWireToBeReconnected(int i)
    {
        m_WireAreReadyToBeReconnected[i] = true;
    }

    bool AreWiresReadyToBeReconnected()
    {
        foreach (var reconnectedWire in m_WireAreReadyToBeReconnected)
        {
            if (!reconnectedWire) return false;
        }
        return true;
    }

    public bool GetMiniGameStatus()
    {
        return m_MiniGameCompleted;
    }
}
