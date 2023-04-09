using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class LiftOff : MonoBehaviour
{
    [SerializeField] Transform m_StartingPos;
    [SerializeField] Transform m_EndingPos;

    [SerializeField] GameObject[] m_Effects;

    [SerializeField] float m_Speed = 1.0f;

    private bool m_ReadyToLiftOff;
    private bool m_ActivatingEffects;
    private float m_CurrentTime;

    [SerializeField] CinemachineVirtualCamera m_Cinemachine;
    private CinemachineTrackedDolly trackedDolly;

    // Start is called before the first frame update
    void Start()
    {
        m_ActivatingEffects = true;
        trackedDolly = m_Cinemachine.GetCinemachineComponent<CinemachineTrackedDolly>();
    }

    // Update is called once per frame
    void Update()
    {
        if (trackedDolly.m_PathPosition == 1.0f) m_ReadyToLiftOff = true;

        if (m_ReadyToLiftOff)
        {
            if (m_ActivatingEffects)
            {
                foreach (var effect in m_Effects)
                {
                    effect.SetActive(true);
                }

                m_ActivatingEffects = false;
            }

            m_CurrentTime += Time.deltaTime * m_Speed; // calculate the current time based on the speed
            float t = Mathf.Clamp01(m_CurrentTime); // clamp the interpolation factor between 0 and 1
            transform.position =
                Vector3.Lerp(m_StartingPos.position, m_EndingPos.position, t); // interpolate the position
        }
    }
}