using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BayatGames.SaveGameFree;

[System.Serializable]
public class PlayerData
{
    public int money;
    public int lives;
    public int waves;

    public PlayerData ()
    {
        
        money = PlayerStats.Money;
        lives = PlayerStats.Lives;
        SaveGame.Save<int>("score", PlayerStats.Lives);
        
    }
    
}
