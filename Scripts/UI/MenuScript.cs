using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*************************************
Class: MenuScript
Author: Charlie Weir
Created: 
Last Modified: 27/01/23
Description: This class is in charge of controlling the menu.

Change Log
**********
Date: 27/01/23
Author: Alexander Achorn
Verified By: Antoine Plouffe
Changes: Added Vr Settings

Date: 02/02/23
Author: Antoine Plouffe
Verified By: Alexander Achorn 
Changes: Null Check

Date: 07/04/23
Author: Alexander Achorn
Verified By: 
Changes: Fixed the fact that the game doesn't load saves when you start from menu and added a Scene Handler so things arnt so rittled with errors
*************************************/

public class MenuScript : MonoBehaviour
{
    SavingSystem m_SavingSystem;
    [SerializeField] GameObject StartButton;
    [SerializeField] GameObject ScenariosButton;
    [SerializeField] GameObject ScenariosMode;
    [SerializeField] GameObject LoadButton;
    [SerializeField] GameObject NewBackButton;
    [SerializeField] GameObject QuitButton;
    [SerializeField] GameObject OptionsButton;
    [SerializeField] GameObject CreditsButton;
    [SerializeField] GameObject HelpButton;
    [SerializeField] GameObject LoadScene1Button;
    [SerializeField] GameObject LoadScene2Button;
    [SerializeField] GameObject LoadScene3Button;
    [SerializeField] GameObject LoadScene4Button;
    [SerializeField] GameObject LoadTestingSceneButton;
    [SerializeField] GameObject LoadCharlieTestingSceneButton;
    [SerializeField] GameObject VolumeSlider;
    [SerializeField] GameObject MusicVolumeSlider;
    
    [SerializeField] GameObject SoundFXVolumeSlider;
    [SerializeField] GameObject BeginButton;
    [SerializeField] GameObject MuseumMode;
    [SerializeField] GameObject CreditsText;
    [SerializeField] Dropdown GravityDropdown;
    [SerializeField] Dropdown QualityDropdown;

    [SerializeField] GameObject ControlsLayout;
    [SerializeField] GameObject LeftController;
    [SerializeField] GameObject RightController;
    [SerializeField] GameObject JumpText;
    [SerializeField] GameObject MovementText;
    [SerializeField] GameObject GrabText;
    [SerializeField] GameObject TeleportText;
    [SerializeField] GameObject RotationText;
    [SerializeField] GameObject RightGrabText;

    [SerializeField] GameObject JumpButton;
    [SerializeField] GameObject MovementButton;
    [SerializeField] GameObject GrabButton;
    [SerializeField] GameObject TeleportButton;
    [SerializeField] GameObject RotationButton;
    [SerializeField] GameObject RightGrabButton;

    [SerializeField] AudioSource m_BackgroundMusic;


    [SerializeField] Text VolumeText;
    [SerializeField] Text MusicVolumeText;
    [SerializeField] Text SoundFXVolumeText;
    [SerializeField] Camera m_camera;
    [SerializeField] GameObject BackButton;
    [SerializeField] Text Text;

    //[SerializeField] GameObject MenuMusic;

    Fade fader;
    AudioSource MenuMusicAudio;
    GravityController gravity;
    [SerializeField] VRSettingsController vrSettings;
    public bool IsMuseumMode = false;
    Vector3 InitialStartButtonLocation;
    Vector3 StartButtonLocation;
    float MasterVolume;
    float MusicVolume;
    float SFXVolume;
    float DialogueVolume;
    //float FadeDur = 2.69f;

    [SerializeField] AudioMixer m_AudioVolume;
    [SerializeField] private Slider m_SliderVolumeMaster;
    [SerializeField] private Slider m_SliderMusicVolume;
    [SerializeField] private Slider m_SliderDialoguesMaster;
    [SerializeField] private Slider m_SliderSFXVolume;

    [SerializeField] GameObject VolumesToggles;
    [SerializeField] GameObject DialogueVolumeSlider;

