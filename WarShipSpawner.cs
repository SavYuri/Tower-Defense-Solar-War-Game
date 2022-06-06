using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarShipSpawner : MonoBehaviour
{
    GameObject prefab;
    public GameObject warrShipPrefab1;
    public GameObject warrShipPrefab2;
    public GameObject warrShipPrefab3;
    public GameObject warrShipPrefab4;
    public Transform spawnPoint;
    
    


    public static WarShipSpawner warShipSpawner;

   
    void Start()
    {
        if (warShipSpawner != null) return; else warShipSpawner = this;
    }


    void CheckLevelOfUpgrade()
    {
        int levelUpgrade = PlayerPrefs.GetInt("WarShipSWLevel");

        if (levelUpgrade == 0)
        {
            
            prefab = warrShipPrefab1;
        }
        if (levelUpgrade == 1)
        {
           
            prefab = warrShipPrefab2;
        }
        if (levelUpgrade == 2)
        {
            
            prefab = warrShipPrefab3;
        }
        if (levelUpgrade == 3)
        {
           prefab = warrShipPrefab4;
        }

    }

    public void SpawnWarShip()
    {
        StartCoroutine(WarShipCreate());
    }

    IEnumerator WarShipCreate()
    {

        CheckLevelOfUpgrade();
        
        SpetialWeaponShop.spetialWeaponShop.spetialWeaponsButtons[4].interactable = false;
        Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        yield return new WaitForSeconds(3);
        SpetialWeaponShop.spetialWeaponShop.spetialWeaponsButtons[4].interactable = true;
        
        
        yield break;
    }
}
