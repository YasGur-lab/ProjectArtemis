using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator_Trigger : MonoBehaviour
{
    [SerializeField] Elevator m_Elevator;

    [SerializeField] private bool m_OutsideTrigger = true;
    [SerializeField] private bool m_InsideTrigger = false;

    private void OnTriggerEnter(Collider other)
    {
        // If the player enters the door's collider, start opening the door
        if (other.CompareTag("Player"))
        {
            if(m_OutsideTrigger)
                m_Elevator.OpenDoorsRequested();
            else if(m_InsideTrigger)
                m_Elevator.SetElevatorDirection();

        }
    }
}
