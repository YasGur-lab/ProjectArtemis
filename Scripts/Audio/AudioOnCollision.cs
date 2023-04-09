using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*************************************
Class: AudioOnCollision
Author: Charlie Weir
Created: 06/03/2022
Last Modified: 06/03/2022
Description: This class will cause a sound to be played on collision.

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

[RequireComponent(typeof(Rigidbody))]
public class AudioOnCollision : MonoBehaviour
{
    // Start is called before the first frame update
    bool CanPlay = false;
    float defaultvolume;
    private Rigidbody m_rigidbody;
    public AudioSource m_audio;

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        
        if (m_audio == null)
        {
            //Debug.LogWarning("Missing Audio Source Component on Collision Audio Player");
        }
        else
        {
            m_audio.volume= 0.0f;
            defaultvolume= m_audio.volume;
        }

        CanPlay = false ;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public  void SetAllowPlay()
    {
        CanPlay = true;
    }

    public void SetForbidPlay()
    {
        CanPlay = false;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        
        if (m_audio != null && other.collider.tag != "Player" && CanPlay)
        {
            Vector3 othervel;

            if (other.collider.attachedRigidbody != null)
            {
                othervel = other.collider.attachedRigidbody.velocity;
            }
            else
            {
                othervel = Vector3.zero;
            }

            Vector3 ObjVel = m_rigidbody.velocity;

            float VelocityDifferential = Vector3.Magnitude(othervel - ObjVel);

            Debug.Log(VelocityDifferential);

        
            if (m_audio != null)
            m_audio.volume = Mathf.Clamp(VelocityDifferential * VelocityDifferential * 0.125f, 0.1f, 1);
            m_audio.Play();
            CanPlay = false;
        }
    }
        
}
