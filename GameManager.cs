using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static bool GameIsOver;
    public GameObject gameOverUI;
    public GameObject completeLevelUI;
    public Text diamondText;
    public Text difficultLevelText;
    public Image[] stars;
    public Color standartStarColor;
    public Color wintStarColor;
    public SceneFader sceneFader;

    public bool gizmoActive = false;
    public static string[] gameLevels;
    public int nextLevel;
    public Animator mainCamera;

    public AudioClip gameOver;
    public AudioClip winLevel;
    public AudioSource audioSource;
    public AudioSource generalMusic;
    public static GameManager gameManager;

    public InterstitialAdExample adExample;

    private void Start()
    {

       // PlayerPrefs.SetInt("LastLevel", 7);

        if (gameManager != null) return;
        else gameManager = this;

        //Отключить уход экрана в спящий режим
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        InitializeGameLevels();
        GameIsOver = false;

        adExample = gameObject.GetComponent<InterstitialAdExample>();
    }

    void Update()
    {
        if (GameIsOver)
        {
            return;
        }

        //конец игры
        if (PlayerStats.Lives <= 0)
        {
            if (gameOverUI != null)
            {
                EndGame();
            }
           
        }
    }

    public void Continue()
    {
        
        sceneFader.FadeTo(gameLevels[nextLevel]);
        /*
        if(PlayerPrefs.GetInt("LastLevel") < nextLevel)
        {
            PlayerPrefs.SetInt("LastLevel", nextLevel);
        }
        */
        
        adExample.ShowAd();
    }

    void InitializeGameLevels()
    {
        gameLevels = new string[8];
        gameLevels[0] = "Level01";
        gameLevels[1] = "Level02";
        gameLevels[2] = "Level03";
        gameLevels[3] = "Level04";
        gameLevels[4] = "Level05";
        gameLevels[5] = "Level06";
        gameLevels[6] = "Level07";
        gameLevels[7] = "Level08";
    }
    void EndGame()
    {
        Time.timeScale = 1.0f;
        GameIsOver = true;
        gameOverUI.SetActive(true);
        generalMusic.volume = 0.2f;
        audioSource.clip = gameOver;
        audioSource.volume = 0.9f;
        audioSource.Play();

    }

    

    void winLevelPanel()
    {
        completeLevelUI.SetActive(true);
    }

    void LounchFireWork()
    {
        WinFireWork.winFireWork.StartWinFireWork();
    }

    void StartAnimWinCamera()
    {
        mainCamera.enabled = true;
        mainCamera.SetTrigger("Win");
    }


    int newStar;
    void RewardConditions()
    {
        int lives = PlayerStats.Lives;
        int difficultLevel = PlayerPrefs.GetInt("LevelOfDifficulty");
        int correctLevel = nextLevel - 1;
        int oldStar;
        
        int oldDiamonds = PlayerPrefs.GetInt("Diamonds");
        int rewardDiamonds;
        string pPrefsName = "DifficultStar";

        if (difficultLevel == 0)
        {
            difficultLevelText.text = "DIFFICULTY: NORMAL";

            if (lives == 20)
            {
                newStar = 4;
            }
            else if (lives > 15)
            {
                newStar = 3;
            }
            else if (lives > 10)
            {
                newStar = 2;
            }
            else
            {
                newStar = 1;
            }
        }

        else if (difficultLevel == 1)
        {
            difficultLevelText.text = "DIFFICULTY: HARD";

            if (lives == 15)
            {
                newStar = 4;
            }
            else if ( lives > 10)
            {
                newStar = 3;
            }
            else if (lives > 5)
            {
                newStar = 2;
            }
            else
            {
                newStar = 1;
            }
        }

        else if (difficultLevel == 2)
        {
            difficultLevelText.text = "DIFFICULTY: EXTREME";

            if (lives == 10)
            {
                newStar = 4;
            }
            else if (lives > 7)
            {
                newStar = 3;
            }
            else if (lives > 4)
            {
                newStar = 2;
            }
            else
            {
                newStar = 1;
            }
        }

        pPrefsName += correctLevel.ToString() + difficultLevel.ToString();
        oldStar = PlayerPrefs.GetInt(pPrefsName);
        for (int i = 0; i<4; i++)
        {
            stars[i].color = standartStarColor;
        }
        for (int i = 0; i < newStar; i++)
        {
            stars[i].color = wintStarColor;
        }
        


            if (oldStar < newStar)
        {
            rewardDiamonds = (newStar - oldStar) * 25;
            PlayerPrefs.SetInt(pPrefsName, newStar);
            oldDiamonds += rewardDiamonds;
            PlayerPrefs.SetInt("Diamonds", oldDiamonds);
            diamondText.text = "+ " + rewardDiamonds;
            
            return;
        }
        diamondText.text = "+ " + 0;
    }

    public void WinLevel()
    {
        RewardConditions();

        if (NodeUI.nodeUI.ui.activeSelf == false)
        {
            
        }
        else
        {
            NodeUI.nodeUI.Cancel();
        }
        
        WaveSpawner.waveSpawner.cameraController.SetActive(false);
        AnimMenu.animMenu.HideAllNavigation();
        //камеру на исходную
        WinMoveCamera.winMoveCamera.SwitchCameraMovement();

       Invoke("LounchFireWork", 2);
        Time.timeScale = 1.0f;
        
        Invoke("StartAnimWinCamera", 3);
        generalMusic.Stop();
        audioSource.clip = winLevel;
        audioSource.Play();
        
        GameIsOver = true;
       Invoke( "winLevelPanel",17);
        PlayerPrefs.SetInt("FirstStart", 1);

       
        
        if (PlayerPrefs.GetInt("LastLevel") < nextLevel)
        {
            PlayerPrefs.SetInt("LastLevel", nextLevel);
        }
       
        

    }

    public void ActivateGizmo()
    {
        GameObject[] Gizmo;
        Gizmo = GameObject.FindGameObjectsWithTag("GizmoTurret");
        foreach (GameObject tagged in Gizmo)
        {
            tagged.SetActive(false); // or true
        }
        Debug.Log("GizmoOn");
       
    }

    public void DeletePrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
