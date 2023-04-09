using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDoor_Trigger : MonoBehaviour
{
    [SerializeField] Car_Controller m_Car;

    private void OnTriggerEnter(Collider other)
    {
        if (!m_Car.IsPlayerInCar() && other.gameObject.tag == "Player")
        {
            StartCoroutine(m_Car.SetPlayerInCar());
        }
    }
}
