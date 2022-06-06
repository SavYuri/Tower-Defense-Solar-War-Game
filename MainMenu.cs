using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    

    public SceneFader sceneFader;

    public Text playButtonText;

    public GameObject shopPanel;
    public GameObject mainMenuPanel;
    public GameObject creditsPanel;
    public ShopMenu ShopMenuClass;
    public Animator buttonsAnim;

    private void Start()
    {
        FirstStartGame();
        Invoke("DestroyButtonsAnim", 5);
        
    }

    void DestroyButtonsAnim()
    {
        if (buttonsAnim != null)
        {
            buttonsAnim.enabled = false;
        }
       
    }

    public void Play ()
    {
        sceneFader.FadeTo(GameManager.gameLevels[PlayerPrefs.GetInt("LastLevel")]);
    }

    public void LevelSelect()
    {
        sceneFader.FadeTo("LevelSelect");
    }

    void FirstStartGame()
    {
        if (PlayerPrefs.GetInt("FirstDiamonds") == 0)
        {
            PlayerPrefs.SetInt("Diamonds", 150);
            PlayerPrefs.SetInt("FirstDiamonds", 1);
        }

       if ( PlayerPrefs.GetInt("FirstStart") == 0)
        {
            if (playButtonText != null)
            {
                playButtonText.text = "P L A Y";
               

            }
            PlayerPrefs.SetInt("LastLevel", 0);
        }
        else
        {
            if (playButtonText != null)
            {
                playButtonText.text = "C O N T I N U E";
            }
           
            
        }
    }

    public void GotoMainMenu()
    {
        sceneFader.FadeTo("MainMenu");
    }

    public void GoToShopMenu()
    {
        shopPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        ShopMenuClass.ChooseSW(0);
        
    }

    public void GoToCreditsMenu()
    {
        shopPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(true);

    }

    public void GoBackFromShopMenu()
    {
        shopPanel.SetActive(false);
        creditsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}
