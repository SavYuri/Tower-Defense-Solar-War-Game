using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MisslePoints : MonoBehaviour
{
    public static Transform[] firePoint;
    public Transform GObjFirePoint;
    //public GameObject GObjFP;
    AudioSource audioSource;
    public AudioClip turretShot;
    public GameObject buletPrefab;
    private Transform [] target;
    public string enemyTag = "Enemy";
    [HideInInspector]
    public Enemy targetEnemy;

    public static MisslePoints misslePoints;

    public SpetialWeaponShop SWShopClass;
    SpetialWeaponBlueprint spetialWeaponBlueprint;
    

    private void Awake()
    {
        
        firePoint = new Transform[GObjFirePoint.childCount];
        for (int i = 0; i < firePoint.Length; i++)
        {
            firePoint[i] = GObjFirePoint.GetChild(i);
        }
        audioSource = GetComponent<AudioSource>();

       
    }

    void Start()
    {
        if (misslePoints != null) return; else misslePoints = this; 

     InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }
    int enemyLength;
    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        target = new Transform[enemies.Length];
        enemyLength = enemies.Length;


        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                target[i] = enemies[i].transform;
                targetEnemy = enemies[i].GetComponent<Enemy>();
            }
        }
    }

    int MissleCount()
    {
        int i = PlayerPrefs.GetInt("MissleLaunchSWLevel");

        if ( i == 0)
        {
            return 5;
        }
        if (i == 1)
        {
            return 9;
        }
        if (i == 2)
        {
            return 13;
        }
        if (i == 3)
        {
            return 17;
        }
        else return 5;
    }

    public void Shoot()
    {
        buletPrefab = SWShopClass.MissleLaunch();
        audioSource.clip = turretShot;
        audioSource.Play();

        int missleCount = MissleCount();

        GameObject[] bulletGo = new GameObject[firePoint.Length];
        for (int i = 0; i < missleCount; i++)
        {
            bulletGo[i] = (GameObject)Instantiate(buletPrefab, firePoint[i].position, firePoint[i].rotation);
            Bullet bullet = bulletGo[i].GetComponent<Bullet>();
            int randomEnemy = Random.Range(0, enemyLength);
            if (bullet != null)
                bullet.Seek(target[randomEnemy]);
        }

            
        
    }
}
