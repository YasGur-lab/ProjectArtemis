using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UIElements;

/*************************************
Class: SavingSystem
Author: Antoine Plouffe
Created: 02/01/2023
Last Modified: 02/01/2023
Description: This class is in charge of the EarthRise.

Change Log
**********
Date: 02/01/2023
Author: Antoine Plouffe
Verified By: Alexander Achorn
Changes: Created.

Date: 
Author: 
Verified By: 
Changes: 
*************************************/
public class EarthRise : MissionObject
{
    [SerializeField] GameObject m_Target;

    //[SerializeField] Slider m_Sider;

    [SerializeField] float m_Speed = 1;
    [SerializeField] float m_Radius = 1;
    [SerializeField] float m_Offset = 0;
    [SerializeField] float m_MaxWatchTimer = 15.0f;

    private Vector3 m_PositionOffset;
    private float m_Angle;
    private float m_WatchTimer;

    private bool m_PlayerIsInPlace;

    public EarthRiseStages m_Stage;
    public enum EarthRiseStages
    {
        PlayerHasArrivedInMission1Scene,
        PlayerHasArrivedToMissionMarker,
        Complete,
    }

    void Start()
    {
        m_Stage = EarthRiseStages.PlayerHasArrivedInMission1Scene;
        //m_Sider = GameObject.Find("Tablet").GetComponentInChildren<Slider>();
    }

    private void Update()
    {
        if (m_Stage == EarthRiseStages.PlayerHasArrivedInMission1Scene)
        {
            //Wow….!
            //Let’s try to find a higher ground.Maybe over there!
            if (m_PlayerIsInPlace) m_Stage = EarthRiseStages.PlayerHasArrivedToMissionMarker;
        }

        if (isTriggered && m_PlayerIsInPlace)
        {
            //AjustRiseSpeed();

            m_PositionOffset.Set(Mathf.Cos(m_Angle) * m_Radius, Mathf.Sin(m_Angle) * m_Radius, m_Offset);
            transform.position = m_Target.transform.position + m_PositionOffset;

            float angle = Vector3.Angle(-m_Target.transform.forward, transform.position - m_Target.transform.position);

            angle += 90f;

            transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);

            m_Angle += Time.deltaTime * m_Speed;
            m_WatchTimer += Time.deltaTime;

            if (m_Stage == EarthRiseStages.PlayerHasArrivedToMissionMarker)
            {
                //Oh my God! Look at that picture over there!
                //There's the Earth coming up. Wow, that's pretty.

                if (m_WatchTimer > m_MaxWatchTimer)
                {
                    m_Stage = EarthRiseStages.PlayerHasArrivedToMissionMarker;
                }
            }
            else if (m_Stage == EarthRiseStages.Complete)
            {
                //Maybe we should get going. We need to locate ice to replenish our reserves.
                isComplete = true;
            }
        }
    }

    public void AjustRiseSpeed()
    {
        //m_Speed = m_Sider.value;
    }

    public void SetPlayerIsInPlace(bool b)
    {
        m_PlayerIsInPlace = b;
    }
}
