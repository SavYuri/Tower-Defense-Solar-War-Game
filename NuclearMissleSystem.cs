using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//количество ракет в обойме ограничено переменной countOfMissiles в скрипте Turret 

public class NuclearMissleSystem : MonoBehaviour
{
    public Transform[] missleStartPoint;
    
    public GameObject[] missle;
    NuclearBullet[] bullet;
    public Turret Turret;
    public Transform worldPivot;

    public Transform target;

    bool missileCreatorInProcess;

    private void Awake()
    {
        GameObject world = GameObject.Find("WorldPivot");
        worldPivot = world.transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        missle = new GameObject[Turret.countOfMissiles];
        
        bullet = new NuclearBullet[Turret.countOfMissiles];
        FirstMissleCreater();
        InvokeRepeating("CheckFullMissils", 1, 0.5f);
       
    }

    // Update is called once per frame
    void Update()
    {
        //EnemyDetector();
        EnemyDetectorToTheEnd();


    }

    

    void CheckFullMissils()
    {
        if (missileCreatorInProcess == false)
        {
            MissilesCreator();
        }
    }

    void MissilesCreator()
    {
        StartCoroutine("MissileCreator");
    }

   

    IEnumerator MissileCreator()
    {
        missileCreatorInProcess = true;
        for (int i = 0; i < Turret.countOfMissiles; i++)
        {
           
            if (missle[i] == null)
            {
                
                missle[i] = Instantiate(Turret.buletPrefab, missleStartPoint[i].position, missleStartPoint[i].rotation);
                bullet[i] = missle[i].GetComponent<NuclearBullet>();
                bullet[i].enabled = false;
                //делаем объект дочерним
                missle[i].transform.parent = Turret.partToRotate;
                yield return new WaitForSeconds(5);
            }
            
        }

        missileCreatorInProcess = false;
        yield break;
    }


    IEnumerator MissileLauncher()
    {
        for (int i = 0; i < Turret.countOfMissiles; i++)
        {
            if (missle[i] != null && target != null)
            {
                
                missle[i].transform.parent = worldPivot;
                bullet[i].enabled = true;
                bullet[i].target = target;
                missle[i] = null;
                bullet[i] = null;
                yield return new WaitForSeconds(0.5f);

            }
           
           
        }
        yield break;
    }


        void FirstMissleCreater()
    {
        for (int i = 0; i < Turret.countOfMissiles; i++)
        {
            missle[i] = Instantiate(Turret.buletPrefab, missleStartPoint[i].position, missleStartPoint[i].rotation);
            bullet[i] = missle[i].GetComponent<NuclearBullet>();
            bullet[i].enabled = false;
            missle[i].transform.parent = Turret.partToRotate;
            
        }
        
    }


    //Ближайшая цель к пушке
    void EnemyDetector()
    {
        
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(Turret.enemyTag);
            if (enemies == null)
            {
            return;
            }

            float shortestDistance = Mathf.Infinity;
            GameObject nearestEnemy = null;


            foreach (GameObject enemy in enemies)
            {
                float distanceToEnemy = Vector3.Distance(Turret.transform.position, enemy.transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                }
            }

            if (nearestEnemy != null && shortestDistance <= Turret.range)
            {
                target = nearestEnemy.transform;
                Turret.targetEnemy = nearestEnemy.GetComponent<Enemy>();
                Seek(target);
                
            }
            

    }

    //Ближайшая цель к финишу
    void EnemyDetectorToTheEnd()
    {

        GameObject[] enemies = GameObject.FindGameObjectsWithTag(Turret.enemyTag);
        if (enemies == null)
        {
            return;
        }

        //float shortestDistance = Mathf.Infinity;
        GameObject nearestToEnd = null;
        int biggestIndextoEnd = 0;
        float nearestToPoint = Mathf.Infinity;
        //GameObject nearestEnemy = null;


        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(Turret.transform.position, enemy.transform.position);
            if (distanceToEnemy < Turret.range)
            {
                if (enemy.GetComponent<EnemyMovement>() != null)
                {
                    if (enemy.GetComponent<EnemyMovement>().wavepointIndex >= biggestIndextoEnd)
                    {
                        biggestIndextoEnd = enemy.GetComponent<EnemyMovement>().wavepointIndex;
                        float distanceToPoint = Vector3.Distance(enemy.transform.position, Waypoints.points[enemy.GetComponent<EnemyMovement>().wavepointIndex].transform.position);
                        if (nearestToPoint >= distanceToPoint)
                        {
                            nearestToPoint = distanceToPoint;
                            nearestToEnd = enemy;
                        }


                    }
                }
            }
        }

        if (nearestToEnd != null)
        {
            target = nearestToEnd.transform;
            Turret.targetEnemy = nearestToEnd.GetComponent<Enemy>();
            Seek(target);

        }
        else
        {
            target = null;
        }


    }

    public void Seek(Transform _target)
    {
        target = _target;
      
    }
}
