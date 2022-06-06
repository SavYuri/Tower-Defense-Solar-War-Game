using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBig : MonoBehaviour
{
    public GameObject smallSpider;
    public float timeBetwinSpawn;

    void Start()
    {
        InvokeRepeating("SpawnSmallSpider", 5, timeBetwinSpawn);
    }

    void SpawnSmallSpider()
    {

        GameObject sSpider = Instantiate(smallSpider, transform.position, transform.rotation);
        sSpider.GetComponent<EnemyMovement>().wavepointIndex = gameObject.GetComponent<EnemyMovement>().wavepointIndex;


    }
}
