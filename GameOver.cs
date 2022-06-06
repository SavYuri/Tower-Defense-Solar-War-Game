using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    

    public string menuSceneName = "MainMenu";

    public SceneFader sceneFader;

    public InterstitialAdExample adExample;

   

    public void Retry()
    {
        //загружает активную сцену
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        adExample.ShowAd();
        sceneFader.FadeTo(SceneManager.GetActiveScene().name); //с затемнением
    }

    public void Menu()
    {
        sceneFader.FadeTo(menuSceneName);
    }
}
