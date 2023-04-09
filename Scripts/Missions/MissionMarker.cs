using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/*************************************
Class: Tutorial_Trigger
Author: Antoine Plouffe
Created: 01/27/23
Last Modified: 01/27/23
Description: This class is in charge of the Mission Marker.

Change Log
**********
Date: 01/27/23
Author: Antoine Plouffe
Verified By: Alexander Achorn
Changes: Created

Date: 02/02/2023
Author: Antoine Plouffe
Verified By: Alexander Achorn
Changes: Adding Rotation Behavior

Date: 06/03/2022
Author: Charlie Weir
Verified By: Alexander Achorn
Changes: Added scaling to maintain size

Date: 07/04/23
Author: Alexander Achorn
Verified By: 
Changes: Fixed error caused when transfering from one scene to another
*************************************/

public class MissionMarker : MissionObject
{
    [SerializeField] float m_ScaleStrength;
    [SerializeField] float m_MinimumDistance = 10.0f;
    [SerializeField] private Image m_Prefab;
    [SerializeField] RectTransform m_Canvas;
    [SerializeField] private Transform m_Target;
    [SerializeField][Range(0.0f, 10.0f)] private float m_RotationSpeed;
    [SerializeField] GameObject m_Player;
    Image m_Marker;

    // Start is called before the first frame update
    void Start()
    {
        if (m_Prefab && m_Target)
            m_Marker = Instantiate(m_Prefab, m_Canvas);

        m_Player = FindObjectOfType<XROrigin>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (isComplete)
        {
            if(m_Marker)
            {
                m_Marker.enabled = false;
            }
            return;
        }
        else if (isTriggered && m_Marker)
        {
            m_Marker.enabled = true;
            //Vector3 pos = Camera.main.WorldToScreenPoint(m_Target.position);
            //m_Marker.gameObject.SetActive(pos.x > 0);
            m_Marker.transform.position = m_Target.position;
            m_Marker.transform.LookAt(m_Player.transform);


            float distance = Vector3.Distance(m_Player.transform.position, m_Target.position);


            m_Marker.transform.localScale =Vector3.one* (distance * m_ScaleStrength);

            ClampToSide();
            if (distance < m_MinimumDistance)
            {
                isComplete = true;
            }

        }
        else
        {
            if(m_Marker)
            {
                m_Marker.enabled = false;
            }
        }
    }

    Vector3 ClampToSide()
    {
        float MinX = m_Marker.GetPixelAdjustedRect().width / 2;
        float MaxX = Screen.width - MinX;
        float MinY = m_Marker.GetPixelAdjustedRect().height / 2;
        float MaxY = Screen.height - MinY;

        Vector2 pos = Camera.main.WorldToScreenPoint(m_Target.position);

        pos.x = Mathf.Clamp(pos.x, MinX, MaxX);
        pos.y = Mathf.Clamp(pos.y, MinY, MaxY);
        return pos;
    }
    public void SetTarget(Transform target)
    {
        m_Target = target;
        isComplete = false;
    }
}

