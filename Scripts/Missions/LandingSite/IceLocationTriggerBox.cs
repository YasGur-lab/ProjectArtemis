using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*************************************
Class: MissionObject
Author: Alexander Achorn
Created: 17/02/2022
Last Modified: 
Description: This class is meant to be inherited from to assure all events in a mission have consistant Complete and Triggered states.

Change Log
**********
Date: 17/02/2022
Author: Alexander Achorn
Verified By:
Changes: Created

Date: 
Author: 
Verified By: 
Changes: 
*************************************/

[System.Serializable]
public class IceLocationTriggerBox : MonoBehaviour
{
    [SerializeField] GameObject searchArea;
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            searchArea.SetActive(true);
        }

    }
}
