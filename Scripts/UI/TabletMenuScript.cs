using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System;
using TMPro;

/*************************************
Class: TabletMenuScript
Author: Antoine Plouffe
Created: 10/02/2023
Last Modified: 10/02/2023
Description: This class is in charge of handling the menu for the tablet.

Change Log
**********
Date: 10/02/2023
Author: Antoine Plouffe
Verified By: Alexander Achorn
Changes: Created.

Date: 
Author: 
Verified By: 
Changes: 
*************************************/

public class TabletMenuScript : MonoBehaviour
{
    [SerializeField] AudioMixer m_AudioVolume;

    [SerializeField] GameObject[] m_Tabs;

    private int m_CurrentTabIndex;
    private int m_PreviousTabIndex;

    bool m_CanSelect = true;
    private float m_TimerBeforeResetSelect;
    [SerializeField] [Range(0.0f, 1.0f)] private float m_CurrentTimerBeforeResetSelect = 1.0f;
    VRSettingsController m_VrSettings;

    [SerializeField] TMP_Text m_MasterVol;
    [SerializeField] TMP_Text m_MusicVol;
    [SerializeField] TMP_Text m_SFXVol;
    [SerializeField] TMP_Text m_DialogueVol;
    [SerializeField] TMP_Text m_SnapDegrees;

    float m_CurrentSnapDegrees = 5.0f;

    float m_CurrentMasterVol = 50.0f;
    float m_CurrentMusicVol = 50.0f;
    float m_CurrentSFXVol = 50.0f;
    float m_CurrentDialogueVol = 50.0f;

    public enum FunctionToCall
    {
        None,
        SwitchScene,
        SwitchMission,
        QualitySettings,
        SwitchVrSettings,
        ChangeFloatValue,
        Back,
    }

    // Start is called before the first frame update
    void Start()
    {
        m_CurrentTabIndex = -1;
        m_VrSettings = GetComponent<VRSettingsController>();
        m_CurrentTimerBeforeResetSelect = m_TimerBeforeResetSelect;

        if (m_MasterVol)
        {
            m_AudioVolume.GetFloat("Master", out m_CurrentMasterVol);
            m_CurrentMasterVol = DecibleToPercent(m_CurrentMasterVol);
            m_MasterVol.text = m_CurrentMasterVol.ToString();
        }

        if (m_MusicVol)
        {
            m_AudioVolume.GetFloat("Music", out m_CurrentMusicVol);
            m_CurrentMusicVol = DecibleToPercent(m_CurrentMusicVol);
            m_MusicVol.text = m_CurrentMusicVol.ToString();
        }

        if (m_SFXVol)
        {
            m_AudioVolume.GetFloat("SFX", out m_CurrentSFXVol);
            m_CurrentSFXVol = DecibleToPercent(m_CurrentSFXVol);
            m_SFXVol.text = m_CurrentSFXVol.ToString();
        }

        if (m_DialogueVol)
        {
            m_AudioVolume.GetFloat("Dialogue", out m_CurrentDialogueVol);
            m_CurrentDialogueVol = DecibleToPercent(m_CurrentDialogueVol);
            m_DialogueVol.text = m_CurrentDialogueVol.ToString();
        }

        if (m_SnapDegrees)
        {
            m_CurrentSnapDegrees = m_VrSettings.GetSnapDegrees();
            m_SnapDegrees.text = m_CurrentSnapDegrees.ToString();
        }
    }

    void Update()
    {
        if (!m_CanSelect)
        {
            m_TimerBeforeResetSelect -= Time.deltaTime;
            if (m_TimerBeforeResetSelect < 0)
            {
                m_CanSelect = true;
                m_CurrentTimerBeforeResetSelect = m_TimerBeforeResetSelect;
            }
        }
    }

    public void ButtonSelected(FunctionToCall function, int index)
    {
        if (function == FunctionToCall.None) return;
        if (function == FunctionToCall.SwitchScene) SwitchScene(index);
        else if (function == FunctionToCall.SwitchMission) SwitchMission(index);
        else if (function == FunctionToCall.QualitySettings) SwitchQuality(index);
        else if (function == FunctionToCall.SwitchVrSettings) SwitchVrSettings(index);
        else if (function == FunctionToCall.ChangeFloatValue) ChangeFloatValue(index);
        else if (function == FunctionToCall.Back) Back();
    }

/*    public void SliderSelected(FunctionToCall function, int index, float sliderValue)
    {
        if (function == FunctionToCall.None) return;
        if (function == FunctionToCall.ChangeSliderValue) ChangeSliderValue(index, sliderValue);
    }*/

