using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*************************************
Class: LanderElevator
Author: Antoine Plouffe
Created: 06/04/2023
Last Modified: 06/04/2023
Description: This class is in charge of the lander elevator mechanic.

Change Log
**********
Date: 06/04/2023
Author: Antoine Plouffe
Verified By: 
Changes: Created.

Date: 07/04/23
Author: Alexander Achorn
Verified By: 
Changes: Changed what was needed to prevent the elevator from breaking the players arms when going up and allowed it to be set to be down at the start of a level.
Also stripped alot of unused code
*************************************/

public class LanderElevator : MonoBehaviour
{
    [Header("VALUES")]
    [SerializeField] float ElevatorMoveSpeed = 1.0f;

    //FLOORS
    [Header("FLOORS")]
    [SerializeField] GameObject m_MainFloor;
    [SerializeField] GameObject m_FirstFloor;

    //LR
    [Header("LR_WIRES")]
    [SerializeField] private LineRenderer[] m_LRs;
    [SerializeField] private Transform[] m_TopPos_LRs;
    [SerializeField] private Transform[] m_BottomPos_LRs;

    [Header("Elevator Position")]
    [SerializeField] bool ShouldMoveElevatorUp;
    [SerializeField] bool ShouldMoveElevatorDown;
    bool m_HasReachDestination = false;

    // Update is called once per frame
    void Update()
    {
        if (ShouldMoveElevatorDown)
        {
            if (transform.position != m_MainFloor.transform.position)
            {
                StartCoroutine(MovingElevatorDown());
            }
            else
            {
                m_HasReachDestination = true;
                ShouldMoveElevatorDown = false;
            }
        }

        if (ShouldMoveElevatorUp)
        {
            if (transform.position != m_FirstFloor.transform.position)
            {
                StartCoroutine(MovingElevatorUp());
            }
            else
            {
                m_HasReachDestination = true;
                ShouldMoveElevatorUp = false;
            }
        }

        if (m_HasReachDestination)
        {
            m_HasReachDestination = false;
        }

        if (m_TopPos_LRs.Length > 0 && m_BottomPos_LRs.Length > 0 && m_LRs.Length > 0 && ShouldMoveElevatorUp || ShouldMoveElevatorDown)
        {
            for (int i = 0; i < m_LRs.Length; i++)
            {
                m_LRs[i].SetPosition(0, m_TopPos_LRs[i].position);
                m_LRs[i].SetPosition(1, m_BottomPos_LRs[i].transform.position);
            }
        }
    }

    IEnumerator MovingElevatorUp()
    {
        yield return new WaitForSeconds(0);
        transform.position = Vector3.MoveTowards(transform.position, m_FirstFloor.transform.position, ElevatorMoveSpeed * Time.deltaTime);
    }
    IEnumerator MovingElevatorDown()
    {
        yield return new WaitForSeconds(0);
        transform.position = Vector3.MoveTowards(transform.position, m_MainFloor.transform.position, ElevatorMoveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (transform.position != m_MainFloor.transform.position && !ShouldMoveElevatorDown)
            {
                ShouldMoveElevatorDown = true;
            }
            else if (transform.position != m_FirstFloor.transform.position && !ShouldMoveElevatorUp)
            {
                ShouldMoveElevatorUp = true;
            }
        }
    }
}
