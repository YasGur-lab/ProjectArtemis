using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*************************************
Class: UnderTerrainCollider
Author: Antoine Plouffe
Created: 10/02/2023
Last Modified: 10/02/2023
Description: This class is in charge of handling the player if he goes under the terrain.

Change Log
**********
Date: 10/02/2023
Author: Antoine Plouffe
Verified By: Alexander Achorn
Changes: Created.

Date: 
Author: 
Verified By: 
Changes: 
*************************************/


public class UnderTerrainCollider : MonoBehaviour
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
