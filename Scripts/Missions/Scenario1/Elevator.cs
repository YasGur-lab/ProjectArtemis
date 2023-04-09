using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*************************************
Class: Elevator
Author: Antoine Plouffe
Created: 01/11/22
Last Modified: 17/01/23
Description: This class is in charge of the elevator mechanic.

Change Log
**********
Date: 17/01/23
Author: Antoine Plouffe
Verified By: Alexander Achorn
Changes: Updating the elevator sequences. Cleaning code.

Date: 
Author: 
Verified By: 
Changes: 
*************************************/

public class Elevator : MonoBehaviour
{
    Vector3 ElevatorPosition;
    Vector3 InitialElevatorPosition;
    Vector3 ElevatorDirection;
    Vector3 ElevatorDestination;

    [Header("VALUES")]
    [SerializeField] float ElevatorMoveDist = 20;
    [SerializeField] float ElevatorMoveSpeed = 1.0f;
    [SerializeField] float DoorMoveSpeed = 1.0f;

    //PASSENGER HANDLER
    [Header("PASSENGER HANDLER")]
    GameObject[] m_Passengers;
    [SerializeField] Transform m_PlayerParent;
    [SerializeField] Transform m_NPCParent;

    [SerializeField] GameObject m_MainFloor;
    [SerializeField] GameObject m_FirstFloor;

    //DOORS
    [Header("DOORS")]
    [SerializeField] GameObject[] m_Doors;
    [SerializeField] GameObject[] m_DoorsClosePosition;
    [SerializeField] GameObject[] m_DoorsOpenPosition;

    //PANNEL
    [Header("PANNEL")]
    [SerializeField] GameObject m_Panel;
    [SerializeField] GameObject m_InsidePanel;

    [Header("DEBUG")]
    [SerializeField] bool ShouldMoveElevatorUp;
    [SerializeField] bool ShouldMoveElevatorDown;
    [SerializeField] bool m_OpenDoorsRequested = false;
    bool m_CloseDoorsRequested = false;
    bool HasElevatorGoneDown = false;
    bool m_HasReachDestination = false;
    bool m_ReadyToGoDown = false;

    void Start()
    {
        ShouldMoveElevatorUp = false;
        ShouldMoveElevatorDown = false;

        ElevatorPosition = transform.position;
        InitialElevatorPosition = transform.position;
        ElevatorDirection = transform.up;
        ElevatorDestination = InitialElevatorPosition + (ElevatorDirection * ElevatorMoveDist);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ShouldMoveElevatorDown)
        {
            if (transform.position != m_MainFloor.transform.position)
            {
                StartCoroutine(CloseDoors(0));
                if (AreDoorsClosed()) StartCoroutine(MovingElevatorDown());
            }
            else
            {
                m_ReadyToGoDown = false;
                m_HasReachDestination = true;
                HasElevatorGoneDown = true;

                StartCoroutine(OpenDoors());
                if (AreDoorsOpen()) ShouldMoveElevatorDown = false;
            }
        }

        if (ShouldMoveElevatorUp)
        {
            if (transform.position != m_FirstFloor.transform.position)
            {
                StartCoroutine(CloseDoors(0));
                if (AreDoorsClosed()) StartCoroutine(MovingElevatorUp());
            }
            else
            {
                m_ReadyToGoDown = false;
                m_HasReachDestination = true;
                HasElevatorGoneDown = false;

                StartCoroutine(OpenDoors());
                if (AreDoorsOpen()) ShouldMoveElevatorUp = false;
            }
        }

        if (m_HasReachDestination)
        {
            StartCoroutine(CloseDoors(5));
            if (AreDoorsClosed()) m_HasReachDestination = false;
        }

        if (m_OpenDoorsRequested)
        {
            StartCoroutine(OpenDoors());
            m_ReadyToGoDown = true;
            if (AreDoorsOpen()) m_OpenDoorsRequested = false;
        }

        if (m_CloseDoorsRequested)
        {
            StartCoroutine(CloseDoors(0));
            if (AreDoorsClosed()) m_CloseDoorsRequested = false;
        }

        if (m_ReadyToGoDown)
            m_InsidePanel.GetComponent<Highlight>().IsActive(true);

    }

    public IEnumerator CloseDoors(float timer)
    {
        yield return new WaitForSeconds(timer);
        for (int i = 0; i < m_Doors.Length; i++)
        {
            m_Doors[i].transform.position = Vector3.MoveTowards(m_Doors[i].transform.position, m_DoorsClosePosition[i].transform.position, DoorMoveSpeed * Time.deltaTime);
        }
    }
    public IEnumerator OpenDoors()
    {
        yield return new WaitForSeconds(0);

        for (int i = 0; i < m_Doors.Length; i++)
        {
            m_Doors[i].transform.position = Vector3.MoveTowards(m_Doors[i].transform.position, m_DoorsOpenPosition[i].transform.position, DoorMoveSpeed * Time.deltaTime);
        }
    }

    public bool AreDoorsClosed()
    {
        if (m_Doors[0].transform.position == m_DoorsClosePosition[0].transform.position &&
            m_Doors[1].transform.position == m_DoorsClosePosition[1].transform.position)
            return true;
        else return false;
    }

    private bool AreDoorsOpen()
    {
        if (m_Doors[0].transform.position == m_DoorsOpenPosition[0].transform.position &&
            m_Doors[1].transform.position == m_DoorsOpenPosition[1].transform.position)
            return true;
        else return false;
    }

    IEnumerator MovingElevatorUp()
    {
        //HasElevatorGoneDown = false;
        yield return new WaitForSeconds(0);
        transform.position = Vector3.MoveTowards(transform.position, m_FirstFloor.transform.position, ElevatorMoveSpeed * Time.deltaTime);
    }
    IEnumerator MovingElevatorDown()
    {
        //HasElevatorGoneDown = true;
        yield return new WaitForSeconds(0);
        transform.position = Vector3.MoveTowards(transform.position, m_MainFloor.transform.position, ElevatorMoveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "NPC")
        {
            other.transform.parent = this.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player") other.transform.parent = m_PlayerParent;
        else if(other.gameObject.tag == "NPC") other.transform.parent = m_NPCParent;
    }

    public void SetElevatorDirection()
    {
        if (HasElevatorGoneDown)
        {
            HasElevatorGoneDown = false;
            ShouldMoveElevatorUp = true;
            ShouldMoveElevatorDown = false;

        }
        else if (!HasElevatorGoneDown)
        {
            HasElevatorGoneDown = true;
            ShouldMoveElevatorUp = false;
            ShouldMoveElevatorDown = true;
        }
    }

    public void OpenDoorsRequested()
    {
        if (!m_OpenDoorsRequested)
        {
            m_OpenDoorsRequested = true;
            m_ReadyToGoDown = true;
            m_Panel.GetComponent<Highlight>().ClearLists();
        }
    }

    public bool IsReadyToGoDown()
    {
        return m_ReadyToGoDown;
    }

    public bool IsAtCarFloor()
    {
        if (transform.position == m_MainFloor.transform.position) return true;
        else return false;
    }

    public void SetElevatorGoingDown()
    {
        ShouldMoveElevatorUp = false;
        ShouldMoveElevatorDown = true;
    }

    public void SetElevatorGoingUp()
    {
        ShouldMoveElevatorUp = true;
        ShouldMoveElevatorDown = false;
    }
}
