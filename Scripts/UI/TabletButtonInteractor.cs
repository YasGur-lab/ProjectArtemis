using UnityEngine;
using UnityEngine.UI;

/*************************************
Class: TabletButtonInteractor
Author: Antoine Plouffe
Created: 10/02/2023
Last Modified: 10/02/2023
Description: This class is in charge of handling the interactions between the player and the buttons.

Change Log
**********
Date: 10/02/2023
Author: Antoine Plouffe
Verified By: Alexander Achorn
Changes: Created.

Date: 07/04/23
Author: Alexander Achorn
Verified By:
Changes: Setup selected, toggle and other GUI improvements
*************************************/

public class TabletButtonInteractor : MonoBehaviour
{
    [SerializeField] private bool m_Disabled;
    [SerializeField] private bool m_IsTab;
    private float m_CurrentPressedTime;
    [SerializeField][Range(0.0f, 100.0f)] private float m_PressedTime;
    private bool m_HasPressed;
    [SerializeField] TabletMenuScript m_Menu;
    [SerializeField][Range(0.0f, 100.0f)] private int m_Index;
    [SerializeField] Image m_Overlay;

    [SerializeField] bool m_ButtonToFunction;
    [SerializeField] TabletMenuScript.FunctionToCall m_Functions;
    private Material m_Material;

    private float m_PercOfHoldTime = 1.0f;

    [SerializeField] VRSettingsController m_VrSettings = null;

    // Start is called before the first frame update
    void Start()
    {
        m_CurrentPressedTime = m_PressedTime;
        m_Material = gameObject.GetComponent<MeshRenderer>().material;

        if(m_Functions == TabletMenuScript.FunctionToCall.SwitchVrSettings && m_VrSettings)
        {
            switch (m_Index)
            {
                case 0:
                    if(m_VrSettings.UsingFreeLocomotion())
                    {
                        m_Material.SetFloat("_IsSelected", 1.0f);
                    }
                    break;
                case 1:
                    if (m_VrSettings.UsingControllerRotation())
                    {
                        m_Material.SetFloat("_IsSelected", 1.0f);
                    }
                    break;
                case 2:
                    if (m_VrSettings.UsingSnapRotation())
                    {
                        m_Material.SetFloat("_IsSelected", 1.0f);
                    }
                    break;
                default:
                    break;
            }
        }
    }

    void Update()
    {
        if (m_Disabled && gameObject.activeSelf) gameObject.SetActive(false);

        if(m_IsTab)
        {
            if(m_Menu.GetCurrentIndex() == m_Index || (m_Menu.GetCurrentIndex() == 6 && m_Index == 5) || (m_Menu.GetCurrentIndex() == 8 && m_Index == 5) || (m_Menu.GetCurrentIndex() == 9 && m_Index == 5))
            {
                m_Material.SetFloat("_IsSelected", 1.0f);
            }
            else
            {
                m_Material.SetFloat("_IsSelected", 0.0f);
            }
        }

        if (m_Functions == TabletMenuScript.FunctionToCall.SwitchVrSettings && m_VrSettings)
        {
            switch (m_Index)
            {
                case 0:
                    if (m_VrSettings.UsingFreeLocomotion())
                    {
                        m_Material.SetFloat("_IsSelected", 1.0f);
                    }
                    else
                    {
                        m_Material.SetFloat("_IsSelected", 0.0f);
                    }
                    break;
                case 1:
                    if (m_VrSettings.UsingControllerRotation())
                    {
                        m_Material.SetFloat("_IsSelected", 1.0f);
                    }
                    else
                    {
                        m_Material.SetFloat("_IsSelected", 0.0f);
                    }
                    break;
                case 2:
                    if (m_VrSettings.UsingSnapRotation())
                    {
                        m_Material.SetFloat("_IsSelected", 1.0f);
                    }
                    else
                    {
                        m_Material.SetFloat("_IsSelected", 0.0f);
                    }
                    break;
                default:
                    break;
            }
        }
    }

    void OnTriggerStay(Collider coll)
    {
        if (!m_Menu.GetCanPlayerSelect()) return;
        if (coll.gameObject.tag == "Finger")
        {
            if (!m_HasPressed)
            {
                m_CurrentPressedTime -= Time.deltaTime;
                m_PercOfHoldTime = m_CurrentPressedTime / m_PressedTime;
                if (m_CurrentPressedTime < 0)
                {
                    m_HasPressed = true;
                    m_Menu.SetCanPlayerSelect(false);
                    if (m_ButtonToFunction)
                        m_Menu.ButtonSelected(m_Functions, m_Index);
                    else
                        m_Menu.SwitchMissionTab(m_Index);

                    m_CurrentPressedTime = m_PressedTime;
                    m_PercOfHoldTime = 1.0f;
                }
            }
            if (m_Material)
            {
                m_Material.SetFloat("_ProgressPercent", m_PercOfHoldTime);
            }
        }
    }

    void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.tag == "Finger")
        {
            if (m_HasPressed)
                m_HasPressed = false;
            //Debug.Log("m_HasPressed" + m_HasPressed);
        }

        m_CurrentPressedTime = m_PressedTime;
        m_PercOfHoldTime = 1.0f;
        if (m_Material)
        {
            m_Material.SetFloat("_ProgressPercent", m_PercOfHoldTime);
        }
    }
}
