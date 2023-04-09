using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    [SerializeField] float m_OpenAngle = 90.0f;
    [SerializeField] float m_OpenSpeed = 1.0f;
    [SerializeField] bool m_ReverseDirection = false;
    [SerializeField] bool m_ReadyToBeOpen = false;

    private bool m_IsOpen = false;
    private Quaternion m_ClosedRotation;
    private Quaternion m_OpenRotation;

    private void Start()
    {
        m_ClosedRotation = transform.rotation;

        if (m_ReverseDirection)
            m_OpenRotation = Quaternion.AngleAxis(-m_OpenAngle, Vector3.up) * m_ClosedRotation;
        else
            m_OpenRotation = Quaternion.AngleAxis(m_OpenAngle, Vector3.up) * m_ClosedRotation;
    }

    private void Update()
    {
        if (transform.rotation == m_ClosedRotation && !m_IsOpen ||
            transform.rotation == m_OpenRotation && m_IsOpen || !m_ReadyToBeOpen) return;

        if (m_IsOpen)
            transform.rotation = Quaternion.Lerp(transform.rotation, m_OpenRotation, m_OpenSpeed * Time.deltaTime);
        else 
            transform.rotation = Quaternion.Lerp(transform.rotation, m_ClosedRotation, m_OpenSpeed * Time.deltaTime);
    }

    public void Open()
    {
        m_IsOpen = true;
    }

    public void Close()
    {
        m_IsOpen = false;
    }

    public void SetReadyToBeOpen(bool b)
    {
        m_ReadyToBeOpen = b;
    }
}
