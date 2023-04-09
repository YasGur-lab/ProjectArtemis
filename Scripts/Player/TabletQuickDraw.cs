using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TabletQuickDraw : MonoBehaviour
{
    [Header("Tablet")]
    [SerializeField] GameObject tablet;
    [SerializeField] Transform leftAttachment;
    [SerializeField] Transform rightAttachment;
    [Header("Interactor")]
    [SerializeField] XRGrabInteractable tabletInteractor;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "LeftHand")
        {
            tabletInteractor.attachTransform = leftAttachment;
        }
        else if(other.tag == "RightHand")
        {
            tabletInteractor.attachTransform = rightAttachment;
        }
    }

}
