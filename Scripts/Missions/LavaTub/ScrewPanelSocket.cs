using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/*************************************
Class: ScrewedPanelSocket
Author: Antoine Plouffe
Created: 01/02/2023
Last Modified: 02/02/2023
Description: This class is in charge of handling the sockets of the ScrewedPanel.

Change Log
**********
Date: 01/02/2023
Author: Antoine Plouffe
Verified By: Alexander Achorn
Changes: Created.

Date: 
Author: 
Verified By: 
Changes: 
*************************************/

public class ScrewPanelSocket : XRSocketInteractor
{
    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
        if (!GetComponent<XRSocketInteractor>().hasSelection)
        {
            var storeInteractionLayerMask = args.interactableObject.transform.GetComponent<XRBaseInteractable>().interactionLayers;
            args.interactableObject.transform.GetComponent<XRBaseInteractable>().interactionLayers = 0;
            args.interactableObject.transform.GetComponent<XRBaseInteractable>().gameObject.transform.position = transform.position;
            args.interactableObject.transform.GetComponent<XRBaseInteractable>().interactionLayers = storeInteractionLayerMask;
            GetComponent<XRSocketInteractor>().StartManualInteraction(args.interactableObject.transform.GetComponent<IXRSelectInteractable>());
            StartCoroutine(ResetScrew(args.interactableObject.transform.gameObject));
        }
    }

    IEnumerator ResetScrew(GameObject go)
    {
        yield return new WaitForSeconds(0.5f);
        go.transform.GetComponent<Screw>().ResetFirstEnter();
        
    }
}
