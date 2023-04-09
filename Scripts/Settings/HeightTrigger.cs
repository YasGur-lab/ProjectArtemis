using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightTrigger : MonoBehaviour
{
    private Transform m_PlayerStartingPos;
    // Start is called before the first frame update
    void Start()
    {
        m_PlayerStartingPos = GameObject.Find("XR Origin").transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.position = m_PlayerStartingPos.position;
            other.transform.rotation = m_PlayerStartingPos.rotation;
        }
    }
}
