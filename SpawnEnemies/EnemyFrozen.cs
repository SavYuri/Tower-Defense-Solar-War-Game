using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFrozen : MonoBehaviour
{
    public Transform target;
    public float range;
    public EnemyMovement enemyMovement;
    public bool enableFrozing;
    public bool waitingForFrozing;
    public float startTimeToFroze;
    public float startTimeBetwinFrozing;
    float timeToFroze;
    float timeBetwinFrozing;
    public bool makeDamage;
    public float frozenDamage;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget",1,1);
        timeToFroze = startTimeToFroze;
        timeBetwinFrozing = startTimeBetwinFrozing;
    }

    // Update is called once per frame
    void Update()
    {
        WaitingForFrozingTimer();
        FrozingTimer();
    }

    void UpdateTarget()
    {
        if (waitingForFrozing) return;
        if (target != null) return;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Turret");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        if (enemies != null)
        {
            foreach (GameObject enemy in enemies)
            {
                if (enemy.GetComponent<EnergyGenerator>() != null) continue;
                if (enemy.GetComponent<TurretSlow>() != null) continue;
                if (enemy.GetComponent<Turret>().turretIsFrozen) continue;
                if (enemy.gameObject.GetComponent<Turret>().frozenEnemyLockOnTurret) continue;
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < shortestDistance)
                {

                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy;

                }


            }
        }
        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
            

        }
        else
        {
            target = null;
        }
    }


    public void ToFroze()
    {
        enableFrozing = true;
    }

    void FrozingTimer()
    {

        if (enableFrozing)
        {
            if (target == null)
            {
                waitingForFrozing = true;
                enableFrozing = false;
                timeToFroze = startTimeToFroze;
                return;
            }

            if (timeToFroze <= 0)
            {
                
                waitingForFrozing = true;
                enableFrozing = false;
                timeToFroze = startTimeToFroze;
                target.gameObject.GetComponent<Turret>().turretIsFrozen = false;
                target.gameObject.GetComponent<Turret>().frozenEnemyLockOnTurret = false;
                enemyMovement.target = null;
                target = null;
                return;
            }
            timeToFroze -= Time.deltaTime;
            target.gameObject.GetComponent<Turret>().turretIsFrozen = true;
            if (makeDamage)
            {
                target.gameObject.GetComponent<Enemy>().health -= Time.deltaTime * frozenDamage;
            }
        }
       
    }

    void WaitingForFrozingTimer()
    {
        if (waitingForFrozing)
        {
            if (timeBetwinFrozing <= 0)
            {
                waitingForFrozing = false;
                timeBetwinFrozing = startTimeBetwinFrozing;
            }
            timeBetwinFrozing -= Time.deltaTime;
        }
    }

    private void OnDestroy()
    {
        if (target != null)
        {
            target.gameObject.GetComponent<Turret>().turretIsFrozen = false;
            target.gameObject.GetComponent<Turret>().frozenEnemyLockOnTurret = false;
        }
    }
}
