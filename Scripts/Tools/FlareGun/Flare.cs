using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/*************************************
Class: Flare
Author: Antoine Plouffe
Created: 08/02/2023
Last Modified: 08/02/2023
Description: This class is in charge of the Flare.

Change Log
**********
Date: 
Author: Antoine Plouffe
Verified By:
Changes: Created.

Date: 
Author: 
Verified By: 
Changes: 
*************************************/

public class Flare : MonoBehaviour
{
    Vector3 m_Velocity;
    [SerializeField][Range(0, 10)] float m_Speed = 0.1f;
    [SerializeField] float m_lifeTime = 1200.0f;

    private bool m_HasLanded;

    public void Init(Transform flare, Vector3 velocity)
    {
        transform.position = flare.position;
        transform.rotation = Quaternion.Euler(flare.rotation.eulerAngles - new Vector3(90,0,0));
        m_Velocity = velocity;
    }

    // Update is called once per frame
    void Update()
    {
        m_lifeTime -= Time.deltaTime;

        if(!m_HasLanded)
            transform.position += m_Velocity * m_Speed * Time.deltaTime;

        if(m_lifeTime < 0)
        {
            Object.Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Terrain" || coll.gameObject.layer == 13) //13 = Ground
        {
            if (!m_HasLanded) m_HasLanded = true;
        }
    }
}