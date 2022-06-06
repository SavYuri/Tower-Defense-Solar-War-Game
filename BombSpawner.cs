using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSpawner : MonoBehaviour
{
    GameObject prefab;
    public GameObject bombPrefab1;
    public GameObject bombPrefab2;
    public GameObject bombPrefab3;
    public GameObject bombPrefab4;
    public Transform spawnPoint;
    int numberOfBomb;
    public int intervalCreateBomb;
    

   public static BombSpawner bombSpawner;

    // Start is called before the first frame update
    void Start()
    {
        if (bombSpawner != null) return; else bombSpawner = this;
    }

    
    void CheckLevelOfUpgrade()
    {
        int levelUpgrade = PlayerPrefs.GetInt("AttackDronsSWLevel");

        if (levelUpgrade == 0)
        {
            numberOfBomb = 4;
            prefab = bombPrefab1;
        }
        if (levelUpgrade == 1)
        {
            numberOfBomb = 6;
            prefab = bombPrefab2;
        }
        if (levelUpgrade == 2)
        {
            numberOfBomb = 8;
            prefab = bombPrefab3;
        }
        if (levelUpgrade == 3)
        {
            numberOfBomb = 10;
            prefab = bombPrefab4;
        }

    }

    public void SpawnBomb()
    {
        StartCoroutine(BombCreate());
    }

    IEnumerator BombCreate()
    {

        CheckLevelOfUpgrade();
        for (int i = 0; i < numberOfBomb; i++)
        {
            yield return new WaitForSeconds(intervalCreateBomb);
            Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        }
        yield break;
    }
}
