using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TabletSetting_Save : MonoBehaviour
{
    [SerializeField] XRSimpleInteractable m_Interactor;

    void Start()
    {
        m_Interactor.selectEntered.AddListener(Save);
    }

    private void OnDestroy()
    {
        m_Interactor.selectEntered.RemoveListener(Save);
    }

    private void Save(SelectEnterEventArgs arg0)
    {
        GameObject system = GameObject.Find("Saving");

        if (system)
            system.GetComponent<SavingWrapper>().Save();
    }
}
