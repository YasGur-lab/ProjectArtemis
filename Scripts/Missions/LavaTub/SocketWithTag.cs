using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketWithTag : XRSocketInteractor
{
    public string targetTag = string.Empty;

    public override bool CanHover(IXRHoverInteractable interactable)
    {
        return base.CanHover(interactable) && interactable.transform.tag == "Helmet";
    }

    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        return base.CanSelect(interactable) && interactable.transform.tag == "Helmet";
    }

    private bool MatchUsingTag(XRBaseInteractable interactable)
    {
        return interactable.CompareTag(targetTag);
    }

}
