using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.XR.Interaction.Toolkit;

/*************************************
Class: Screw
Author: Antoine Plouffe
Created: 01/02/2023
Last Modified: 02/02/2023
Description: This class is in charge of the Screw.

Change Log
**********
Date: 01/02/2023
Author: Antoine Plouffe
Verified By: Alexander Achorn
Changes: Created.

Date: 
Author: 
Verified By: 
Changes: 
*************************************/

public class Screw : MonoBehaviour
{
    [SerializeField] private bool m_FirstEnter = false;
    [SerializeField] private float m_Timer;

    void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.name == "Tip" || coll.GetComponent<ScrewPanelSocket>())
            IsTriggeredCollider(false);
    }

    public void ResetFirstEnter()
    {
        m_FirstEnter = false;
    }

    public void FirstEnter()
    {
        m_FirstEnter = true;
    }

    public bool GetFirstEnter()
    {
        return m_FirstEnter;
    }

    public void IsTriggeredCollider(bool b)
    {
        GetComponentInChildren<CapsuleCollider>().isTrigger = b;
    }
}
