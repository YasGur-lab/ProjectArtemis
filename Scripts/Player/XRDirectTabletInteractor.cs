using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRDirectTabletInteractor : XRDirectInteractor
{
    public static event Action<string> m_TabletHandActivated;
    public static event Action<string> m_TabletHandDeactivated;

    private string m_ControllerName;

    [SerializeField] private GameObject m_Player;

    //private bool m_AddRenderers = true;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start(); 
        m_ControllerName = gameObject.name;
        m_Player = GameObject.Find("XR Origin");
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        if (args.interactableObject.transform.gameObject.tag == "Climable")
        {
            m_TabletHandActivated?.Invoke(m_ControllerName);
        }

    }
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        m_TabletHandDeactivated?.Invoke(m_ControllerName);
    }
}
