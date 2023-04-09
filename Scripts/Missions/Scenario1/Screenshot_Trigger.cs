using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshot_Trigger : MonoBehaviour
{
    [SerializeField] Screenshots m_Screenshot;

    private void OnTriggerEnter(Collider other)
    {
        if (m_Screenshot.IsTakingPicture() && other.gameObject.tag == "Player")
        {
            StartCoroutine(m_Screenshot.TakeScreenShot());
        }
    }
}