    public void SwitchMissionTab(int tabNumber)
    {
        if (m_CurrentTabIndex != -1)
            m_Tabs[m_CurrentTabIndex].SetActive(!m_Tabs[m_CurrentTabIndex].activeSelf);
        m_Tabs[tabNumber].SetActive(!m_Tabs[tabNumber].activeSelf);
        m_PreviousTabIndex = m_CurrentTabIndex;
        m_CurrentTabIndex = tabNumber;
    }

    void SwitchScene(int sceneNumber)
    {
        if (sceneNumber == 0) SceneManager.LoadScene("MainMenu");
        else if (sceneNumber == 1) SceneManager.LoadScene("Scenario_1");
        else if (sceneNumber == 2) SceneManager.LoadScene("Scenario_2");
        else if (sceneNumber == 3) SceneManager.LoadScene("Mission_1");
        else if (sceneNumber == 4) SceneManager.LoadScene("LavaTube");
        else if (sceneNumber == 5) SceneManager.LoadScene("Apollo_15");
        else if (sceneNumber == 5) SceneManager.LoadScene("TestingEnvironment");

    }

    void SwitchMission(int missionNumber)
    {
        if (missionNumber == 0) Debug.Log("Mission " + (missionNumber + 1) + " Activated");
        else if (missionNumber == 1) Debug.Log("Mission " + (missionNumber + 1) + " Activated");
        else if (missionNumber == 2) Debug.Log("Mission " + (missionNumber + 1) + " Activated");
        else if (missionNumber == 3) Debug.Log("Mission " + (missionNumber + 1) + " Activated");
        else if (missionNumber == 4) Debug.Log("Mission " + (missionNumber + 1) + " Activated");
        else if (missionNumber == 5) Debug.Log("Mission " + (missionNumber + 1) + " Activated");
        else if (missionNumber == 6) Debug.Log("Mission " + (missionNumber + 1) + " Activated");
        else if (missionNumber == 7) Debug.Log("Mission " + (missionNumber + 1) + " Activated");
    }

    void SwitchQuality(int qualityIndex)
    {
        if (qualityIndex == 0) QualitySettings.SetQualityLevel(qualityIndex);
        else if (qualityIndex == 1) QualitySettings.SetQualityLevel(qualityIndex);
        else if (qualityIndex == 2) QualitySettings.SetQualityLevel(qualityIndex);
    }

    void SwitchVrSettings(int settingIndex)
    {
        if (settingIndex == 0) m_VrSettings.SetFreeLocomotion(!m_VrSettings.UsingFreeLocomotion());
        else if (settingIndex == 1) m_VrSettings.SetControllerRotation(!m_VrSettings.UsingControllerRotation());
        else if (settingIndex == 2) m_VrSettings.SetSnapRotation(!m_VrSettings.UsingSnapRotation());
        else if (settingIndex == 3) m_VrSettings.SetHandsForward(!m_VrSettings.UsingHandsForward());
        else if (settingIndex == 4) m_VrSettings.SetRightHanded(!m_VrSettings.IsRightHanded());
    }

/*    void ChangeSliderValue(int sliderIndex, float value)
    {
        if (sliderIndex == 0) m_AudioVolume.SetFloat("Master", Mathf.Log(value));// * 20);
        else if (sliderIndex == 1) m_AudioVolume.SetFloat("Music", Mathf.Log(value));// * 20);
        else if (sliderIndex == 2) m_AudioVolume.SetFloat("SFX", Mathf.Log(value));// * 20);
        else if (sliderIndex == 3) m_AudioVolume.SetFloat("Dialogue", Mathf.Log(value));// * 20);
        else if (sliderIndex == 4) m_VrSettings.SetSnapDegrees(value);
    }*/

