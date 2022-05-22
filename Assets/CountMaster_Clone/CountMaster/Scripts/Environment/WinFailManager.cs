using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinFailManager : MonoBehaviour
{
    public void Win()
    {
        if ((PlayerPrefs.GetInt("level") + 1) < SceneManager.sceneCountInBuildSettings - 1)
        {
            PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level") + 1);
        }
        else
            PlayerPrefs.SetInt("level", 0);
        SceneManager.LoadScene(PlayerPrefs.GetInt("level") + 1);
    }

    public void Fail()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("level") + 1);
    }
}
