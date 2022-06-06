using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BayatGames.SaveGameFree;

public class SaveFunction : MonoBehaviour
{
   public void SavePlayer()
    {
        SaveSystem.SavePlayer();
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();
    }
}
