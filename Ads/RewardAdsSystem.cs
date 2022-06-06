using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardAdsSystem : MonoBehaviour
{
    float timer;
    float startTime = 10;
    [HideInInspector]
    public int countOfReward;
    [Space]
    [Header("Rewards")]
    //7 times
    public int [] rewardMoney;
    public int [] rewardEnergy;
    public int [] rewardDiamond;
    [Space]
    int difficultyPlusMoneyReward;
    int difficultyPlusEnergyReward;


    private void Start()
    {
        SetDifficultLevel();
    }

    void Update()
    {
        timer -= Time.deltaTime;
    }

   public void ToReward ()
    {
        if ( timer <= 0 && countOfReward <= 7)
        {
            
                PlayerStats.Money += rewardMoney[countOfReward] + difficultyPlusMoneyReward;
                PlayerStats.Energy += rewardEnergy[countOfReward] + difficultyPlusEnergyReward;
                int x;
                x = PlayerPrefs.GetInt("Diamonds");
                x += rewardDiamond[countOfReward];
                PlayerPrefs.SetInt("Diamonds", x);
                timer = startTime;
                countOfReward++;



        }
       
    }

    void SetDifficultLevel()
    {
        int difficultyLevel = PlayerPrefs.GetInt("LevelOfDifficulty");

        if (difficultyLevel == 0)
        {
            difficultyPlusMoneyReward = 0;
            difficultyPlusEnergyReward = 0;
        }
        else if (difficultyLevel == 1)
        {
            difficultyPlusMoneyReward = 200;
            difficultyPlusEnergyReward = 50;
        }
        else if (difficultyLevel == 2)
        {
            difficultyPlusMoneyReward = 400;
            difficultyPlusEnergyReward = 100;
        }
    }
}
