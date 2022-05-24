using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInteraction : MonoBehaviour
{
    [SerializeField] GameObject menu;
    [SerializeField] GameObject help;

    private void Update()
    {
        if (Input.GetKey(KeyCode.H))
        {
            help.SetActive(true);
            menu.SetActive(false);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("LevelSelection");
    }

    public void EnableHelp()
    {
        help.SetActive(true);
        menu.SetActive(false);
    }

    public void Confirm()
    {
        if (help.activeInHierarchy)
        {
            menu.SetActive(true);
            help.SetActive(false);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
