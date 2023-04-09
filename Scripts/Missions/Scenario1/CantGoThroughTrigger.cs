using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CantGoThroughTrigger : MonoBehaviour
{
    Highlight m_Highlight;

    // Start is called before the first frame update
    void Start()
    {
        m_Highlight = GetComponent<Highlight>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            m_Highlight.IsActiveOnce(true);
            if(other.GetComponent<ActivateTeleportationRay>().enabled)
                other.GetComponent<ActivateTeleportationRay>().enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            m_Highlight.IsActiveOnce(false);
            if(!other.GetComponent<ActivateTeleportationRay>().enabled)
                other.GetComponent<ActivateTeleportationRay>().enabled = true;
        }
    }
}
