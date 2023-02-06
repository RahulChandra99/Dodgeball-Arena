using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void Playgame()
    {
        SceneManager.LoadSceneAsync("02_Desert");
    }

    public void Exitgame()
    {
        Application.Quit();
    }
}
