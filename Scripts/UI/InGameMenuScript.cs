using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class InGameMenuScript : MonoBehaviour
{
    [SerializeField] GameObject QuitButton;
    [SerializeField] GameObject OptionsButton;
    [SerializeField] GameObject HelpButton;
    [SerializeField] GameObject BackToMainMenu;
    [SerializeField] GameObject LoadScene1Button;
    [SerializeField] GameObject LoadScene2Button;
    [SerializeField] GameObject LoadScene3Button;
    [SerializeField] GameObject LoadScene4Button;
    [SerializeField] GameObject LoadTestingSceneButton;
    [SerializeField] GameObject VolumeSlider;
    [SerializeField] GameObject MusicVolumeSlider;
    [SerializeField] GameObject SoundFXVolumeSlider;
    [SerializeField] Dropdown QualityDropdown;
    [SerializeField] GameObject ControlLayout;


    [SerializeField] Text VolumeText;
    [SerializeField] Text MusicVolumeText;
    [SerializeField] Text SoundFXVolumeText;
    [SerializeField] GameObject BackButton;
    [SerializeField] Text Text;
    
    [SerializeField] GameObject MenuMusic;

    Fade fader;
    AudioSource MenuMusicAudio;
    int Volume = 69;
    int MusicVolume = 20;
    int SoundFXVolume = 20;
    //float FadeDur = 2.69f;
    // Start is called before the first frame update
    void Start()
    {
        OpenMainMenu();
        if (MenuMusic == null)
        {

            MenuMusic = GameObject.FindGameObjectWithTag("Sound");
        }
        fader = Camera.main.GetComponent<Fade>();
        MenuMusicAudio = MenuMusic.GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {/*
        if (SceneManager.GetActiveScene().buildIndex != 0)
            gameObject.SetActive(false);*/


    }

  
    public void OpenSettings()
    {

        VolumeText.text = "Volume: " + Volume.ToString();
        MusicVolumeText.text = "Music Volume: " + MusicVolume.ToString();
        SoundFXVolumeText.text = "SoundFX Volume: " + SoundFXVolume.ToString();
        Text.text = "Settings";
        QuitButton.SetActive(false);
        OptionsButton.SetActive(false);
        BackToMainMenu.SetActive(false);
        HelpButton.SetActive(false);
        ControlLayout.SetActive(false);
        BackButton.SetActive(true);
        LoadScene1Button.SetActive(true);
        LoadScene2Button.SetActive(true);
        LoadScene3Button.SetActive(true);
        LoadScene4Button.SetActive(true);
        LoadTestingSceneButton.SetActive(true);
        VolumeSlider.SetActive(true);
        MusicVolumeSlider.SetActive(true);
        SoundFXVolumeSlider.SetActive(true);
        QualityDropdown.gameObject.SetActive(true);
    }
    public void OpenHelp()
    {
        Text.text = "Help";
        QuitButton.SetActive(false);
        BackToMainMenu.SetActive(false);
        OptionsButton.SetActive(false);
        HelpButton.SetActive(false);
        ControlLayout.SetActive(true);
        BackButton.SetActive(true);
        LoadScene1Button.SetActive(false);
        LoadScene2Button.SetActive(false);
        LoadScene3Button.SetActive(false);
        LoadScene4Button.SetActive(false);

        LoadTestingSceneButton.SetActive(false);
        VolumeSlider.SetActive(false);
        MusicVolumeSlider.SetActive(false);
        SoundFXVolumeSlider.SetActive(false);
        QualityDropdown.gameObject.SetActive(false);
    }

    public void OpenMainMenu()
    {
        AudioListener.volume = 69.0f;
        Text.text = "Main Menu";

        QuitButton.SetActive(true);
        BackToMainMenu.SetActive(true);
        OptionsButton.SetActive(true);
        HelpButton.SetActive(true);
        BackButton.SetActive(false);
        ControlLayout.SetActive(false);
        LoadScene1Button.SetActive(false);
        LoadScene2Button.SetActive(false);
        LoadScene3Button.SetActive(false);
        LoadScene4Button.SetActive(false);
        LoadTestingSceneButton.SetActive(false);
        VolumeSlider.SetActive(false);
        MusicVolumeSlider.SetActive(false);
        SoundFXVolumeSlider.SetActive(false);
        QualityDropdown.gameObject.SetActive(false);

    }

    public void SetMasterVolume(float volume)
    {
        AudioListener.volume = volume;
        Volume = (int)(volume * 100);
        int volumenum = (int)Volume;
        VolumeText.text = "Master Volume: " + volumenum.ToString();
        SoundPlayer sound = GetComponent<SoundPlayer>();
        sound.SetVolume(volume);
    }
    public void SetMusicVolume(float volume)
    {
        MenuMusicAudio.volume = volume;
        float temp = volume * 100;
        MusicVolume = (int)temp;
        MusicVolumeText.text = "Music Volume: " + MusicVolume.ToString();
    }
    public void SetSoundFXVolume(float volume)

    {
        float temp = volume * 100;
        SoundFXVolume = (int)temp;
        SoundFXVolumeText.text = "SoundFX Volume: " + SoundFXVolume.ToString();
    }
    public void SetQuality(int value)
    {
        QualitySettings.SetQualityLevel(value);
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
        SceneManager.LoadScene("TestingEnvironment");
    }
    public void LoadScenario1()
    {
        SceneManager.LoadScene("Scenario_1");

    }
    public void LoadScenario2()
    {
        SceneManager.LoadScene("Scenario_2");

    }
    public void LoadScenario3()
    {
        SceneManager.LoadScene("Scenario_3");

    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");

    }
    public void LoadScenario4()
    {
        SceneManager.LoadScene("Scenario_4");

    }
}