    void ChangeFloatValue(int sliderIndex)
    {
        float newValue = 0.0f;

        switch (sliderIndex)
        {
            case 0: //Master Up
                if(m_CurrentMasterVol < 0.1)
                {
                    m_CurrentMasterVol = 0;
                }
                m_CurrentMasterVol += 5.0f;
                m_CurrentMasterVol = Math.Clamp(m_CurrentMasterVol, 0.0f, 100.0f);
                m_MasterVol.text = m_CurrentMasterVol.ToString();
                newValue = Math.Clamp(m_CurrentMasterVol, 0.001f, 100.0f);
                m_AudioVolume.SetFloat("Master", PercentToDecible(newValue));
                break;
            case 1: //Master Down
                if (m_CurrentMasterVol < 0.1)
                {
                    m_CurrentMasterVol = 0;
                }
                m_CurrentMasterVol -= 5.0f;
                m_CurrentMasterVol = Math.Clamp(m_CurrentMasterVol, 0.0f, 100.0f);
                m_MasterVol.text = m_CurrentMasterVol.ToString();
                newValue = Math.Clamp(m_CurrentMasterVol, 0.001f, 100.0f);
                m_AudioVolume.SetFloat("Master", PercentToDecible(newValue));
                break;
            case 2: //Music Up
                if (m_CurrentMusicVol < 0.1)
                {
                    m_CurrentMusicVol = 0;
                }
                m_CurrentMusicVol += 5.0f;
                m_CurrentMusicVol = Math.Clamp(m_CurrentMusicVol, 0.0f, 100.0f);
                m_MusicVol.text = m_CurrentMusicVol.ToString();
                newValue = Math.Clamp(m_CurrentMusicVol, 0.001f, 100.0f);
                m_AudioVolume.SetFloat("Music", PercentToDecible(newValue));
                break;
            case 3: //Music Down
                if (m_CurrentMusicVol < 0.1)
                {
                    m_CurrentMusicVol = 0;
                }
                m_CurrentMusicVol -= 5.0f;
                m_CurrentMusicVol = Math.Clamp(m_CurrentMusicVol, 0.0f, 100.0f);
                m_MusicVol.text = m_CurrentMusicVol.ToString();
                newValue = Math.Clamp(m_CurrentMusicVol, 0.001f, 100.0f);
                m_AudioVolume.SetFloat("Music", PercentToDecible(newValue));
                break;
            case 4: //SFX Up
                if (m_CurrentSFXVol < 0.1)
                {
                    m_CurrentSFXVol = 0;
                }
                m_CurrentSFXVol += 5.0f;
                m_CurrentSFXVol = Math.Clamp(m_CurrentSFXVol, 0.0f, 100.0f);
                m_SFXVol.text = m_CurrentSFXVol.ToString();
                newValue = Math.Clamp(m_CurrentSFXVol, 0.001f, 100.0f);
                m_AudioVolume.SetFloat("SFX", PercentToDecible(newValue));
                break;
            case 5: //SFX Down
                if (m_CurrentSFXVol < 0.1)
                {
                    m_CurrentSFXVol = 0;
                }
                m_CurrentSFXVol -= 5.0f;
                m_CurrentSFXVol = Math.Clamp(m_CurrentSFXVol, 0.0f, 100.0f);
                m_SFXVol.text = m_CurrentSFXVol.ToString();
                newValue = Math.Clamp(m_CurrentSFXVol, 0.001f, 100.0f);
                m_AudioVolume.SetFloat("SFX", PercentToDecible(newValue));
                break;
            case 6: //Dialogue Up
                if (m_CurrentDialogueVol < 0.1)
                {
                    m_CurrentDialogueVol = 0;
                }
                m_CurrentDialogueVol += 5.0f;
                m_CurrentDialogueVol = Math.Clamp(m_CurrentDialogueVol, 0.0f, 100.0f);
                m_DialogueVol.text = m_CurrentDialogueVol.ToString();
                newValue = Math.Clamp(m_CurrentDialogueVol, 0.001f, 100.0f);
                m_AudioVolume.SetFloat("Dialogue", PercentToDecible(newValue));
                break;
            case 7: //Dialogue Down
                if (m_CurrentDialogueVol < 0.1)
                {
                    m_CurrentDialogueVol = 0;
                }
                m_CurrentDialogueVol -= 5.0f;
                m_CurrentDialogueVol = Math.Clamp(m_CurrentDialogueVol, 0.0f, 100.0f);
                m_DialogueVol.text = m_CurrentDialogueVol.ToString();
                newValue = Math.Clamp(m_CurrentDialogueVol, 0.001f, 100.0f);
                m_AudioVolume.SetFloat("Dialogue", PercentToDecible(newValue));
                break;
            case 8: //Snap Rotation Up
                m_CurrentSnapDegrees = Math.Clamp(m_CurrentSnapDegrees + 5f, 5f, 45f);
                m_VrSettings.SetSnapDegrees(m_CurrentSnapDegrees);
                m_SnapDegrees.text = m_CurrentSnapDegrees.ToString();
                break;
            case 9: //Snap Rotation Down
                m_CurrentSnapDegrees = Math.Clamp(m_CurrentSnapDegrees - 5f, 5f, 45f);
                m_VrSettings.SetSnapDegrees(m_CurrentSnapDegrees);
                m_SnapDegrees.text = m_CurrentSnapDegrees.ToString();
                break;
            default:
                break;
        }
    }

    float PercentToDecible(float perc)
    {
        float dB = (10.0f * Mathf.Log(10.0f * perc)) - (10.0f * Mathf.Log(10.0f * 50.0f));
        return dB;
    }

    float DecibleToPercent(float dB)
    {
        float perc = Mathf.Pow(10.0f, dB / 10.0f) * 50;
        return perc;
    }




    void Back() { SwitchMissionTab(m_PreviousTabIndex); }
    public void SetCanPlayerSelect(bool b) => m_CanSelect = b;
    public bool GetCanPlayerSelect() { return m_CanSelect; }

    public int GetCurrentIndex() { return m_CurrentTabIndex; }

}
