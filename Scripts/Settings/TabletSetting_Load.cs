using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TabletSetting_Load : MonoBehaviour
{
    [SerializeField] XRSimpleInteractable m_Interactor;

    void Start()
    {
        m_Interactor.selectEntered.AddListener(Load);
    }

    private void OnDestroy()
    {
        m_Interactor.selectEntered.RemoveListener(Load);
    }

    private void Load(SelectEnterEventArgs arg0)
    {
        GameObject system = GameObject.Find("Saving");

        if (system)
            system.GetComponent<SavingWrapper>().Load();
    }
}
