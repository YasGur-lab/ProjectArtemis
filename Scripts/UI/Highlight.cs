using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/*************************************
Class: Highlight
Author: Antoine Plouffe
Created: 01/09/2023
Last Modified: 02/12/2022
Description: This class is in charge of the highlights.

Change Log
**********
Date: 02/09/2022
Author: Antoine Plouffe
Verified By: Alexander Achorn
Changes: Created.

Date: 
Author: 
Verified By: 
Changes: 
*************************************/
public class Highlight : MonoBehaviour
{
    List<Material> m_ListMaterials;
    [SerializeField] public List<Renderer> m_ListRenderers;
    [SerializeField] AnimationCurve m_Curve;
    [SerializeField] AnimationCurve m_CurveHighlightOnce;

    float m_CurveDeltaTime;

    private void Start()
    {
        m_ListMaterials = new List<Material>();
        /*foreach (Renderer renderer in m_ListRenderers)
        {
            m_ListMaterials.AddRange(new List<Material>(renderer.materials));
        }*/
    }

    public void IsActive(bool Highlighted) //bool Highlighted
    {
        if (Highlighted)
        {
            if (m_ListMaterials == null) return;
            foreach (Material material in m_ListMaterials)
            {
                material.EnableKeyword("_EMISSION");
                material.SetFloat("_GlowStrength", GetCurveValue());
            }
        }
        else
        {
            if (m_ListMaterials == null) return;
            foreach (Material material in m_ListMaterials)
            {
                material.SetFloat("_GlowStrength", Mathf.Clamp(-GetCurveHighlightOnceValue(), 0, 1));
                material.DisableKeyword("_EMISSION");
            }
        }
    }

    public void IsActiveOnce(bool Highlighted)
    {
        if (Highlighted)
        {
            foreach (Material material in m_ListMaterials)
            {
                material.EnableKeyword("_EMISSION");
                material.SetFloat("_GlowStrength", Mathf.Clamp(GetCurveHighlightOnceValue(), 0, 1));
            }
        }
        else
        {
            foreach (Material material in m_ListMaterials)
            {
                material.SetFloat("_GlowStrength", Mathf.Clamp(-GetCurveHighlightOnceValue(), 0, 1));
                material.DisableKeyword("_EMISSION");
            }
        }
    }

    private float GetCurveValue()
    {
        m_CurveDeltaTime += Time.deltaTime;
        return m_Curve.Evaluate(m_CurveDeltaTime);
    }

    private float GetCurveHighlightOnceValue()
    {
        m_CurveDeltaTime += Time.deltaTime;
        return m_CurveHighlightOnce.Evaluate(m_CurveDeltaTime);
    }

    public void AddRenderers(Renderer renderer)
    {
        m_ListRenderers.Add(renderer);
        m_ListMaterials.AddRange(new List<Material>(renderer.materials));
    }

    public void ClearLists()
    {
        m_ListRenderers.Clear();
        foreach (Material material in m_ListMaterials)
        {
            Color color = Color.clear;
            material.SetFloat("_GlowStrength", Mathf.Clamp(-GetCurveHighlightOnceValue(), 0, 1));
            material.SetColor("_EmissionColor", color);
        }
        m_ListMaterials.Clear();
    }
}
