using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadHelp : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.H))
        {
            SceneManager.LoadScene("Help");
        }
    }
}
