using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

/*************************************
Class: GravityController
Author: Antoine Plouffe
Created: 01/09/2022
Last Modified: 02/12/2022
Description: This class is in charge of the gravity settings.

Change Log
**********
Date: 02/09/2022
Author: Antoine Plouffe
Verified By: Alexander Achorn
Changes: Created.

Date: 06/03/2022
Author: Charles Weir
Verified By: Alexander Achorn
Changes: Added Lerp

Date: 29/3/2023
Author: Charles Weir
Verified By: 
Changes: OnSceneLoaded() assigns gravity accordingly at the start of any level


Date: 
Author: 
Verified By: 
Changes: 
*************************************/
public class GravityController : MonoBehaviour, ISaveable
{
    enum GravityTypes
    {
        Earth,
        Zero,
        Moon
    }
    
    GravityTypes CurrentGravityType = GravityTypes.Earth;

    readonly Vector3 m_EarthGravity = new(0, -9.81f, 0);
    readonly Vector3 m_ZeroGravity = new(0, 0, 0);
    readonly Vector3 m_MoonGravity = new(0, -1.62f, 0);

    readonly float m_TransitionDuration = 2;
    
    private Vector3 m_LerpStartGravity = Vector3.zero;
    private Vector3 m_LerpEndGravity = Vector3.zero;

    private float LerpPercent = 0;

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Update()
    {
        if(LerpPercent < 1)
        {
            LerpPercent += Time.deltaTime / m_TransitionDuration;
            LerpPercent = Mathf.Clamp01(LerpPercent);
            float smoothPercent = ((0.69f+0.31f) - Mathf.Cos(Mathf.PI * LerpPercent)) / 2.0f;
            Physics.gravity = Vector3.Lerp(m_LerpStartGravity, m_LerpEndGravity, smoothPercent);
        }
    }


    public void OnSceneLoaded(Scene scene, LoadSceneMode loadmode)
    {
        string scenename = scene.name;       
        
        if(scenename == "Mission_1" || scenename == "LavaTube")
        {
            SetMoonGravity();
            return;
        }
        SetEarthGravity();
    }
    public void SetEarthGravity()
    {
        CurrentGravityType = GravityTypes.Earth;
        m_LerpStartGravity = Physics.gravity;
        m_LerpEndGravity = m_EarthGravity;
        LerpPercent = 0;
    }

    public void SetMoonGravity()
    {
        CurrentGravityType = GravityTypes.Moon;
        m_LerpStartGravity = Physics.gravity;
        m_LerpEndGravity = m_MoonGravity;
        LerpPercent = 0;
    }
    
    public void SetZeroGravity()
    {
        CurrentGravityType = GravityTypes.Zero;
        m_LerpStartGravity = Physics.gravity;
        m_LerpEndGravity = m_ZeroGravity;
        LerpPercent = 0;
    }

    public void LoadScenario1()
    {
        SceneManager.LoadScene("Scenario_1");
    }

    public bool IsEarthGravityOn()
    {
        return CurrentGravityType == GravityTypes.Earth;
    }
    public bool IsMoonGravityOn()
    {
        return CurrentGravityType == GravityTypes.Moon;
    }
    public bool IsZeroGravityOn()
    {
        return CurrentGravityType == GravityTypes.Zero;
    }

    public object SaveState()
    {
        return new SaveData()
        {
            CurrentGravityType = this.CurrentGravityType,
        };
    }

    public void LoadState(object state)
    {
        SaveData saveData = (SaveData)state;
        CurrentGravityType = saveData.CurrentGravityType;
    }

    [Serializable]
    private struct SaveData
    {
        public GravityTypes CurrentGravityType;
    }

}