    //[SerializeField] AudioMixerGroup m_AudioVolumeMaster;
    //[SerializeField] AudioMixerGroup m_AudioVolumeMusic;
    //[SerializeField] AudioMixerGroup m_AudioVolumeDialogues;
    //[SerializeField] AudioMixerGroup m_AudioVolumeVFX;

    [SerializeField] GameObject m_Controls;

    //bools
    public bool m_MasterVolumeUp;
    public bool m_MasterVolumeDown;
    public bool m_MusicVolumeUp;
    public bool m_MusicVolumeDown;
    public bool m_DialogueVolumeUp;
    public bool m_DialogueVolumeDown;
    public bool m_SFXVolumeUp;
    public bool m_SFXVolumeDown;

    // Start is called before the first frame update
    void Start()
    {
        m_SavingSystem = FindObjectOfType<SavingSystem>();
        //if (MenuMusic == null)
        //{

        //    MenuMusic = GameObject.FindGameObjectWithTag("Sound");
        //}

        this.gameObject.SetActive(true);
        gravity = GetComponent<GravityController>();
        OpenMainMenu();

        if(m_camera)
            fader = m_camera.GetComponent<Fade>();
        //MenuMusicAudio = MenuMusic.GetComponent<AudioSource>();
        //DontDestroyOnLoad(gameObject);
        InitialStartButtonLocation = StartButton.transform.position;
        StartButtonLocation = InitialStartButtonLocation;

        MasterVolume = 1.0f;
        MusicVolume = 1.0f;
        DialogueVolume = 1.0f;
        SFXVolume = 1.0f;

        SetMasterVolume(MasterVolume);
        SetMusicVolume(MusicVolume);
        SetSoundDialogueVolume(DialogueVolume);
        SetSoundFXVolume(SFXVolume);
    }

    private void Update()
    {/*
        if(SceneManager.GetActiveScene().buildIndex != 0)
            gameObject.SetActive(false);
*/
        //if (LeftController.gameObject.activeSelf)
        //{
        //    //ControlsHelp();
        //}

        //if (SceneManager.GetActiveScene() != SceneManager.GetSceneByBuildIndex(0))
        //{
        //    StartButton.SetActive(false);
        //}

        if (m_MasterVolumeUp)
            MasterVolumeUp();
        else if (m_MasterVolumeDown)
            MasterVolumeDown();

        if (m_MusicVolumeUp)
            MusicVolumeUp();
        else if (m_MusicVolumeDown)
            MusicVolumeDown();

        if (m_DialogueVolumeUp)
            DialogueVolumeUp();
        else if (m_DialogueVolumeDown)
            DialogueVolumeDown();

        if (m_SFXVolumeUp)
            SFXVolumeUp();
        else if (m_SFXVolumeDown)
            SFXVolumeDown();

    }

