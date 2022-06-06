using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static int Money;
    public int starMoneyNormal;
    public int starMoneyHard;
    public int starMoneyExtreme;

    public static int Lives;
    
    public int startEnergyNormal;
    public int startEnergyHard;
    public int startEnergyExtreme;
    public static int Rounds;

    public static int Energy;
   public static PlayerStats playerStatsClass;
    private void Awake()
    {
        Rounds = 0;
    }
    private void Start()
    {
        if (playerStatsClass != null)
        {
            return;
        }
        else
        {
            playerStatsClass = this;
        }

        SetDifficultLevel();

    }

    void SetDifficultLevel()
    {
        int difficultyLevel = PlayerPrefs.GetInt("LevelOfDifficulty");

        if (difficultyLevel == 0)
        {
            Lives = 20;
            Money = starMoneyNormal;
            Energy = startEnergyNormal;
        }
        else if (difficultyLevel == 1)
        {
            Lives = 15;
            Money = starMoneyHard;
            Energy = startEnergyHard;
        }
        else if (difficultyLevel == 2)
        {
            Lives = 10;
            Money = starMoneyExtreme;
            Energy = startEnergyExtreme;
        }
    }
}
