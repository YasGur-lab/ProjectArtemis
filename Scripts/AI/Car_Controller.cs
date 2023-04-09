using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/*************************************
Class: Car_Controller
Author: Antoine Plouffe
Created: 10/16/22
Last Modified: 01/27/23
Description: This class is in charge of Controlling the AI Car.

Change Log
**********
Date: 01/27/23
Author: Antoine Plouffe
Verified By: Alexander Achorn
Changes: Updated for new Scene Handler

Date: 
Author: 
Verified By: 
Changes: 
*************************************/

public class Car_Controller : MonoBehaviour
{
    [SerializeField] IK_Sitting m_PlayerSit;

    private Vector3 m_CarPosition;
    [SerializeField] AI_Path m_AIPath;

    //Waypoint settings
    private int m_CurrentWaypointIndex = 0;
    [Range(0, 10)][SerializeField] private float m_WaypointTolerance = 0.5f;

    //Movements
    private float m_CurveDeltaTime = 0.0f;
    private float m_Speed = 0.0f;
    private float m_InitialSpeed;
    [SerializeField] AnimationCurve m_Curve;
    [Range(0, 10)][SerializeField] private float m_MaxSpeed = 2.0f;

    //Player handleing
    [SerializeField] private bool m_PlayerIsInCar = false;
    private bool m_HasArrived = false;
    [SerializeField] GameObject m_Player;
    [SerializeField] Transform m_InsideAnchor;
    [SerializeField] Transform m_ExitPoint;
    [Range(0, 10)][SerializeField] float m_WaitTime = 2.0f;
    [Range(0, 10)][SerializeField] float m_TimeBeforeCarStops = 0.5f;
    //[Range(0, 10)][SerializeField] float m_TimeBeforeSettingPlayerInCar = 4.0f;

    //Fader
    [SerializeField] Fade m_Fader;
    //float m_CurrentTimeBeforeFadeIn = 0.0f;
    //[SerializeField] float m_MaxTimeBeforeFadeIn = 10.0f;

    //AIs
    [SerializeField] GameObject[] m_Ais;
    [SerializeField] Transform m_PassengerAnchor;

    //Dialogues
    string[] m_Dialogues;
    private bool m_DialogueDone;
    SoundPlayer m_SoundPlayer;

    // Start is called before the first frame update
    void Start()
    {
        m_SoundPlayer = GetComponent<SoundPlayer>();
        //m_Fader = m_Player.GetComponentInChildren<Fade>();
        m_CarPosition = transform.position;
        m_InitialSpeed = m_Speed;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (m_PlayerIsInCar)
        {
            m_PlayerSit.isTriggered = true;

            foreach (var ai in m_Ais)
            {
                ai.transform.parent = this.transform;
            }
            StartCoroutine(PatrolBehavior());
            if(m_SoundPlayer != null)
                StartCoroutine(StartDialogue());
            if (!m_HasArrived)
            {
                m_Player.transform.position = m_InsideAnchor.position;

                if (m_Ais.Length > 1)
                {
                    //m_Ais[1].GetComponent<AI_Controller>().IsPlayerInCar(m_PlayerIsInCar);
                    //m_Ais[1].transform.position = m_PassengerAnchor.position;
                    //m_Ais[1].GetComponent<AI_Controller>().RemovePath();
                    //m_Ais[1].GetComponent<AI_Controller>().LookAtDir(GetDirection(GetCurrentWaypoint()));
                }
            }
            else
            {
                m_Player.transform.position = m_ExitPoint.position;
                m_PlayerIsInCar = false;
                foreach (var ai in m_Ais)
                {
                    ai.transform.parent = ai.transform;
                }
            }

        }

        if (m_PlayerIsInCar)
        {
        }

        if (m_HasArrived) m_PlayerIsInCar = false;
    }

    private IEnumerator StartDialogue()
    {
        yield return new WaitForSeconds(5);
        if (!m_DialogueDone)
        {
           /* m_Dialogues = new string[3];
            m_Dialogues[0] = "Buckle in everyone its a bit of a trek. Kennedy Space Center is 570 square kilometers, and we've gotta leave now to stay on schedule.";
            m_Dialogues[1] = "You know, we get some crazy lightning storms out here. They had to put lightening protection everywhere.";
            m_Dialogues[2] = "They even have a lightening detection device. Diane showed it to me once, it's pretty cool.";
            m_SoundPlayer.SetDialogues(m_Dialogues);
            m_SoundPlayer.PlayDisk("Leaving_Scenario_1", 1);*/
            m_DialogueDone = true;
        }
        yield return new WaitForSeconds(m_SoundPlayer.GetSumTrackLength());
    }

    private IEnumerator PatrolBehavior()
    {
        yield return new WaitForSeconds(m_WaitTime);

        Vector3 nextPos = m_CarPosition;

        if (m_AIPath)
        {
            if (AtWaypoint())
            {
                if (GetCurrentWaypoint() == GetFinalWaypoint())
                {
                    yield return new WaitForSeconds(m_TimeBeforeCarStops);
                    m_HasArrived = true;
                    
                    yield break;
                }
                CycleWaypoint();
            }
            nextPos = GetCurrentWaypoint();
        }

        Vector3 newDir = Vector3.RotateTowards(transform.forward, GetDirection(nextPos), Time.deltaTime, 0.0F);

        transform.rotation = Quaternion.LookRotation(newDir);
        m_Speed += GetCurveValue();
        m_Speed = Mathf.Clamp(m_Speed, m_InitialSpeed, m_MaxSpeed);
        transform.position += transform.forward * m_Speed * Time.deltaTime;
    }

    private Vector3 GetFinalWaypoint()
    {
        return m_AIPath.GetFinalWaypoint(m_CurrentWaypointIndex);
    }

    private Vector3 GetCurrentWaypoint()
    {
        return m_AIPath.GetWaypoint(m_CurrentWaypointIndex);
    }

    private void CycleWaypoint()
    {
        m_CurrentWaypointIndex = m_AIPath.GetNextWaypoint(m_CurrentWaypointIndex);
    }

    private bool AtWaypoint()
    {
        float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
        return distanceToWaypoint < m_WaypointTolerance;
    }

    private Vector3 GetDirection(Vector3 WaypointToGoTo)
    {
        return WaypointToGoTo - transform.position;
    }

    private float GetCurveValue()
    {
        m_CurveDeltaTime += Time.deltaTime;
        return m_Curve.Evaluate(m_CurveDeltaTime);
    }

    public IEnumerator SetPlayerInCar()
    {
        m_Fader.FadeIn();
        yield return new WaitForSeconds(m_Fader.GetFadeDuration());
        if (!m_PlayerIsInCar)
            m_PlayerIsInCar = true;
        m_Fader.FadeOut();
        yield return new WaitForSeconds(m_Fader.GetFadeDuration());
    }

    public void SetPlayerInCarViaTeleports()
    {
        StartCoroutine(SetPlayerInCar());
    }

    public void SetCarToStart()
    {
        if (!m_PlayerIsInCar)
            m_PlayerIsInCar = true;
    }

    public bool IsPlayerInCar() => m_PlayerIsInCar;
}
