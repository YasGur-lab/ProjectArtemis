
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*************************************
Class: TabletSlider
Author: Antoine Plouffe
Created: 10/02/2023
Last Modified: 10/02/2023
Description: This class is in charge of handling the sliders for the menu (WIP).

Change Log
**********
Date: 10/02/2023
Author: Antoine Plouffe
Verified By: Alexander Achorn
Changes: Created.

Date: 07/03/2023
Author: Antoine Plouffe
Verified By: Alexander Achorn
Changes: Making the slider go up and down based on the rotation of its parent.

Date: 
Author: 
Verified By: 
Changes: 
*************************************/

public class TabletSlider : MonoBehaviour
{
    [SerializeField] private bool m_Disabled;
    private float m_CurrentPressedTime;
    [SerializeField][Range(0.0f, 100.0f)] private float m_PressedTime;

    [SerializeField] private Transform m_0PercSliderPos;
    [SerializeField] private Transform m_100PercSliderPos;

    [SerializeField] TabletMenuScript m_Menu;

    [SerializeField] Image m_Overlay;

    [SerializeField] bool m_ButtonToFunction;
    [SerializeField][Range(0.0f, 100.0f)] private int m_Index;
    [SerializeField] TabletMenuScript.FunctionToCall m_Functions;

    [SerializeField] private TextMeshProUGUI m_Text;

    // Start is called before the first frame update
    void Start()
    {
        m_CurrentPressedTime = m_PressedTime;
    }

    void Update()
    {
        //if(m_Text.text != ((int)GetSliderValue()).ToString().ToUpper())
        //    m_Text.text = ((int)GetSliderValue()).ToString().ToUpper();

        if (m_Disabled && gameObject.activeSelf) gameObject.SetActive(false);

        if (transform.position.y < m_0PercSliderPos.position.y)
            transform.position = m_0PercSliderPos.position;
        else if(transform.position.y > m_100PercSliderPos.position.y)
            transform.position = m_100PercSliderPos.position;
    }

    void OnTriggerStay(Collider coll)
    {
        //if (!m_Menu.GetCanPlayerSelect()) return;
        //if (coll.gameObject.tag == "Finger")
        //{
        //    m_CurrentPressedTime -= Time.deltaTime;
        //    if (m_CurrentPressedTime < 0)
        //    {
        //        var tempPos = transform.position;
        //        tempPos.y = coll.transform.position.y;
        //        transform.position = tempPos;

        //        //var tempPos = transform.position;
        //        //tempPos.y = coll.transform.position.y;
        //        //tempPos = transform.parent.rotation * tempPos;
        //        //transform.position = tempPos;

        //        if (m_ButtonToFunction)
        //            m_Menu.SliderSelected(m_Functions, m_Index, GetSliderValue());
        //    }
        //}

        if (!m_Menu.GetCanPlayerSelect()) return;
        if (coll.gameObject.tag == "Finger")
        {
            m_CurrentPressedTime -= Time.deltaTime;
            if (m_CurrentPressedTime < 0)
            {
                var tempPos = transform.position;
                tempPos.y = coll.transform.position.y;

                // Apply the rotation of the parent to the slider's position
                tempPos = transform.parent.rotation * tempPos;

                transform.position = tempPos;

/*                if (m_ButtonToFunction)
                    m_Menu.SliderSelected(m_Functions, m_Index, GetSliderValue());*/
            }
        }
    }

    void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.tag == "Finger")
        {
            m_CurrentPressedTime = m_PressedTime;
        }
    }

    float GetSliderValue()
    {
        float minY = m_0PercSliderPos.position.y;
        float maxY = m_100PercSliderPos.position.y;
        float currentY = transform.position.y;

        if (currentY <= minY)
            return 0f;
        else if (currentY >= maxY)
            return 100f;
        else
            return (Mathf.InverseLerp(minY, maxY, currentY))*100;
    }
}
