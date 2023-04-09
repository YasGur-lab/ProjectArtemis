using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandlingInputs : MonoBehaviour
{
    //Input
    [SerializeField] InputActionAsset m_InputAction;

    public void DisableInputs(string actionMapsName)
    {
        if (m_InputAction != null)
        {
            foreach (var actionMap in m_InputAction.actionMaps)
            {
                if (actionMap.name == actionMapsName)
                    actionMap.Disable();
            }
        }
    }
    public void EnableInputs(string actionMapsName, string actionName)
    {
        if (m_InputAction != null)
        {
            foreach (var actionMap in m_InputAction.actionMaps)
            {
                if (actionMap.name == actionMapsName)
                {
                    foreach (var action in actionMap)
                    {
                        if (action.name == actionName)
                            action.Enable();
                    }
                }
            }
        }
    }
    public void EnableInputs(string actionMapsName)
    {
        if (m_InputAction != null)
        {
            foreach (var actionMap in m_InputAction.actionMaps)
            {
                if (actionMap.name == actionMapsName)
                    actionMap.Enable();
            }
        }
    }
}
