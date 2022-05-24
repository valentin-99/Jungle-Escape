using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionInteraction : MonoBehaviour
{
    [SerializeField] GameObject help;

    void Update()
    {
        if (Input.GetKey(KeyCode.H))
        {
            help.SetActive(true);
        }

        else if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
        }
    }

    public void Confirm()
    {
        if (help.activeInHierarchy)
        {
            help.SetActive(false);
        }
    }
}
