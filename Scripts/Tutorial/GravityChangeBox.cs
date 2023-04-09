using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*************************************
Class: GravityChangeBox
Author: Charlie Weir
Created: 06/03/2022
Last Modified: 06/03/2022
Description: This class will charge the gravity when you pass through the collider.

Change Log
**********
Date: 06/03/2022
Author: Charlie Weir
Verified By: Alexander Achorn
Changes: Created.

Date: 
Author: 
Verified By: 
Changes: 
*************************************/

public class GravityChangeBox : MonoBehaviour
{

    BoxCollider box;
    GravityController gravitySettings;
    // Start is called before the first frame update
    void Start()
    {
        box = GetComponent<BoxCollider>();
        gravitySettings = GetComponent<GravityController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
                    gravitySettings.SetZeroGravity();
        box.enabled = false;
        box.isTrigger = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
