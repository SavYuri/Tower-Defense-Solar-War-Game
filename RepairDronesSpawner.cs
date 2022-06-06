using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairDronesSpawner : MonoBehaviour
{
   GameObject prefab;
    public GameObject repairDronPrefab1;
    public GameObject repairDronPrefab2;
    public GameObject repairDronPrefab3;
    public GameObject repairDronPrefab4;
    public Transform spawnPoint;
    public SpetialWeaponShop spetialWeaponShop;
    
    


    public static RepairDronesSpawner repairDronesSpawner;
    int numberOfDrones;
    public int intervalCreateBomb;

    void Start()
    {
        if (repairDronesSpawner != null) return; else repairDronesSpawner = this;
    }


    void CheckLevelOfUpgrade()
    {
        int levelUpgrade = PlayerPrefs.GetInt("RepairDronesSWLevel");

        if (levelUpgrade == 0)
        {
            numberOfDrones = 2;
            prefab = repairDronPrefab1;
        }
        if (levelUpgrade == 1)
        {
            numberOfDrones = 2;
            prefab = repairDronPrefab2;
        }
        if (levelUpgrade == 2)
        {
            numberOfDrones = 2;
            prefab = repairDronPrefab3;
        }
        if (levelUpgrade == 3)
        {
            numberOfDrones = 2;
            prefab = repairDronPrefab4;
        }

    }

    public void SpawnRepairDrones()
    {
        StartCoroutine(RepairDronesCreate());
    }

    IEnumerator RepairDronesCreate()
    {
        spetialWeaponShop.spetialWeaponsButtons[5].interactable = false;
        CheckLevelOfUpgrade();
        for (int i = 0; i < numberOfDrones; i++)
        {
            yield return new WaitForSeconds(intervalCreateBomb);
            Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        }
        spetialWeaponShop.spetialWeaponsButtons[5].interactable = true;
        yield break;
    }
}

