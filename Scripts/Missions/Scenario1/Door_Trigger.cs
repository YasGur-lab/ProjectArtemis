using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Door_Trigger : MonoBehaviour
{
    [SerializeField] DoorOpener[] m_Doors;
    private void OnTriggerEnter(Collider other)
    {
        // If the player enters the door's collider, start opening the door
        if (other.CompareTag("Player") || other.CompareTag("NPC"))
        {
            foreach (var door in m_Doors)
            {
                door.Open();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If the player exits the door's collider, close the door
        if (other.CompareTag("Player") || other.CompareTag("NPC"))
        {
            foreach (var door in m_Doors)
            {
                door.Close();
            }
        }
    }

    public void SetReadyToBeOpen()
    {
        foreach (var door in m_Doors)
        {
            door.SetReadyToBeOpen(true);
        }
    }
}
