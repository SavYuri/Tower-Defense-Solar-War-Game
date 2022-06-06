using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour
{
    public Transform target;
    public int wavepointIndex = 0;

    public Enemy enemy;

    [HideInInspector]
    public bool enebleBackTime = false;
    public Turret enemyTurret;

    public EnemyFrozen enemyFrozen;
    public bool wayRight;
    public bool wayLeft;
    Transform[] wayPoints;

   

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }
    void Start()
    {
        

        if (wayRight)
        {
            wayPoints = WaypointsRight.points;
        }
        else if (wayLeft)
        {
            wayPoints = WaypointsLeft.points;
        }
        else
        {
            wayPoints = Waypoints.points;
        }

        target = wayPoints[wavepointIndex];
    }

    void checkChild()
    {
        if(transform.childCount == 0)
        {
            Debug.Log("DestroyParent");
            Destroy(gameObject);
            return;
        }
    }

    void Update()
    {
        checkChild();

        Vector3 dir;

        //условие для врага с турелью
        if (enemyTurret != null)
        {
            if (enemyTurret.targetEnemy != null)
            {
                dir = enemyTurret.targetEnemy.transform.position - transform.position;
                Quaternion lookRotation = Quaternion.LookRotation(dir);
                Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 5).eulerAngles;
                transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
            }
            
            else
            {
                dir = target.position - transform.position;
                Quaternion lookRotation = Quaternion.LookRotation(dir);
                Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 5).eulerAngles;
                transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);

            }
        }

        if (enemyFrozen != null && enemyFrozen.target != null)
        {
            enemyFrozen.target.gameObject.GetComponent<Turret>().frozenEnemyLockOnTurret = true;
            target = enemyFrozen.target;
            dir = target.position - transform.position;
            if (Vector3.Distance(transform.position, target.position) <= 1f)
            {
                if (enemyFrozen.enableFrozing) return;
                else
                {
                    enemyFrozen.enableFrozing = true;
                }
                return;
            }
            transform.Translate(dir.normalized * enemy.speed * Time.deltaTime, Space.World);
            return;
        }

        if (target == null)
        {
            target = wayPoints[wavepointIndex];
        }

        dir = target.position - transform.position;
        transform.Translate(dir.normalized * enemy.speed * Time.deltaTime, Space.World);

        

        if (enemyTurret == null)
        {
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 5).eulerAngles;
            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        }
           
        
        if (Vector3.Distance(transform.position, target.position) <= 5f)
        {
            if (!enebleBackTime)
            {
                GetNextWaypoint();
            }
            else
            {
                GetBackWaypoint();
            }
        }
        

    }

    public void GetNextWaypoint()
    {
        if (wavepointIndex >= wayPoints.Length - 1)
        {
            EndPath();

            return;
        }
        enemy.speed = enemy.startSpeed;
        wavepointIndex++;
        target = wayPoints[wavepointIndex];
    }
    public void GetBackWaypoint()
    {
        if (wavepointIndex <=0)
        {
            enemy.speed = 0;

            return;
        }

        wavepointIndex--;
        target = wayPoints[wavepointIndex];
    }

    void EndPath()
    {
        PlayerStats.Lives = Mathf.Clamp(PlayerStats.Lives - 1, 0, 100);
        WaveSpawner.EnemiesAlive -= enemy.CheckAliveHelpDrones() + 1;
        Destroy(gameObject);
    }
}
