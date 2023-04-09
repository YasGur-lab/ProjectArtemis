using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/*************************************
Class: XRDirectClimbInteractor
Author: Antoine Plouffe
Created: 01/09/22
Last Modified: 17/01/23
Description: This class is in charge of interacting with objects, specifically the climb pieces.

Change Log
**********
Date: 17/01/23
Author: Antoine Plouffe
Verified By: Alexander Achorn
Changes: Updated the attach transform based on if the player is grabbing or not an object

Date: 07/04/23
Author: Alexander Achorn
Verified By: 
Changes: Removed code that disabled the jumper while holding tools. This cause I cascade of bugs from bad logic.
*************************************/

public class XRDirectClimbInteractor : XRDirectInteractor
{
    public static event Action<string> m_ClimbHandActivated;
    public static event Action<string> m_ClimbHandDeactivated;

    public static event Action<string> m_TabletHandActivated;
    public static event Action<string> m_TabletHandDeactivated;

    private string m_ControllerName;

    [SerializeField] private GameObject m_Player;
    [SerializeField] private Material m_HighlightMaterial;

    private bool m_AddRenderers = true;

    [SerializeField] private Transform m_PokingTransform;
    [SerializeField] private Transform m_GrabbingTransform;

    //WireMiniGame
    [SerializeField] private WirePanel_MiniGame m_WireMiniGame;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start(); 
        m_ControllerName = gameObject.name;
        m_AddRenderers = true;
        m_Player = GameObject.Find("XR Origin");
        GetComponent<XRDirectInteractor>().attachTransform = m_PokingTransform;
    }

    void Update()
    {
        //m_Player.GetComponent<Highlight>().IsActive(true);
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        
        if(args.interactableObject.transform.gameObject.name != "Tablet_3.0")
            GetComponent<XRDirectInteractor>().attachTransform = m_GrabbingTransform;
        else if(args.interactableObject.transform.gameObject.name == "Tablet_3.0")
            GetComponent<XRDirectInteractor>().attachTransform = m_PokingTransform;

        if (args.interactableObject.transform.gameObject.tag == "Climable")
        {
            //if (m_AddRenderers)
            //{
            //    Renderer[] tempRend = new Renderer[1];
            //    tempRend[0] = args.interactableObject.transform.gameObject.GetComponent<Renderer>();
            //    AddRenderer(tempRend, m_Player.GetComponent<Highlight>());
            //}

            m_ClimbHandActivated?.Invoke(m_ControllerName);
        }

        if(args.interactableObject.transform.gameObject.name == "Tablet_3.0")
        {
            m_TabletHandActivated?.Invoke(m_ControllerName);
        }

        if (args.interactableObject.transform.gameObject.name == "Drill" && m_WireMiniGame.m_Stage == WirePanel_MiniGame.MiniGameStages.PlayerIsInFrontOfAsagumo)
        {
            m_WireMiniGame.DrillHasBeenTaken();
        }
    }
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        GetComponent<XRDirectInteractor>().attachTransform = m_PokingTransform;

        //m_Player.GetComponent<Highlight>().ClearLists();
        m_ClimbHandDeactivated?.Invoke(m_ControllerName);

        if (args.interactableObject.transform.gameObject.name == "Tablet_3.0")
        {
            m_TabletHandDeactivated?.Invoke(m_ControllerName);
        }
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);

        if (args.interactableObject.transform.gameObject.tag == "Climable")
        {
            if (m_AddRenderers)
            {
                Renderer[] tempRend = new Renderer[1];
                tempRend[0] = args.interactableObject.transform.gameObject.GetComponent<Renderer>();
                AddRenderer(tempRend, m_Player.GetComponent<Highlight>());
            }   
        }

    }
    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);
        //m_Player.GetComponent<Highlight>().ClearLists();
        m_AddRenderers = true;
    }

    private void AddRenderer(Renderer[] renderer, Highlight climbPieceToHighlight)
    {
        foreach (var rend in renderer)
        {
            if (m_AddRenderers) { }
                //climbPieceToHighlight.AddRenderers(rend);
        }
        m_AddRenderers = false;
    }
}
