using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK_Hip : MonoBehaviour
{
    public Transform m_Hip;
    [SerializeField] private GameObject m_Head;
    public GameObject m_CharacterModel;

    private Vector3 m_CurrentNormal;


    private Quaternion m_OriginalRotation;

    [SerializeField] private Climber m_Climber;

    // Start is called before the first frame update
    void Start()
    {
        m_OriginalRotation = m_Hip.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Climber.IsClimbing())
        {
            RaycastHit hit;
            Debug.DrawRay(m_Head.transform.position, -m_Head.transform.forward, Color.red);

            if (Physics.Raycast(m_Head.transform.position, m_CharacterModel.transform.forward, out hit, 2.0f))
            {
                Debug.DrawRay(m_Head.transform.position, m_CharacterModel.transform.forward, Color.green);

                if (hit.transform.gameObject.tag == "ClimbingWall")
                {
                    if (m_CurrentNormal != hit.normal)
                    {
                        Quaternion rot = Quaternion.LookRotation(-hit.normal, m_CharacterModel.transform.up);
                        Quaternion xRot = Quaternion.Euler(-90f, 0f, 0f);
                        Quaternion finalRot = rot * xRot;
                        m_Hip.rotation = finalRot;

                        m_CurrentNormal = hit.normal;
                    }

                    Debug.DrawRay(hit.transform.position, hit.normal, Color.blue);
                }
            }
            else
            {
                //reset
                if (m_Hip.rotation != m_OriginalRotation)
                    m_Hip.rotation = m_OriginalRotation;
                if (m_CurrentNormal != Vector3.zero)
                    m_CurrentNormal = Vector3.zero;
            }
        }
        else
        {
            //reset
            /*if(m_Hip.rotation != m_OriginalRotation)
                m_Hip.rotation = m_OriginalRotation;
            if(m_CurrentNormal != Vector3.zero)
                m_CurrentNormal = Vector3.zero;*/
        }
    }
}
