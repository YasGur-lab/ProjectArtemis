using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/*************************************
Class: AI_Controller
Author: Antoine Plouffe
Created: 01/11/22
Last Modified: 17/01/23
Description: This class is in charge of the the AI_Controller.

Change Log
**********
Date: 17/01/23
Author: Antoine Plouffe
Verified By: Alexander Achorn
Changes: AIs now uses NavMesh to navigate the terrain instead of following a spline.

Date: 
Author: 
Verified By: 
Changes: 
*************************************/

public class AI_Controller : MonoBehaviour
{
    //Player handling
    [SerializeField] GameObject m_Player;
    //bool m_PlayerNotInCar = false;

    //AI
    [SerializeField] [Range(0, 10)] private int m_DistanceBuffer = 1;
    [SerializeField] private Transform m_Destination;
    private NavMeshAgent m_NavMesh;

    private bool m_IsReadyToMove = false;
    // Start is called before the first frame update
    void Start()
    {
        m_NavMesh = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (m_PlayerNotInCar) return;
        if(m_IsReadyToMove || m_Player.GetComponent<Equipment>().IsSpacesuitOn())
        {
            if (m_NavMesh.isStopped) m_NavMesh.isStopped = false;

            if (!InRangeOfDestination()) MovementBehavior();
                else RotationBehavior();
        }
        else if(m_NavMesh.isStopped == false)
        {
            m_NavMesh.isStopped = true;
            m_NavMesh.ResetPath();
            RotationBehavior();
        }
    }

    private void MovementBehavior()
    {
        m_NavMesh.SetDestination(m_Destination.position);
        m_NavMesh.updateRotation = true;
    }

    private void RotationBehavior()
    {
        LookAtDir(m_Player.transform.position);
    }


    public void LookAtDir(Vector3 direction)
    {
        var rotationAngle = Quaternion.LookRotation(direction - transform.position);
        var rotationSlerp = Quaternion.Slerp(transform.rotation, rotationAngle, Time.deltaTime * m_NavMesh.speed);
        rotationSlerp.x = 0.0f;
        rotationSlerp.z = 0.0f;
        transform.rotation = rotationSlerp;
    }

    public bool InRangeOfPlayer()
    {
        float distanceToPlayer = Vector3.Distance(m_Player.transform.position, transform.position);
        return distanceToPlayer < m_DistanceBuffer;
    }

    public bool InRangeOfDestination()
    {
        float distanceToDestination = Vector3.Distance(m_Destination.position, transform.position);
        return distanceToDestination < m_DistanceBuffer;
    }

    void CancelAIMovement()
    {
        m_NavMesh.isStopped = true;
        m_NavMesh.ResetPath();
    }

    public void SetAIToMove(bool b)
    {
        m_IsReadyToMove = b;
    }

    public void SetDestination(Transform transform)
    {
        m_Destination = transform;
    }
    //public void IsPlayerInCar(bool b)
    //{
    //    m_PlayerNotInCar = b;
    //}
}