    private void ControlsHelp()
    {
        LeftController.GetComponent<Highlight>().IsActive(true);
        RightController.GetComponent<Highlight>().IsActive(true);

        JumpText.SetActive(true);
        JumpText.GetComponent<LineRenderer>().SetPosition(0, JumpText.transform.GetChild(0).gameObject.transform.position);
        JumpText.GetComponent<LineRenderer>().SetPosition(1,
            LeftController.GetComponent<Highlight>().m_ListRenderers[1].transform.parent.transform.GetChild(1).gameObject
                .transform.position);

        MovementText.SetActive(true);
        MovementText.GetComponent<LineRenderer>()
            .SetPosition(0, MovementText.transform.GetChild(0).gameObject.transform.position);
        MovementText.GetComponent<LineRenderer>().SetPosition(1,
            LeftController.GetComponent<Highlight>().m_ListRenderers[0].transform.parent.transform.GetChild(1).gameObject
                .transform.position);

        GrabText.SetActive(true);
        GrabText.GetComponent<LineRenderer>().SetPosition(0, GrabText.transform.GetChild(0).gameObject.transform.position);
        GrabText.GetComponent<LineRenderer>().SetPosition(1,
            LeftController.GetComponent<Highlight>().m_ListRenderers[2].transform.parent.transform.GetChild(1).gameObject
                .transform.position);
        //GrabText.GetComponent<LineRenderer>().SetPosition(2, MovementText.transform.GetChild(0).gameObject.transform.position);
        //GrabText.GetComponent<LineRenderer>().SetPosition(3, LeftController.GetComponent<Highlight>().m_ListRenderers[3].transform.parent.transform.GetChild(1).gameObject.transform.position);

        TeleportText.SetActive(true);
        TeleportText.GetComponent<LineRenderer>()
            .SetPosition(0, TeleportText.transform.GetChild(0).gameObject.transform.position);
        TeleportText.GetComponent<LineRenderer>().SetPosition(1,
            RightController.GetComponent<Highlight>().m_ListRenderers[1].transform.parent.transform.GetChild(1).gameObject
                .transform.position);

        RotationText.SetActive(true);
        RotationText.GetComponent<LineRenderer>()
            .SetPosition(0, RotationText.transform.GetChild(0).gameObject.transform.position);
        RotationText.GetComponent<LineRenderer>().SetPosition(1,
            RightController.GetComponent<Highlight>().m_ListRenderers[0].transform.parent.transform.GetChild(1).gameObject
                .transform.position);

        RightGrabText.SetActive(true);
        RightGrabText.GetComponent<LineRenderer>()
            .SetPosition(0, RightGrabText.transform.GetChild(0).gameObject.transform.position);
        RightGrabText.GetComponent<LineRenderer>().SetPosition(1,
            RightController.GetComponent<Highlight>().m_ListRenderers[2].transform.parent.transform.GetChild(1).gameObject
                .transform.position);
    }

    public void ControllerLeftRotationTrigger()
    {
        JumpText.GetComponent<LineRenderer>().enabled = true;
        MovementText.GetComponent<LineRenderer>().enabled = false;
        GrabText.GetComponent<LineRenderer>().enabled = false;
        TeleportText.GetComponent<LineRenderer>().enabled = false;
        RotationText.GetComponent<LineRenderer>().enabled = false;
        RightGrabText.GetComponent<LineRenderer>().enabled = false;
        LeftController.transform.rotation = Quaternion.Euler(90, 0.0f, 206);
    }

    public void ControllerLeftRotationTrackpad()
    {
        JumpText.GetComponent<LineRenderer>().enabled = false;
        MovementText.GetComponent<LineRenderer>().enabled = true;
        GrabText.GetComponent<LineRenderer>().enabled = false;
        TeleportText.GetComponent<LineRenderer>().enabled = false;
        RotationText.GetComponent<LineRenderer>().enabled = false;
        RightGrabText.GetComponent<LineRenderer>().enabled = false;
        LeftController.transform.rotation = Quaternion.Euler(90, 0.0f, 345);
    }

