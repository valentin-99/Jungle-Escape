using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MenuInteraction : MonoBehaviour
{
    [SerializeField] GameObject menu;
    [SerializeField] GameObject help;
    [SerializeField] GameObject highscore;

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
    }

    public void Quit()
    {
        Application.Quit();
    }
}
