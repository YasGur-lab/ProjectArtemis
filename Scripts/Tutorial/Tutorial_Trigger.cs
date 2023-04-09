using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*************************************
Class: Tutorial_Trigger
Author: Antoine Plouffe
Created: 01/11/22
Last Modified: 17/01/23
Description: This class is in charge of triggering the tutorial.

Change Log
**********
Date: 17/01/23
Author: Antoine Plouffe
Verified By: Alexander Achorn
Changes: Changing the if statement to check !IsTutorialTriggered instead of !TutorialHasBegan.

Date: 
Author: 
Verified By: 
Changes: 
*************************************/

public class Tutorial_Trigger : MonoBehaviour
{
    [SerializeField] ControllerTutorial m_Tutorial;

    private void OnTriggerEnter(Collider other)
    {
        if (!m_Tutorial.IsTutorialTriggered() && other.gameObject.tag == "Player")
        {
            m_Tutorial.TutorialHasBegan();
        }
    }
}