    public void ControllerLeftRotationSideButtons()
    {
        JumpText.GetComponent<LineRenderer>().enabled = false;
        MovementText.GetComponent<LineRenderer>().enabled = false;
        GrabText.GetComponent<LineRenderer>().enabled = true;
        TeleportText.GetComponent<LineRenderer>().enabled = false;
        RotationText.GetComponent<LineRenderer>().enabled = false;
        RightGrabText.GetComponent<LineRenderer>().enabled = false;
        LeftController.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 180));
    }

    public void ControllerRightRotationTrigger()
    {
        TeleportText.GetComponent<LineRenderer>().enabled = false;
        MovementText.GetComponent<LineRenderer>().enabled = false;
        GrabText.GetComponent<LineRenderer>().enabled = false;
        TeleportText.GetComponent<LineRenderer>().enabled = true;
        RotationText.GetComponent<LineRenderer>().enabled = false;
        RightGrabText.GetComponent<LineRenderer>().enabled = false;
        RightController.transform.rotation = Quaternion.Euler(90, 0.0f, -206);
    }

    public void ControllerRightRotationTrackpad()
    {
        TeleportText.GetComponent<LineRenderer>().enabled = false;
        MovementText.GetComponent<LineRenderer>().enabled = false;
        GrabText.GetComponent<LineRenderer>().enabled = false;
        TeleportText.GetComponent<LineRenderer>().enabled = false;
        RotationText.GetComponent<LineRenderer>().enabled = true;
        RightGrabText.GetComponent<LineRenderer>().enabled = false;
        RightController.transform.rotation = Quaternion.Euler(90, 0.0f, -345);
    }

    public void ControllerRightRotationSideButtons()
    {
        TeleportText.GetComponent<LineRenderer>().enabled = false;
        MovementText.GetComponent<LineRenderer>().enabled = false;
        GrabText.GetComponent<LineRenderer>().enabled = false;
        TeleportText.GetComponent<LineRenderer>().enabled = false;
        RotationText.GetComponent<LineRenderer>().enabled = false;
        RightGrabText.GetComponent<LineRenderer>().enabled = true;
        RightController.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 180));
    }

    /*public void Load()
    {

        GameObject system = GameObject.Find("Saving");

        if (system)
            system.GetComponent<SavingWrapper>().Load();
        StartCoroutine(Loading());
    }*/
    /*   private IEnumerator Loading()
       {
           //Fade out

           // Load Level

           //Fade in
           SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();

           wrapper.Save();
           fader.FadeOut(FadeDur);
           yield return SceneManager.LoadSceneAsync("Scenario_1");
           //yield return SceneManager.LoadSceneAsync("Scenario_" + SceneToLoad.ToString());
           fader.FadeIn(FadeDur);

       }*/

    public void OpenScenarios()
    {

        //VolumeText.text = "Volume: " + Volume.ToString();
        //MusicVolumeText.text = "Music Volume: " + MusicVolume.ToString();
        //SoundFXVolumeText.text = "SoundFX Volume: " + SoundFXVolume.ToString();
        Text.text = "Scenarios";
        m_Controls.SetActive(false);
        ScenariosMode.SetActive(true);
        LoadScene2Button.SetActive(true);
        LoadScene3Button.SetActive(true);
        LoadScene4Button.SetActive(true);
        LoadTestingSceneButton.SetActive(true);
        NewBackButton.SetActive(true);

        BackButton.SetActive(false);
        ScenariosButton.SetActive(false);
        LoadCharlieTestingSceneButton.SetActive(false);
        StartButton.SetActive(false);
        LoadButton.SetActive(false);
        QuitButton.SetActive(false);
        OptionsButton.SetActive(false);
        CreditsButton.SetActive(false);
        HelpButton.SetActive(false);
        ControlsLayout.SetActive(false);
        VolumeSlider.SetActive(false);
        MusicVolumeSlider.SetActive(false);
        SoundFXVolumeSlider.SetActive(false);
        MuseumMode.SetActive(false);
        BeginButton.SetActive(false);
        GravityDropdown.gameObject.SetActive(false);
        QualityDropdown.gameObject.SetActive(false);
        CreditsText.SetActive(false);
        LeftController.SetActive(false);
        RightController.SetActive(false);
        JumpText.GetComponent<LineRenderer>().enabled = false;
        MovementText.GetComponent<LineRenderer>().enabled = false;
        GrabText.GetComponent<LineRenderer>().enabled = false;
        TeleportText.GetComponent<LineRenderer>().enabled = false;
        RotationText.GetComponent<LineRenderer>().enabled = false;
        RightGrabText.GetComponent<LineRenderer>().enabled = false;
        if (VolumesToggles)
            VolumesToggles.SetActive(false);
        DialogueVolumeSlider.SetActive(false);
    }

    public void OpenSettings()
    {
        
        VolumeText.text = "Master Volume: ";
        MusicVolumeText.text = "Music Volume: ";
        SoundFXVolumeText.text = "SoundFX Volume: ";
        Text.text = "Settings";
        m_Controls.SetActive(false);
        if (VolumesToggles)
            VolumesToggles.SetActive(true);
        StartButton.SetActive(false);
        ScenariosButton.SetActive(false);
        LoadButton.SetActive(false); 
        QuitButton.SetActive(false); 
        OptionsButton.SetActive(false);
        CreditsButton.SetActive(false);
        HelpButton.SetActive(false);
        ControlsLayout.SetActive(false);
        BackButton.SetActive(false);
        LoadScene1Button.SetActive(false);
        LoadScene2Button.SetActive(false);
        LoadScene3Button.SetActive(false);
        LoadScene4Button.SetActive(false);
        LoadTestingSceneButton.SetActive(false);
        LoadCharlieTestingSceneButton.SetActive(false);
        VolumeSlider.SetActive(true);
        MusicVolumeSlider.SetActive(true);
        SoundFXVolumeSlider.SetActive(true);
        MuseumMode.SetActive(true);
        BeginButton.SetActive(false);
        //GravityDropdown.gameObject.SetActive(true);  
        QualityDropdown.gameObject.SetActive(true);  
        CreditsText.SetActive(false);
        NewBackButton.SetActive(true);
        LeftController.SetActive(false);
        RightController.SetActive(false);
        JumpText.GetComponent<LineRenderer>().enabled = false;
        MovementText.GetComponent<LineRenderer>().enabled = false;
        GrabText.GetComponent<LineRenderer>().enabled = false;
        TeleportText.GetComponent<LineRenderer>().enabled = false;
        RotationText.GetComponent<LineRenderer>().enabled = false;
        RightGrabText.GetComponent<LineRenderer>().enabled = false;
        JumpButton.SetActive(false);
        MovementButton.SetActive(false);
        GrabButton.SetActive(false);
        TeleportButton.SetActive(false);
        RotationButton.SetActive(false);
        RightGrabButton.SetActive(false);
        DialogueVolumeSlider.SetActive(true);
    }
    public void OpenHelp()
    {
        Text.text = "Help";
        //LeftController.SetActive(true);
        //RightController.SetActive(true);

        //JumpButton.SetActive(true);
        //MovementButton.SetActive(true);
        //GrabButton.SetActive(true);
        //TeleportButton.SetActive(true);
        //RotationButton.SetActive(true);
        //RightGrabButton.SetActive(true);

        m_Controls.SetActive(true);

        StartButton.SetActive(false);
        ScenariosButton.SetActive(false);
        LoadButton.SetActive(false);
        QuitButton.SetActive(false);
        OptionsButton.SetActive(false);
        HelpButton.SetActive(false);
        ControlsLayout.SetActive(false);
        CreditsButton.SetActive(false);
        BackButton.SetActive(false);
        LoadScene1Button.SetActive(false);
        LoadScene2Button.SetActive(false);
        LoadScene3Button.SetActive(false);
        LoadScene4Button.SetActive(false);
        BeginButton.SetActive(false);

        LoadTestingSceneButton.SetActive(false);
        LoadCharlieTestingSceneButton.SetActive(false);
        VolumeSlider.SetActive(false);
        MusicVolumeSlider.SetActive(false);
        SoundFXVolumeSlider.SetActive(false);
        MuseumMode.SetActive(false);
        GravityDropdown.gameObject.SetActive(false);
        QualityDropdown.gameObject.SetActive(false);
        CreditsText.SetActive(false);

        NewBackButton.SetActive(true);
        JumpText.GetComponent<LineRenderer>().enabled = false;
        MovementText.GetComponent<LineRenderer>().enabled = false;
        GrabText.GetComponent<LineRenderer>().enabled = false;
        TeleportText.GetComponent<LineRenderer>().enabled = false;
        RotationText.GetComponent<LineRenderer>().enabled = false;
        RightGrabText.GetComponent<LineRenderer>().enabled = false;
        if (VolumesToggles)
            VolumesToggles.SetActive(false);
        DialogueVolumeSlider.SetActive(false);
    }
    public void SetMuseumMode()
    {
        IsMuseumMode = !IsMuseumMode;
        //if(IsMuseumMode)
        //{
        //    StartButtonLocation.y -= 75.0f;
        //}
        //else
        //{
        //    StartButtonLocation = InitialStartButtonLocation;
        //}

        StartButton.transform.position = StartButtonLocation;
    }

    public void OpenCredits()
    {

        Text.text = "Credits";
        m_Controls.SetActive(false);
        StartButton.SetActive(false);
        ScenariosButton.SetActive(false);
        LoadButton.SetActive(false);
        QuitButton.SetActive(false);
        OptionsButton.SetActive(false);
        HelpButton.SetActive(false);
        ControlsLayout.SetActive(false);
        CreditsButton.SetActive(false);
        BackButton.SetActive(false);
        LoadScene1Button.SetActive(true);
        LoadScene2Button.SetActive(true);
        LoadScene3Button.SetActive(true);
        LoadScene4Button.SetActive(true);

        NewBackButton.SetActive(true);

        BeginButton.SetActive(false);

        LoadTestingSceneButton.SetActive(true);
        LoadCharlieTestingSceneButton.SetActive(true);
        VolumeSlider.SetActive(false);
        MusicVolumeSlider.SetActive(false);
        SoundFXVolumeSlider.SetActive(false);
        MuseumMode.SetActive(false);
        GravityDropdown.gameObject.SetActive(false);
        QualityDropdown.gameObject.SetActive(false);
        CreditsText.SetActive (true);

        LeftController.SetActive(false);
        RightController.SetActive(false);
        JumpText.GetComponent<LineRenderer>().enabled = false;
        MovementText.GetComponent<LineRenderer>().enabled = false;
        GrabText.GetComponent<LineRenderer>().enabled = false;
        TeleportText.GetComponent<LineRenderer>().enabled = false;
        RotationText.GetComponent<LineRenderer>().enabled = false;
        RightGrabText.GetComponent<LineRenderer>().enabled = false;
        JumpButton.SetActive(false);
        MovementButton.SetActive(false);
        GrabButton.SetActive(false);
        TeleportButton.SetActive(false);
        RotationButton.SetActive(false);
        RightGrabButton.SetActive(false);
        if (VolumesToggles)
            VolumesToggles.SetActive(false);
        DialogueVolumeSlider.SetActive(false);
    }
    public void OpenMainMenu()
    {
        Text.text = "Main Menu";
        StartButton.SetActive(true);

        //if(IsMuseumMode== false)
        //{
        //LoadButton.SetActive(true);
        //}
        m_Controls.SetActive(false);
        NewBackButton.SetActive(false);
        ScenariosButton.SetActive(true);
        ScenariosMode.SetActive(false);
        QuitButton.SetActive(true);
        OptionsButton.SetActive(true);
        CreditsButton.SetActive(true);
        HelpButton.SetActive(true);
        ControlsLayout.SetActive(false);
        BackButton.SetActive(false);
        LoadScene1Button.SetActive(true);
        LoadScene2Button.SetActive(true);
        LoadScene3Button.SetActive(true);
        LoadScene4Button.SetActive(true);
        BeginButton.SetActive(false);
        LoadTestingSceneButton.SetActive(true);
        LoadCharlieTestingSceneButton.SetActive(true);
        MuseumMode.SetActive(false);
        VolumeSlider.SetActive(false);
        MusicVolumeSlider.SetActive(false);
        SoundFXVolumeSlider.SetActive(false);
        GravityDropdown.gameObject.SetActive(false);
        QualityDropdown.gameObject.SetActive(false);
        CreditsText.SetActive(false);
        LeftController.SetActive(false);
        RightController.SetActive(false);
        JumpText.GetComponent<LineRenderer>().enabled = false;
        MovementText.GetComponent<LineRenderer>().enabled = false;
        GrabText.GetComponent<LineRenderer>().enabled = false;
        TeleportText.GetComponent<LineRenderer>().enabled = false;
        RotationText.GetComponent<LineRenderer>().enabled = false;
        RightGrabText.GetComponent<LineRenderer>().enabled = false;
        JumpButton.SetActive(false);
        MovementButton.SetActive(false);
        GrabButton.SetActive(false);
        TeleportButton.SetActive(false);
        RotationButton.SetActive(false);
        RightGrabButton.SetActive(false);
        if(VolumesToggles)
            VolumesToggles.SetActive(false);
        DialogueVolumeSlider.SetActive(false);
    }

    public void StartingGame()
    {
        Text.text = "Game Setup";
        m_Controls.SetActive(false);
        NewBackButton.SetActive(false);
        StartButton.SetActive(false);
        ScenariosButton.SetActive(false);
        LoadButton.SetActive(false);
        QuitButton.SetActive(false);
        OptionsButton.SetActive(false);
        CreditsButton.SetActive(false);
        HelpButton.SetActive(false);
        ControlsLayout.SetActive(false);
        BackButton.SetActive(true);
        LoadScene1Button.SetActive(false);
        LoadScene2Button.SetActive(false);
        LoadScene3Button.SetActive(false);
        LoadScene4Button.SetActive(false);
        LoadTestingSceneButton.SetActive(false);
        LoadCharlieTestingSceneButton.SetActive(false);
        VolumeSlider.SetActive(false);
        MusicVolumeSlider.SetActive(false);
        SoundFXVolumeSlider.SetActive(false);
        MuseumMode.SetActive(false);
        GravityDropdown.gameObject.SetActive(false);
        QualityDropdown.gameObject.SetActive(false);
        CreditsText.SetActive(false);
        BeginButton.SetActive(true);
        LeftController.SetActive(false);
        RightController.SetActive(false);
        JumpText.GetComponent<LineRenderer>().enabled = false;
        MovementText.GetComponent<LineRenderer>().enabled = false;
        GrabText.GetComponent<LineRenderer>().enabled = false;
        TeleportText.GetComponent<LineRenderer>().enabled = false;
        RotationText.GetComponent<LineRenderer>().enabled = false;
        RightGrabText.GetComponent<LineRenderer>().enabled = false;
        JumpButton.SetActive(false);
        MovementButton.SetActive(false);
        GrabButton.SetActive(false);
        TeleportButton.SetActive(false);
        RotationButton.SetActive(false);
        RightGrabButton.SetActive(false);
        if (VolumesToggles)
            VolumesToggles.SetActive(false);
        DialogueVolumeSlider.SetActive(false);
    }

    public void MasterVolumeUp()
    {
        MasterVolume += 0.01f;
        Math.Clamp(MasterVolume, 0.001f, 1.0f);
        SetMasterVolume(MasterVolume);
    }

    public void MasterVolumeDown()
    {
        MasterVolume -= 0.01f;
        Math.Clamp(MasterVolume, 0.001f, 2.0f);
        SetMasterVolume(MasterVolume);
    }

    public void BoolMasterVolumeUp(bool b) { m_MasterVolumeUp = b; }
    public void BoolMasterVolumeDown(bool b) { m_MasterVolumeDown = b; }

    public void BoolMusicVolumeUp(bool b) { m_MusicVolumeUp = b; }
    public void BoolMusicVolumeDown(bool b) { m_MusicVolumeDown = b; }

    public void BoolDialogueVolumeUp(bool b) { m_DialogueVolumeUp = b; }
    public void BoolDialogueVolumeDown(bool b) { m_DialogueVolumeDown = b; }

    public void BoolSFXVolumeUp(bool b) { m_SFXVolumeUp = b; }
    public void BoolSFXVolumeDown(bool b) { m_SFXVolumeDown = b; }
    public void MusicVolumeUp()
    {
        MusicVolume += 0.01f;
        Math.Clamp(MusicVolume, 0.001f, 2.0f);
        SetMusicVolume(MusicVolume);
    }

    public void MusicVolumeDown()
    {
        MusicVolume -= 0.01f;
        Math.Clamp(MusicVolume, 0.001f, 2.0f);
        SetMusicVolume(MusicVolume);
    }

    public void DialogueVolumeUp()
    {
        DialogueVolume += 0.01f;
        Math.Clamp(DialogueVolume, 0.001f, 2.0f);
        SetSoundDialogueVolume(DialogueVolume);
    }

    public void DialogueVolumeDown()
    {
        DialogueVolume -= 0.01f;
        Math.Clamp(DialogueVolume, 0.001f, 2.0f);
        SetSoundDialogueVolume(DialogueVolume);
    }

    public void SFXVolumeUp()
    {
        SFXVolume += 0.01f;
        Math.Clamp(SFXVolume, 0.001f, 2.0f);
        SetSoundFXVolume(SFXVolume);
    }

    public void SFXVolumeDown()
    {
        SFXVolume -= 0.01f;
        Math.Clamp(SFXVolume, 0.001f, 2.0f);
        SetSoundFXVolume(SFXVolume);
    }

    public void SetMasterVolume(float volume)
    {
        m_AudioVolume.SetFloat("Master", Mathf.Log(volume) * 20);
        m_SliderVolumeMaster.value = volume;
    }
    public void SetMusicVolume(float volume)
    {
        m_AudioVolume.SetFloat("Music", Mathf.Log(volume) * 20);
        m_SliderMusicVolume.value = volume;
    }

    public void SetSoundFXVolume(float volume)
    {
        m_AudioVolume.SetFloat("SFX", Mathf.Log(volume) * 20);
        m_SliderSFXVolume.value = volume;
    }
    public void SetSoundDialogueVolume(float volume)
    {
        m_AudioVolume.SetFloat("Dialogue", Mathf.Log(volume) * 20);
        m_SliderDialoguesMaster.value = volume;
    }
    public void SetQuality(int value)
    {
        QualitySettings.SetQualityLevel(value);
    }

    public void SetGravity(int value)
    {
  
        switch (value)
        {
            case 0:
            gravity.SetEarthGravity();

                break;

            case 1:
                gravity.SetMoonGravity();

                break;
            case 2:
                gravity.SetZeroGravity();

                break;
        }
        GravityDropdown.value = value;

        Debug.Log("value is " + value);
    }

    public void SetFreeLocomotion(bool state)
    {
        vrSettings.SetFreeLocomotion(state);
    }

    public void SetControllerRotation(bool state)
    {
        vrSettings.SetControllerRotation(state);
    }

    public void SetSnapRotation(bool state)
    {
        vrSettings.SetSnapRotation(state);
    }

    public void SetHandForward(bool state)
    {
        //vrSettings.SetHandsForward(state);
    }

    public void SetRightHanded(bool state)
    {
        //vrSettings.SetRightHanded(state);
    }

    public void SetSnapDegrees(float degrees)
    {
        vrSettings.SetSnapDegrees(degrees);
    }

    public void SetPlayerHeight()
    {
        vrSettings.SetPlayerHeight();
    }


    public void Quit()
    {
        
    #if UNITY_STANDALONE
            Application.Quit();
    #endif
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #endif
        
    }
    public void LoadTestingScene()
    {
        m_SavingSystem.Save("save");
        SceneManager.LoadScene("TestingEnvironment");
    }
    public void LoadCharlieTestingScene()
    {
        m_SavingSystem.Save("save");
        SceneManager.LoadScene("CharlieTestingScene");
    }

    public void LoadScenario1()
    {
        //m_SavingSystem.Delete("save");
        m_SavingSystem.Save("save");
        SceneManager.LoadScene("Scenario_1");
        m_SavingSystem.Load("save");

    }
    public void LoadScenario2()
    {
        m_SavingSystem.Save("save");
        SceneManager.LoadScene("Mission_1");
        m_SavingSystem.Load("save");

    }
    public void LoadScenario3()
    {
        m_SavingSystem.Save("save");
        SceneManager.LoadScene("LavaTube");
        m_SavingSystem.Load("save");

    }
    public void LoadScenario4()
    {
        m_SavingSystem.Save("save");
        SceneManager.LoadScene("Apollo_15");
        m_SavingSystem.Load("save");
    }

    public void LoadMainMenu()
    {
        m_SavingSystem.Save("save");
        SceneManager.LoadScene("MainMenu");
        m_SavingSystem.Load("save");
    }
}
