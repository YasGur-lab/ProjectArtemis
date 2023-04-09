using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthRise_Trigger : MonoBehaviour
{
    private EarthRise m_EarthRise;
    // Start is called before the first frame update
    void Start()
    {
        m_EarthRise = FindObjectOfType<EarthRise>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the player enters the door's collider, start opening the door
        if (other.CompareTag("Player"))
        {
            m_EarthRise.SetPlayerIsInPlace(true);
        }
    }
}
