using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.IO;

public class MenuInteraction : MonoBehaviour
{
    [SerializeField] GameObject menu;
    [SerializeField] GameObject help;
    [SerializeField] GameObject highscore;
    [SerializeField] GameObject settings;

    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    [HideInInspector] public float valueMusic;
    [HideInInspector] public float valueSFX;

    Resolution[] resolutions;
    [SerializeField] Dropdown resolutionDropdown;

    private void Start()
    {
        // Load volume datas
        if (SaveLoadSystem.LoadVolumeData() == null)
        {
            musicSlider.value = musicSlider.maxValue;
            sfxSlider.value = sfxSlider.maxValue;
        }
        else
        {
            VolumeData data = SaveLoadSystem.LoadVolumeData();

            // Set volumes
            audioMixer.SetFloat("MusicVolume", data.valueMusic);
            audioMixer.SetFloat("SFXVolume", data.valueSFX);

            // Set sliders values;
            musicSlider.value = Mathf.Pow(10, data.valueMusic / 20);
            sfxSlider.value = Mathf.Pow(10, data.valueSFX / 20);
        }

        // Get available resolutions
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        int currentResolution = 0;
        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            options.Add(resolutions[i].width + " x " + resolutions[i].height);

            // Get current resolution
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolution = i;
            }
        }
        
        // Load the options
        resolutionDropdown.AddOptions(options);
        // Load current resolution
        resolutionDropdown.value = currentResolution;
        resolutionDropdown.RefreshShownValue();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.H))
        {
            help.SetActive(true);
            menu.SetActive(false);
        }
    }

    public void NewGame()
    {
        string pathSolo = Application.persistentDataPath + "/solo.out";
        string pathRunner = Application.persistentDataPath + "/runner.out";
        
        if (File.Exists(pathSolo)) {
            File.Delete(pathSolo);
        }
        if (File.Exists(pathRunner))
        {
            File.Delete(pathRunner);
        }

        SceneManager.LoadScene("LevelSelection");
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("LevelSelection");
    }

    public void EnableHelp()
    {
        help.SetActive(true);
        menu.SetActive(false);
    }

    public void EnableHighScore()
    {
        highscore.SetActive(true);
        menu.SetActive(false);
        Transform score1Transform = highscore.transform.Find("Score1");
        Transform score2Transform = highscore.transform.Find("Score2");
        Transform score3Transform = highscore.transform.Find("Score3");
        Transform score4Transform = highscore.transform.Find("Score4");
        Transform score5Transform = highscore.transform.Find("Score5");
        Transform score6Transform = highscore.transform.Find("Score6");
        Transform scoreDayTransform = highscore.transform.Find("ScoreDay");
        Transform scoreSunsetTransform = highscore.transform.Find("ScoreSunset");
        Transform scoreNightTransform = highscore.transform.Find("ScoreNight");

        Text score1Text = score1Transform.GetComponent<Text>();
        Text score2Text = score2Transform.GetComponent<Text>();
        Text score3Text = score3Transform.GetComponent<Text>();
        Text score4Text = score4Transform.GetComponent<Text>();
        Text score5Text = score5Transform.GetComponent<Text>();
        Text score6Text = score6Transform.GetComponent<Text>();
        Text scoreDayText = scoreDayTransform.GetComponent<Text>();
        Text scoreSunsetText = scoreSunsetTransform.GetComponent<Text>();
        Text scoreNightText = scoreNightTransform.GetComponent<Text>();

        if (SaveLoadSystem.LoadSoloData() == null)
        {
            score1Text.text = "0";
            score2Text.text = "0";
            score3Text.text = "0";
            score4Text.text = "0";
            score5Text.text = "0";
            score6Text.text = "0";
        }
        else
        {
            SoloData data = SaveLoadSystem.LoadSoloData();
            score1Text.text = data.record1.ToString();
            score2Text.text = data.record2.ToString();
            score3Text.text = data.record3.ToString();
            score4Text.text = data.record4.ToString();
            score5Text.text = data.record5.ToString();
            score6Text.text = data.record6.ToString();
        }

        if (SaveLoadSystem.LoadRunnerData() == null)
        {
            scoreDayText.text = "0";
            scoreSunsetText.text = "0";
            scoreNightText.text = "0";
        }
        else
        {
            RunnerData data = SaveLoadSystem.LoadRunnerData();
            scoreDayText.text = data.record1.ToString();
            scoreSunsetText.text = data.record2.ToString();
            scoreNightText.text = data.record3.ToString();
        }
    }

    public void EnableSettings()
    {
        settings.SetActive(true);
        menu.SetActive(false);
    }

    public void SetVolumeMusic (float volume)
    {
        if (SaveLoadSystem.LoadVolumeData() == null)
        {
            SaveLoadSystem.SaveVolumeData(this);
        }

        valueMusic = Mathf.Log10(volume) * 20;
        audioMixer.SetFloat("MusicVolume", valueMusic);

        SaveLoadSystem.SaveVolumeData(this);
    }

    public void SetVolumeSFX(float volume)
    {
        if (SaveLoadSystem.LoadVolumeData() == null)
        {
            SaveLoadSystem.SaveVolumeData(this);
        }

        valueSFX = Mathf.Log10(volume) * 20;
        audioMixer.SetFloat("SFXVolume", valueSFX);

        SaveLoadSystem.SaveVolumeData(this);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution(int index)
    {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void Confirm()
    {
        if (help.activeInHierarchy)
        {
            menu.SetActive(true);
            help.SetActive(false);
        }
        else if (highscore.activeInHierarchy)
        {
            menu.SetActive(true);
            highscore.SetActive(false);
        }
        else if (settings.activeInHierarchy)
        {
            menu.SetActive(true);
            settings.SetActive(false);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
